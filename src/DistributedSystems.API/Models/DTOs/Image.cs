using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public DateTime UploadedDate { get; set; }
        public ImageStatus Status { get; set; }
    }
}
