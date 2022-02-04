namespace jointLessonServer.ModelsAPI
{
    public class ResponseBase : IResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
