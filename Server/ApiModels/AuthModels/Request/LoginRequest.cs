using JL.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace jointLessonServer.ModelsAPI.AuthModels.Request
{
    public class LoginRequest : IRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
