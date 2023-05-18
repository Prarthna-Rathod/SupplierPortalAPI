namespace BusinessLogic.ValueConstants
{
    public class DocumentStatusValues
    {
        public static string[] DocumentStatuses =
       {
            NotValidated,
            Validated,
            HasErrors,
            Processing
        };
        public const string NotValidated = "Not-validated";
        public const string Validated = "Validated";
        public const string HasErrors = "Has errors";
        public const string Processing = "Processing";
    }
}
