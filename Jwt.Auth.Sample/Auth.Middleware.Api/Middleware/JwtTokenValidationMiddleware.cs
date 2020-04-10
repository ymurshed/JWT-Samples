using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Constants = Auth.Contracts.Constants;

namespace Auth.Middleware.Api.Middleware
{
    public class JwtTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        private static TokenValidationParameters _tokenValidationParameters;
        private static readonly JwtSecurityTokenHandler SecurityTokenHandler = new JwtSecurityTokenHandler();

        private enum TokenStatus
        {
            Valid = 200,
            Invalid = 498,
            Expired = 440,
            NotPresent = 449,
            Forbidden = 403
        }

        private class TokenResponse
        {
            public TokenStatus TokenStatus { get; set; }
            public string Message { get; set; }
        }

        public JwtTokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            
            var jwtService = new JwtService(configuration);
            _tokenValidationParameters = jwtService.GeTokenValidationParameters();
        }

        public async Task Invoke(HttpContext context)
        {
            // For token creation request, no validation is required 
            if (context.Request.Path.ToString().ToLower().Contains(Constants.SafePath))
            {
                await _next.Invoke(context);
            }
            else
            {
                var tokenResponse = AuthenticateRequest(context.Request);
                if (tokenResponse.TokenStatus == TokenStatus.Valid)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    var message = $"{(int)tokenResponse.TokenStatus} : {tokenResponse.Message}";
                    context.Response.StatusCode = tokenResponse.TokenStatus == TokenStatus.Forbidden ? 
                                                    StatusCodes.Status403Forbidden : StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(message);
                }
            }
        }

        private static TokenResponse AuthenticateRequest(HttpRequest request)
        {
            var response = new TokenResponse();
            string token = request.Headers[Constants.Authorization];
            token = token.Replace($"{Constants.Bearer} ", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                response.TokenStatus = TokenStatus.NotPresent;
                response.Message = Constants.TokenNotFound;
                return response;
            }

            try
            {
                var claimsPrincipal = SecurityTokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!claimsPrincipal.HasClaim(o =>
                    o.Type.Equals(Constants.AdminClaimType) && o.Value.Equals(true.ToString())))
                {
                    throw new Exception(Constants.AdminClaimTypeMissing);
                }
            }
            catch (SecurityTokenExpiredException stEx)
            {
                response.Message = stEx.Message;
                response.TokenStatus = TokenStatus.Expired;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.TokenStatus = ex.Message.Contains(Constants.AdminClaimTypeMissing) ? TokenStatus.Forbidden : TokenStatus.Invalid;
                return response;
            }

            response.TokenStatus = TokenStatus.Valid;
            return response;
        }
    }

    public static class JwtTokenValidationMiddlewareExtension
    {
        public static IApplicationBuilder UseJwtTokenValidation(this IApplicationBuilder builder, IConfiguration configuration)
        {
            return builder.UseMiddleware<JwtTokenValidationMiddleware>();
        }
    }
}
