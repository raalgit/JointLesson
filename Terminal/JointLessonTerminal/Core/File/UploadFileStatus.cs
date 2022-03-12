using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.File
{
    public class UploadFileStatus
    {
        public UploadFileStatus(int fileId, bool isSuccess)
        {
            FileId = fileId;
            IsSuccess = isSuccess;
        }

        public int FileId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
