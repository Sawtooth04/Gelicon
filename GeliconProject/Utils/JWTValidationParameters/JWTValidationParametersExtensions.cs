namespace GeliconProject.Utils.JWTValidationParameters
{
    public static class JWTValidationParametersExtensions
    {
        public static void AddJWTValidationParameters(this IServiceCollection collection)
        {
            collection.AddSingleton<IJWTValidationParameters, JWTValidationParameters>();
        }
    }
}
