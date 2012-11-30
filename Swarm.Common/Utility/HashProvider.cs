using System;
using System.Security.Cryptography;
using System.Text;

namespace Swarm.Common.Utility
{
    public class HashProvider
    {
        internal byte[] Compute(string input)
        {
            SHA1 sha = SHA1.Create();
            byte[] encoded = Encoding.ASCII.GetBytes(input);
            byte[] checksum = sha.ComputeHash(encoded);
            return checksum;
        }

        /// <summary>
        /// Returns a SHA1 hash of any provided string, in Int64 format.
        /// </summary>
        public long ComputeAsLong(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            byte[] checksum = Compute(input);
            long hash = BitConverter.ToInt64(checksum, 0);
            return hash;
        }

        /// <summary>
        /// Returns a SHA1 hash of any provided string, in String format.
        /// </summary>
        public string ComputeAsString(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            byte[] checksum = Compute(input);
            string hash = BitConverter.ToString(checksum, 0);
            return hash;
        }
    }
}
