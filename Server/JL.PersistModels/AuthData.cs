using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("AuthData", Schema = "JL")]
    public class AuthData : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
