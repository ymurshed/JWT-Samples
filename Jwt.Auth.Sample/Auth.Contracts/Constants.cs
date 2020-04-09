namespace Auth.Contracts
{
    public class Constants
    {
        #region User Constants
        public const string AdminUserName = "ymurshed";
        public const string AdminUserEmail = "murshed.yaad@gmail.com";
        public const string AdminUserRole = "admin-user";
        public const string OtherUserRole = "other-user";
        public const string AdminUserPolicy = "RequireAdminAccess";
        public const string OtherUserPolicy = "RequireOtherAccess";
        #endregion

        #region Jwt Contants
        public static readonly string JwtIssuer = "Jwt:Issuer";
        public static readonly string JwtSecretKey = "Jwt:SecretKey";
        public static readonly string JwtExpireTimeInMinutes = "Jwt:ExpireTimeInMinutes";

        public const string SafePath = "auth/token";
        public const string Authorization = "Authorization";
        public const string Bearer = "bearer";
        public const string TokenNotFound = "Token Not Found in Headers";
        #endregion
    }
}
