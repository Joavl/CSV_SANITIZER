using CSV_IMPORTER.Domain;
using System.Globalization;
using System.Text;

namespace CSV_IMPORTER.Infrastructure
{
    public class ArquivoCsv
    {
        public string[] LerCsv(string path)
        {
            string[] linhas = File.ReadAllLines(path, Encoding.UTF8);

            if (linhas.Length == 0)
            {
                throw new InvalidDataException("O arquivo CSV está vazio.");
            }

            string cabecalhosEsperados = "origem,destino,data_hora,duracao_segundos,tipo";

            if (!string.Equals(linhas[0].Trim(), cabecalhosEsperados, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("Cabeçalho inválido. Use :" + cabecalhosEsperados);
            }

            return linhas;
        }
        public void Exportar(string path, List<Ligacao> ligacoes)
        {
            List<string> linhasPlanilha = new List<string>();

            linhasPlanilha.Add(
                "EntidadeOrigem,EntidadeDestino,DataHora,DuracaoSegundos,TipoLigacao,Direcao");

            foreach (Ligacao ligacao in ligacoes)
            {
                string dataHora = ligacao.DataHora.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                string duracao = ligacao.DuracaoEmSegundos.ToString(CultureInfo.InvariantCulture);
                string direcao = ligacao.Origem + " -> " + ligacao.Destino;
                string linha =
                    ligacao.Origem + "," +
                    ligacao.Destino + "," +
                    dataHora + "," +
                    duracao + "," +
                    ligacao.Tipo + "," +
                    direcao;

                linhasPlanilha.Add(linha);
            }

            File.WriteAllLines(path, linhasPlanilha, new UTF8Encoding(true));
        }
    }
}
