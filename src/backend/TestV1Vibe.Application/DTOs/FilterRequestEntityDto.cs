﻿namespace TestV1Vibe.Application.DTOs;

public class FilterRequestEntityDto
{
    public string Cliente { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public string RuaCruzamento { get; set; } = string.Empty;
}
