using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using System;
using System.IO;
using File = StackX.ServiceModel.Types.File;

namespace StackX.ServiceInterface
{
    public class FileFeature : IPlugin
    {

        public Func<FileUpload, string> UploadedFileFullPathBuilder = null;

        public bool UseRandomFileName { get; set; } = false;


        public Func<string> RandomFileName = Path.GetRandomFileName;
        
        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();
            var connection = container.Resolve<IDbConnectionFactory>();
            var db = connection.CreateDbConnection();
            db.Open();
            db.CreateTableIfNotExists<File>();
            
            if (!db.ColumnExists<File>(c => c.ExtraAttribute1))
            {
                db.AddColumn<File>(c => c.ExtraAttribute1);
            }
            if (!db.ColumnExists<File>(c => c.ExtraAttribute2))
            {
                db.AddColumn<File>(c => c.ExtraAttribute2);
            }
            
            if (!db.ColumnExists<File>(c => c.ExtraAttribute3))
            {
                db.AddColumn<File>(c => c.ExtraAttribute3);
            }
            if (!db.ColumnExists<File>(c => c.ExtraAttribute4))
            {
                db.AddColumn<File>(c => c.ExtraAttribute4);
            }
            
            StackXAppHostBase.ExcludedAutoQueryTypes.Remove(typeof(QueryFiles));
            StackXAppHostBase.ExcludedAutoCrudTypes.Remove(typeof(UpdateFile));
            appHost.ServiceController.RegisterService(typeof(FileService));
        }
    }
}