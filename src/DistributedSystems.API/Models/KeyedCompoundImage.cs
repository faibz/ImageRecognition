using System.Collections.Generic;

namespace DistributedSystems.API.Models
{
    public class KeyedCompoundImage
    {
        public IList<MapImagePart> Images { get; set; }
        public string ImageKey { get; set; }

    }
}
