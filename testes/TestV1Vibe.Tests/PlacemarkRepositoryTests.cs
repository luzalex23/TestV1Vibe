using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestV1Vibe.Domain.Entities;
using TestV1Vibe.Infrastructure.Repositories;
using Xunit;

namespace TestV1Vibe.Tests.Infrastructure
{
    public class PlacemarkRepositoryTests
    {
        // Caminho para o arquivo KML fornecido
        private readonly string _validKmlPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "TestFiles",
                    "DIRECIONADORES1.kml"
                );
        [Fact]
        public async Task BuscaPlacemark_ShouldReturnPlacemarks_WhenKmlIsValid()
        {
            // Arrange: Repositório com arquivo KML válido
            var repository = new PlacemarkRepository(_validKmlPath);

            // Act: Busca os placemarks no arquivo
            var result = await repository.BuscaPlacemark();

            // Assert
            Assert.NotNull(result); // Verifica que o resultado não é nulo
            Assert.NotEmpty(result); // Garante que há placemarks retornados
            Assert.All(result, placemark =>
            {
                Assert.False(string.IsNullOrEmpty(placemark.Nome), "O nome do placemark deve estar preenchido.");
                Assert.False(string.IsNullOrEmpty(placemark.Coordenadas), "As coordenadas do placemark devem estar preenchidas.");

                // Aceite casos em que Bairro ou Cliente podem estar ausentes
                if (!string.IsNullOrEmpty(placemark.Bairro))
                {
                    Assert.False(string.IsNullOrEmpty(placemark.Cliente), "Se o Bairro estiver preenchido, Cliente também deve estar.");
                }
            });
        }

        [Fact]
        public async Task ExportarKml_ShouldReturnValidDocument_WhenPlacemarksAreProvided()
        {
            // Arrange: Configura os placemarks para exportar
            var repository = new PlacemarkRepository(_validKmlPath);
            var placemarks = new[]
            {
                new Placemark { Nome = "Ponto 1", Cliente = "GRADE", Bairro = "13 DE JULHO", Coordenadas = "-10.9313079,-37.0472967,0" },
                new Placemark { Nome = "Ponto 2", Cliente = "Outro Cliente", Bairro = "Outro Bairro", Coordenadas = "-10.000000,-37.000000,0" }
            };

            // Act: Exporta o KML baseado nos placemarks fornecidos
            var result = await repository.ExportarKml(placemarks);

            // Assert
            Assert.NotNull(result); // Garante que o documento não é nulo
            Assert.True(result.Descendants().Any(x => x.Name.LocalName == "Placemark"), "O KML exportado deve conter placemarks.");

            // Verifica se os valores dos placemarks estão no arquivo exportado
            Assert.Contains("Ponto 1", result.ToString());
            Assert.Contains("GRADE", result.ToString());
            Assert.Contains("13 DE JULHO", result.ToString());
        }

        [Fact]
        public async Task ExportarKml_ShouldThrowException_WhenPlacemarkListIsEmpty()
        {
            // Arrange: Repositório com lista de placemarks vazia
            var repository = new PlacemarkRepository(_validKmlPath);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repository.ExportarKml(Enumerable.Empty<Placemark>()));
        }
    }
}
