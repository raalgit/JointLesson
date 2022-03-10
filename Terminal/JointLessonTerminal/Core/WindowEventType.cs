using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core
{
    public enum WindowEventType
    {
        AUTHORIZED,
        COURSESELECTED,
        NEEDTOOPENEDITORPAGE,
        NEEDTOOPENLESSONPPAGE,
        NEEDTOOPENSRSLESSON,
        EXITFROMLESSON,
        EXITFROMSRSLESSON,

        AUTH_EMPTYLOGIN,
        AUTH_ERROR,

        COURSELIST_GETERROR,

        LESSON_LOADMANUALERROR,
        LESSON_SYNCERROR,

        EDITOR_MYMANUALSLOADED,
        EDITOR_MANUALDATALOADED,
        EDITOR_MANUALCREATEDERROR,
        EDITOR_MANUALCREATED,
        EDITOR_MANUALUPDATEDERROR,
        EDITOR_MANUALUPDATED,
        EDITOR_ONFILEUPLOAD,
        EDITOR_ONFILEUPLOADERROR,
    }
}
