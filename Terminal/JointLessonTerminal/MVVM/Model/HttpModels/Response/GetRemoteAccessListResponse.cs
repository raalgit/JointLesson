using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model.ServerModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Response
{
    [Serializable]
    public class GetRemoteAccessListResponse : IResponse
    {
        public ObservableCollection<UserRemoteAccessWithUserData> userRemoteAccesses { get; set; }

        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
}
