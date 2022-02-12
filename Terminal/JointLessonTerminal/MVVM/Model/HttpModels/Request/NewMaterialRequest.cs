﻿using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Core.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class NewMaterialRequest : IRequest
    {
        public string OriginalName { get; set; }
        public ManualData ManualData { get; set; }
    }
}
