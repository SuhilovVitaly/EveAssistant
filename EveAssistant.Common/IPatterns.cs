using System.Collections.Generic;
using System.IO;

namespace EveAssistant.Common
{
    public interface IPatterns
    {
        List<IPattern> Get(string key);

        IPatternsCollection GetPatterns(string key);
    }
}