namespace TestV1Vibe.Application.DTOs;

public class PlacemarkDto
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Coordenadas { get; set; } = string.Empty;
    public IEnumerable<string> Imagens { get; set; } = [];
}
