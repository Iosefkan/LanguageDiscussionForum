using Microsoft.AspNetCore.Mvc;
using SvelteApp1.Server.Models;
using HashLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SvelteApp1.Server.Services;
using SvelteApp1.Server.Data;

namespace SvelteApp1.Server.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> PostRegister([FromServices] ApplicationContext appContext, [FromBody] RegUserInfo regUser, [FromServices] IEmailConfirmationService emailService)
        {
            if (!ModelState.IsValid) 
                return BadRequest("Недостаточно информации для регистрации");
            if (await appContext.Users.FirstOrDefaultAsync(x => x.Email == regUser.Email) is not null)
                return Json(new { mess = "User with this email already exists" });
            string guid = Guid.NewGuid().ToString();
            var userRole = await appContext.Roles.FirstOrDefaultAsync(x => x.Name == "user");
            if (userRole is null)
                return StatusCode(500);
            var user = new User { Email = regUser.Email!, Password = SecurePasswordHasher.Hash(regUser.Password!), Name = regUser.Name!, Guid = guid, EmailConfirmed = false, Role = userRole, RoleId = userRole.Id };
            await appContext.Users.AddAsync(user);
            await appContext.SaveChangesAsync();
            string confLink = string.Concat(Request.Scheme, "://", Request.Host.ToUriComponent(), "/auth/", "confirm/", guid);
            await emailService.SendConfirmationEmailAsync(regUser.Email!, confLink);
            return Ok();
        }

        [HttpGet]
        [Route("confirm/{guid:guid}")]
        public async Task<IActionResult> Confirm([FromServices] ApplicationContext appContext, [FromRoute] string guid)
        {
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
                return BadRequest("No such user exists");
            if (user.EmailConfirmed == true)
                return Redirect("/");
            user.EmailConfirmed = true;
            await appContext.SaveChangesAsync();
            return Redirect("/");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> PostLogin([FromServices] ApplicationContext appContext, [FromServices] ITokenService tokenService, [FromBody] LoginData loginData)
        {
            // var Admin = new Role() { Id = 1, Name = "admin" };
            // var User = new Role() { Id = 2, Name = "user" };
            // appContext.Roles.AddRange([Admin, User]);
            // appContext.SaveChanges();
            // var admin = new User() { Guid = Guid.NewGuid().ToString(), Email = "vsedletski@gmail.com", Name = "George", Password = SecurePasswordHasher.Hash("NeverDie"), RoleId = Admin.Id, Role = Admin, EmailConfirmed = true };
            // appContext.Users.Add(admin);
            // appContext.SaveChanges();
            // var lang1 = new Language() { Lang = "Irish" };
            // var lang2 = new Language() { Lang = "Japanese" };
            // var lang3 = new Language() { Lang = "Russian" };
            // var lang4 = new Language() { Lang = "English" };
            // var lang5 = new Language() { Lang = "French" };
            // var lang6 = new Language() { Lang = "Spanish" };
            // var lang7 = new Language() { Lang = "Korean" };
            // var lang8 = new Language() { Lang = "Mandarin" };
            // appContext.Languages.AddRange([lang1, lang2, lang3, lang4, lang5, lang6, lang7, lang8]);
            // appContext.SaveChanges();
            // var question = new Question() { Language = lang1, ViewCount = 10, Title = "Trying out", Upvotes = 1, Quest = "This is my question", User = admin, UserGuid = admin.Guid};
            // var question2 = new Question() { Language = lang2, ViewCount = 8, Title = "Figuring out", Upvotes = 4, Quest = "This is not my question", User = admin, UserGuid = admin.Guid};
            // appContext.Questions.AddRange([question, question2]);
            // appContext.SaveChanges();
            // var comment1To1 = new Comment() { User = admin, UserGuid = admin.Guid, Dislikes = 1, Likes = 2, Question = question, QuestionId = question.Id, Comm = "I am commenting this" };
            // var comment2To1 = new Comment() { User = admin, UserGuid = admin.Guid, Dislikes = 5, Likes = 25, Question = question, QuestionId = question.Id, Comm = "I hate this comment" };
            // var comment1To2 = new Comment() { User = admin, UserGuid = admin.Guid, Dislikes = 0, Likes = 0, Question = question2, QuestionId = question2.Id, Comm = "Don't trust this guy, he's crazy" };
            // var comment2To2 = new Comment() { User = admin, UserGuid = admin.Guid, Dislikes = 0, Likes = 0, Question = question2, QuestionId = question2.Id, Comm = "NeverDoThisEverAgain" };
            // appContext.Comments.AddRange([comment1To1, comment2To1, comment1To2, comment2To2]);
            // appContext.SaveChanges();
            // return Ok();
            if (!ModelState.IsValid)
                return BadRequest("Email и/или пароль не установлены");
            User? user = await appContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == loginData.Email);
            if (user is null || !SecurePasswordHasher.Verify(loginData.Password!, user.Password) || user.EmailConfirmed == false) return Unauthorized();
            var accessToken = tokenService.GenerateAccessToken(JwtConfig.AccessTokenExpiration, user);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.Add(JwtConfig.RefreshTokenExpiration);
            var logged = new LogginStatistic() { DateTime = DateTime.UtcNow, User = user };
            await appContext.logginStatistics.AddAsync(logged);
            await appContext.SaveChangesAsync();
            return Json(new AuthenticatedUserInfo{AccessToken = accessToken, RefreshToken = refreshToken});
        }

        [HttpPost]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromServices] ApplicationContext appContext, [FromBody] AuthenticatedUserInfo authInfo, [FromServices] RevokedJWTCashingService revokedJWTCashingService)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            revokedJWTCashingService.RevokeJWT(authInfo.AccessToken!, authInfo.RefreshToken!);
            var user = await appContext.Users.FirstOrDefaultAsync(x => x.AccessToken == authInfo.AccessToken && x.RefreshToken == authInfo.RefreshToken);
            if (user is not null)
            {
                user.AccessToken = null;
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await appContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromServices] ApplicationContext appContext, [FromServices] ITokenService tokenService, [FromBody] AuthenticatedUserInfo authInfo)
        {
            System.Console.WriteLine("\n First \n");
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            System.Console.WriteLine("\n Second \n");
            System.Console.WriteLine("Acc   " + authInfo.AccessToken);
            System.Console.WriteLine("Ref    " + authInfo.RefreshToken);
            var user = await appContext.Users.Include(x => x.Role).FirstOrDefaultAsync(u => u.AccessToken == authInfo.AccessToken && u.RefreshToken == authInfo.RefreshToken);
            System.Console.WriteLine("\n Third \n");
            if (user is null){
                System.Console.WriteLine("\n Null \n");
            }
            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow){
                System.Console.WriteLine("\n Fourth \n");
                return BadRequest("Invalid client request");
            }
            System.Console.WriteLine("\n Fifth \n");

            var accessToken = tokenService.GenerateAccessToken(JwtConfig.AccessTokenExpiration, user);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.Add(JwtConfig.RefreshTokenExpiration);
            await appContext.SaveChangesAsync();
            return Json(new AuthenticatedUserInfo { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost]
        [Route("resend")]
        public async Task<IActionResult> Resend([FromServices] ApplicationContext appContext, [FromServices] IEmailConfirmationService emailService, [FromBody] EmailInfo emailInfo)
        {
            if (!ModelState.IsValid){
                return BadRequest();
            }
            var user = await appContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == emailInfo.Email);
            if (user is null){
                return Json(new ResultInfo() { Result = "No such email registered"});
            }
            if (user.EmailConfirmed == true){
                return Json(new ResultInfo() { Result = "Email already confirmed"});
            } 
            string confLink = string.Concat(Request.Scheme, "://", Request.Host.ToUriComponent(), "/fallback", "/auth/", "confirm/", user.Guid);
            await emailService.SendConfirmationEmailAsync(user.Email!, confLink);
            return Json(new ResultInfo() { Result = "We resent you the confirmation letter, please check your email"});
        }
    }
}
