using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestV1Vibe.Domain.Entities;

public class FilterRequestEntity
{
    public string Cliente { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public string RuaCruzamento { get; set; } = string.Empty;
}
