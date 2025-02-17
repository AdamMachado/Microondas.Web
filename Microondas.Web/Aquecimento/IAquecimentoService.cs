
namespace Microondas.Web.Aquecimento
{
    public interface IAquecimentoService
    {
        string IniciarAquecimento(int tempo, int? potencia);
        string PausarOuCancelar();
        string IniciarProgramaPreDefinido(string nomePrograma);
    }
}
