using Microondas.Web.Models;
using Microondas.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Microondas.Web.Controllers
{
    public class MicroondasController : Controller
    {
        private AquecimentoService _service;

        public MicroondasController()
        {
            _service = new AquecimentoService();
        }

        // GET: /Microondas/
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Microondas/Iniciar


        [HttpPost]
        public ActionResult Iniciar(int tempo, int? potencia)
        {
            string msg = _service.IniciarAquecimento(tempo, potencia);
            ViewBag.Mensagem = msg;
            return View("Index");
        }

        [HttpPost]
        public ActionResult PausarCancelar()
        {
            string msg = _service.PausarOuCancelar();
            ViewBag.Mensagem = msg;
            return View("Index");
        }

    }
}

