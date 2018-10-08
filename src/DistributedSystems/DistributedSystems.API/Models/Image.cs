using System;
using DistributedSystems.API.Models.Requests;

namespace DistributedSystems.API.Models
{
    public class Image
    {
        //TODO: HOW WORTH IS THIS CONSTRUCTOR VS JUST SETTING DEFAULT ON ID TO NEW GUID? XD
        //TODO: DEFINITELY NEED TO THINK ABOUT THAT MAP
        //TODO: COULD ADD IN OTHER INFORMATION HERE LIKE DATE ETC OR JUST DO RIGHT BEFORE AN INSERT

        public Image()
        {

        }

        public Image(ImageRequest imageRequest)
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Location { get; set; }
    }
}
