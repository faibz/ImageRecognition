using System;

namespace DistributedSystems.API.Models
{
    public class Map
    {
        public Map(int columnCount, int rowCount)
        {
            Id = Guid.NewGuid();
            ColumnCount = columnCount;
            RowCount = rowCount;
        }

        public Guid Id { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
    }
}