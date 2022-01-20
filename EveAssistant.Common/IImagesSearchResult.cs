using System.Collections.Generic;

namespace EveAssistant.Common
{
    public interface IImagesSearchResult
    {
        bool IsFound { get; set; }
        double ExecuteTimeInMilliseconds { get; set; }
        List<IImageSearchResult> SearchResults { get; set; }

    }
}