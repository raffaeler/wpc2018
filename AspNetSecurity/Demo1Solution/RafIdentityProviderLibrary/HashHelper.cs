using System;

namespace RafIdentityProviderLibrary
{
    public static class HashHelper
    {
        public static string Create(string input)
        {
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] blob = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(blob);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}