using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Core.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Response
{
    [Serializable]
    public class GetMaterialResponse : IResponse
    {
        public string originalName { get; set; }
        public ManualData manualData { get; set; }

        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
}
