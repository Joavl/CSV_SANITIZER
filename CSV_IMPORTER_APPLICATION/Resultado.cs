using CSV_IMPORTER.Domain;
using CSV_IMPORTER.Infrastructure;

namespace CSV_IMPORTER_Application
{
    public class Resultado
    {
        public int TotalLido { get; set; }
        public int TotalAceito { get; set; }
        public List<string> Rejeicoes { get; set; } = new List<string>();

    }

    public class ImportarLigacoesUseCase
    {
        public Resultado Executar(string entrada, string saida)
        {
            var arquivo = new ArquivoCsv();
            var resultado = new Resultado();
            var ligacoesValidas = new List<Ligacao>();

            string[] linhas = arquivo.LerCsv(entrada);

            for (int i = 1; i < linhas.Length; i++)
            {
                resultado.TotalLido++;

                string[] colunas = linhas[i].Split(',');

                if (colunas.Length != 5)
                {
                    resultado.Rejeicoes.Add($"Linha {i + 1} : quantidade de colunas diferentes de 5 ");
                    continue;
                }

                string? erro = ValidadorLigacao.Validador(
                    colunas[0],
                    colunas[1],
                    colunas[2],
                    colunas[3],
                    colunas[4],
                    out Ligacao ligacao);

                if (erro != null)
                {
                    resultado.Rejeicoes.Add($"Linha {i + 1} : {erro}");
                }

                else
                {
                    ligacoesValidas.Add(ligacao);
                }
            }

            arquivo.Exportar(saida, ligacoesValidas);

            resultado.TotalAceito = ligacoesValidas.Count;

            return resultado; // 21999999999,22999999998,21-10-2026 11:54,120,Efetuada

        }
    }
}



