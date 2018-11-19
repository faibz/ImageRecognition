using DistributedSystems.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistributedSystems.API.Utils
{
    public interface ITagsAnalyser
    {
        Task<TagAnalysisAction> AnalyseTagConfidence(IList<Tag> tags);
    }

    public class TagsAnalyser : ITagsAnalyser
    {
        public async Task<TagAnalysisAction> AnalyseTagConfidence(IList<Tag> tags)
        {
            throw new System.NotImplementedException();
        }
    }
}