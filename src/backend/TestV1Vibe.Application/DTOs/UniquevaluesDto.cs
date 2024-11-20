namespace TestV1Vibe.Application.DTOs;

public class UniquevaluesDto
{
    public IEnumerable<string> Clientes { get; set; } = [];
    public IEnumerable<string> Situacoes { get; set; } = [];
    public IEnumerable<string> Bairros { get; set; } = [];
}
