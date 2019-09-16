using System;

namespace ImageRecognition.Shared.Models
{
    public class CompoundImageTag
    {
        public Guid CompoundImageId { get; set; }
        public string Tag { get; set; }
        public decimal Confidence { get; set; }
    }
}