using Microondas.Web.Models;
using Microondas.Web.Services;
using Microondas.Web.Services.Repository;
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



        // POST: /Microondas/Iniciar

        [HttpGet]
        public ActionResult Index()
        {
            // Carrega a lista de programas pré-definidos
            var lista = ProgramasPreDefinidosRepository.Programas;
            return View(lista);
        }

        // POST normal do Nível 1
        [HttpPost]
        public ActionResult Iniciar(int tempo, int? potencia)
        {
            string mensagem = _service.IniciarAquecimento(tempo, potencia);
            ViewBag.Mensagem = mensagem;
            return View("Index", ProgramasPreDefinidosRepository.Programas);
        }

        // POST para iniciar programa pré-definido
        [HttpPost]
        public ActionResult IniciarPrograma(string nomePrograma)
        {
            string mensagem = _service.IniciarProgramaPreDefinido(nomePrograma);
            ViewBag.Mensagem = mensagem;
            return View("Index", ProgramasPreDefinidosRepository.Programas);
        }

        // Pausa e cancela
        [HttpPost]
        public ActionResult PausarCancelar()
        {
            string mensagem = _service.PausarOuCancelar();
            ViewBag.Mensagem = mensagem;
            return View("Index", ProgramasPreDefinidosRepository.Programas);
        }
    }
}

