using authuserAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.IdentityModel.Tokens.Jwt;

namespace authuserAPI.Models
{
    public class GenerateToken
    {
        public string Token (Guid id,string email, ICollection<Role>roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes("ide-jon-az-a-kulcs-amivel-azonositjuk-issuer-es-a-audience");
            var claimList = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.UniqueName, email)
            };
            foreach (var role in roles)
            {
                claimList.Add(new System.Security.Claims.Claim("role", role.Name));
            }   
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
