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
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            serviceResponse.Data = user.Id;
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> Login (string userName, string password){
            var serviceResponse = new ServiceResponse<int>();
            serviceResponse.Data = 12;
            return serviceResponse;
        }

        public async Task<int> UserExists (UserRegisterDto userRegisterRequest){
            return 12;
        }

        private void CreatePasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}