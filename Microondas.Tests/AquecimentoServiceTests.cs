using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microondas.Web.Services.Repository;
using System.Collections.Generic;
using Microondas.Web.Aquecimento;
using Microondas.Web.Programa;

namespace Microondas.Tests
{
    [TestClass]
    public class AquecimentoServiceTests
    {
        private AquecimentoService _service;
        private Mock<IProgramasAquecimentoRepository> _mockRepo; // Mock do reposit�rio

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IProgramasAquecimentoRepository>();

            // Simulando que o reposit�rio retorna uma lista de programas vazia
            _mockRepo.Setup(repo => repo.TodosProgramas)
                     .Returns(new List<ProgramaAquecimento>());

            // Passando o mock para a inst�ncia de servi�o
            _service = new AquecimentoService(_mockRepo.Object);
        }

        [TestMethod]
        public void IniciarAquecimento_ValidInput_ShouldReturnExpectedMessage()
        {
            int tempo = 60;
            int? potencia = 5;

            string result = _service.IniciarAquecimento(tempo, potencia);

            StringAssert.Contains(result, "Aquecendo por 1:00 (pot�ncia 5)");
            StringAssert.Contains(result, "Aquecimento conclu�do");
        }

        [TestMethod]
        public void IniciarAquecimento_InicioRapido_TempoZero_ShouldUse30sAndPot10()
        {
            int tempo = 0;
            int? potencia = null;

            string result = _service.IniciarAquecimento(tempo, potencia);

            StringAssert.Contains(result, "Aquecendo por 30s (pot�ncia 10)");
        }

        [TestMethod]
        public void IniciarAquecimento_AlreadyRunning_ShouldAdd30Seconds()
        {
            _service.IniciarAquecimento(30, 5);
            string result = _service.IniciarAquecimento(30, 5);

            StringAssert.Contains(result, "Tempo acrescido");
        }

        [TestMethod]
        public void IniciarAquecimento_AlreadyPaused_ShouldResume()
        {
            _service.IniciarAquecimento(30, 5);
            _service.PausarOuCancelar();

            string result = _service.IniciarAquecimento(30, 5);

            StringAssert.Contains(result, "Aquecimento retomado");
        }

        [TestMethod]
        public void IniciarAquecimento_InvalidTempo_ShouldReturnError()
        {
            string result = _service.IniciarAquecimento(200, 5);

            StringAssert.Contains(result, "Tempo inv�lido");
        }

        [TestMethod]
        public void IniciarAquecimento_InvalidPotencia_ShouldReturnError()
        {
            string result = _service.IniciarAquecimento(60, 12);

            StringAssert.Contains(result, "Pot�ncia inv�lida");
        }
    }
}
