using JL.ApiModels;

namespace jointLessonServer.ModelsAPI.AuthModels.Request
{
    public class LoginRequest : IRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
