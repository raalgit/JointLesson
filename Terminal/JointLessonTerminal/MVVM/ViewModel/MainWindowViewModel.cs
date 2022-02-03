using JointLessonTerminal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public AuthWindowViewModel AuthVM { get; set; }
        public CourseListViewModel CourseVM { get; set; }
        public EditorWindowViewModel EditorVM { get; set; }
        public CurrentCourseWindowViewModel CurrentCourseVM { get; set; }


        public RelayCommand AuthOpenCommand { get; set; }
        public RelayCommand CourseOpenCommand { get; set; }
        public RelayCommand EditorOpenCommand { get; set; }
        public RelayCommand CurrentCourseOpenCommand { get; set; }


        private object _currentView;
        public object CurrentView { 
            get {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropsChanged();
            }
        }


        public MainWindowViewModel()
        {
            AuthVM = new AuthWindowViewModel();
            CourseVM = new CourseListViewModel();
            EditorVM = new EditorWindowViewModel();
            CurrentCourseVM = new CurrentCourseWindowViewModel();

            CurrentView = AuthVM;

            AuthOpenCommand = new RelayCommand(x => CurrentView = AuthVM);
            CourseOpenCommand = new RelayCommand(x => CurrentView = CourseVM);
            EditorOpenCommand = new RelayCommand(x => CurrentView = EditorVM);
            CurrentCourseOpenCommand = new RelayCommand(x => CurrentView = CurrentCourseVM);
        }

    }
}
