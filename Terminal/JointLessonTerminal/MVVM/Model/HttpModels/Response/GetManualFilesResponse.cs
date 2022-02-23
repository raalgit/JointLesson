using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model.ServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Response
{
    [Serializable]
    public class GetManualFilesResponse : IResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public List<FileData> fileDatas { get; set; }
    }
}
