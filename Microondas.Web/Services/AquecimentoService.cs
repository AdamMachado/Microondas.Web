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
            // Se já está em execução e não está pausado => Acrescenta 30s
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                _estado.TempoSegundos += 30;
                return $"Tempo acrescido. Tempo atual: {FormatarTempo(_estado.TempoSegundos)}.";
            }
            // Se já está em execução e ESTÁ pausado => Retoma
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                _estado.EmPausa = false;
                return $"Aquecimento retomado. Tempo restante: {FormatarTempo(_estado.TempoSegundos)}.";
            }
            else
            {
                // Se NÃO está em execução, vamos configurar do zero
                if (tempo == 0) tempo = 30; // Início rápido

                // 1) Validar tempo (1 a 120)
                if (tempo < 1 || tempo > 120)
                    return "Tempo inválido! Deve ser entre 1 e 120 segundos.";

                // 2) Validar potência (1 a 10, ou default 10)
                int pot = potencia ?? 10;
                if (pot < 1 || pot > 10)
                    return "Potência inválida! (1 a 10).";

                // Configura estado
                _estado.TempoSegundos = tempo;
                _estado.Potencia = pot;
                _estado.EmExecucao = true;
                _estado.EmPausa = false;

                // Gera string de aquecimento
                string aquecimentoStr = GerarStringAquecimento(tempo, pot);

                // Marca como finalizado (simulamos tudo de uma vez)
                _estado.EmExecucao = false;
                _estado.ResultadoAquecimento = aquecimentoStr;

                // Retorna mensagem
                return $"Aquecer por {FormatarTempo(tempo)} (potência {pot}).\\n" +
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
                // Separador entre cada segundo (opcional)
                if (i < tempo - 1) sb.Append(" ");
            }
            sb.Append("\\nAquecimento concluído");
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
                return $"{m}:{s:D2}\"; // ex.: 90 => \"1:30";
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
            // Se está em execução e NÃO está pausado => PAUSA
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                _estado.EmPausa = true;
                return $"Aquecimento pausado. Tempo restante: {FormatarTempo(_estado.TempoSegundos)}";
            }
            // Se está em execução e JÁ está pausado => CANCELA
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                Resetar();
                return "Aquecimento cancelado e dados limpos.";
            }
            else
            {
                // Se não estava nem iniciado, zera do mesmo jeito
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
            // 1) Localiza o programa
            var programa = ProgramasPreDefinidosRepository.Programas
                             .Find(p => p.Nome.Equals(nomePrograma, StringComparison.OrdinalIgnoreCase));
            if (programa == null)
            {
                return "Programa inexistente.";
            }

            // 2) Se já estiver em execução => ver se é permitido
            //   (Requisito (e) diz que para programas pré-definidos não permitimos acréscimo de tempo)
            if (_estado.EmExecucao && !_estado.EmPausa)
            {
                return "Não é permitido acrescentar tempo em um programa pré-definido!";
            }
            else if (_estado.EmExecucao && _estado.EmPausa)
            {
                return "Não é permitido retomar outro programa pré-definido enquanto há um pausado.";
            }

            // 3) Configura o estado do aquecimento usando dados do programa
            _estado.TempoSegundos = programa.TempoSegundos;
            _estado.Potencia = programa.Potencia;
            _estado.EmExecucao = true;
            _estado.EmPausa = false;

            // 4) Gerar a string de aquecimento usando o caracter do programa
            //    Ao final, marca como concluído (ou, se preferir, simula o \"loop\")

            string aquecimentoStr = GerarAquecimentoPersonalizado(_estado.TempoSegundos, _estado.Potencia, programa.StringDeAquecimento);

            _estado.EmExecucao = false;
            _estado.ResultadoAquecimento = aquecimentoStr;

            // 5) Retorna mensagem final
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
                if (i < tempo - 1) sb.Append(" "); // separador
            }
            sb.Append("\nAquecimento concluído");
            return sb.ToString();
        }
    }

}
