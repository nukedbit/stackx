namespace StackX.ServiceInterface
{
    public class ServiceConfig
    {
        private ServiceConfig(){}
        
        public static ServiceConfig Instance { get; } = new ServiceConfig();
        
        public string DefaultUploadsPath { get; set; } = "~/uploads";

        public string[] AllowedUploadSubFolders { get; set; } = new string [0];

        public bool DisableSubFolderCheck { get; set; } = false;
    }
}