namespace GeliconProject.Utils.JwtValidationParameters
{
    public static class JwtValidationParametersExtensions
    {
        public static void AddJwtValidationParameters(this IServiceCollection collection)
        {
            collection.AddSingleton<IJwtValidationParameters, JwtValidationParameters>();
        }
    }
}
