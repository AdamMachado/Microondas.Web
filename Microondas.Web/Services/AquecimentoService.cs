using Microondas.Web.Models;
using Microondas.Web.Services.Repository;
using System.Text;

namespace Microondas.Web.Services
{
    public class AquecimentoService
    {

        private static AquecimentoModel _estado = new AquecimentoModel();

    
        public string IniciarAquecimento(int tempo, int? potencia)
        {
            // 1) Se já está em execução e NÃO está pausado => soma +30 segundos
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                // Opcional: verificar se estouraria o limite de 120s
                int novoTempo = _estado.TempoSegundos + 30;
                if (novoTempo > 120)
                {
                    return "Não é possível ultrapassar o tempo máximo de 2 minutos.";
                }

                _estado.TempoSegundos = novoTempo;
                return $"Tempo acrescido! Agora faltam {FormatarTempo(_estado.TempoSegundos)}.";
            }
            // 2) Se está em execução e ESTÁ pausado => retoma do ponto onde parou
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                _estado.EmPausa = false;
                return $"Aquecimento retomado! Tempo restante: {FormatarTempo(_estado.TempoSegundos)}.";
            }
            // 3) Caso contrário, não está em execução => inicia do zero
            else
            {
                // a) Início rápido se tempo=0
                if (tempo == 0)
                    tempo = 30;

                // b) Validar tempo (1..120)
                if (tempo < 1 || tempo > 120)
                    return "Tempo inválido! O tempo deve ser entre 1 segundo e 2 minutos (120s).";

                // c) Validar potência (1..10), assumindo 10 se nulo
                int pot = potencia ?? 10;
                if (pot < 1 || pot > 10)
                    return "Potência inválida! Escolha entre 1 e 10.";

                // d) Configura o estado
                _estado.TempoSegundos = tempo;
                _estado.Potencia = pot;
                _estado.EmExecucao = true;
                _estado.EmPausa = false;

                // e) Gera string de aquecimento já "concluída" (simulação imediata)
                string aquecimentoStr = GerarStringAquecimento(_estado.TempoSegundos, _estado.Potencia);

                // f) Marca como finalizado (para Nível 1: tudo acontece de uma vez)
                _estado.EmExecucao = false;
                _estado.ResultadoAquecimento = aquecimentoStr;

                return $"Aquecendo por {FormatarTempo(_estado.TempoSegundos)} (potência) {pot}.\n" +
                       aquecimentoStr;
            }
        }

        /// <summary>
        /// Gera a string do aquecimento (pontinhos) + \"Aquecimento concluído\".
        /// Exemplo: 5s e potência 3 => \"... ... ... ... ...\"
        /// </summary>
        private string GerarStringAquecimento(int tempo, int potencia)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tempo; i++)
            {
                for (int j = 0; j < potencia; j++)
                {
                    sb.Append(".");
                }
                if (i < tempo - 1) sb.Append(" ");
            }
            sb.Append("\n Aquecimento concluído");
            return sb.ToString();
        }

        /// <summary>
        /// Formata tempo (1..120) => \"1:30\" se entre 60 e 99, ou \"2min 0s\" se >= 120, etc.
        /// </summary>
        private string FormatarTempo(int segundos)
        {
            if (segundos >= 60 && segundos < 100)
            {
                int m = segundos / 60;
                int s = segundos % 60;
                return $"{m}:{s:D2}\"; ex.: 90 => \"1:30";
            }
            else if (segundos >= 60)
            {
                int m = segundos / 60;
                int s = segundos % 60;
                return $"{m}min {s}s";
            }
            else
            {
                return $"{segundos}s";
            }
        }

        /// <summary>
        /// Pausa ou cancela o aquecimento, dependendo do estado.
        /// </summary>
        /// <returns>Mensagem de status.</returns>
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

        private void Resetar()
        {
            _estado = new AquecimentoModel();
        }

        public string IniciarProgramaPreDefinido(string nomePrograma)
        {
            var programa = ProgramasAquecimentoRepository.TodosProgramas
                .FirstOrDefault(p => p.Nome.Equals(nomePrograma, StringComparison.OrdinalIgnoreCase));
            if (programa == null)
                return "Programa não encontrado.";

          
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                return "Não é permitido acrescentar tempo em um programa pré-definido!";
            }
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                return "Não é permitido retomar outro programa pré-definido enquanto há um pausado.";
            }

           
            _estado.TempoSegundos = programa.TempoSegundos;
            _estado.Potencia = programa.Potencia;
            _estado.EmExecucao = true;
            _estado.EmPausa = false;


            string aquecimentoStr = GerarAquecimentoPersonalizado(_estado.TempoSegundos, _estado.Potencia, programa.CaractereAquecimento);

            _estado.EmExecucao = false;
            _estado.ResultadoAquecimento = aquecimentoStr;

            return $"Iniciando programa: {programa.Nome} ({programa.Alimento}).\n" +
                   $"Tempo: {FormatarTempo(programa.TempoSegundos)}, Potência: {programa.Potencia}\n" +
                   $"Instruções: {programa.Instrucoes}\n\n" +
                   aquecimentoStr;
        }
        private string GerarAquecimentoPersonalizado(int tempo, int potencia, string caractere)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tempo; i++)
            {
                for (int j = 0; j < potencia; j++)
                {
                    sb.Append(caractere);
                }
                if (i < tempo - 1) sb.Append(" ");
            }
            sb.Append("\n Aquecimento concluído");
            return sb.ToString();
        }
    }

}
