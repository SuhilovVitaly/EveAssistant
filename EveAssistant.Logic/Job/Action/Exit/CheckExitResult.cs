namespace EveAssistant.Logic.Job.Action.Exit
{
    public class CheckExitResult
    {
        public bool IsExitFromAction { get; set; } = false;

        public ExitFromActionReason Reason { get; set; }
    }
}