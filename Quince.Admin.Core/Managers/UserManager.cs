using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.User;
using Quince.Utils.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class UserManager : IDisposable
    {
        public static UserManager Create()
        {
            return new UserManager();
        }
        public static Response Create(RegisterModel registerModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            if (context.Users.Any(u => u.Email.Equals(registerModel.Email)))
            {
                response.AddMessage(false, "This email is already registered", ResponseMessageType.Warning);
            }
            else
            {
                var user = new User();
                user.Email = registerModel.Email;
                user.Password = GenerateHashWithSalt(registerModel.Password, registerModel.Email);
                user.PasswordSalt = Path.GetRandomFileName();
                context.Users.Add(user);
                context.SaveChanges();
            }
            return response;
        }
        public static async Task<Response> CreateUserAsync(RegisterModel registerModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            if (context.Users.Any(u => u.Email.Equals(registerModel.Email)))
            {
                response.AddMessage(false, "This email is already registered", ResponseMessageType.Warning);
            }
            else
            {
                var user = new User();
                user.Email = registerModel.Email;
                user.Password = GenerateHashWithSalt(registerModel.Password, registerModel.Email);
                user.PasswordSalt = Path.GetRandomFileName();
                user.RegisterDate = DateTime.Now;
                user.Active = true;
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Response> CreateUserAsync(AddUserModel registerModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            ValidateUser(registerModel, response);
            if (!response.Success)
            {
                return response;
            }
            if (context.Users.Any(u => u.Email.Equals(registerModel.Email)))
            {
                response.AddMessage(false, "This email is already registered", ResponseMessageType.Warning);
            }
            else
            {
                var user = new User();
                user.Email = registerModel.Email;
                user.Password = GenerateHashWithSalt(registerModel.Password, registerModel.Email);
                user.PasswordSalt = Path.GetRandomFileName();
                user.RegisterDate = registerModel.RegisterDate;
                user.Active = registerModel.Active;
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Response> UpdateUserAsync(AddUserModel registerModel)
        {
            var response = new Response();

            ValidateUser(registerModel, response);
            if (!response.Success)
            {
                return response;
            }
            else
            {
                var context = new AdminDbContext();
                var user = context.Users.Find(registerModel.Id);
                if (user != null)
                {
                    user.Email = registerModel.Email;
                    if (!string.IsNullOrEmpty(registerModel.Password))
                    {
                        user.Password = GenerateHashWithSalt(registerModel.Password, registerModel.Email);
                    }
                    user.PasswordSalt = Path.GetRandomFileName();
                    user.RegisterDate = registerModel.RegisterDate;
                    user.Active = registerModel.Active;
                    await context.SaveChangesAsync();
                }
            }
            return response;
        }
        public static async Task<Response> DeleteUserAsync(long id)
        {
            var response = new Response();
            var context = new AdminDbContext();
            var user = await context.Users.FindAsync(id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return response;
        }
        public static Response ValidateUser(AddUserModel registerModel, Response response = null)
        {
            response = response ?? new Response();
            if (string.IsNullOrEmpty(registerModel.Email))
            {
                response.AddMessage(false, "Email is mandatory", ResponseMessageType.Warning);
            }
            if (registerModel.Id == 0)
            {
                if (string.IsNullOrEmpty(registerModel.Password))
                {
                    response.AddMessage(false, "Password is mandatory", ResponseMessageType.Warning);
                }
            }
            return response;
        }
        public static Response GetUser(string username, string password)
        {
            var response = new Response();
            var context = new AdminDbContext();
            var hashedPassword = GenerateHashWithSalt(password, username);
            var user = context.Users.FirstOrDefault(u => u.Email.Equals(username) && u.Password.Equals(hashedPassword));
            if (user != null)
            {
                response.Data = user;
            }
            else
            {
                response.AddMessage(false, "Username or password is invalid", ResponseMessageType.Warning);
            }
            return response;
        }
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            System.Security.Cryptography.HashAlgorithm algorithm = new System.Security.Cryptography.SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }
        public static IEnumerable<UserModel> GetUsers()
        {
            var context = new AdminDbContext();
            return context.Users.Select(u => new UserModel { Id = u.Id, Email = u.Email, Active = u.Active, RegisterDate = u.RegisterDate });
        }
        public static DTResponse GetUsers(DTRequest request)
        {
            var context = new AdminDbContext();
            var response = new DTResponse()
            {
                draw = request.draw
            };
            var users = context.Users;
            var data = users.AsEnumerable();
            if (request.order != null && request.order.Any())
            {
                var asc = request.order[0].dir == "asc";
                switch (request.order[0].column)
                {
                    case 0:
                        {
                            data = asc ? data.OrderBy(u => u.Id) : data.OrderByDescending(u => u.Id);
                            break;
                        }
                    case 1:
                        {
                            data = asc ? data.OrderBy(u => u.Email) : data.OrderByDescending(u => u.Email);
                            break;
                        }
                    case 2:
                        {
                            data = asc ? data.OrderBy(u => u.RegisterDate) : data.OrderByDescending(u => u.RegisterDate);
                            break;
                        }
                    case 3:
                        {
                            data = asc ? data.OrderBy(u => u.Active) : data.OrderByDescending(u => u.Active);
                            break;
                        }
                    default:
                        {
                            data = data.OrderBy(u => u.Id);
                            break;
                        }

                }
            }
            response.recordsFiltered = data.Count();
            data = data.Skip(request.start).Take(request.length);
            var responseData = data.Select(u => new UserModel { Id = u.Id, Email = u.Email, Active = u.Active, RegisterDate = u.RegisterDate });
            response.data = responseData;
            response.recordsFiltered = users.Count();
            response.recordsTotal = context.Users.Count();
            return response;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public static object GetUser(int id)
        {
            var context = new AdminDbContext();
            var dbUser = context.Users.Find(id);
            if (dbUser != null)
            {
                var user = new AddUserModel();
                user.Id = dbUser.Id;
                user.Email = dbUser.Email;
                user.RegisterDate = dbUser.RegisterDate;
                user.Active = dbUser.Active;
                return user;
            }
            return null;
        }


    }
}
