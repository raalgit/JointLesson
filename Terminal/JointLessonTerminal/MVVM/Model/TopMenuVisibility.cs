using JointLessonTerminal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.MVVM.Model
{
    public class TopMenuVisibility : ObservableObject
    {
        public Visibility ProfileBtnVisibility
        { 
            get 
            {
                return profileBtnVisibility;
            } 
            set 
            { 
                profileBtnVisibility = value;
                OnPropsChanged("ProfileBtnVisibility");
            } 
        }

        public Visibility BackBtnVisibility
        {
            get
            {
                return backBtnVisibility;
            }
            set 
            { 
                backBtnVisibility = value;
                OnPropsChanged("BackBtnVisibility");
            }
        }

        public Visibility ExitBtnVisibility
        {
            get
            {
                return exitBtnVisibility;
            }
            set
            {
                exitBtnVisibility = value;
                OnPropsChanged("ExitBtnVisibility");
            }
        }

        private Visibility profileBtnVisibility;
        private Visibility backBtnVisibility;
        private Visibility exitBtnVisibility;
    }
}
