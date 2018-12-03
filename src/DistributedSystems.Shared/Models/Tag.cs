namespace DistributedSystems.Shared.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public decimal Confidence { get; set; }

        public Tag(string name, decimal confidence)
        {
            Name = name;
            Confidence = confidence;
        }

        public static explicit operator Tag(CompoundImageTag compoundImageTag)
        {
            return new Tag(compoundImageTag.Tag, compoundImageTag.Confidence);
        }
    }
}