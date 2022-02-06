namespace JointLessonTerminal.Core.HTTPRequests
{
    public interface IResponse
    {
        bool isSuccess { get; set; }
        string message { get; set; }
    }
}
