namespace jointLessonServer.ModelsAPI
{
    public class ResponseBase : IResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
