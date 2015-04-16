using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Security
{
    public static class HashAlgorithmExtender
    {
        public static string ComputeHashString(this HashAlgorithm hashAlgorithm, string fileName)
        {
            var hashBytes = ComputeHash(hashAlgorithm, fileName);
            var hashBuffer = new StringBuilder(hashBytes.Length * 2);

            foreach (var value in hashBytes)
                hashBuffer.Append(value.ToString("x2"));

            return hashBuffer.ToString();
        }

        public static byte[] ComputeHash(this HashAlgorithm hashAlgorithm, string fileName)
        {
            if (hashAlgorithm == null)
                throw new ArgumentNullException("hashAlgorithm");

            if (fileName == null)
                throw new ArgumentNullException("filename");

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Unable to locate file", fileName);

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return hashAlgorithm.ComputeHash(stream);
            }
        }
    }
}
