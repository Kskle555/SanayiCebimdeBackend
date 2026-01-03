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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var (items, totalCount) = await _ustaService.GetPagedUstalarsAsync(page, pageSize);
                return Ok(new { items, totalCount, page, pageSize });
            }
            catch(Exception Ex)
            {
                Console.WriteLine($"hata{ Ex.Message}");
                return BadRequest("hata");
            }
        }
    }
}
