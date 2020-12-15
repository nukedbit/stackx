using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using StackX.ServiceInterface;
using StackX.ServiceModel;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        
        [Test]
        public void UploadFileRandom()
        {
            var random = appHost.GetPlugin<FileFeature>().RandomFileName;
            try
            {
                appHost.GetPlugin<FileFeature>().UseRandomFileName = true;
                appHost.GetPlugin<FileFeature>().RandomFileName = () => "random";
                
                ServiceConfig.Instance.DisableSubFolderCheck = true;
                appHost.GetPlugin<FileFeature>().UploadedFileFullPathBuilder = null;
                var client = CreateAdminAuthClient();

                var memoryStream = new MemoryStream();

                var writer = new StreamWriter(memoryStream);
            
                writer.Write("file content");
                
                memoryStream.Position = 0;

                var request = new FileUpload()
                {
                    Folder = "folder",
                    ApplicationId = 1,
                };
                var fname = "test.txt";
                var response = client.PostFileWithRequest<FileUploadResponse>(memoryStream, fname, request);
                var randomFileName = "random.txt";
                response.Results.First().FileName.Should().Be(randomFileName);
            
                var fileFullPath =
                    ServiceConfig.Instance.GetDefaultUploadFullFilePath(randomFileName, request.Folder);

                appHost.VirtualFiles.GetFile(fileFullPath).Exists().Should().BeTrue();
            }
            finally
            {
                appHost.GetPlugin<FileFeature>().UseRandomFileName = false;
                appHost.GetPlugin<FileFeature>().RandomFileName = random;
            }
        }
        
        [Test]
        public void UploadFile()
        {
            ServiceConfig.Instance.DisableSubFolderCheck = true;
            appHost.GetPlugin<FileFeature>().UploadedFileFullPathBuilder = null;
            var client = CreateAdminAuthClient();

            var memoryStream = new MemoryStream();

            var writer = new StreamWriter(memoryStream);
            
            writer.Write("file content");
            

            memoryStream.Position = 0;

            var request = new FileUpload()
            {
                Folder = "folder",
                ApplicationId = 1,
            };
            var fname = "test.txt";
            var response = client.PostFileWithRequest<FileUploadResponse>(memoryStream, fname, request);

            response.Results.First().FileName.Should().Be(fname);
            
            var fileFullPath =
                ServiceConfig.Instance.GetDefaultUploadFullFilePath(fname, request.Folder);

            appHost.VirtualFiles.GetFile(fileFullPath).Exists().Should().BeTrue();
        }
        
        [Test]
        public void UploadFileCustomFileNameHook()
        {
            ServiceConfig.Instance.DisableSubFolderCheck = true;
            var customFileName = "custom.txt";
            appHost.GetPlugin<FileFeature>().UploadedFileFullPathBuilder = (req) =>
            {
                return ServiceConfig.Instance.GetDefaultUploadFullFilePath(customFileName, req.Folder);
            };
            var client = CreateAdminAuthClient();

            var memoryStream = new MemoryStream();

            var writer = new StreamWriter(memoryStream);
            
            writer.Write("file content");

            memoryStream.Position = 0;

            var request = new FileUpload()
            {
                Folder = "folder",
                ApplicationId = 1,
            };
            var fname = "custom.txt";
            var response = client.PostFileWithRequest<FileUploadResponse>(memoryStream, fname, request);

            response.Results.First().FileName.Should().Be(fname);
            
            var fileFullPath =
                ServiceConfig.Instance.GetDefaultUploadFullFilePath(customFileName, request.Folder);


            appHost.VirtualFiles.GetFile(fileFullPath).Exists().Should().BeTrue();
        }
    }
}