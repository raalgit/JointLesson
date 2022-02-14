using JointLessonTerminal.Core;
using JointLessonTerminal.Core.Material;
using JointLessonTerminal.Model.ServerModels;
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
        
        
        public Manual Manual { get { return manual; } set { manual = value; OnPropsChanged("Manual"); } }
        private Manual manual;

        private MaterialHandler materialHandler;

        private ManualData manualData;
        public ManualData ManualData { get { return manualData; } set { manualData = value; OnPropsChanged("ManualData"); } }


        public CurrentCourseWindowViewModel()
        {

        }

        public void InitData(CourseModel course)
        {
            Course = course;
            materialHandler = new MaterialHandler();
            Manual = null;
            ManualData = null;

            Task.Factory.StartNew(async x => {
                ManualData = await loadManualData();
            }, null);
        }

        private async Task<Manual> loadManual(int manualId)
        {
            var courseManual = await materialHandler.LoadManualById(manualId);
            Manual = courseManual;
            return courseManual;
        }

        private async Task<ManualData> loadManualData()
        {
            var currentManual = await loadManual(Course.ManualId);
            ManualData = await materialHandler.LoadById(manual.fileDataId.Value);
            return ManualData;
        }
    }
}
