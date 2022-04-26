using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken(string userEmail);
    }

    public class TokenService : ITokenService
    {
        private readonly AppDbContext context;
        private readonly SymmetricSecurityKey _key;

        public TokenService(AppDbContext context, IConfiguration configuration )
        {
            this.context = context;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }

        public async Task<string> GetToken(string userEmail)
        {
            var user = await context.UserMaster.FirstOrDefaultAsync(u=> u.Email== userEmail);

            

            if (user == null)   
                return null;
            var role = await context.UserRoles.Include(r => r.Role).FirstOrDefaultAsync(u => u.UserId == user.Id);
            string rolename = "Member";
            if (role != null)
            {
                rolename = role.Role.Name;
            }
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Email),
                new Claim(ClaimTypes.Role, rolename)
            };

            //var roles = await _userManager.GetRolesAsync(user);

           // claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
