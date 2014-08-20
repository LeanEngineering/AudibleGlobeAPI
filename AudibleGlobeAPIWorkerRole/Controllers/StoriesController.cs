using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AudibleGlobeApiWorkerRole.DAO;
using AudibleGlobeApiWorkerRole.Models;
using AudibleGlobeAPIWorkerRole.Models;
using Dapper;

namespace AudibleGlobeApiWorkerRole.Controllers
{
    public class StoriesController : ApiController
    {
        private readonly StoriesRepository _storiesRepository;

        public StoriesController()
        {
            _storiesRepository = new StoriesRepository();
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}/stories/{storyId:int}")]
        public IHttpActionResult GetStoryById(int providerId, int channelId, int storyId)
        {
            new StoriesRepository().GetStoryById(storyId);
            return Ok();
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}/stories")]
        public IHttpActionResult GetStoriesForChannel(int providerId, int channelId)
        {
            return Ok(new StoriesRepository().GetStoriesByChannelId(channelId));
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}/stories/{storyId:int}")]
        public IHttpActionResult PutStoryUpdates(int providerId, int channelId, int storyId, Story updatedStory)
        {
            if (updatedStory == null)
            {
                return BadRequest("Story object cannot be null");
            }

            if (channelId != updatedStory.StoryChannelId)
            {
                return BadRequest("ProviderId and ChannelId of API request must match Id's of body");
            }

            new StoriesRepository().UpdateStory(updatedStory);

            return Ok();
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}/stories/{storyId:int}/coordinates")]
        public IHttpActionResult PutCoordinateUpdates(int providerId, int channelId, int storyId, GeoCoordinates coordinates)
        {
            new StoriesRepository().UpdateCoordinatesOfStory(storyId, coordinates);

            return Ok();
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}/stories/{storyId:int}")]
        public IHttpActionResult DeleteStory(int providerId, int channelId, int storyId)
        {
            new StoriesRepository().DeleteStory(channelId, storyId);

            return Ok();
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}/stories")]
        public IHttpActionResult PostNewStoryForChannel(int providerId, int channelId, Story newStory)
        {
            if (newStory == null)
            {
                return BadRequest("Story object cannot be null");
            }

            if (newStory.Latitude == null || newStory.Longitude == null)
            {
                return Ok(new StoriesRepository().AddNewStoryWithoutCoordinatesToChannel(providerId, channelId, newStory));    
            }

            return Ok(new StoriesRepository().AddNewStoryToChannel(providerId, channelId, newStory));
        }

        [Route("stories/{lat:double},{lon:double}/within/{radius:float}")]
        public IHttpActionResult GetStoriesWithinRadiusOfPoint(double lat, double lon, float radius)
        {
            var stories = (new StoriesRepository()).GetStoriesWithinRadiusOfPoint(lat, lon, radius);
            return Ok(stories);
        }
    }

    public class StoriesRepository
    {
        public IEnumerable<Story> GetStoriesByChannelId(int channelId)
        {
            using (var connections = Db.AzureSql())
            {
                return connections.Query<Story>("SELECT StoryID, StoryChannelID, StoryTitle, StoryDescription, StoryCoordinates.Lat as Latitude, StoryCoordinates.Long as Longitude FROM Stories " +
                                                "WHERE StoryChannelID = @StoryChannelId",
                                                  new { StoryChannelId = channelId });
            }
        }

        public Story AddNewStoryToChannel(int providerId, int channelId, Story newStory)
        {
            using (var connections = Db.AzureSql())
            {
                return connections.Query<Story>("INSERT INTO Stories (StoryChannelID, StoryTitle, StoryDescription, StoryCoordinates) " +
                                                  "OUTPUT INSERTED.StoryID, INSERTED.StoryChannelID, INSERTED.StoryTitle, INSERTED.StoryDescription, INSERTED.StoryCoordinates.Lat as Latitude, INSERTED.StoryCoordinates.Long as Longitude " +
                                                  "VALUES (@StoryChannelId, @StoryTitle, @StoryDescription, GEOGRAPHY::Point(@Latitude, @Longitude, 4326))",
                                                  newStory).FirstOrDefault();
            }
        }

        public Story AddNewStoryWithoutCoordinatesToChannel(int providerId, int channelId, Story newStory)
        {
            using (var connections = Db.AzureSql())
            {
                return connections.Query<Story>("INSERT INTO Stories (StoryChannelID, StoryTitle, StoryDescription) " +
                                                  "OUTPUT INSERTED.StoryID, INSERTED.StoryChannelID, INSERTED.StoryTitle, INSERTED.StoryDescription " +
                                                  "VALUES (@StoryChannelId, @StoryTitle, @StoryDescription)",
                                                  newStory).FirstOrDefault();
            }
        }

        public void UpdateStory(Story story)
        {
            using (var connections = Db.AzureSql())
            {
                if (story.Latitude == null || story.Longitude == null)
                {
                    connections.Execute("UPDATE Stories SET StoryChannelID = @StoryChannelId, StoryTitle = @StoryTitle, StoryDescription = @StoryDescription, StoryCoordinates = null WHERE StoryID = @StoryId", story);
                }
                else
                {
                    connections.Execute("UPDATE Stories SET StoryChannelID = @StoryChannelId, StoryTitle = @StoryTitle, StoryDescription = @StoryDescription, StoryCoordinates = GEOGRAPHY::Point(@Latitude, @Longitude, 4326) WHERE StoryID = @StoryId", story);
                }
            }
        }

        public void DeleteStory(int channelId, int storyId)
        {
            using (var connections = Db.AzureSql())
            {
                connections.Execute("DELETE FROM Stories WHERE StoryID = @storyId AND StoryChannelID = @channelId", new { channelId, storyId });
            }
        }

        public IEnumerable<Story> GetStoriesWithinRadiusOfPoint(double lat, double lon, float radius)
        {
            using (var connection = Db.AzureSql())
            {
                return connection.Query<Story>("DECLARE @point GEOGRAPHY = GEOGRAPHY::Point(@lat, @lon, 4326); " +
                                               "SELECT StoryID, StoryChannelID, StoryTitle, StoryDescription, StoryCoordinates.Lat as Latitude, StoryCoordinates.Long as Longitude  " +
                                               "FROM Stories " +
                                               "WHERE @point.STBuffer(@radius).STIntersects(Stories.StoryCoordinates) = 1", new { lat, lon, radius });
            }
        }

        public void UpdateCoordinatesOfStory(int storyId, GeoCoordinates coordinates)
        {
            using (var connection = Db.AzureSql())
            {
                connection.Execute(
                    "UPDATE Stories SET StoryCoordinates = GEOGRAPHY::Point(@Latitude, @Longitude, 4326) WHERE StoryID = @StoryId",
                    new { StoryId = storyId, Latitude = coordinates.Latitude, Longitude = coordinates.Longitude});
            }
        }

        public void GetStoryById(int storyId)
        {
            throw new NotImplementedException();
        }
    }
}
