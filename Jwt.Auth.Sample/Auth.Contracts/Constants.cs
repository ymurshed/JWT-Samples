namespace Auth.Contracts
{
    public class Constants
    {
        #region User Constants
        public const string Admin = "Admin";
        public const string Other = "Other";
        public const string AdminClaimType = "IsAdmin";
        public const string AdminUserPolicy = "AdminUserPolicy";
        public const string AdminUserName = "ymurshed";
        public const string AdminUserEmail = "murshed.yaad@gmail.com";
        #endregion

        #region Jwt Contants
        public static readonly string JwtIssuer = "Jwt:Issuer";
        public static readonly string JwtSecretKey = "Jwt:SecretKey";
        public static readonly string JwtExpireTimeInMinutes = "Jwt:ExpireTimeInMinutes";

        public const string SafePath = "auth/token";
        public const string Bearer = "bearer";
        public const string Authorization = "Authorization";
        public const string TokenNotFound = "Token Not Found in Headers";
        public const string AdminClaimTypeMissing = "Admin claim type missing!";
        #endregion
    }
}
