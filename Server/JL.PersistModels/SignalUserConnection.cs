using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.PersistModels
{
    [Table("SignalUserConnection", Schema = "JL")]
    public class SignalUserConnection : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
