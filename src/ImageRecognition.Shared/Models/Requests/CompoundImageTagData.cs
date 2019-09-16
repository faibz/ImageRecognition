using System;
using System.Collections.Generic;

namespace ImageRecognition.Shared.Models.Requests
{
    public class CompoundImageTagData
    {
        public Guid CompoundImageId { get; set; }
        public Guid MapId { get; set; }
        public IList<Tag> Tags { get; set; }
        public string Key { get; set; }
    }
} 