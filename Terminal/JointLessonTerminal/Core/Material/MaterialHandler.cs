using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Model.ServerModels;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.Core.Material
{
    public class MaterialHandler
    {
        public async Task SaveAtDataBase(ManualData manual)
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            var manualSaveRequest = new RequestModel<NewMaterialRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new NewMaterialRequest()
                {
                    ManualData = manual,
                    OriginalName = manual.name
                },
            };

            var sender = new RequestSender<NewMaterialRequest, NewMaterialResponse>();
            var responsePost = await sender.SendRequest(manualSaveRequest, "/editor/material");
            var responseString = responsePost.isSuccess ? "Материал добавлен" : "Ошибка!" + responsePost.message;
            MessageBox.Show(responseString);
        }

        public async void UpdateAtDataBase(ManualData manual, int originalFileId)
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            manual.materialDate.modified = DateTime.Now;
            var manualUpdateRequest = new RequestModel<UpdateMaterialRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Put,
                Body = new UpdateMaterialRequest()
                {
                    OriginalName = manual.name ?? "Без имени",
                    OriginalFileDataId = originalFileId,
                    ManualData = manual
                },
            };

            var sender = new RequestSender<UpdateMaterialRequest, UpdateMaterialResponse>();
            var responsePost = await sender.SendRequest(manualUpdateRequest, "/editor/material");
            var responseString = responsePost.isSuccess ? "Материал обновлен" : "Ошибка!" + responsePost.message;
        }

        public async Task<ObservableCollection<Manual>> GetMyMaterials()
        {
            var manualGetMyMaterialsRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get
            };
            var sender = new RequestSender<object, GetMyMaterialsResponse>();
            var responsePost = await sender.SendRequest(manualGetMyMaterialsRequest, "/editor/my-materials");

            var responseString = responsePost.isSuccess ? "Материал получен" : "Ошибка!" + responsePost.message;
            return responsePost.manuals;
        }

        public async Task<ManualData> LoadById(int fileId)
        {
            var manualGetRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{fileId}"
            };
            var sender = new RequestSender<object, GetMaterialResponse>();
            var responsePost = await sender.SendRequest(manualGetRequest, "/editor/material");

            var responseString = responsePost.isSuccess ? "Материал получен" : "Ошибка!" + responsePost.message;
            return responsePost.manualData;
        }
    }
}
