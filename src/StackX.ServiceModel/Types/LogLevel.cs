using ServiceStack.DataAnnotations;

namespace StackX.ServiceModel.Types
{
    [EnumAsInt]
    public enum LogLevel : int
    {
        Debug = 10,
        Info = 20,
        Warn = 30,
        Error = 40,
        Fatal = 50,
        Trace = 60
    }
}