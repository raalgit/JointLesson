using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.ServerModels
{
    [Serializable]
    public class FileData
    {
        public int id { get; set; }
        public string mongoName { get; set; }
        public string originalName { get; set; }
        public string mongoId { get; set; }
    }
}
