using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    public class AddNewFileRequest
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
    }
}
