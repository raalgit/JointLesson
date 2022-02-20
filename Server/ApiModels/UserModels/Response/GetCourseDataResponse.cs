using JL.Persist;
using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Response
{
    public class GetCourseDataResponse : ResponseBase, IResponse
    {
        public List<CourseTeacher> CourseTeachers { get; set; }
        
        public bool IsTeacher { get; set; }
        public bool LessonIsActive { get; set; }

        // Для учащихся
        public GroupAtCourse CourseData { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
