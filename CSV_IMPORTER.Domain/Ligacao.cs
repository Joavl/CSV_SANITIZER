using System.Globalization;

namespace CSV_IMPORTER.Domain
{
    public class Ligacao
    {
        public string Origem { get; set; } = "";
        public string Destino { get; set; } = "";
        public DateTime DataHora { get; set; }
        public int DuracaoEmSegundos { get; set; }
        public TipoLigacao Tipo { get; set; }
    }

    public enum TipoLigacao
    {
        Recebida,
        Efetuada,
        Perdida
    }

    public static class ValidadorLigacao
    {
        public static string? Validador(
            string origem, string destino, string dataHora, string duracao, string tipo,
            out Ligacao ligacao)
        {
            ligacao = new Ligacao();
            origem = origem.Trim();
            destino = destino.Trim();

            if (string.IsNullOrWhiteSpace(origem)) return "Origem não pode ser vazia";
            if (string.IsNullOrWhiteSpace(destino)) return "Destino não pode ser vazio";
            if (!IsValidCellphone(origem)) return "Origem inválida. Informe de 8 a 15 dígitos";
            if (!IsValidCellphone(destino)) return "Destino inválido. Informe de 8 a 15 dígitos";
            if (origem == destino) return "O destino deve ser diferente da origem";

            if (!DateTime.TryParseExact(dataHora.Trim(), "dd-MM-yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                return "Data/hora inválida. Use dd-MM-yyyy HH:mm:ss";
            if (data > DateTime.Now) return "Data/hora no futuro";

            if (!int.TryParse(duracao.Trim(), out int segundos)) return "Duração deve ser um inteiro em segundos";
            if (segundos < 0) return "Duração não pode ser negativa";

            if (!Enum.TryParse(tipo.Trim(), true, out TipoLigacao tipoConvertido)
                || !Enum.IsDefined(tipoConvertido))
                return "Tipo inválido. Use: Recebida, Efetuada ou Perdida";

            ligacao = new Ligacao
            {
                Origem = origem,
                Destino = destino,
                DataHora = data,
                DuracaoEmSegundos = segundos,
                Tipo = tipoConvertido
            };
            return null;
        }

        public static bool IsValidCellphone(string telefone)
        {
            return telefone.Length >= 8
                && telefone.Length <= 15
                && telefone.All(char.IsAsciiDigit);
        }
    }
}