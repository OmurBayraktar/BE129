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

        [HttpPut("{id}")]
        [Consumes("application/json")]
        public IResult PutItem(int id, string title, string content)
        {
            var index = articles.FindIndex(x => x.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            if (title == null)
            {
                return Results.BadRequest("Title boş bırakılamaz!");
            }

            articles[index].Title = title;
            articles[index].Content = content;

            return Results.Ok("Makale güncellendi");
        }

        [HttpDelete("{id}")]
        public IResult DeleteItem(int id)
        {
            var index = articles.FindIndex(x => x.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            if (id == null)
            {
                return Results.NoContent();
            }

            articles.RemoveAt(index);

            return Results.Ok("Makale silindi");
        }

    }
}
