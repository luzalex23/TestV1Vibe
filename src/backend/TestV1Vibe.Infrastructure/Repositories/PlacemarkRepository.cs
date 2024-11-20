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
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"O arquivo KML no caminho {_filePath} não foi encontrado.");

        try
        {
            XNamespace nameSpace = "http://www.opengis.net/kml/2.2";
            var document = XDocument.Load(_filePath);

            var kmlPercorrido = document.Descendants(nameSpace + "Placemark")
                .Select(p => new Placemark
                {
                    Nome = (string)p.Element(nameSpace + "name")?.Value,
                    Descricao = (string)p.Element(nameSpace + "description")?.Value,
                    Cliente = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "CLIENTE")?.Element(nameSpace + "value")?.Value,
                    Situacao = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "SITUAÇÃO")?.Element(nameSpace + "value")?.Value,
                    Bairro = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "BAIRRO")?.Element(nameSpace + "value")?.Value,
                    Referencia = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "REFERENCIA")?.Element(nameSpace + "value")?.Value,
                    RuaCruzamento = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "RUA/CRUZAMENTO")?.Element(nameSpace + "value")?.Value,
                    Data = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "DATA")?.Element(nameSpace + "value")?.Value,
                    Coordenadas = (string)p.Descendants(nameSpace + "Data").FirstOrDefault(d => (string)d.Attribute("name") == "COORDENADAS")?.Element(nameSpace + "value")?.Value,
                    Imagens = p.Descendants(nameSpace + "Data").Where(d => (string)d.Attribute("name") == "gx_media_links")
                                .Select(d => (string)d.Element(nameSpace + "value")?.Value)
                });

            return kmlPercorrido;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao carregar o arquivo KML: {ex.Message}", ex);
        }
    }


    public async Task<XDocument> ExportarKml(IEnumerable<Placemark> placemarks)
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"O arquivo KML no caminho {_filePath} não foi encontrado.");

        XNamespace ns = "http://www.opengis.net/kml/2.2";
        var document = XDocument.Load(_filePath);

        try
        {
            var kmlDoc = new XDocument(
                new XElement(ns + "kml",
                    new XElement(ns + "Document",
                        placemarks.Select(p =>
                        {
                            var originalPlacemark = document.Descendants(ns + "Placemark")
                                .FirstOrDefault(x => x.Descendants(ns + "Data")
                                    .Any(d => (string)d.Attribute("name") == "CLIENTE" &&
                                              (string)d.Element(ns + "value")?.Value?.ToUpper() == p.Cliente.ToUpper()));

                            if (originalPlacemark == null)
                                return null;

                            return new XElement(ns + "Placemark",
                                new XElement(ns + "name", p.Nome),
                                new XElement(ns + "description", p.Descricao),
                                new XElement(ns + "Data",
                                    new XAttribute("name", "CLIENTE"),
                                    new XElement(ns + "value", p.Cliente)
                                ),
                                new XElement(ns + "Data",
                                    new XAttribute("name", "BAIRRO"),
                                    new XElement(ns + "value", p.Bairro)
                                ),
                                p.Imagens.Select(img =>
                                    new XElement(ns + "Data",
                                        new XAttribute("name", "gx_media_links"),
                                        new XElement(ns + "value", img)
                                    ))
                            );
                        })
                        .Where(x => x != null) 
                    )
                )
            );

            return kmlDoc;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao exportar o KML: {ex.Message}", ex);
        }
    }

}
