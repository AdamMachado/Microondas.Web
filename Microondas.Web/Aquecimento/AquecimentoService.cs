using Microondas.Web.Programa;
using System.Text;

namespace Microondas.Web.Aquecimento
{
    public class AquecimentoService : IAquecimentoService
    {
        private readonly IProgramasAquecimentoRepository _programasRepository;
        private readonly List<ProgramaAquecimento> _programasCustomizados = new();
        private AquecimentoModel _estado = new();

        public AquecimentoService(IProgramasAquecimentoRepository programasRepository)
        {
            _programasRepository = programasRepository;
        }

        public string IniciarAquecimento(int tempo, int? potencia)
        {
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                int novoTempo = _estado.TempoSegundos + 30;
                if (novoTempo > 120)
                    return "Não é possível ultrapassar o tempo máximo de 2 minutos.";

                _estado.TempoSegundos = novoTempo;
                return $"Tempo acrescido! Agora faltam {FormatarTempo(_estado.TempoSegundos)}.";
            }
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                _estado.EmPausa = false;
                return $"Aquecimento retomado! Tempo restante: {FormatarTempo(_estado.TempoSegundos)}.";
            }
            else
            {
                if (tempo == 0) tempo = 30;
                if (tempo < 1 || tempo > 120) return "Tempo inválido! O tempo deve ser entre 1 e 2 minutos.";

                int pot = potencia ?? 10;
                if (pot < 1 || pot > 10) return "Potência inválida! Escolha entre 1 e 10.";

                _estado.TempoSegundos = tempo;
                _estado.Potencia = pot;
                _estado.EmExecucao = true;
                _estado.EmPausa = false;

                string aquecimentoStr = GerarStringAquecimento(_estado.TempoSegundos, _estado.Potencia);

                return $"Aquecendo por {FormatarTempo(_estado.TempoSegundos)} (potência {pot}).\n{aquecimentoStr}";
            }
        }

        public string PausarOuCancelar()
        {
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                _estado.EmPausa = true;
                return $"Aquecimento pausado. Tempo restante: {FormatarTempo(_estado.TempoSegundos)}";
            }
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                Resetar();
                return "Aquecimento cancelado e dados limpos.";
            }
            else
            {
                Resetar();
                return "Nenhum aquecimento em andamento. Limpeza concluída.";
            }
        }

        public string IniciarProgramaPreDefinido(string nomePrograma)
        {
            var programa = TodosProgramas.FirstOrDefault(p => p.Nome.Equals(nomePrograma, StringComparison.OrdinalIgnoreCase));
            if (programa == null)
                return "Programa não encontrado.";

            if (_estado.EmExecucao && !_estado.EmPausa)
                return "Não é permitido acrescentar tempo em um programa pré-definido!";

            if (_estado.EmExecucao && _estado.EmPausa)
                return "Não é permitido retomar outro programa pré-definido enquanto há um pausado.";

            _estado.TempoSegundos = programa.TempoSegundos;
            _estado.Potencia = programa.Potencia;
            _estado.EmExecucao = true;
            _estado.EmPausa = false;

            string aquecimentoStr = GerarAquecimentoPersonalizado(_estado.TempoSegundos, _estado.Potencia, programa.CaractereAquecimento);
            _estado.EmExecucao = false;
            _estado.ResultadoAquecimento = aquecimentoStr;

            return $"Iniciando programa: {programa.Nome} ({programa.Alimento}).\n" +
                   $"Tempo: {FormatarTempo(programa.TempoSegundos)}, Potência: {programa.Potencia}\n" +
                   $"Instruções: {programa.Instrucoes}\n\n{aquecimentoStr}";
        }

        public string AdicionarCustomizado(ProgramaAquecimento programa)
        {
            if (string.IsNullOrWhiteSpace(programa.Nome) || string.IsNullOrWhiteSpace(programa.Alimento))
                return "Nome e alimento são obrigatórios.";

            if (programa.TempoSegundos < 1 || programa.TempoSegundos > 120)
                return "Tempo deve estar entre 1 e 120 segundos.";

            if (programa.Potencia < 1 || programa.Potencia > 10)
                return "Potência deve estar entre 1 e 10.";

            if (string.IsNullOrWhiteSpace(programa.CaractereAquecimento) || programa.CaractereAquecimento == ".")
                return "Caractere de aquecimento inválido.";

            if (TodosProgramas.Any(p => p.CaractereAquecimento == programa.CaractereAquecimento))
                return "Caractere já em uso em outro programa!";

            programa.IsCustom = true;
            _programasCustomizados.Add(programa);

            return "Programa customizado adicionado com sucesso!";
        }

        public List<ProgramaAquecimento> TodosProgramas => _programasRepository.TodosProgramas.Concat(_programasCustomizados).ToList();

        private void Resetar() => _estado = new AquecimentoModel();

        private string GerarStringAquecimento(int tempo, int potencia)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tempo; i++)
            {
                sb.Append('.', potencia);
                if (i < tempo - 1) sb.Append(' ');
            }
            sb.Append("\nAquecimento concluído");
            return sb.ToString();
        }

        private string FormatarTempo(int segundos)
        {
            if (segundos >= 60 && segundos < 100)
                return $"{segundos / 60}:{segundos % 60:D2}";
            if (segundos >= 100)
                return $"{segundos / 60}min {segundos % 60}s";
            return $"{segundos}s";
        }

        private string GerarAquecimentoPersonalizado(int tempo, int potencia, string caractere)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tempo; i++)
            {
                sb.Append(caractere[0], potencia);
                if (i < tempo - 1) sb.Append(' ');
            }
            sb.Append("\nAquecimento concluído");
            return sb.ToString();
        }

    }
}
