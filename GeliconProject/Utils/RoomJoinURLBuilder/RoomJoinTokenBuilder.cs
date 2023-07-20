using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace GeliconProject.Utils.RoomJoinURLBuilder
{
    public static class RoomJoinTokenBuilder
    {
        private static DateTime GetExpiresTime()
        {
            return DateTime.UtcNow.AddMinutes(30);
        }

        private static string GetHeader()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"alg\": \"HS512\", \"typ\": \"JWT\"}"));
        }

        private static string GetPayload(int roomID)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{{\"roomID\": \"{roomID}\", \"exp\": \"{GetExpiresTime()}\"}}"));
        }

        private static string GetSign(string header, string payload)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(JWTValidationParameters.IJWTValidationParameters.key)))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes($"{header}.{payload}")));
            }
        }

        public static string Build(int roomID)
        {
            string header = GetHeader(), payload = GetPayload(roomID), sign = GetSign(header, payload);

            return $"{header}.{payload}.{sign}";
        }

        public static string Build(string header, string payload)
        {
            string sign = GetSign(header, payload);

            return $"{header}.{payload}.{sign}";
        }
    }
}
