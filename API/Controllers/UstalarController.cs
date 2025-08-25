using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanayiCebimdeBackend.Application.Interfaces;

namespace SanayiCebimdeBackend.API.Controllers
{
    public class UstalarController : Controller
    {

        private readonly IUstalarService _ustaService;
        private readonly IConfiguration _configuration;
        public UstalarController(IUstalarService ustaService, IConfiguration configuration)
        {
            _ustaService = ustaService;
            _configuration = configuration;
        }

      

       

        [HttpGet("ustalar")]
        //[Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            try
            {
                var ustalar = _ustaService.GetAllUstalarAsync().Result;
                return Ok(ustalar);
            }
            catch(Exception Ex)
            {
                Console.WriteLine($"hata{ Ex.Message}");
                return BadRequest("hata");
            }
        }
    }
}
