using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System.Data;

namespace DistributedSystems.API.Repositories
{
    public interface ICompoundImagesRepository
    {
        void InsertCompoundImage(CompoundImage compoundImage);
    }

    public class CompoundImagesRepository : ICompoundImagesRepository
    {
        private readonly IDbConnection _connection;

        public CompoundImagesRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public void InsertCompoundImage(CompoundImage compoundImage)
        {
            throw new System.NotImplementedException();
        }
    }
}