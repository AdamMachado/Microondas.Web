using Microondas.Web.Models;

namespace Microondas.Web.Services.Repository
{
    public class ProgramasPreDefinidosRepository
    {
        public static readonly List<ProgramaPreDefinido> Programas = new List<ProgramaPreDefinido>
        {
            new ProgramaPreDefinido
            {
                Nome = "Pipoca",
                Alimento = "Pipoca (de micro-ondas)",
                TempoSegundos = 180, // 3 minutos
                Potencia = 7,
                StringDeAquecimento = "*", // Exemplo: Use asterisco
                Instrucoes = "Observar o barulho de estouros. Intervalo de 10s sem estouro => interrompa."
            },
            new ProgramaPreDefinido
            {
                Nome = "Leite",
                Alimento = "Leite",
                TempoSegundos = 300, // 5 minutos
                Potencia = 5,
                StringDeAquecimento = "#",
                Instrucoes = "Cuidado com aquecimento de líquidos. Pode causar fervura imediata."
            },
            new ProgramaPreDefinido
            {
                Nome = "Carnes de boi",
                Alimento = "Carne em pedaço ou fatias",
                TempoSegundos = 840, // 14 minutos
                Potencia = 4,
                StringDeAquecimento = "~",
                Instrucoes = "Interrompa na metade e vire para descongelar uniformemente."
            },
            new ProgramaPreDefinido
            {
                Nome = "Frango",
                Alimento = "Frango (qualquer corte)",
                TempoSegundos = 480, // 8 minutos
                Potencia = 7,
                StringDeAquecimento = "@",
                Instrucoes = "Interrompa na metade e vire para descongelar uniformemente."
            },
            new ProgramaPreDefinido
            {
                Nome = "Feijão",
                Alimento = "Feijão congelado",
                TempoSegundos = 480, // 8 minutos
                Potencia = 9,
                StringDeAquecimento = "&",
                Instrucoes = "Deixe o recipiente destampado. Cuidado ao retirar recipientes plásticos."
            }
        };
    }
}
