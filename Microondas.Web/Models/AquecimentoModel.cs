namespace Microondas.Web.Models
{
    public class AquecimentoModel
    {
        /// <summary>
        /// Tempo restante em segundos.
        /// </summary>
        public int TempoSegundos { get; set; }

        /// <summary>
        /// Potência de 1 a 10 (10 = padrão).
        /// </summary>
        public int Potencia { get; set; } = 10;

        /// <summary>
        /// Indica se está em aquecimento no momento.
        /// </summary>
        public bool EmExecucao { get; set; }

        /// <summary>
        /// Indica se o aquecimento está pausado.
        /// </summary>
        public bool EmPausa { get; set; }

        /// <summary>
        /// String final do aquecimento (\"...\" + \"Aquecimento concluído\").
        /// </summary>
        public string ResultadoAquecimento { get; set; }
    }
}
