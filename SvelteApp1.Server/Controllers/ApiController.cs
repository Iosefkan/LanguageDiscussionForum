using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SvelteApp1.Server.Data;
using SvelteApp1.Server.Models;

namespace SvelteApp1.Server.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        [HttpGet]
        [Route("questions")]
        public async Task<IActionResult> GetQuestions([FromServices] ApplicationContext appContext)
        {
            return Json(await appContext.Questions.Include(x => x.Language).Select(x => new { x.Id, x.Title, x.Quest, x.Upvotes, x.Language!.Lang }).ToListAsync());
        }

        [HttpGet]
        [Route("user_info")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetUserInfo([FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid) 
                return BadRequest("Недостаточно информации для регистрации");
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Guid == guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            int upvotesOnQuestions = await appContext.Questions.Where(x => x.UserGuid == guid).SumAsync(x => x.Upvotes);
            int upvotesOnComments = await appContext.Comments.Where(x => x.UserGuid == guid).SumAsync(x => x.Likes);
            int questionCount = await appContext.Questions.Where(x => x.UserGuid == guid).CountAsync();
            int commentCount = await appContext.Comments.Where(x => x.UserGuid == guid).CountAsync();
            bool isAdmin = user.Role.Name == "admin" ? true : false;
            var userInfo = new UserInfo() {
                Name = user.Name,
                Email = user.Email,
                Upvotes = upvotesOnComments + upvotesOnQuestions,
                IsAdmin = isAdmin,
                QuestionCount = questionCount,
                CommentCount = commentCount
            };
            return Json(userInfo);
        }
        [HttpGet]
        [Route("statistics/by_day")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "admin")]
        public async Task<IActionResult> GetQuestionsUser([FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var data = await appContext.logginStatistics.GroupBy(x => x.DateTime.Date).Select(x => new { Date = x.Key.ToShortDateString(), LoginCount = x.Count()}).ToArrayAsync();
            if (data is null){
                return StatusCode(500);
            }
            return Json(data.OrderByAlphaNumeric(x => x.Date));
        }
    }
}
