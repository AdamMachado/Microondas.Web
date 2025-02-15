namespace Microondas.Web.Models
{
    public class ProgramaAquecimento
    {
        public string Nome { get; set; }
        public string Alimento { get; set; }
        public int TempoSegundos { get; set; }
        public int Potencia { get; set; }
        /// <summary>
        /// Caractere (ou sequência) utilizada na string de aquecimento (diferente de '.').
        /// </summary>
        public string CaractereAquecimento { get; set; }
        /// <summary>
        /// Instruções adicionais (opcional).
        /// </summary>
        public string Instrucoes { get; set; }
        /// <summary>
        /// Indica se é um programa customizado ou não.
        /// </summary>
        public bool IsCustom { get; set; }
    }
}
