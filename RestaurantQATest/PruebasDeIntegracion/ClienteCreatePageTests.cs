using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Pages.Clientes;
using Restaurant.Entities;
using Restaurant.Repositories;

namespace RestaurantTests.Pages.Clientes
{
    public class ClienteCreatePageTests
    {
        [Fact]
        public async Task OnGetAsync_DeberiaCargarClienteExistente_ParaEdicion()
        {
            var repo = new Mock<IGenericRepository<Cliente>>();
            var clienteExistente = new Cliente 
            { 
                Id = 1, 
                Nombre = "Pedro", 
                Apellido = "Perez",
                Correo = "pedro@test.com",
                Telefono = "12345678",
                Estado = 1 
            };
            repo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(clienteExistente);

            var page = new CreateModel(repo.Object);

            var result = await page.OnGetAsync(1);

            page.Cliente.Should().NotBeNull();
            page.Cliente.Nombre.Should().Be("Pedro");
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnPostAsync_DeberiaCrearCliente_HappyPath()
        {
            var repo = new Mock<IGenericRepository<Cliente>>();

            var page = new CreateModel(repo.Object)
            {
                Cliente = new Cliente { Nombre = "Juan", Apellido = "Landivar", Telefono = "78992336", Id = 0 }
            };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);

            result.Should().BeOfType<RedirectToPageResult>();
        }
        
        //Unhappy Path
        [Fact]
        public async Task OnPostAsync_ModelStateInvalido_NoDebeCrearCliente()
        {
            var repo = new Mock<IGenericRepository<Cliente>>();
            var page = new CreateModel(repo.Object);
            page.ModelState.AddModelError("Nombre", "Requerido");

            page.Cliente = new Cliente { Nombre = "" };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Never);
            repo.Verify(r => r.SaveChangesAsync(), Times.Never);
            result.Should().BeOfType<PageResult>();
        }
    }
}