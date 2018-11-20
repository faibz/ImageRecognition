﻿using DistributedSystems.API.Models;
using System.Collections.Generic;
using System.Linq;
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
            if (tags.Any(tag => tag.Confidence < 0.5m)) return TagAnalysisAction.RequestCompoundImage;

            return TagAnalysisAction.Continue;
        }
    }
}