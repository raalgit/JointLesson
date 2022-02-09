using JL.Persist;

namespace jointLessonServer.ModelsAPI.AuthModels.Response
{
    public class LoginResponse : ResponseBase, IResponse
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
        public string JWT { get; set; }
    }
}
