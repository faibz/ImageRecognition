namespace DistributedSystems.Shared.Models
{
    public enum ImageStatus
    {
        Errored = 0,
        UploadComplete = 1,
        AwaitingProcessing = 2,
        Processing = 3,
        Reprocessing = 4,
        Complete = 5
    }
}