namespace SimpleAccount.Domains
{
    public class Consent
    {
        public string ConsentId { get; set; }

        public string ConnectorId { get; set; }

        public string AccessTokenRaw { get; set; }

        public string AccessTokenExpiry { get; set; }

        public string RefreshTokenRaw { get; set; }

        public string RefreshTokenExpiry { get; set; }
    }
}