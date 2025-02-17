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

        [HttpGet]
        public ActionResult Index()
        {
            var lista = ProgramasAquecimentoRepository.TodosProgramas;
            return View(lista);
        }

        [HttpPost]
        public ActionResult Iniciar(int tempo, int? potencia)
        {
            string mensagem = _service.IniciarAquecimento(tempo, potencia);
            ViewBag.Mensagem = mensagem;

            return View("Index");
        }

        [HttpPost]
        public ActionResult IniciarPrograma(string nomePrograma)
        {
            string mensagem = _service.IniciarProgramaPreDefinido(nomePrograma);
            ViewBag.Mensagem = mensagem;
            return View("Index", ProgramasAquecimentoRepository.TodosProgramas);
        }

        [HttpPost]
        public ActionResult PausarCancelar()
        {
            string mensagem = _service.PausarOuCancelar();
            ViewBag.Mensagem = mensagem;
            return View("Index", ProgramasAquecimentoRepository.TodosProgramas);
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult CadastrarCustom(ProgramaAquecimento novoPrograma)
        {
            string msg = ProgramasAquecimentoRepository.AdicionarCustomizado(novoPrograma);
            ViewBag.Mensagem = msg;

            var lista = ProgramasAquecimentoRepository.TodosProgramas;
            return View("Index", lista);
        }
    }
}

