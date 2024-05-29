using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Birds;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {
        Bird bird;

        public TestAPIController(Bird bird)
        {
            this.bird = bird;
        }

        [HttpGet("Test")] // domain/api/TestAPI/Test
        public string Test()
        {
            return bird.Eat();
        }
    }
}
