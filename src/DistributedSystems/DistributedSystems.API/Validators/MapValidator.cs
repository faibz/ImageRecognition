using System.Collections.Generic;
using DistributedSystems.API.Models;

namespace DistributedSystems.API.Validators
{
    public interface IMapValidator
    {
        IList<Error> ValidateCreateImageMapRequest(int columnCount, int rowCount);
    }

    public class MapValidator : IMapValidator
    {
        public IList<Error> ValidateCreateImageMapRequest(int columnCount, int rowCount)
        {
            var errors = new List<Error>();

            if (columnCount <= 0 || rowCount <= 0) errors.Add(new Error("map_dimensions", "Column and row counts must be above zero."));

            return errors;
        }
    }
}