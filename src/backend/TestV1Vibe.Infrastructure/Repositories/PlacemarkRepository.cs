using System.Xml.Linq;
using TestV1Vibe.Domain.Entities;
using TestV1Vibe.Domain.Repositories;

namespace TestV1Vibe.Infrastructure.Repositories;

public class PlacemarkRepository : IPlacemarkRepository
{
    private readonly string _filePath;
    public PlacemarkRepository(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<IEnumerable<Placemark>> BuscaPlacemark()
    {
        XNamespace ns = "http://www.opengis.net/kml/2.2";
        var document = XDocument.Load(_filePath);

        var placemarks = document.Descendants(ns + "Placemark")
            .Select(p => new Placemark
            {
                Nome = (string)p.Element(ns + "name") ?? string.Empty,
                Descricao = (string)p.Element(ns + "description") ?? string.Empty,
                Cliente = (string)p.Descendants(ns + "Data")
                                   .FirstOrDefault(d => (string)d.Attribute("name") == "CLIENTE")
                                   ?.Element(ns + "value") ?? string.Empty,
                Bairro = (string)p.Descendants(ns + "Data")
                                   .FirstOrDefault(d => (string)d.Attribute("name") == "BAIRRO")
                                   ?.Element(ns + "value") ?? string.Empty,
                Coordenadas = (string)p.Descendants(ns + "coordinates").FirstOrDefault() ?? string.Empty,
                Data = (string)p.Descendants(ns + "Data")
                                .FirstOrDefault(d => (string)d.Attribute("name") == "DATA")
                                ?.Element(ns + "value") ?? string.Empty,
            })
            // Filtra placemarks inválidos
            .Where(p => !string.IsNullOrEmpty(p.Cliente) && !string.IsNullOrEmpty(p.Bairro))
            .ToList();

        return placemarks;
    }




    public async Task<XDocument> ExportarKml(IEnumerable<Placemark> placemarks)
    {
        if (placemarks == null || !placemarks.Any())
            throw new ArgumentException("A lista de placemarks não pode estar vazia.");

        XNamespace ns = "http://www.opengis.net/kml/2.2";

        var kmlDoc = new XDocument(
            new XElement(ns + "kml",
                new XElement(ns + "Document",
                    placemarks.Select(p => new XElement(ns + "Placemark",
                        new XElement(ns + "name", p.Nome),
                        new XElement(ns + "ExtendedData",
                            new XElement(ns + "Data", new XAttribute("name", "CLIENTE"),
                                new XElement(ns + "value", p.Cliente)),
                            new XElement(ns + "Data", new XAttribute("name", "BAIRRO"),
                                new XElement(ns + "value", p.Bairro))
                        ),
                        new XElement(ns + "Point",
                            new XElement(ns + "coordinates", p.Coordenadas)
                        )
                    ))
                )
            )
        );

        return kmlDoc;
    }

}
