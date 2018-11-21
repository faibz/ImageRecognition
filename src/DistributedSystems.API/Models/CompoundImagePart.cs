namespace DistributedSystems.API.Models
{
    public class CompoundImagePart
    {
        public CompoundImagePart(MapImagePart mapImagePart)
        {
            Image = new Image(mapImagePart.ImageId);
            Coordinate = new Coordinate(mapImagePart.CoordinateX, mapImagePart.CoordinateY);
        }

        public Image Image { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
