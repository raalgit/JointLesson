using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Request
{
    [Serializable]
    public class UpHandRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
