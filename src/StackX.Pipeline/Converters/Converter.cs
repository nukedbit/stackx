using System;

namespace StackX.Pipeline.Converters
{
    public abstract class Converter
    {
        public abstract object Convert(object source);

        public abstract bool CanConvert(Type type);
    }
}