using ImageRecognition.Shared.Models;

namespace ImageRecognition.Client
{
    public class ImageRequest
    {
        public string Image { get; set; }
        public MapData MapData { get; set; }
    }
}
