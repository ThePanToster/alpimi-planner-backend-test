﻿namespace AlpimiAPI.Settings
{
    public static class AuthSettings
    {
        public const int MinimumPasswordLength = 8;
        public const int MaximumPasswordLength = 256;
        public const string JWTKey = "KeyNotFoundButThisMustBeLongEnough";
        public const string JWTIssuer = "example.com";
        public const double JWTExpire = 5.0;
        public const int HashIterations = 10;
        public const int KeySize = 20;
        public const string HashAlgorithm = "SHA1";
    }
}
