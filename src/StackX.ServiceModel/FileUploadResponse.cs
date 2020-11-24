using System.Collections.Generic;
using ServiceStack;

namespace StackX.ServiceModel
{
    public class FileUploadResponse : ICrudListResponse<ServiceModel.Types.File>
    {
        public List<int> Ids { get; set; }
        public List<ServiceModel.Types.File> Results { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}