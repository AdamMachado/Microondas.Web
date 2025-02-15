﻿namespace Microondas.Web.Models
{
    public class ProgramaPreDefinido
    {
        public string Nome { get; set; }
        public string Alimento { get; set; }
        public int TempoSegundos { get; set; }
        public int Potencia { get; set; }
        public string StringDeAquecimento { get; set; } // caractere(s) usados no lugar de "."
        public string Instrucoes { get; set; }
    }
}
