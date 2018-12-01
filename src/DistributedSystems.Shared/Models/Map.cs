using System;

namespace DistributedSystems.Shared.Models
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
    }
}