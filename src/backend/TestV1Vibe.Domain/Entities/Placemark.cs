namespace TestV1Vibe.Domain.Entities;

public class Placemark : FilterRequestEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Coordenadas { get; set; } = string.Empty;
    public IEnumerable<string> Imagens { get; set; } = [];

}
