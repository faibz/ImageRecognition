﻿using System;

namespace DistributedSystems.API.Models.Requests
{
    public class ImageProcessRequest
    {
        public ImageProcessRequest(Image img, Guid mapId)
        {
            Id = img.Id;
            Location = img.Location;
            ImageKey = img.ImageKey;
            MapId = mapId;
        }
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string ImageKey { get; set; }
        public Guid MapId { get; set; }
    }
}
