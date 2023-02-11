using DataLayer.Security.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Classes.Common;
using static Classes.Common.DBEnums;
using GenericRepositoryCore.Utilities;
using AutoMapper;
using Tazkara.DataLayer.ViewEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.Identity;

namespace EtqanArchive.BackEnd.Services
{
    public interface IUserService
    {
        Task<JsonResponse<AuthenticationResponse>> LoginUserAsync(UserLoginViewModel model);
        Task<JsonResponse<bool>> SignOutAsync();
    }
    
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration,  IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<JsonResponse<AuthenticationResponse>> LoginUserAsync(UserLoginViewModel model)
        {
            try
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var signinResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);
                var result = await UserSignInAsync(signinResult, model.Username);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<JsonResponse<AuthenticationResponse>> UserSignInAsync(Microsoft.AspNetCore.Identity.SignInResult signInResult, string username)
        {
            if (signInResult.Succeeded)
            {
                //get user information
                var user = await _userManager.FindByNameAsync(username);
                //create refresh token
                var refreshToken = new RefreshTokenService().GenerateRefreshToken();
                //create token
                var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(IdentityCustomClaims.UserId, user.Id.ToString()),
                    new Claim(IdentityCustomClaims.UserName, user.UserName),
                    new Claim(IdentityCustomClaims.UserFullName, user.UserFullName),
                    new Claim(IdentityCustomClaims.UserType, user.UserTypeId.ToString()),
                    new Claim(IdentityCustomClaims.Email, user.Email ?? ""),
                    new Claim(IdentityCustomClaims.EmailConfirmed, user.EmailConfirmed.ToString()),
                    new Claim(IdentityCustomClaims.RefreshToken, refreshToken),
                };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audince"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );
                return new JsonResponse<AuthenticationResponse>()
                {
                    Status = 1,
                    Result = new AuthenticationResponse()
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        ExpireDate = token.ValidTo
                    },
                    Message = "Expired Date:" + token.ValidTo.ToString("yyyy-MM-dd hh:mm tt")
                };
            }
 
            if (signInResult.IsLockedOut)
            {
                return new JsonResponse<AuthenticationResponse>(null, $"{Localization.Resources.Security.UserAccountLockedOut}", false);
            }
            else
            {
                return new JsonResponse<AuthenticationResponse>(null, Localization.Resources.Security.LoginFailed, false);
            }
        }



        public async Task<JsonResponse<bool>> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return new JsonResponse<bool>(true);
        }


      

    }
}

