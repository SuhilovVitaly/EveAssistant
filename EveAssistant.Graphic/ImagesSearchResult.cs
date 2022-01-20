using System.Collections.Generic;
using EveAssistant.Common;

namespace EveAssistant.Graphic
{
    public class ImagesSearchResult: IImagesSearchResult
    {
        public bool IsFound { get; set; } = false;

        public double ExecuteTimeInMilliseconds { get; set; }
        public List<IImageSearchResult> SearchResults { get; set; } = new List<IImageSearchResult>();
    }
}