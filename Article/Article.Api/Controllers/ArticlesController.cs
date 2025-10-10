using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Article.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private static List<Article> articles = new();

        [HttpGet]
        public IResult GetList()
        {
            return Results.Json(articles, statusCode: StatusCodes.Status200OK);
        }

        [HttpGet("{id}")]
        public IResult GetId(int id) 
        {
            var itemId = articles.Find(x => x.Id == id);

            if (itemId == null)
            { 
                return Results.NotFound("Böyle bir id yok.");           
            }

            return Results.Json(itemId, statusCode:StatusCodes.Status200OK);
        }

        [HttpPost]
        [Consumes("application/json")]  
        public IResult PostItem(string title, string content)
        {
            var item = new Article
            {   
                Id = articles.Count() + 1,
                Title = title,
                Content = content
            };
            if (title == null)
               return Results.BadRequest("Title boş olamaz!");

            articles.Add(item);

            return Results.Created($"/api/articles/{item.Id}", item);
        }


    }
}
