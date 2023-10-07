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

namespace dotnet_api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext){
            _dataContext = dataContext;
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

        public async Task<ServiceResponse<int>> Login (string userName, string password){
            var serviceResponse = new ServiceResponse<int>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            if(user is null){
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found !";
            }else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
                serviceResponse.Success = false;
                serviceResponse.Message = "User or password not correct !";
            }else{
                serviceResponse.Message = "User founded !";
                serviceResponse.Data = user.Id;
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
    }
}