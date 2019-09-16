using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageRecognition.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly ICompoundImagesRepository _compoundImagesRepository;


        public DataController(IImagesRepository imagesRepository, ICompoundImagesRepository compoundImagesRepository)
        {
            _imagesRepository = imagesRepository;
            _compoundImagesRepository = compoundImagesRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ImageProcessingTime()
        {
            var recentProcessedImages = await _imagesRepository.GetRecentProcessedImageDates();

            if (recentProcessedImages.Count == 0) return Ok(new TimeSpan(50000000)); //50m ticks = 5 seconds

            var dateTimeDeltas = new List<TimeSpan>();

            foreach(var img in recentProcessedImages)
            {
                dateTimeDeltas.Add((img.ProcessedDate - img.UploadedDate).Value);
            }

            var averageTicks = Convert.ToInt64(dateTimeDeltas.Average(timespan => timespan.Ticks));

            return Ok(new TimeSpan(averageTicks));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CompoundImageProcessingTime()
        {
            var recentProcessedImages = await _compoundImagesRepository.GetRecentProcessedImageDates();

            if (recentProcessedImages.Count == 0) return Ok(new TimeSpan(50000000)); //50m ticks = 5 seconds

            var dateTimeDeltas = new List<TimeSpan>();

            foreach (var img in recentProcessedImages)
            {
                dateTimeDeltas.Add((img.ProcessedDate - img.UploadedDate).Value);
            }

            var averageTicks = Convert.ToInt64(dateTimeDeltas.Average(timespan => timespan.Ticks));

            return Ok(new TimeSpan(averageTicks));
        }
    }
}