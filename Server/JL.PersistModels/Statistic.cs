using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Statistic", Schema = "JL")]
    public class Statistic : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SuccessExecution { get; set; }
        public string FailedExecution { get; set; }
    }
}
