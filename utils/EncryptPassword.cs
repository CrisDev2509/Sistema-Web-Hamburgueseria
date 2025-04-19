using System.Security.Cryptography;

namespace Bigtoria.utils
{
    public class EncryptPassword
    {
        public static string HashPassword(string password)
        {
            // Generar un salt aleatorio
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; 
                rng.GetBytes(salt);

                using (var sha256 = SHA256.Create())
                {
                    byte[] saltedPassword = new byte[password.Length + salt.Length];
                    Array.Copy(password.ToCharArray(), 0, saltedPassword, 0, password.Length);
                    Array.Copy(salt, 0, saltedPassword, password.Length, salt.Length);

                    // Generamos el hash
                    byte[] hashedPassword = sha256.ComputeHash(saltedPassword);

                    string saltString = Convert.ToBase64String(salt);
                    string hashString = Convert.ToBase64String(hashedPassword);

                    return $"{saltString}:{hashString}";
                }
            }
        }

        public static bool VerifyPassword(string storedHash, string inputPassword)
        {
            // Recuperamos el salt y el hash almacenado
            var parts = storedHash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedPasswordHash = Convert.FromBase64String(parts[1]);

            // Crear el hash con el salt
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = new byte[inputPassword.Length + salt.Length];
                Array.Copy(inputPassword.ToCharArray(), 0, saltedPassword, 0, inputPassword.Length);
                Array.Copy(salt, 0, saltedPassword, inputPassword.Length, salt.Length);

                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);

                // Comparamos el hash generado con el almacenado
                return CompareHashes(storedPasswordHash, hashedPassword);
            }
        }

        private static bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
