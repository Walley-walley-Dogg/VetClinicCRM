using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCrypt.Net.BCrypt;

namespace VetClinicCRM.Core.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return Verify(password, hash);
        }

        public static string GenerateTemporaryPassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(password);
        }
    }
}
