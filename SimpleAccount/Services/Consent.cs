namespace SimpleAccount.Services
{
    public class Consent
    {
        public string ConsentId { get; set; }

        public string AccessTokenRaw { get; set; }

        public string AccessTokenExpiry { get; set; }

        public string RefreshTokenRaw { get; set; }

        public string RefreshTokenExpiry { get; set; }
    }
}