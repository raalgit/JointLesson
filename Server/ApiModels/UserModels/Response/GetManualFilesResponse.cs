using JL.PersistModels;
using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Response
{
    public class GetManualFilesResponse : ResponseBase, IResponse
    {
        public List<FileData> FileDatas { get; set; }
    }
}
