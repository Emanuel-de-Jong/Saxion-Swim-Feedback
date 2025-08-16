using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swim_Feedback.Data;
using Swim_Feedback.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Swim_Feedback.Services
{
    public class AccountService
    {
        /*  [Inject]
          private IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

          [Inject]
          private ConfigurationManager? configmanager { get; set; }


          public ResponseMessage Login(string username, string password)
          {

              Account account = checkAccount(username);

              if(username.Equals("") || password.Equals("")) {
                  return new ResponseMessage(HttpStatusCode.BadRequest, "Unable to login with empty data");
              }

              if (account == null) {
                  return new ResponseMessage(HttpStatusCode.NotFound, "User does not exist");
              }
  if (!HashPassword(password, account.Salt).SequenceEqual(account.Password))
      {
          return new ResponseMessage(HttpStatusCode.BadRequest, "Incorrect password");
      }

      string Token = CreateToken(account);
      return new ResponseMessage(HttpStatusCode.OK, "Token: " + Token);
  }

  public ResponseMessage CreateAccount(string username, string email, string password)
          {
              ApplicationDbContext dbContext = DbContextFactory.CreateDbContext();
              if (username.Equals("") || email.Equals("") || password.Equals("")) 
              {
                  return new ResponseMessage(HttpStatusCode.BadRequest, "Unable to create a account with empty data");
              }

              Account account = checkAccount(username);
              if (account != null)
              {
                  return new ResponseMessage(HttpStatusCode.Conflict, "User already exists");
              }

              byte[] salt = RandomNumberGenerator.GetBytes(32);
              byte[] hashedpassword = HashPassword(password, salt);

              account = new Account(username, hashedpassword, email, salt);
              dbContext.Accounts.Add(account);
              return new ResponseMessage(HttpStatusCode.Created, "User created");
          }


          private byte[] HashPassword(string givenPassword, byte[] salt)
          {
              string pepper = configmanager["Pepper"];
              string seasonedPassword = givenPassword + pepper;
              byte[] hashedPassword = KeyDerivation.Pbkdf2(password: seasonedPassword,
                                                   salt: salt,
                                                   prf: KeyDerivationPrf.HMACSHA256,
                                                   iterationCount: 100000, 
                                                   numBytesRequested: 256 / 8);
              return hashedPassword;
          }


          private string CreateToken(Account account)
          {

              List<Claim> claims = new()
              {
                  new Claim(ClaimTypes.Name, account.Username)
              };


              byte[] key = Encoding.UTF8.GetBytes(configmanager["JWT"]);
              SymmetricSecurityKey secret = new(key);

              SigningCredentials credentials = new(secret, SecurityAlgorithms.HmacSha256);

              JwtSecurityToken token = new(
                 claims: claims,
                 expires: DateTime.Now.AddHours(6),
                 signingCredentials: credentials);

              string Jwt = new JwtSecurityTokenHandler().WriteToken(token);

              return Jwt;
          }

          private Account? checkAccount(string username)
          {
              ApplicationDbContext dbContext = DbContextFactory.CreateDbContext();

              Account account = dbContext.Accounts.FirstOrDefault(Account => Account.Username == username);

              if (account == null)
              {
                  return null;
              }
              return account;
          }*/
    }
}
