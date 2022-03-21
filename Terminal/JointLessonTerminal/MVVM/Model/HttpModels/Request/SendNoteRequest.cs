﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class SendNoteRequest
    {
        public byte[] File { get; set; }
        public string Page { get; set; }
    }
}
