using System;
using System.Collections.Generic;

namespace ImageRecognition.Shared.Models.Requests
{
    public class ImageTagData
    {
        public Guid ImageId { get; set; }
        
        public IList<Tag> TagData { get; set; }
        public string Key { get; set; }
        public Guid MapId { get; set; }
    }
} 