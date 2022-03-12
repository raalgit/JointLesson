using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.PersistModels
{
    [Table("UserRemoteAccess", Schema = "JL")]
    public class UserRemoteAccess : IPersist
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string ConnectionData { get; set; }
        public DateTime StartDate { get; set; }
    }
}
