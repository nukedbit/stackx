using StackX.ServiceModel.Types;

namespace StackX.ServiceModel
{
    public class ApplicationResponse : ICrudResponse<Application>
    {
        public int Id { get; set; }
        public Application Result { get; set; }
    }
}