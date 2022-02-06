namespace jointLessonServer.ModelsAPI.AuthModels.Response
{
    public class LoginResponse : ResponseBase, IResponse
    {
        public string JWT { get; set; }
    }
}
