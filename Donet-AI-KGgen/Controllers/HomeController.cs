using Donet_AI_KGgen.Models;
using Donet_AI_KGgen.Services;
using Microsoft.AspNetCore.Mvc;

namespace Donet_AI_KGgen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKnowledgeGraphService _knowledgeGraphService;

        public HomeController(ILogger<HomeController> logger, IKnowledgeGraphService knowledgeGraphService)
        {
            _logger = logger;
            _knowledgeGraphService = knowledgeGraphService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Generate([FromBody] GenerateRequestDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Input))
                return BadRequest("Invalid input");

            KnowledgeGraphDto result = await _knowledgeGraphService.Generate(dto);
            return Json(result);
        }
    }
}
