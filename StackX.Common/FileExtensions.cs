using System;
using System.IO;
using System.Security.Cryptography;

namespace StackX.Common
{
    public static class FileExtensions
    {
        public static string ComputeFileMd5(this string filename)
        {
            using var stream = File.OpenRead(filename);
            return ComputeFileMd5(stream);
        }
        
        public static string ComputeFileMd5(this Stream stream)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}