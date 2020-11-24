using System.Diagnostics;
using System.Reflection;

namespace StackX.Common
{
    public static class ReflectionExtensions
    {
        public static void SetValue(this object o, string propertyName, object value)
        {
            o.GetType()
                .GetProperty(propertyName)
                .SetProperty(o, value);
        }
        
        public static void SetProperty(this PropertyInfo propertyInfo, object obj, object value)
        {
            if (!propertyInfo.CanWrite)
            {
                Debug.WriteLine("Attempted to set read only property '{0}'", propertyInfo.Name);
                return;
            }

            var propertySetMethodInfo = propertyInfo.GetSetMethod(nonPublic:true);
            if (propertySetMethodInfo != null)
            {
                propertySetMethodInfo.Invoke(obj, new[] { value });
            }
        }
    }
}