using Microondas.Web.Aquecimento;
using Microondas.Web.Programa;
using Microsoft.AspNetCore.Mvc;

namespace Microondas.Web.Controllers
{
    public class MicroondasController : Controller
    {
        private readonly IAquecimentoService _aquecimentoService;
        private readonly IProgramasAquecimentoRepository _programasRepository;

        public MicroondasController(IAquecimentoService aquecimentoService, IProgramasAquecimentoRepository programasRepository)
        {
            _aquecimentoService = aquecimentoService;
            _programasRepository = programasRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var lista = _programasRepository.TodosProgramas;
            return View(lista);
        }

        [HttpPost]
        public ActionResult Iniciar(int tempo, int? potencia)
        {
            string mensagem = _aquecimentoService.IniciarAquecimento(tempo, potencia);
            ViewBag.Mensagem = mensagem;
            return View("Index", _programasRepository.TodosProgramas);
        }

        [HttpPost]
        public ActionResult IniciarPrograma(string nomePrograma)
        {
            string mensagem = _aquecimentoService.IniciarProgramaPreDefinido(nomePrograma);
            ViewBag.Mensagem = mensagem;
            return View("Index", _programasRepository.TodosProgramas);
        }

        [HttpPost]
        public ActionResult PausarCancelar()
        {
            string mensagem = _aquecimentoService.PausarOuCancelar();
            ViewBag.Mensagem = mensagem;
            return View("Index", _programasRepository.TodosProgramas);
        }

        [HttpPost]
        public IActionResult CadastrarCustom(ProgramaAquecimento novoPrograma)
        {
            string msg = _programasRepository.AdicionarCustomizado(novoPrograma);
            ViewBag.Mensagem = msg;
            return View("Index", _programasRepository.TodosProgramas);
        }
    }
}
