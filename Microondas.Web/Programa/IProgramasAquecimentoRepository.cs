using System.Collections.Generic;

namespace Microondas.Web.Programa
{
    public interface IProgramasAquecimentoRepository
    {
        IEnumerable<ProgramaAquecimento> TodosProgramas { get; }
        string AdicionarCustomizado(ProgramaAquecimento novo);
        void CarregarCustomizados();
    }
}
