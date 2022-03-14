using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.MVVM.Model.EventModels.Inner
{
    [Serializable]
    public class UserAtLesson
    {
        public int UserId { get; set; }
        public bool UpHand { get; set; }
        public bool IsTeacher { get; set; }
        public string UserFio { get; set; }


        [JsonIgnore]
        public Visibility UpHandVisibility { get; set; }
        [JsonIgnore]
        public Visibility IsTeacherVisibility { get; set; }
    }
}
