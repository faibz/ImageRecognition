using System.Threading.Tasks;
using DistributedSystems.API.Models;

namespace DistributedSystems.API.Repositories
{
    public interface IImagesRepository
    {
        Task<Image> InsertImage(Image image);
    }

    public class ImagesRepository : IImagesRepository
    {
        public Task<Image> InsertImage(Image image)
        {
            throw new System.NotImplementedException();
        }
    }
}
