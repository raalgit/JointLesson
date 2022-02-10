using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Mongo.Repository
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IGridFSBucket gridFS;

        public MongoRepository(IMongoDbSettings mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);

            gridFS = new GridFSBucket(database);
        }

        public string GetNewFileName(string fileExtension)
        {
            var fmt = "yyyy-MM-dd HH:mm:ss.fffffff";
            var now = DateTime.Now;
            return now.ToString(fmt) + "." + fileExtension;
        }

        public async Task<string> UploadFileAsync(FileStream fileStream, string fileName)
        {
            ObjectId id = await gridFS.UploadFromStreamAsync(fileName, fileStream);
            return Convert.ToString(id) ?? throw new NullReferenceException(nameof(id));
        }

        public async Task DeleteFileAsync(string id)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            await gridFS.DeleteAsync(objectId);
        }

        public async Task ChangeFileAsync(string id, FileStream newFileStream, string newFileName)
        {
            await DeleteFileAsync(id);
            await UploadFileAsync(newFileStream, newFileName);
        }

        public async Task DownloadFileByNameAsync(string id, string fullFileName)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            using (Stream fs = new FileStream(fullFileName, FileMode.OpenOrCreate))
            {
                await gridFS.DownloadToStreamAsync(objectId, fs);
            }
        }
    }
}
