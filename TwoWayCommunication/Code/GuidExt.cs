using System.Security.Cryptography;
using System.Text;

namespace TwoWayCommunication.Code
{
    public static class GuidExtensions
    {
        public static Guid GenerateUniqueGuid(string u1, string u2)
        {
            // Create two different combinations of u1 and u2
            var combo1 = u1;
            var combo2 = u2;

            // Combine and hash the combinations to generate a unique GUID
            if (combo1.CompareTo(combo2)>0)
                return GenerateGuidFromString(combo1 + combo2);
            else return GenerateGuidFromString(combo2 + combo1);
        }

        private static Guid GenerateGuidFromString(string input)
        {
            using (MD5 md5 = MD5.Create()) // You can also use SHA256 for better uniqueness
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return new Guid(hash);
            }
        }
    }
}
