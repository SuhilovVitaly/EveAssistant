using System.Collections;
using System.Collections.Generic;

namespace EveAssistant.Common
{
    public interface IPatternsCollection : IEnumerable
    {
        List<IPattern> PatternEntity { get; set; }
        IEnumerator GetEnumerator();
    }
}