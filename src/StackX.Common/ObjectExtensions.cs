namespace StackX.Common
{
    public static class ObjectExtensions
    {
        public static bool IsEqual<TItem>(this TItem item, object to)
        {
            return item.Equals(to);
        }
    }
}