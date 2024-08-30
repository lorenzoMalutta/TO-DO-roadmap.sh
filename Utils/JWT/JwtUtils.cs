using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Todo_List_API.Utils.JWT
{
    public class JwtUtils
    {
        #region Properties
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        public TokenModel GetTokenData(string username, ClaimsIdentity claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int tokenValidityIndDays);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(tokenValidityIndDays),
                SigningCredentials = signingCredentials,
                Subject = claims
            };

            var token = handler.CreateToken(tokenDescriptor);

            return new TokenModel
            {
                AccessToken = handler.WriteToken(token),
                RefreshToken = GenerateRefreshToken()
            };
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        #endregion

        #region Private Methods
        private SecurityToken CreateToken(ClaimsIdentity authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = authClaims,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        #endregion
    }


    public class JwtModel
    {
        public JwtSecurityToken? Token { get; internal set; }
        public string? RefreshToken { get; internal set; }
        public DateTime RefreshTokenExpiryTime { get; internal set; }

        public object GetTokenData()
        {
            return new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(Token),
                RefreshToken = RefreshToken,
                Expiration = Token.ValidTo
            };
        }
    }

}