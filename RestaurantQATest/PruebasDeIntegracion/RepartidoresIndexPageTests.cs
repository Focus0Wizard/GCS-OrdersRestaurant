using Xunit;
using Moq;
using FluentAssertions;
using Restaurant.Pages.Repartidores;
using Restaurant.Entities;
using Restaurant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantTests.Pages.Repartidores
{
    public class RepartidoresIndexPageTests
    {
        [Fact]
        public async Task OnGetAsync_DeberiaFiltrarRepartidoresActivos()
        {
            var repo = new Mock<IGenericRepository<Repartidore>>();

            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Repartidore>
            {
                new Repartidore { Id=1, Nombre="A", Estado=1 },
                new Repartidore { Id=2, Nombre="B", Estado=0 }
            });

            var page = new RepartidoresModel(repo.Object);

            await page.OnGetAsync();

            page.Repartidores.Should().HaveCount(1);
        }
    }
}