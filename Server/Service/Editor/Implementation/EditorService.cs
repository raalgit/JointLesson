using JL.ApiModels.EditorModels.Request;
using JL.ApiModels.EditorModels.Response;
using JL.DAL.Mongo.Repository;
using JL.DAL.Repository.Abstraction;
using JL.Service.Editor.Abstraction;
using JL.Settings;
using JL.Utility2L.Implementation;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JL.Utility2L.Models.Material;
using JL.Utility2L.Abstraction;
using JL.Persist;

namespace JL.Service.Editor.Implementation
{
    public class EditorService : IEditorService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IManualRepository _manualRepository;

        private readonly IFileUtility _fileUtility;

        public EditorService(IServiceProvider serviceProvider,
                             IMongoRepository mongoRepository,
                             IFileDataRepository fileDataRepository,
                             IManualRepository manualRepository)
        {
            _serviceProvider = serviceProvider;
            _mongoRepository = mongoRepository;
            _fileDataRepository = fileDataRepository;
            _manualRepository = manualRepository;

            _fileUtility = new FileUtility(_mongoRepository, _fileDataRepository);
        }

        public async Task<NewMaterialResponse> NewMaterial(NewMaterialRequest request, UserSettings userSettings)
        {
            var response = new NewMaterialResponse();

            var manualJsonBuffer = JsonSerializer.SerializeToUtf8Bytes<ManualData>(request.ManualData);
            Stream stream = new MemoryStream(manualJsonBuffer);
            var fileId = await _fileUtility.CreateNewFileAsync(stream, request.OriginalName, "jl");

            var manual = new Manual()
            {
                AuthorId = userSettings.User.Id,
                FileDataId = fileId,
                Title = request.OriginalName
            };

            _manualRepository.Insert(manual);
            _manualRepository.SaveChanges();
            return response;
        }

        public async Task<UpdateMaterialResponse> UpdateMaterial(UpdateMaterialRequest request, UserSettings userSettings)
        {
            var response = new UpdateMaterialResponse();
            var manualJsonBuffer = JsonSerializer.SerializeToUtf8Bytes<ManualData>(request.ManualData);
            Stream stream = new MemoryStream(manualJsonBuffer);

            var originalFileData = _fileDataRepository.GetById(request.OriginalFileDataId) ?? throw new NullReferenceException();
            var updatedFileId = await _fileUtility.UpdateFileAsync(stream, originalFileData.MongoId, request.OriginalName, "jl");
            
            var fileData = _manualRepository.Get().Where(x => x.FileDataId == request.OriginalFileDataId).First();
            fileData.FileDataId = updatedFileId;

            _manualRepository.Update(fileData);
            _manualRepository.SaveChanges();

            return response;
        }

        public async Task<GetMaterialResponse> GetMaterial(int fileId, UserSettings userSettings)
        {
            var response = new GetMaterialResponse();
            
            var fileData = _fileUtility.GetFileData(fileId);
            var fileBytes = await _fileUtility.GetFileAsBytesById(fileData.MongoId);
            var manual = JsonSerializer.Deserialize<ManualData>(fileBytes) ?? throw new NullReferenceException();
            response.ManualData = manual;

            return response;
        }

        public async Task<GetMyMaterialsResponse> GetMyMaterials(UserSettings userSettings)
        {
            var response = new GetMyMaterialsResponse();

            response.Manuals = _manualRepository.Get().Where(x => x.AuthorId == userSettings.User.Id).ToList();

            return response;
        }
    }
}
