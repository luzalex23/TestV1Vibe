using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestV1Vibe.Application.DTOs;
using TestV1Vibe.Domain.Entities;
using TestV1Vibe.Domain.Repositories;

namespace TestV1Vibe.Api.Controllers;

[ApiController]
[Route("api/placemarks")]
public class PlacemarkController : ControllerBase
{
    private readonly IPlacemarkRepository _placemarkRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<FilterRequestEntityDto> _filterValidator;

    public PlacemarkController(IPlacemarkRepository placemarkRepository, IMapper mapper, IValidator<FilterRequestEntityDto> filterValidator)
    {
        _placemarkRepository = placemarkRepository;
        _mapper = mapper;
        _filterValidator = filterValidator;
    }

    [HttpPost("export")]
    public async Task<IActionResult> ExportKml([FromBody] FilterRequestEntityDto filters)
    {
        var validationResult = _filterValidator.Validate(filters);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var placemarks = await _placemarkRepository.BuscaPlacemark();
        var filteredPlacemarks = ApplyFilters(placemarks, filters);

        var kmlDocument = await _placemarkRepository.ExportarKml(_mapper.Map<IEnumerable<Placemark>>(filteredPlacemarks));

        return File(System.Text.Encoding.UTF8.GetBytes(kmlDocument.ToString()), "application/vnd.google-earth.kml+xml", "filtered.kml");
    }

    [HttpGet]
    public async Task<IActionResult> GetPlacemarks([FromQuery] FilterRequestEntityDto filters)
    {
        var validationResult = _filterValidator.Validate(filters);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var placemarks = await _placemarkRepository.BuscaPlacemark();
        var filteredPlacemarks = ApplyFilters(placemarks, filters);

        return Ok(filteredPlacemarks);
    }

    [HttpGet("filters")]
    public async Task<IActionResult> GetAvailableFilters()
    {
        var placemarks = await _placemarkRepository.BuscaPlacemark();
        var uniqueFilters = new UniquevaluesDto
        {
            Clientes = placemarks.Select(p => p.Cliente).Distinct(),
            Situacoes = placemarks.Select(p => p.Situacao).Distinct(),
            Bairros = placemarks.Select(p => p.Bairro).Distinct()
        };

        return Ok(uniqueFilters);
    }

    private IEnumerable<PlacemarkDto> ApplyFilters(IEnumerable<Placemark> placemarks, FilterRequestEntityDto filters)
    {
        return placemarks
            .Where(p => string.IsNullOrEmpty(filters.Cliente) || p.Cliente == filters.Cliente)
            .Where(p => string.IsNullOrEmpty(filters.Situacao) || p.Situacao == filters.Situacao)
            .Where(p => string.IsNullOrEmpty(filters.Bairro) || p.Bairro == filters.Bairro)
            .Where(p => string.IsNullOrEmpty(filters.Referencia) || p.Referencia.Contains(filters.Referencia))
            .Where(p => string.IsNullOrEmpty(filters.RuaCruzamento) || p.RuaCruzamento.Contains(filters.RuaCruzamento))
            .Select(p => _mapper.Map<PlacemarkDto>(p));
    }
}
