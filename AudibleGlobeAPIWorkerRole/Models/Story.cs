using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AudibleGlobeApiWorkerRole.Models
{
    public class AudioFile
    {
        public int AudioeFileId { get; set; }
        public string AudioFileName { get; set; }
    }

    public class Story
    {
        public int StoryId { get; set; }
        public int StoryChannelId { get; set; }
        public string StoryTitle { get; set; }
        public string StoryDescription { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}