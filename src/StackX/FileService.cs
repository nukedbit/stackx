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
using File = System.IO.File;

namespace StackX.ServiceInterface
{
    public class FileService : Service
    {
        public IAutoQueryDb AutoQuery { get; set; }

        public async Task<object> PostAsync(FileUpload request)
        {
            var feature = HostContext.GetPlugin<FileFeature>();
            
            List<ServiceModel.Types.File> newFiles = new List<ServiceModel.Types.File>();
            using (var trans = Db.OpenTransaction())
            {
                try
                {
                    foreach (IHttpFile requestFile in Request.Files)
                    {

                        var fileNameFromReq = requestFile.FileName;
                        var fileFullPath =
                            feature.UploadedFileFullPathBuilder?.Invoke(request) ?? ServiceConfig.Instance.GetDefaultUploadFullFilePath(fileNameFromReq, request.Folder);

                        VirtualFiles.WriteFile(fileFullPath, requestFile.InputStream);

                        var fileRecord = new ServiceModel.Types.File()
                        {
                            ApplicationId = request.ApplicationId,
                            ContentType = requestFile.ContentType,
                            ExtraAttribute1 = request.ExtraAttribute1,
                            ExtraAttribute2 = request.ExtraAttribute2,
                            ExtraAttribute3 = request.ExtraAttribute3,
                            ExtraAttribute4 = request.ExtraAttribute4,
                            FileName = requestFile.FileName,
                            ReferencedBy = request.ReferencedBy,
                            FileHash = requestFile.InputStream.ComputeFileMd5(),
                            CreatedBy = GetSession().GetUserAuthName(),
                            CreatedDate = DateTime.UtcNow,
                            ModifiedBy = GetSession().GetUserAuthName(),
                            ModifiedDate = DateTime.UtcNow
                        };
                        var a = await Db.InsertAsync(fileRecord, selectIdentity:true);
                        fileRecord.Id = (int)a;
                        newFiles.Add(fileRecord);
                    }

                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw new HttpError(HttpStatusCode.InternalServerError, e);
                }
            }

            return new FileUploadResponse() {Ids = newFiles.Select(f => f.Id).ToList(), Results = newFiles};

        }

        public async Task<object> Get(GoToFileUrl request)
        {
            var file = (await Db.SelectByIdsAsync<ServiceModel.Types.File>(new int[] { request.FileId })).FirstOrDefault();
            if (file is null)
            {
                return new HttpResult(System.Net.HttpStatusCode.NotFound);
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