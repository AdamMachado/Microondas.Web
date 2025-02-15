namespace Microondas.Web.Models
{
    public class AquecimentoModel
    {
        /// <summary>
        /// Tempo restante em segundos.
        /// </summary>
        public int TempoSegundos { get; set; }

        /// <summary>
        /// Pot�ncia de 1 a 10 (10 = padr�o).
        /// </summary>
        public int Potencia { get; set; } = 10;

        /// <summary>
        /// Indica se est� em aquecimento no momento.
        /// </summary>
        public bool EmExecucao { get; set; }

        /// <summary>
        /// Indica se o aquecimento est� pausado.
        /// </summary>
        public bool EmPausa { get; set; }

        /// <summary>
        /// String final do aquecimento (\"...\" + \"Aquecimento conclu�do\").
        /// </summary>
        public string ResultadoAquecimento { get; set; }
    }
}
