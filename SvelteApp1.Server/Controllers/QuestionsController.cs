using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SvelteApp1.Server.Models;
using System.Security.Claims;
using SvelteApp1.Server.Data;
using Microsoft.EntityFrameworkCore;
using SvelteApp1.Server.Services;
using HashLibrary;

namespace SvelteApp1.Server.Controllers
{
    [Route("quest")]
    public class QuestionsController : Controller
    {
        [HttpPost]
        [Route("create")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> CreateQuestion([FromForm] NewQuestionInfo questionInfo, [FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            var lang = await appContext.Languages.FirstOrDefaultAsync(x => x.Lang == questionInfo.Language);
            if (lang is null){
                return StatusCode(500);
            }
            var question = new Question() { Title = questionInfo.Title!, User = user!, Quest = questionInfo.Question!, Language = lang, UserGuid = user.Guid };
            await appContext.Questions.AddAsync(question);
            await appContext.SaveChangesAsync();
            var id = question.Id;
            return Redirect($"/quest/{question.Id}");
        }

        // [HttpPost]
        // [Route("{id:int}")]
        // [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        // public async Task<IActionResult> CreateComment([FromRoute] int id, [FromBody] NewCommentInfo commentInfo, [FromServices] ApplicationContext appContext)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest("You ruined everything");
        //     }
        //     var question = await appContext.Questions.FindAsync(id);
        //     if (question is null)
        //     {
        //         return StatusCode(500);
        //     }
        //     string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
        //     var user = await appContext.Users.FindAsync(guid);
        //     if (user is null)
        //     {
        //         return StatusCode(500);
        //     }
        //     var comment = new Comment() { Comm = commentInfo.Comment!, User = user, Question = question, QuestionId = id };
        //     await appContext.Comments.AddAsync(comment);
        //     await appContext.SaveChangesAsync();
        //     return Redirect($"/quest/{id}");
        //}

        [HttpGet]
        //[Route("/")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetAllQuestions([FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var data = await (from q in appContext.Questions.Include(x => x.User).Include(x => x.Language)
                        join uq in appContext.UserQuestions.Where(x => x.UserGuid == guid)
            on q.Id equals uq.QuestionId into comm
                        from res in comm.DefaultIfEmpty()
                        select new QuestionInfo()
                        {
                            QuestionId = q.Id,
                            Question = q.Quest,
                            UserName = q.User.Name,
                            Title = q.Title,
                            Upvotes = q.Upvotes,
                            Language = q.Language!.Lang,
                            ViewCount = q.ViewCount,
                            Liked = res == null ? false : true
                        }).ToListAsync();
            if (data is null)
            {
                return StatusCode(500);
            }
            return Json(data);
        }
        [HttpGet]
        [Route("user")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetQuestionsUser([FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var data = await (from q in appContext.Questions.Include(x => x.User).Include(x => x.Language).Where(x => x.UserGuid == guid)
                        join uq in appContext.UserQuestions.Where(x => x.UserGuid == guid)
            on q.Id equals uq.QuestionId into comm
                        from res in comm.DefaultIfEmpty()
                        select new QuestionInfo()
                        {
                            QuestionId = q.Id,
                            Question = q.Quest,
                            UserName = q.User.Name,
                            Title = q.Title,
                            Upvotes = q.Upvotes,
                            Language = q.Language!.Lang,
                            ViewCount = q.ViewCount,
                            Liked = res == null ? false : true
                        }).ToListAsync();
            if (data is null)
            {
                return StatusCode(500);
            }
            return Json(data);
        }
        [HttpGet]
        [Route("language/{language}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetQuestionsLanguage([FromRoute] string language, [FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var data = await (from q in appContext.Questions.Include(x => x.User).Include(x => x.Language).Where(x => x.Language!.Lang == language)
                        join uq in appContext.UserQuestions.Include(x => x.Question).Include(x => x.Question.Language).Where(x => x.UserGuid == guid && x.Question.Language!.Lang == language)
            on q.Id equals uq.QuestionId into comm
                        from res in comm.DefaultIfEmpty()
                        select new QuestionInfo()
                        {
                            QuestionId = q.Id,
                            Question = q.Quest,
                            UserName = q.User.Name,
                            Title = q.Title,
                            Upvotes = q.Upvotes,
                            Language = q.Language!.Lang,
                            ViewCount = q.ViewCount,
                            Liked = res == null ? false : true
                        }).ToListAsync();
            if (data is null)
            {
                return BadRequest();
            }
            return Json(data);
        }
        [HttpGet]
        [Route("languages")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetLanguages([FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            return Json(await appContext.Languages.ToListAsync());
        }
        [HttpGet]
        [Route("languages/used")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetLanguagesWithQuestions([FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var data = await appContext.Questions.Include(x => x.Language).GroupBy(x => x.Language).Select(x => x.Key!.Lang).ToArrayAsync();
            return Json(data);
        }
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetQuestion([FromRoute] int id, [FromServices] ApplicationContext appContext, [FromServices] QuestionMapperService mapper)
        {
            Console.WriteLine("\nFirst\n");
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            Console.WriteLine("\nSecond\n");
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var question = await appContext.Questions.Include(x => x.User).Include(x => x.Language).FirstOrDefaultAsync(x => x.Id == id);
            Console.WriteLine("\nThird\n");
            UserQuestion? reaction = null;
            try{
                reaction  = await appContext.UserQuestions.FirstAsync(x => x.QuestionId == id && x.UserGuid == guid);
            }
            catch {}
            Console.WriteLine("\nFourth\n");
            bool likedByCurrectUser = reaction == null ? false : true;
            Console.WriteLine("\nFifth\n");
            if (question is null)
            {
                return StatusCode(500);
            }
            Console.WriteLine("\nI do not know\n");
            question.ViewCount += 1;
            await appContext.SaveChangesAsync();
            return Json(mapper.MapQuestion(question, likedByCurrectUser));
        }
        [HttpGet]
        [Route("comments/{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> GetCommentsToQuestion([FromRoute] int id, [FromServices] ApplicationContext appContext, [FromServices] QuestionMapperService mapper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var question = await appContext.Questions.FindAsync(id);
            Console.WriteLine("\n First \n");
            if (question is null)
            {
                return StatusCode(500);
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            Console.WriteLine("\n Second \n");
            var user = await appContext.Users.FindAsync(guid);
            Console.WriteLine("\n Third \n");
            if (user is null)
            {
                return StatusCode(500);
            }
            Console.WriteLine("\n Fifth \n");
            // var data = appContext.Comments.Include(x => x.User).Where(x => x.QuestionId == question.Id).GroupJoin(
            //     appContext.UserComments.Include(x => x.Comment).Where(x => x.Comment.QuestionId == question.Id),
            //     c => c.UserGuid,
            //     us => us.UserGuid,
            // )
            var data = await (from c in appContext.Comments.Include(x => x.User).Where(x => x.QuestionId == question.Id) join uc in appContext.UserComments.Include(x => x.Comment).Where(x => x.Comment.QuestionId == question.Id && x.UserGuid == guid)
            on c.Id equals uc.CommentId into comm
            from res in comm.DefaultIfEmpty()
            select new CommentInfo()
            {
                CommentId = c.Id,
                Comment = c.Comm,
                UserName = c.User.Name,
                Downvotes = c.Dislikes,
                Upvotes = c.Likes,
                Liked = res.Liked == true ? true : false,
                Disliked = res.Disliked == true ? true : false,
                FileName = c.FileName,
                FileTitle = c.FileTitle,
            }).ToListAsync();
            if (data is null){
                return StatusCode(417);
            }
            return Json(data);
        }
        [HttpPost]
        [Route("upvote/{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> UpvoteQuestion([FromRoute] int id, [FromServices] ApplicationContext appContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var question = await appContext.Questions.FindAsync(id);
            if (question is null)
            {
                return StatusCode(500);
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            var reaction = await appContext.UserQuestions.FirstOrDefaultAsync(x => x.Question.Id == question.Id && x.UserGuid == user.Guid);
            if (reaction is null){
                Console.WriteLine(" + 1");
                reaction = new UserQuestion() { Question = question, QuestionId = question.Id, User = user, UserGuid = user.Guid};
                await appContext.UserQuestions.AddAsync(reaction);
                question.Upvotes += 1;
            }
            else{
                Console.WriteLine(" - 1");
                appContext.UserQuestions.Remove(reaction);
                question.Upvotes -= 1;
            }
            await appContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        [Route("comment/upvote/{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> UpvoteComment([FromRoute] int id, [FromServices] ApplicationContext appContext)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var comment = await appContext.Comments.FindAsync(id);
            if (comment is null)
            {
                return StatusCode(500);
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            var reaction = await appContext.UserComments.FirstOrDefaultAsync(x => x.Comment.Id == comment.Id && x.UserGuid == user.Guid);
            if (reaction is null){
                reaction = new UserComment() { CommentId = comment.Id, Comment = comment, Liked = true, Disliked = false, UserGuid = user.Guid, User = user};
                comment.Likes += 1;
                await appContext.UserComments.AddAsync(reaction);
            }
            else {
                if (reaction.Liked == false){
                    comment.Dislikes -= 1;
                    comment.Likes += 1;
                    reaction.Liked = true;
                    reaction.Disliked = false;
                }
            }
            await appContext.SaveChangesAsync();
            return Ok();
            //return Redirect($"/quest/{comment.QuestionId}");
        }
        [HttpPost]
        [Route("comment/downvote/{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> DownvoteComment([FromRoute] int id, [FromServices] ApplicationContext appContext)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var comment = await appContext.Comments.FindAsync(id);
            if (comment is null)
            {
                return StatusCode(500);
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            var reaction = await appContext.UserComments.FirstOrDefaultAsync(x => x.Comment.Id == comment.Id && x.UserGuid == user.Guid);
            if (reaction is null){
                reaction = new UserComment() { CommentId = comment.Id, Comment = comment, Liked = false, Disliked = true, UserGuid = user.Guid, User = user};
                comment.Dislikes += 1;
                await appContext.UserComments.AddAsync(reaction);
            }
            else {
                if (reaction.Disliked == false){
                    comment.Likes -= 1;
                    comment.Dislikes += 1;
                    reaction.Disliked = true;
                    reaction.Liked = false;
                }
            }
            await appContext.SaveChangesAsync();
            return Ok();
            //return Redirect($"/quest/{comment.QuestionId}");
        }
        [HttpPost]
        [Route("comment/create/{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> CreateComment([FromRoute] int id, [FromServices] ApplicationContext appContext, [FromForm] NewCommentInfo commentInfo)
        {
            System.Console.WriteLine("start");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var question = await appContext.Questions.FindAsync(id);
            if (question is null)
            {
                return StatusCode(500);
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            System.Console.WriteLine("formGet");
            if (commentInfo.Comment is null || (commentInfo.Audio is null && commentInfo.Title is not null )){
                return BadRequest();
            }
            System.Console.WriteLine("fomrSucc");
            if (commentInfo.Audio is not null){
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\audio", commentInfo.Audio.FileName);
                System.Console.WriteLine(filePath);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await commentInfo.Audio.CopyToAsync(fileStream);
                }
            }
            var newComment = new Comment() { Comm = commentInfo.Comment, QuestionId = question.Id, Question = question, FileName = commentInfo.Audio?.FileName, FileTitle = commentInfo.Title, UserGuid = user.Guid, User = user };
            await appContext.Comments.AddAsync(newComment);
            await appContext.SaveChangesAsync();
            return Ok();
            //return Redirect($"/quest/{comment.QuestionId}");
        }
        [HttpPost]
        [Route("comment/unreact/{id:int}")]
        [Authorize(Policy = JwtConfig.Polices.NotLoggedOut, Roles = "user, admin")]
        public async Task<IActionResult> UnreactComment([FromRoute] int id, [FromServices] ApplicationContext appContext)
        {  
            if (!ModelState.IsValid)
            {
                return BadRequest("You ruined everything");
            }
            var comment = await appContext.Comments.FindAsync(id);
            if (comment is null)
            {
                return StatusCode(500);
            }
            string guid = User.FindFirstValue(JwtConfig.ClaimGuid)!;
            var user = await appContext.Users.FindAsync(guid);
            if (user is null)
            {
                return StatusCode(500);
            }
            var reaction = await appContext.UserComments.FirstOrDefaultAsync(x => x.CommentId == comment.Id && x.UserGuid == user.Guid);
            if (reaction is null){
                return StatusCode(400);
            }
            if (reaction.Liked == true && reaction.Disliked == false){
                comment.Likes -= 1;
            }
            else if (reaction.Liked == false && reaction.Disliked == true){
                comment.Dislikes -= 1;
            }
            appContext.Remove(reaction);
            await appContext.SaveChangesAsync();
            return Ok();
            //return Redirect($"/quest/{comment.QuestionId}");
        }
    }
}
