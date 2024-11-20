using Microsoft.AspNetCore.Mvc;
using TestV1Vibe.Application.DTOs;
using TestV1Vibe.Domain.Entities;
using TestV1Vibe.Domain.Repositories;

namespace TestV1Vibe.API.Controllers;

[ApiController]
[Route("api/placemarks")]
public class PlacemarkController : ControllerBase
{
    private readonly IPlacemarkRepository _repository;

    public PlacemarkController(IPlacemarkRepository repository)
    {
        _repository = repository;
    }

    // Endpoint 1: Listar todos os placemarks
    [HttpGet]
    public async Task<IActionResult> GetPlacemarks()
    {
        var placemarks = await _repository.BuscaPlacemark();

        if (!placemarks.Any())
            return NotFound("Nenhum placemark encontrado.");

        return Ok(placemarks);
    }

    // Endpoint 2: Exportar placemarks filtrados
    [HttpPost("export")]
    public async Task<IActionResult> ExportPlacemarks([FromBody] IEnumerable<PlacemarkDto> placemarks)
    {
        if (placemarks == null || !placemarks.Any())
            return BadRequest("A lista de placemarks para exportar está vazia.");

        // Converter DTO para entidade de domínio
        var domainPlacemarks = placemarks.Select(p => new Placemark
        {
            Nome = p.Nome,
            Cliente = p.Cliente,
            Bairro = p.Bairro,
            Coordenadas = p.Coordenadas,
            Descricao = p.Descricao,
            Data = p.Data,
            Imagens = p.Imagens
        });

        var kmlDocument = await _repository.ExportarKml(domainPlacemarks);

        // Retorna o KML como arquivo
        return File(
            System.Text.Encoding.UTF8.GetBytes(kmlDocument.ToString()),
            "application/vnd.google-earth.kml+xml",
            "FilteredPlacemarks.kml"
        );
    }

    // Endpoint 3: Listar valores únicos para filtros
    [HttpGet("filters")]
    public async Task<IActionResult> GetFilterValues()
    {
        var placemarks = await _repository.BuscaPlacemark();

        if (!placemarks.Any())
            return NotFound("Nenhum placemark encontrado para extrair filtros.");

        var uniqueFilters = new
        {
            Clientes = placemarks.Select(p => p.Cliente).Where(c => !string.IsNullOrEmpty(c)).Distinct(),
            Bairros = placemarks.Select(p => p.Bairro).Where(b => !string.IsNullOrEmpty(b)).Distinct()
        };

        return Ok(uniqueFilters);
    }
}
