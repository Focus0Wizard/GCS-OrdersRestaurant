using Xunit;
using FluentAssertions;
using Moq;
using Restaurant.Pages.Repartidores;
using Restaurant.Entities;
using Restaurant.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RestaurantTests.Pages.Repartidores
{
    public class RepartidoresCreatePageTests
    {
        [Fact]
        public async Task OnPostAsync_DeberiaCrearRepartidor_Nuevo()
        {
            var repo = new Mock<IGenericRepository<Repartidore>>();

            var page = new CreateModel(repo.Object)
            {
                Repartidor = new Repartidore { Nombre = "Luis", Id = 0 }
            };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.AddAsync(It.IsAny<Repartidore>()), Times.Once);
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
            result.Should().BeOfType<RedirectToPageResult>();
        }
        
        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_NoDebeCrearRepartidor()
        {
            var repo = new Mock<IGenericRepository<Repartidore>>();
            var page = new CreateModel(repo.Object);
            page.ModelState.AddModelError("Nombre", "Requerido");

            page.Repartidor = new Repartidore { Nombre = "" };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.AddAsync(It.IsAny<Repartidore>()), Times.Never);
            repo.Verify(r => r.SaveChangesAsync(), Times.Never);
            result.Should().BeOfType<PageResult>();
        }

    }
}