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
        public async Task<bool> SaveAtDataBase(ManualData manual)
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

            try
            {
                var responsePost = await sender.SendRequest(manualSaveRequest, "/editor/material");
                return responsePost.isSuccess;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAtDataBase(ManualData manual, int originalFileId)
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

            try
            {
                var responsePost = await sender.SendRequest(manualUpdateRequest, "/editor/material");
                return responsePost.isSuccess;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<ObservableCollection<Manual>> GetMyMaterials()
        {
            var manualGetMyMaterialsRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get
            };
            var sender = new RequestSender<object, GetMyMaterialsResponse>();

            try
            {
                var responsePost = await sender.SendRequest(manualGetMyMaterialsRequest, "/editor/my-materials");
                return responsePost.manuals;
            }
            catch (Exception er)
            {
                return new ObservableCollection<Manual>();
            }
        }

        public async Task<Manual> LoadManualById(int id)
        {
            var getManualRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{id}"
            };
            var sender = new RequestSender<object, GetManualByIdResponse>();

            try
            {
                var responsePost = await sender.SendRequest(getManualRequest, "/editor/course-material");
                return responsePost.manual;
            }
            catch (Exception er)
            {
                return new Manual();
            }
        }

        public async Task<ManualData> LoadById(int fileId)
        {
            var manualGetRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{fileId}"
            };
            var sender = new RequestSender<object, GetMaterialResponse>();

            try
            {
                var responsePost = await sender.SendRequest(manualGetRequest, "/editor/material");
                return responsePost.manualData;
            }
            catch (Exception er)
            {
                return new ManualData();
            }
        }
    }
}
