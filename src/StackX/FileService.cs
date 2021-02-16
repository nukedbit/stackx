using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using StackX.Common;
using StackX.ServiceModel;
using System.IO;
using File = System.IO.File;

namespace StackX.ServiceInterface
{

    public static class FileServiceExtensions
    {
        public static async Task<List<ServiceModel.Types.File>> StoreUploadedFilesAsync(this IRequest request,
            IFileUploadAdditionalMetadata additionalMetadata)
        {
            var db = HostContext.AppHost.GetDbConnection(request);
            using var trans = db.OpenTransaction();
            try
            {
                var feature = HostContext.GetPlugin<FileFeature>();

                List<ServiceModel.Types.File> newFiles = new();
                
                var virtualFiles = HostContext.AppHost.VirtualFiles;
                var session = await request.GetSessionAsync();
                
                foreach (IHttpFile requestFile in request.Files)
                {

                    var fileNameFromReq = requestFile.FileName;
                    if (feature.UseRandomFileName)
                    {
                        fileNameFromReq = feature.RandomFileName() +
                                          Path.GetExtension(fileNameFromReq);
                    }

                    var fileFullPath =
                        ServiceConfig.Instance.GetDefaultUploadFullFilePath(fileNameFromReq, additionalMetadata.Folder);

                    if (feature.UploadedFileFullPathBuilder is not null)
                    {
                        fileFullPath = feature.UploadedFileFullPathBuilder?.Invoke(additionalMetadata);
                    }

                    virtualFiles.WriteFile(fileFullPath, requestFile.InputStream);

                    var fileRecord = new ServiceModel.Types.File()
                    {
                        ApplicationId = additionalMetadata.ApplicationId,
                        ContentType = requestFile.ContentType,
                        ExtraAttribute1 = additionalMetadata.ExtraAttribute1,
                        ExtraAttribute2 = additionalMetadata.ExtraAttribute2,
                        ExtraAttribute3 = additionalMetadata.ExtraAttribute3,
                        ExtraAttribute4 = additionalMetadata.ExtraAttribute4,
                        FileName = Path.GetFileName(fileFullPath),
                        ReferencedBy = additionalMetadata.ReferencedBy,
                        FileHash = requestFile.InputStream.ComputeFileMd5(),
                        CreatedBy = session.GetUserAuthName(),
                        CreatedDate = DateTime.UtcNow,
                        ModifiedBy = session.GetUserAuthName(),
                        ModifiedDate = DateTime.UtcNow
                    };
                    var a = await db.InsertAsync(fileRecord, selectIdentity: true);
                    fileRecord.Id = (int)a;
                    newFiles.Add(fileRecord);
                }

                trans.Commit();
                return newFiles;
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw;
            }
        }
    }
    
    public class FileService : Service
    {
        public IAutoQueryDb AutoQuery { get; set; }


        public async Task<object> PostAsync(FileUpload request)
        {
            List<ServiceModel.Types.File> newFiles = new();
            using var trans = Db.OpenTransaction();
            try
            {
                newFiles = await Request.StoreUploadedFilesAsync(request);
            }
            catch (Exception e)
            {
                throw new HttpError(HttpStatusCode.InternalServerError, e);
            }

            return new FileUploadResponse() {Ids = newFiles.Select(f => f.Id).ToList(), Results = newFiles};
        }

        public async Task<object> Get(GoToFileUrl request)
        {
            var file = (await Db.SelectByIdsAsync<ServiceModel.Types.File>(new int[] { request.FileId })).FirstOrDefault();
            if (file is null)
            {
                return new HttpResult(HttpStatusCode.NotFound);
            }

            var redirectUrl = $"{Request.GetBaseUrl()}/files/{file.FileName}";

            return this.Redirect(redirectUrl);
        }
        
        public async Task<object> Get(GetFileUrl request)
        {
            ServiceModel.Types.File file = await GetFileByIdOrThrowAsync(request.FileId);

            var redirectUrl = $"{Request.GetBaseUrl()}/files/{file.FileName}";

            return new GetFileUrlResponse
            {
                Url = redirectUrl
            };
        }

        private async Task<ServiceModel.Types.File> GetFileByIdOrThrowAsync(int fileId)
        {
            var file = (await Db.SelectByIdsAsync<ServiceModel.Types.File>(new[] {fileId})).FirstOrDefault();
            if (file is null)
            {
                throw new HttpError(HttpStatusCode.NotFound);
            }

            return file;
        }

        public async Task<object> Get(GetFileContent request)
        {
            ServiceModel.Types.File fileDb = await GetFileByIdOrThrowAsync(request.Id);
            var root = ServiceConfig.Instance.DefaultUploadsPath.Replace("~", "").MapServerPath();
            var filePath = $"{root}/{fileDb.FileName}";
            var file = VirtualFiles.GetFile(filePath);
            var content = file.ReadAllBytes();
            return new GetFileContentResponse()
            {
                Content = content,
                ContentType = fileDb.ContentType
            };
        }

        public async Task<object> Any(CompareFileHash request)
        {
            var file = await Db.LoadSingleByIdAsync<ServiceModel.Types.File>(request.FileId);
            if (file is null)
            {
                throw new HttpError(HttpStatusCode.NotFound);
            }
            bool areEqual = file.FileHash == request.Hash;
            return new CompareFileHasResponse {AreEqual = areEqual};
        }
    }
}