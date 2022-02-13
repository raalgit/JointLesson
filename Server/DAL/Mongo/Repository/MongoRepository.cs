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
            return now.ToString(fmt) + fileExtension;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            ObjectId id = await gridFS.UploadFromStreamAsync(fileName, fileStream);
            return Convert.ToString(id) ?? throw new NullReferenceException(nameof(id));
        }

        public async Task DeleteFileAsync(string mongoId)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(mongoId);
            await gridFS.DeleteAsync(objectId);
        }

        public async Task<string> ChangeFileAsync(string mongoId, Stream newFileStream, string newFileName)
        {
            await DeleteFileAsync(mongoId);
            return await UploadFileAsync(newFileStream, newFileName);
        }

        public async Task<Stream> DownloadFileByIdAsync(string mongoId)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(mongoId);
            Stream fileStream = (Stream?)null;
            await gridFS.DownloadToStreamAsync(objectId, fileStream);
            return fileStream ?? throw new NullReferenceException(nameof(fileStream));
        }

        public async Task<byte[]> GetFileBytesByIdAsync(string mongoId)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(mongoId);
            return await gridFS.DownloadAsBytesAsync(objectId);
        }
    }
}
