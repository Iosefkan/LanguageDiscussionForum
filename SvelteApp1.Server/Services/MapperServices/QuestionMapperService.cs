using SvelteApp1.Server.Models;

namespace SvelteApp1.Server.Services
{
    public class QuestionMapperService 
    {
        public QuestionInfo MapQuestion(Question question, bool likedByCurrectUser)
        {
            var questInfo = new QuestionInfo
            {
                QuestionId = question.Id,
                Question = question.Quest,
                UserName = question.User.Name,
                Title = question.Title,
                Upvotes = question.Upvotes,
                Language = question.Language!.Lang,
                ViewCount = question.ViewCount,
                Liked = likedByCurrectUser
            };

            return questInfo;
        }
    }
}