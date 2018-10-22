using System;

namespace DistributedSystems.API.Models
{
    public class ImageTagData
    {
        public Guid ImageId { get; set; }
        
        public Tag[] TagData { get; set; }
    }
} 