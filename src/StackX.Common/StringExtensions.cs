namespace StackX.Common
{
    public static class StringExtensions
    {
        
        /// <summary>
        /// Its equivalent of "my str".ToLowerInvariant().Trim();
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NormalizeForComparison(this string s) => s.ToLowerInvariant().Trim();
    }
}