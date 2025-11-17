using Xunit;
using Moq;
using FluentAssertions;
using Restaurant.Pages.Clientes;
using Restaurant.Entities;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Repositories;

namespace RestaurantTests.Pages.Clientes
{
    public class ClienteIndexPageTests
    {
        [Fact]
        public async Task OnGetAsync_DeberiaListarSoloActivos()
        {
            var repo = new Mock<IGenericRepository<Cliente>>();

            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Cliente>
            {
                new Cliente { Id = 1, Nombre="A", Estado=1 },
                new Cliente { Id = 2, Nombre="B", Estado=0 }
            });

            var page = new ClientesModel(repo.Object);

            await page.OnGetAsync();

            page.Clientes.Should().HaveCount(1);
        }

        [Fact]
        public async Task OnPostEliminarAsync_DeberiaHacerSoftDelete()
        {
            var repo = new Mock<IGenericRepository<Cliente>>();

            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Cliente { Id = 1, Estado = 1 });

            var page = new ClientesModel(repo.Object);

            var result = await page.OnPostEliminarAsync(1);

            repo.Verify(r => r.SoftDeleteAsync(It.IsAny<Cliente>()), Times.Once);
            result.Should().BeOfType<RedirectToPageResult>();
        }
    }
}