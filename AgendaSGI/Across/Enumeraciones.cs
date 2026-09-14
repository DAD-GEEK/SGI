namespace Across
{
    public static class Enumeraciones
    {
        public enum ReponseType
        {
            success,
            error
        }

        public enum ParametrosDeConfiguracionSMTP
        {
            SMTP,
            SMTPPort,
            SMTPEmailFrom,
            SMTPNetworkCredentialUser,
            SMTPNetworkCredentialPassword,
            SMTPEnableSSL
        }
    }
}
