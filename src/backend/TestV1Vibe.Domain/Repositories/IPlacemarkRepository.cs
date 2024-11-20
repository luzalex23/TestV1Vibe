using System.Xml.Linq;
using TestV1Vibe.Domain.Entities;

namespace TestV1Vibe.Domain.Repositories;

public interface IPlacemarkRepository
{
    Task<IEnumerable<Placemark>> BuscaPlacemark();
    Task<XDocument> ExportarKml(IEnumerable<Placemark> placemarks);
}
