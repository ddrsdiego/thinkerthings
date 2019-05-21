using Microsoft.AspNetCore.Mvc;

namespace ThinkerThings.Servicos.Usuarios.Identity.Api.Controllers
{
    public class UsuarioController : Controller
    {
        public UsuarioController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}