namespace DistributedSystems.Shared.Models.Results
{
    public class UploadImageResult
    {
        public UploadImageResult(bool success, Image image)
        {
            Success = success;
            Image = image;
        }

        public bool Success { get; set; }
        public Image Image { get; set; }
    }
}