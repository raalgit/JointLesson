using JointLessonTerminal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model
{
    public class CourseCollection : ObservableObject
    {
        private List<CourseModel> courseModels;

        public List<CourseModel> CourseModels
        {
            get
            {
                return courseModels;
            }
            set
            {
                courseModels = value;
                OnPropsChanged("CourseModels");
            }
        }
    }
}
