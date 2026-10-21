using CSV_IMPORTER_Application;

Console.WriteLine("Caminho do CSV de Entrada : ");
string entrada = (Console.ReadLine() ?? "");

Console.WriteLine("Caminho do CSV de Saída : ");
string saida = (Console.ReadLine() ?? "");

try
{
    var useCase = new ImportarLigacoesUseCase();
    Resultado resultado = useCase.Executar(entrada, saida);

    Console.WriteLine("\n============RESULTADO==============");
    Console.WriteLine($"Total lido : {resultado.TotalLido}");
    Console.WriteLine($"Total aceito : {resultado.TotalAceito}");
    Console.WriteLine($"Total rejeitado : {resultado.Rejeicoes.Count}");

    foreach (var item in resultado.Rejeicoes)
    {
        Console.WriteLine(item);
    }

    Console.WriteLine($"Arquivo exportado em : {saida}");

}
catch (Exception ex)
{
    Console.WriteLine($"Erro : {ex.Message}");
}