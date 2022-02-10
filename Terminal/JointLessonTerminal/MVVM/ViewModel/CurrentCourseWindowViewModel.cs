using JointLessonTerminal.Core;
using JointLessonTerminal.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class CurrentCourseWindowViewModel : ObservableObject
    {
        public CourseModel Course { get; set; }

        public CurrentCourseWindowViewModel()
        {

        }

        public void InitData(CourseModel course)
        {
            Course = course;
        }
    }
}
