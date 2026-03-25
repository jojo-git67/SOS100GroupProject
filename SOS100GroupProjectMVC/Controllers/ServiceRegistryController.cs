using Microsoft.AspNetCore.Mvc;
using SOS100GroupProjectMVC.Services;

namespace SOS100GroupProjectMVC.Controllers
{
    public class ServiceRegistryController : Controller
    {
        private readonly SchemaApiService _schemaApiService;
        private readonly RegistreringApiService _registreringApiService;

        public ServiceRegistryController(
            SchemaApiService schemaApiService,
            RegistreringApiService registreringApiService)
        {
            _schemaApiService = schemaApiService;
            _registreringApiService = registreringApiService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _schemaApiService.GetAllActivitiesAsync();
            return Content(data, "application/json");
        }

        public async Task<IActionResult> Registrering()
        {
            var data = await _registreringApiService.GetAllRegistrationsAsync();
            return Content(data, "application/json");
        }

        public async Task<IActionResult> RegistreringUser(int userId)
        {
            var data = await _registreringApiService.GetByUserAsync(userId);
            return Content(data, "application/json");
        }

        public async Task<IActionResult> RegistreringCourse(int courseId)
        {
            var data = await _registreringApiService.GetByCourseAsync(courseId);
            return Content(data, "application/json");
        }
    }
}