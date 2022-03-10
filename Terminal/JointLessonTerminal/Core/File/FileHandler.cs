using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.Core.File
{
    public class FileHandler
    {
        public async Task<UploadFileStatus> UploadFile(byte[] file, string name)
        {
            UploadFileStatus response = null;

            var fileSaveRequest = new RequestModel<AddNewFileRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new AddNewFileRequest()
                {
                   File = file,
                   Name = name
                },
            };

            var sender = new RequestSender<AddNewFileRequest, AddNewFileResponse>();
            var responsePost = await sender.SendRequest(fileSaveRequest, "/user/file");
            response = new UploadFileStatus(responsePost.fileDataId, responsePost.isSuccess);
            return response;
        }
    }
}
