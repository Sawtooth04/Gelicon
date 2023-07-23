namespace GeliconProject.Models.Validation
{
    public class RegistrationValidation
    {
        public bool EmailAvailable { get; set; }
        public bool EmailRegex { get; set; }
        public bool PasswordCorrect { get; set; }
        public bool UsernameCorrect { get; set; }
    }
}
