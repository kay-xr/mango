using System.Security.Cryptography;
using System.Text;

namespace Mango.Host.Utils;

public class RandomString
{
    public static string Generate(int length = 14)
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
        var password = new StringBuilder();
        using (var rng = RandomNumberGenerator.Create())
        {
            var data = new byte[4];
            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(data);
                var randomIndex = BitConverter.ToUInt32(data, 0) % (uint)allowedChars.Length;
                password.Append(allowedChars[(int)randomIndex]);
            }
        }
        return password.ToString();
    }
}