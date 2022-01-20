using System.Drawing;

namespace EveAssistant.Logic.Jobs.Status
{
    public class CheckStatusResult
    {
        public bool IsFound { get; set; }

        public Point OnScreen { get; set; }
    }
}