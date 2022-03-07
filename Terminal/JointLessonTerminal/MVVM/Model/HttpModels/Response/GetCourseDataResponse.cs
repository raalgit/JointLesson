using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Model.ServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Response
{
    [Serializable]
    public class GetCourseDataResponse : IResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }

        public string lastPage { get; set; }

        public List<CourseTeacher> courseTeachers { get; set; }

        public bool isTeacher { get; set; }
        public bool lessonIsActive { get; set; }

        // Для учащихся
        public GroupAtCourse courseData { get; set; }
        public Lesson lesson { get; set; }
    }
}
