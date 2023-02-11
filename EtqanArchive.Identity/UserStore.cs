using DataLayer.Security.TableEntity;
using EtqanArchive.DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EtqanArchive.Identity
{
    public class UserStore : IUserStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IQueryableUserStore<User>,
        IUserSecurityStampStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User>,
        IUserLockoutStore<User>,
        IUserClaimStore<User>,
        IUserLoginStore<User>,
        IUserRoleStore<User>
    {
        private readonly EtqanArchiveDBContext _context;
        private readonly RoleStore _roleStore;
        public IQueryable<User> Users => _context.Users;

        public UserStore(EtqanArchiveDBContext context, RoleStore roleStore)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context is null");
            }
            _context = context;
            _roleStore = roleStore;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Add(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not create user {user.UserName}." });
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Remove(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete user {user.UserName}." });
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            Guid id;
            if (!Guid.TryParse(userId, out id))
            {
                throw new ArgumentException("Not a valid id", nameof(userId));
            }
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower(),
                cancellationToken);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            //cancellationToken.ThrowIfCancellationRequested();
            //if (user == null) throw new ArgumentNullException(nameof(user));
            //user.NormalizedUserName = normalizedUserName;
            return Task.FromResult<object>(null);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UserName = userName;
            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Update(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not update user {user.UserName}." });
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PhoneNumber = phoneNumber;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool phoneNumberConfirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PhoneNumberConfirmed = phoneNumberConfirmed;
            return Task.FromResult<object>(null);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(normalizedEmail),
                cancellationToken);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(User user, bool emailConfirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.EmailConfirmed = emailConfirmed;
            return Task.FromResult<object>(null);
        }

        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (login == null) throw new ArgumentNullException(nameof(login));
            if(user.UserLogins == null)
            {
                user.UserLogins = new List<UserLogin>();
            }

            user.UserLogins.Add(new UserLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName,
                UserId = user.Id
            });
            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(loginProvider)) throw new ArgumentNullException(nameof(loginProvider));
            if (string.IsNullOrEmpty(providerKey)) throw new ArgumentNullException(nameof(providerKey));

            var userLogin = user.UserLogins.FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            bool success = false;
            if (userLogin != null)
                success = user.UserLogins.Remove(userLogin);

            return Task.FromResult<object>(null);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserLogins == null) return new List<UserLoginInfo>();
            IList<UserLoginInfo> result = user.UserLogins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName)).ToList();
            return await Task.FromResult(result);
        }

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Users.SingleOrDefaultAsync(u => u.UserLogins.Any(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey),
                cancellationToken);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserClaims == null) return new List<Claim>();
            IList<Claim> result = user.UserClaims.Select(x => x.ToClaim()).ToList();
            return await Task.FromResult(result);
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            if (claims.Any())
            {
                foreach (var claim in claims)
                {
                    UserClaim userClaim = new UserClaim();
                    userClaim.InitializeFromClaim(claim);
                    user.UserClaims.Add(userClaim);
                }
            }
            return Task.FromResult<object>(null);
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            if (newClaim == null) throw new ArgumentNullException(nameof(newClaim));
            var userClaim = user.UserClaims.SingleOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            user.UserClaims.Remove(userClaim);
            userClaim.InitializeFromClaim(newClaim);
            user.UserClaims.Add(userClaim);
            return Task.FromResult<object>(null);
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            user.UserClaims = user.UserClaims.Where(x => !claims.Any(y => y.Type == x.ClaimType && y.Value == x.ClaimValue)).ToList();
            return Task.FromResult<object>(null);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (claim != null) throw new ArgumentNullException(nameof(claim));
            IList<User> users = _context.Users.Where(u => u.UserClaims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value)).ToList();
            return await Task.FromResult(users);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool twoFactorEnabled, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.TwoFactorEnabled = twoFactorEnabled;
            return Task.FromResult<object>(null);
        }

        public Task SetSecurityStampAsync(User user, string securityStamp, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.SecurityStamp = securityStamp;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.SecurityStamp);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.LockoutEnd = lockoutEnd;
            return Task.FromResult<object>(null);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.AccessFailedCount = 0;
            return Task.FromResult<object>(null);
        }

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.LockoutEnabled = enabled;
            return Task.FromResult<object>(null);
        }

        #region Cookie
        //public void SetCookie<T>(string name, T model)
        //{
        //    var json = JsonConvert.SerializeObject(model);
        //    var protectedValue = new ProtectedValue<string>();
        //    HttpCookie cookie = new HttpCookie(name)
        //    {
        //        HttpOnly = true,
        //        Value = protectedValue.Protect(json),
        //        Expires = DateTime.Now.AddMinutes(1)
        //    };
        //    HttpContext.Current.Response.Cookies.Add(cookie);
        //}

        //public void DeleteCookie(HttpCookie httpCookie)
        //{
        //    httpCookie.Value = null;
        //    httpCookie.Expires = DateTime.Now.AddMinutes(-20);
        //    HttpContext.Current.Response.Cookies.Add(httpCookie);
        //}

        //public T GetCookie<T>(string name)
        //{
        //    try
        //    {
        //        var cookiesValue = HttpContext.Current.Request.Cookies[name];
        //        var value = cookiesValue == null ? null : HttpContext.Current.Request.Cookies[name].Value.ToString();
        //        if (string.IsNullOrEmpty(value)) return default(T);
        //        var protectedValue = new ProtectedValue<string>();
        //        protectedValue.Encrypted = value;
        //        return JsonConvert.DeserializeObject<T>(protectedValue.Unprotect());
        //    }
        //    catch
        //    {
        //        return default(T);
        //    }
        //}
        #endregion

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();
            if (user != null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
            if (role != null) throw new ArgumentNullException(nameof(role));
            user.UserRoles.Add(new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id,
                //CreateUserId = user.Id,
                //CreateDate = DateTime.Now
            });
            return;
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserRoles == null) return new List<string>();
            IList<string> result = user.UserRoles.Select(x => x.Role.Name).ToList();
            return await Task.FromResult(result);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            if (user.UserRoles == null) return Task.FromResult(false);
            bool result = user.UserRoles.Any(x => x.Role.Name == roleName);
            return Task.FromResult(result);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            var role = await _roleStore.FindByNameAsync(roleName);
            if (role == null || role.UserRoles == null) return await Task.FromResult(new List<User>());
            List<User> result = role.UserRoles.Select(x=> x.User).ToList();
            return await Task.FromResult(result);
        }

        #region Derayah User Store

        //private IList<Claim> GetUserClaims(DerayahUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        //new Claim(ClaimTypes.Name, user.Name),
        //        //new Claim(ClaimTypes.NameIdentifier, user.Name),
        //        new Claim(DerayahCustomClaims.AccountId, user.AccountId),
        //        new Claim(DerayahCustomClaims.ArabicFullName, user.FullNameAr ?? ""),
        //        new Claim(DerayahCustomClaims.EnglishFullName, user.FullName ?? ""),
        //        new Claim(DerayahCustomClaims.ChannelId, user.ChannelId),
        //        new Claim(DerayahCustomClaims.UserCode, user.UserName),
        //        new Claim(DerayahCustomClaims.PartyId, user.PartyId),
        //        new Claim(DerayahCustomClaims.UserNumber, user.UserNumber),
        //        new Claim(DerayahCustomClaims.BrokerageCustomerNationalID, user.NationalID??""),
        //        new Claim(DerayahCustomClaims.BrokerageCustomerID, user.Id.ToString()),
        //        new Claim(DerayahCustomClaims.IsIndividual, user.IsIndividual),
        //        new Claim(DerayahCustomClaims.IBUserCode, user.IBUserCode ?? ""),
        //        new Claim(DerayahCustomClaims.PreferredLang, user.PreferredLang.ToString()),
        //        new Claim(DerayahCustomClaims.BrokerageCustomerStatus, user.Status.ToString()),
        //        new Claim(DerayahCustomClaims.BrokerageCustomerSegmentId, user.SegmentId.GetValueOrDefault().ToString()),
        //  };
        //    if (user.Portfolios != null)
        //    {
        //        claims.Add(new Claim(DerayahCustomClaims.Portfolios, GetPortofliosString(user.Portfolios)));
        //    }
        //    if (user.ApiAccessToken == null)
        //    {
        //        user.ApiAccessToken = GetApiAccessToken(false, string.Empty, user.UserName, user.PasswordHash);
        //        claims.Add(new Claim(DerayahCustomClaims.ApiAccessToken, user.ApiAccessToken));
        //    }
        //    else
        //    {
        //        claims.Add(new Claim(DerayahCustomClaims.ApiAccessToken, user.ApiAccessToken));
        //    }
        //    return claims;
        //}

        //private string GetPortofliosString(List<Portfolio> portfolios)
        //{
        //    if (portfolios == null || !portfolios.Any()) return string.Empty;

        //    if (portfolios.Count > 30)
        //    {
        //        return string.Empty;
        //    }
        //    var seralizer = new JavaScriptSerializer();
        //    var result = seralizer.Serialize(portfolios);
        //    var compressedVersion = Zip(result);
        //    return Convert.ToBase64String(compressedVersion);
        //}

        //public string GetApiAccessToken(bool isRefreshtoken, string refreshtoken, string username, string password)
        //{
        //    var api_url = ApplicationContext.Configurations.Api_identityServer_url + ApplicationContext.Configurations.Api_token_route;
        //    var client = new System.Net.Http.HttpClient();

        //    var dict = new Dictionary<string, string>();
        //    dict.Add("client_id", ApplicationContext.Configurations.Api_client_id);
        //    dict.Add("client_secret", ApplicationContext.Configurations.Api_client_secret);

        //    if (isRefreshtoken)
        //    {
        //        dict.Add("grant_type", "refresh_token");
        //        dict.Add("refresh_token", refreshtoken);
        //    }
        //    else
        //    {
        //        dict.Add("scope", ApplicationContext.Configurations.Api_scope);
        //        dict.Add("grant_type", "password");
        //        dict.Add("username", username);
        //        dict.Add("password", password);
        //    }

        //    var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, api_url) { Content = new System.Net.Http.FormUrlEncodedContent(dict) };

        //    var response = client.SendAsync(request).Result;
        //    string json = response.Content.ReadAsStringAsync().Result;
        //    //var result = JsonConvert.DeserializeObject<ApiAccessToken>(json);
        //    return json;
        //}

        //public byte[] Zip(string value)
        //{
        //    var bytes = Encoding.UTF8.GetBytes(value);

        //    using (var msi = new MemoryStream(bytes))
        //    using (var mso = new MemoryStream())
        //    {
        //        using (var gs = new GZipStream(mso, CompressionMode.Compress))
        //        {
        //            CopyTo(msi, gs);
        //        }
        //        return mso.ToArray();
        //    }
        //}
        //public void CopyTo(Stream src, Stream dest)
        //{
        //    byte[] bytes = new byte[4096];

        //    int cnt;

        //    while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        //    {
        //        dest.Write(bytes, 0, cnt);
        //    }
        //}

        #endregion

        public void Dispose()
        {
        }

        
    }
}
