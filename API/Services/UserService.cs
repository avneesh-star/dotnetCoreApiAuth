using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;

        public UserService(AppDbContext context, IConfiguration configuration, ITokenService tokenService)
        {
            this.context = context;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        public async Task<ApiResponse> RegisterUser(RegistrationModel model)
        {
            if (await context.UserMaster.AnyAsync(x => x.Email == model.email))
            {
                return new ApiResponse("fail", "user already regitered");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.password);
            UserMaster userMaster = new UserMaster
            {
                Email = model.email,
                PasswordHash = passwordHash,
                FirstName = model.firstName,
                LastName = model.lastName,
                DateCreate = DateTime.Now,
                MobileNo = model.mobile
            };
            context.UserMaster.Add(userMaster);
            await context.SaveChangesAsync();
            return new ApiResponse("success", "registration success", model);
        }

        public async Task<ApiResponse> Login(LoginModel model)
        {
            var user = await context.UserMaster.FirstOrDefaultAsync(u => u.Email == model.email);
            if (user == null)
            {
                return new ApiResponse("failed", "invalid user email");
            }
            bool isValidPass = BCrypt.Net.BCrypt.Verify(model.password, user.PasswordHash);
            if (!isValidPass)
            {
                return new ApiResponse("failed", "invalid password");
            }

            var role = await context.UserRoles.Include(r => r.Role).FirstOrDefaultAsync(u => u.UserId == user.Id);

            string rolename = "Member";
            if (role != null)
            {
                rolename = role.Role.Name;
            }

            var response = new
            {
                name = user.FirstName + " " + user.LastName ?? String.Empty,
                email = user.Email,
                mobile = user.MobileNo,
                role = rolename,
                accessToken =await tokenService.GetToken(model.email)
            };

            return new ApiResponse("success", "login success", response);


        }

        public async Task<ApiResponse> GetUser(int UserId)
        {
            var user = await context.UserMaster.FirstOrDefaultAsync(x => x.Id == UserId);
            if (user != null)
            {
                UserModel userModel = new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobileNo = user.MobileNo
                };

                return new ApiResponse("success", "reqest success", userModel);
            }

            else
                return new ApiResponse("failed", "reqest failed");
        }

        public async Task<ApiResponse> GetUsers()
        {
            var users = await context.UserMaster.ToListAsync();
            if (users != null)
            {
                var alluser = users.Select(user => new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobileNo = user.MobileNo
                });
                return new ApiResponse("success", "reqest success", alluser);
            }
                
            else
                return new ApiResponse("failed", "reqest failed");
        }

    }
}
