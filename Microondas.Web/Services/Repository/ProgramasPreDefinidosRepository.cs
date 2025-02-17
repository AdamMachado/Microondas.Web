using Microondas.Web.Models;
using System.Text.Json;

namespace Microondas.Web.Services.Repository
{
    public static class ProgramasAquecimentoRepository
    {
        private static readonly List<ProgramaAquecimento> _preDefinidos = new List<ProgramaAquecimento>
        {
            new ProgramaAquecimento
            {
                Nome = "Pipoca",
                Alimento = "Pipoca (de micro-ondas)",
                TempoSegundos = 180,
                Potencia = 7,
                CaractereAquecimento = "*",
                Instrucoes = "Se houver >10s sem estouro, interrompa.",
                IsCustom = false
            },
            new ProgramaAquecimento
            {
                Nome = "Leite",
                Alimento = "Leite",
                TempoSegundos = 300,
                Potencia = 5,
                CaractereAquecimento = "#",
                Instrucoes = "Cuidado com líquidos.",
                IsCustom = false
            },
            new ProgramaAquecimento
            {
                Nome = "Carnes de boi",
                Alimento = "Carne em pedaço ou fatias",
                TempoSegundos = 840,
                Potencia = 4,
                CaractereAquecimento = "~",
                Instrucoes = "Interrompa na metade e vire a carne.",
                IsCustom = false
            },
            new ProgramaAquecimento
            {
                Nome = "Frango",
                Alimento = "Frango (qualquer corte)",
                TempoSegundos = 480,
                Potencia = 7,
                CaractereAquecimento = "@",
                Instrucoes = "Interrompa na metade e vire o frango.",
                IsCustom = false
            },
            new ProgramaAquecimento
            {
                Nome = "Feijão",
                Alimento = "Feijão congelado",
                TempoSegundos = 480,
                Potencia = 9,
                CaractereAquecimento = "&",
                Instrucoes = "Deixe o recipiente destampado.",
                IsCustom = false
            }
        };

        private static List<ProgramaAquecimento> _customizados = new List<ProgramaAquecimento>();

        private static string _jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "programasCustom.json");

        static ProgramasAquecimentoRepository()
        {
            CarregarCustomizados();
        }

        public static void CarregarCustomizados()
        {
            if (File.Exists(_jsonPath))
            {
                string json = File.ReadAllText(_jsonPath);
                var lista = JsonSerializer.Deserialize<List<ProgramaAquecimento>>(json);
                if (lista != null)
                    _customizados = lista;
            }
        }

        private static void SalvarCustomizados()
        {
            string json = JsonSerializer.Serialize(_customizados, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonPath, json);
        }

        public static IEnumerable<ProgramaAquecimento> TodosProgramas
        {
            get
            {
                return _preDefinidos.Concat(_customizados);
            }
        }

        /// <summary>
        /// Adiciona um programa customizado, validando duplicidade de caractere.
        /// </summary>
        public static string AdicionarCustomizado(ProgramaAquecimento novo)
        {
            if (string.IsNullOrWhiteSpace(novo.Nome) ||
                string.IsNullOrWhiteSpace(novo.Alimento) ||
                novo.TempoSegundos < 1 || novo.TempoSegundos > 120 ||
                novo.Potencia < 1 || novo.Potencia > 10 ||
                string.IsNullOrWhiteSpace(novo.CaractereAquecimento))
            {
                return "Campos obrigatórios inválidos!";
            }
            if (novo.CaractereAquecimento == ".")
            {
                return "O caractere de aquecimento não pode ser '.'!";
            }
            if (TodosProgramas.Any(p => p.CaractereAquecimento == novo.CaractereAquecimento))
            {
                return "Caractere de aquecimento já está em uso em outro programa!";
            }

            novo.IsCustom = true;

            _customizados.Add(novo);
            SalvarCustomizados();

            return "Programa customizado cadastrado com sucesso!";
        }
    }
}
