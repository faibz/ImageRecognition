using System;

namespace DistributedSystems.API.Models.DTOs
{
    public class Map
    {
        public Guid Id { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }

    }
}