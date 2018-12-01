namespace DistributedSystems.Shared.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public decimal Confidence { get; set; }



        public static explicit operator Tag(CompoundImageTag compoundImageTag)
        {
            return new Tag
            {
                Name = compoundImageTag.Tag,
                Confidence = compoundImageTag.Confidence
            };
        }
    }
}