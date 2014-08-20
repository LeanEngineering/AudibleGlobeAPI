using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AudibleGlobeApiWorkerRole.DAO;
using AudibleGlobeApiWorkerRole.Models;
using Dapper;

namespace AudibleGlobeApiWorkerRole.Controllers
{
    public class ChannelsController : ApiController
    {
        private readonly ChannelsRepository _channelsRepository;

        public ChannelsController()
        {
            _channelsRepository = new ChannelsRepository();
        }

        [Route("channels")]
        public IHttpActionResult GetAllChannels()
        {
            return Ok(_channelsRepository.GetAllChannels());
        }

        [Route("providers/{providerId:int}/channels")]
        public IHttpActionResult GetChannelsForProvider(int providerId)
        {
            return Ok(new ChannelsRepository().GetChannelsByProviderId(providerId));
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}")]
        public IHttpActionResult PutChannelUpdatesForProvider(int providerId, int channelId, Channel updatedChannel)
        {
            if (providerId != updatedChannel.ChannelProviderId || channelId != updatedChannel.ChannelId)
            {
                return BadRequest("ProviderId and ChannelId of API request must match Id's of body");
            }

            new ChannelsRepository().UpdateChannelById(updatedChannel);

            return Ok();
        }

        [Route("providers/{providerId:int}/channels/{channelId:int}")]
        public IHttpActionResult DeleteChannel(int providerId, int channelId)
        {
            new ChannelsRepository().DeleteChannel(providerId, channelId);

            return Ok();
        }

        [Route("providers/{providerId:int}/channels")]
        public IHttpActionResult PostNewChannelForProvider(int providerId, Channel newChannel)
        {
            return Ok(new ChannelsRepository().AddNewChannelToProvider(providerId, newChannel));
        }
    }

    public class ChannelsRepository
    {
        public IEnumerable<Channel> GetAllChannels()
        {
            using (var conn = Db.AzureSql())
            {
                return conn.Query<Channel>("SELECT ChannelID, ChannelName, ChannelProviderID FROM Channels");
            }
        }

        public IEnumerable<Channel> GetChannelsByProviderId(int providerId)
        {
            using (var connections = Db.AzureSql())
            {
                return connections.Query<Channel>("SELECT ChannelID, ChannelName, ChannelProviderID FROM Channels WHERE ChannelProviderID = @ChannelProviderId", new { ChannelProviderId = providerId });
            }
        }

        public Channel AddNewChannelToProvider(int providerId, Channel newChannel)
        {
            using (var connections = Db.AzureSql())
            {
                return connections.Query<Channel>("INSERT INTO Channels (ChannelName, ChannelProviderID) OUTPUT INSERTED.ChannelID, INSERTED.ChannelName, INSERTED.ChannelProviderId VALUES (@ChannelName, @ChannelProviderId)", newChannel).FirstOrDefault();
            }
        }

        public void UpdateChannelById(Channel channel)
        {
            using (var connections = Db.AzureSql())
            {
                connections.Execute("UPDATE Channels SET ChannelName = @ChannelName WHERE ChannelID = @ChannelId", new { channel });
            }
        }

        public void DeleteChannel(int providerId, int channelId)
        {
            using (var connections = Db.AzureSql())
            {
                connections.Execute("DELETE FROM Channels WHERE ChannelID = @channelId AND ChannelProviderID = @providerId", new { providerId, channelId });
            }
        }
    }
}
