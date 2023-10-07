using System.IO.Enumeration;
using System.Data;
using System.Security.Claims;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;
using System.Runtime;
using System.Buffers.Text;
using System.Reflection.Metadata;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace dotnet_api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext dataContext, IConfiguration configuration){
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<int>> Register (User user, string password)
        {
            var serviceResponse = new ServiceResponse<int>();

            if(await UserExists(user.UserName)){
                serviceResponse.Message = "User Exists";
                serviceResponse.Success = false;
                return serviceResponse;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            serviceResponse.Data = user.Id;
            serviceResponse.Message = "User Registered Successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> Login (string userName, string password){
            var serviceResponse = new ServiceResponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            if(user is null){
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found !";
            }else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
                serviceResponse.Success = false;
                serviceResponse.Message = "User or password not correct !";
            }else{
                serviceResponse.Message = "User founded !";
                serviceResponse.Data = CreateToken(user);
            }
            return serviceResponse;
        }

        public async Task<bool> UserExists (string userName){
            if(await _dataContext.Users.AnyAsync(c => c.UserName.ToLower() == userName.ToLower())){
                return true;
            }
            return false;
        }

        private void CreatePasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash (string password, byte[] passwordHash, byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (computedHash.SequenceEqual(passwordHash));
            }
        }

        private string CreateToken (User user){

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}