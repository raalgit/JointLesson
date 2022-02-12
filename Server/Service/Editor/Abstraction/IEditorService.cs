using JL.ApiModels.EditorModels.Request;
using JL.ApiModels.EditorModels.Response;
using JL.Settings;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.Editor.Abstraction
{
    public interface IEditorService : IServiceJL
    {
        Task<NewMaterialResponse> NewMaterial(NewMaterialRequest request, UserSettings userSettings);
        Task<GetMaterialResponse> GetMaterial(int fileId, UserSettings userSettings);
        Task<GetMyMaterialsResponse> GetMyMaterials(UserSettings userSettings);
        Task<UpdateMaterialResponse> UpdateMaterial(UpdateMaterialRequest request, UserSettings userSettings);
    }
}
