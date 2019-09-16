using System;

namespace ImageRecognition.API.Models.DTOs
{
    public class Map
    {
        public Guid Id { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public static explicit operator Shared.Models.Map(Map map)
        {
            return new Shared.Models.Map
            {
                Id = map.Id,
                ColumnCount = map.ColumnCount,
                RowCount = map.RowCount
            };
        }

    }
}