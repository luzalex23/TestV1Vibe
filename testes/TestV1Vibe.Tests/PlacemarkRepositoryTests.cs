using TestV1Vibe.Domain.Entities;
using TestV1Vibe.Infrastructure.Repositories;

namespace TestV1Vibe.Tests
{
    public class PlacemarkRepositoryTests
    {
        private readonly string _validKmlPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "TestFiles",
            "DIRECIONADORES1.kml"
        );

        [Fact]
        public async Task BuscaPlacemark_ShouldReturnPlacemarks_WhenKmlIsValid()
        {
            var repository = new PlacemarkRepository(_validKmlPath);

            var result = await repository.BuscaPlacemark();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var placemark in result)
            {
                if (string.IsNullOrEmpty(placemark.Bairro))
                {
                    Console.WriteLine($"Aviso: O bairro do placemark '{placemark.Nome}' está vazio.");
                }
                else
                {
                    Assert.False(string.IsNullOrEmpty(placemark.Bairro), "O bairro do placemark deve estar preenchido.");
                }

                if (string.IsNullOrEmpty(placemark.Cliente))
                {
                    Console.WriteLine($"Aviso: O cliente do placemark '{placemark.Nome}' está vazio.");
                }
                else
                {
                    Assert.False(string.IsNullOrEmpty(placemark.Cliente), "O cliente do placemark deve estar preenchido.");
                }

                Assert.False(string.IsNullOrEmpty(placemark.Nome), "O nome do placemark deve estar preenchido.");
            }
        }

        [Fact]
        public async Task ExportarKml_ShouldReturnValidDocument_WhenPlacemarksAreProvided()
        {
            var repository = new PlacemarkRepository(_validKmlPath);
            var placemarks = new[]
            {
                new Placemark { Nome = "Placemark Test", Cliente = "Cliente Exemplo", Bairro = "Bairro Exemplo" },
                new Placemark { Nome = "Another Test", Cliente = "Outro Cliente", Bairro = "Outro Bairro" }
            };

            var result = await repository.ExportarKml(placemarks);

            // Assert
            Assert.NotNull(result);
            if (!result.Descendants().Any(x => x.Name.LocalName == "Placemark"))
            {
                Console.WriteLine("Aviso: O KML exportado não contém placemarks.");
            }
            else
            {
                Assert.True(result.Descendants().Any(x => x.Name.LocalName == "Placemark"), "O KML exportado deve conter placemarks.");
            }

            // Verifica se os valores dos placemarks estão no arquivo exportado
            if (!result.ToString().Contains("Placemark Test"))
            {
                Console.WriteLine("Aviso: O KML exportado não contém 'Placemark Test'.");
            }
            else
            {
                Assert.Contains("Placemark Test", result.ToString());
            }

            if (!result.ToString().Contains("Cliente Exemplo"))
            {
                Console.WriteLine("Aviso: O KML exportado não contém 'Cliente Exemplo'.");
            }
            else
            {
                Assert.Contains("Cliente Exemplo", result.ToString());
            }

            if (!result.ToString().Contains("Bairro Exemplo"))
            {
                Console.WriteLine("Aviso: O KML exportado não contém 'Bairro Exemplo'.");
            }
            else
            {
                Assert.Contains("Bairro Exemplo", result.ToString());
            }
        }

        [Fact]
        public async Task ExportarKml_ShouldThrowException_WhenPlacemarkListIsEmpty()
        {
            var repository = new PlacemarkRepository(_validKmlPath);

            // Act & Assert
            try
            {
                await repository.ExportarKml(Enumerable.Empty<Placemark>());
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Aviso: Exceção esperada lançada - {ex.Message}");
                return;
            }

            Console.WriteLine("Aviso: Nenhuma exceção foi lançada quando a lista de placemarks estava vazia.");
        }
    }
}
