using System;

namespace DistributedSystems.API.Models
{
    public class Map
    {
        public Map() { }

        public Map(int columnCount, int rowCount)
        {
            Id = Guid.NewGuid();
            ColumnCount = columnCount;
            RowCount = rowCount;
        }

        public Guid Id { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }

        public static explicit operator Map(DTOs.Map map)
        {
            return new Map
            {
                Id = map.Id,
                ColumnCount = map.ColumnCount,
                RowCount = map.RowCount
            };
        }
    }
}