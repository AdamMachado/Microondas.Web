using Microondas.Web.Services;

namespace Microondas.Tests
{
    [TestClass]
    public class AquecimentoServiceTests
    {

        private AquecimentoService _service;

        [TestInitialize]
        public void Setup()
        {
            // Executa antes de cada teste
            _service = new AquecimentoService();
        }

        [TestMethod]
        public void IniciarAquecimento_ValidInput_ShouldReturnExpectedMessage()
        {
            // Arrange
            int tempo = 60;
            int? potencia = 5;

            // Act
            string result = _service.IniciarAquecimento(tempo, potencia);

            // Assert
            // Aqui depende da mensagem que você retorna. Exemplo:
            StringAssert.Contains(result, "Aquecendo por 1:00\"; ex.: 90 => \"1:30 (potência) 5");
            StringAssert.Contains(result, "Aquecimento concluído");
        }

        [TestMethod]
        public void IniciarAquecimento_InicioRapido_TempoZero_ShouldUse30sPot10()
        {
            // Arrange
            int tempo = 0;       // Indica início rápido
            int? potencia = null; // Vazio => 10

            // Act
            string result = _service.IniciarAquecimento(tempo, potencia);

            // Assert
            StringAssert.Contains(result, "Aquecendo por 30s (potência) 10");
        }

        [TestMethod]
        public void IniciarAquecimento_JaExecutando_Soma30s()
        {
            // 1) Inicia uma primeira vez
            _service.IniciarAquecimento(30, 5);

            // 2) Inicia de novo sem pausar => deve somar +30s
            string result = _service.IniciarAquecimento(30, 5);

            StringAssert.Contains(result, "Tempo acrescido");
        }

        [TestMethod]
        public void IniciarAquecimento_JaExecutandoEPausado_Retoma()
        {
            // 1) Inicia
            _service.IniciarAquecimento(30, 5);

            // 2) Pausa
            _service.PausarOuCancelar();
            // Supondo que se estiver executando, a 1a chamada em PausarOuCancelar() pausa

            // 3) Clica em Iniciar => retoma
            string result = _service.IniciarAquecimento(30, 5);

            StringAssert.Contains(result, "Aquecimento retomado");
        }

        [TestMethod]
        public void IniciarAquecimento_TempoInvalido_ShouldReturnError()
        {
            // Tempo = 200 => maior que 120
            string result = _service.IniciarAquecimento(200, 5);

            StringAssert.Contains(result, "Tempo inválido");
        }

        [TestMethod]
        public void IniciarAquecimento_PotenciaInvalida_ShouldReturnError()
        {
            // Potência = 12 => maior que 10
            string result = _service.IniciarAquecimento(60, 12);

            StringAssert.Contains(result, "Potência inválida");
        }
    }
}
