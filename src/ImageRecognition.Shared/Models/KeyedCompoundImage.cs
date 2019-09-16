using System;
using System.Collections.Generic;

namespace ImageRecognition.Shared.Models
{
    public class KeyedCompoundImage
    {
        public Guid CompoundImageId { get; set; }
        public Guid MapId { get; set; }
        public IList<CompoundImagePart> Images { get; set; }
        public string ImageKey { get; set; }
    }
}
