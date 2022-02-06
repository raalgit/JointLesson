namespace jointLessonServer.ModelsAPI
{
    public interface IResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
