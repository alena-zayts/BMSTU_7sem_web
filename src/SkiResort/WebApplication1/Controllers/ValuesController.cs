//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;

//namespace WebApplication1.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class ValuesController : Controller
//    {
//        [Authorize]
//        [Route("getlogin")]
//        public IActionResult GetLogin()
//        {
//            return Ok($"Ваш логин: {User.Identity.Name}");
//        }

//        [Authorize(Roles = "admin")]
//        [Route("getrole")]
//        public IActionResult GetRole()
//        {
//            return Ok("Ваша роль: администратор");
//        }
//    }
//}