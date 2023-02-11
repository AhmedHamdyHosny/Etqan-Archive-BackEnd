using System;
using System.Security.Claims;
using System.Security.Principal;

namespace EtqanArchive.Identity
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this IIdentity identity)
        {
            return GetCustomClaimString(identity, IdentityCustomClaims.UserId);
        }
        public static string GetUserName(this IIdentity identity)
        {
            return GetCustomClaimString(identity, IdentityCustomClaims.UserName);
        }

        public static string GetUserFullName(this IIdentity identity)
        {
            return GetCustomClaimString(identity, IdentityCustomClaims.UserFullName);
        }

        public static string GetUserType(this IIdentity identity)
        {
            return GetCustomClaimString(identity, IdentityCustomClaims.UserType);
        }

        public static string GetEmail(this IIdentity identity)
        {
            return GetCustomClaimString(identity, IdentityCustomClaims.Email);
        }


        private static string GetCustomClaimString(IIdentity identity, string claimType)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }

        private static void SetCustomClaimString(IIdentity identity, string claimType, string value)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(claimType);
            if (claim != null)
            {
                ((ClaimsIdentity)identity).RemoveClaim(claim);
            }
            ((ClaimsIdentity)identity).AddClaim(new Claim(IdentityCustomClaims.RefreshToken, value));
            //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }

        public static Guid? GetCustomClaimGuid(IIdentity identity, string claimType)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(claimType);
            Guid claimValue;
            if (Guid.TryParse(claim?.Value, out claimValue))
                return claimValue;
            return null;
        }

        public static string GetRefreshToken(this IIdentity identity)
        {
            return GetCustomClaimString(identity, IdentityCustomClaims.RefreshToken);
        }

        public static void SetRefreshToken(this IIdentity identity, string refreshToken)
        {
            SetCustomClaimString(identity, IdentityCustomClaims.RefreshToken, refreshToken);
        }
    }

    public static class IdentityCustomClaims
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string UserFullName = "UserFullName";
        public const string UserType = "UserType";
        public const string Email = "Email";
        public const string Role = "Role";
        public const string EmailConfirmed = "EmailConfirmed";
        public const string RefreshToken = "RefreshToken";
    }
}
