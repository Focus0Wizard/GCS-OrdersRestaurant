using Xunit;
using Moq;
using FluentAssertions;
using Restaurant.Pages.Productos;
using Restaurant.Entities;
using Restaurant.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RestaurantTests.Pages.Productos
{
    public class ProductosPageTests
    {
        [Fact]
        public async Task OnGetAsync_DeberiaFiltrarPorNombre()
        {
            var repo = new Mock<IGenericRepository<Producto>>();

            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Producto>
            {
                new Producto { Id=1, Nombre="Pizza", Descripcion="Pizza italiana", Estado=1 },
                new Producto { Id=2, Nombre="Hamburguesa", Descripcion="Hamburguesa clásica", Estado=1 }
            });

            var page = new ProductosModel(repo.Object);

            await page.OnGetAsync(null, "pi");

            page.Productos.Should().ContainSingle(p => p.Nombre == "Pizza");
        }

        [Fact]
        public async Task OnPostEliminarAsync_DeberiaSoftDeleteProducto()
        {
            var repo = new Mock<IGenericRepository<Producto>>();
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Producto { Id=1, Descripcion="Test", Estado=1 });

            var page = new ProductosModel(repo.Object);

            var resp = await page.OnPostEliminarAsync(1);

            repo.Verify(r => r.SoftDeleteAsync(It.IsAny<Producto>()), Times.Once);
            resp.Should().BeOfType<RedirectToPageResult>();
        }

        // ---------------- Happy Path Creación ----------------
        [Fact]
        public async Task OnPostCrearAsync_DeberiaCrearProducto_HappyPath()
        {
            var repo = new Mock<IGenericRepository<Producto>>();
            var page = new ProductosModel(repo.Object)
            {
                Producto = new Producto { Nombre = "Pizza", Descripcion = "Pizza italiana", Id = 0 }
            };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.AddAsync(It.IsAny<Producto>()), Times.Once);
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
            result.Should().BeOfType<RedirectToPageResult>();
        }

        // ---------------- Unhappy Path Creación (ModelState inválido) ----------------
        [Fact]
        public async Task OnPostCrearAsync_ModelStateInvalido_NoDeberiaCrearProducto()
        {
            var repo = new Mock<IGenericRepository<Producto>>();
            var page = new ProductosModel(repo.Object);
            page.ModelState.AddModelError("Nombre", "Requerido");
            page.Producto = new Producto { Nombre = "", Descripcion = "" };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.AddAsync(It.IsAny<Producto>()), Times.Never);
            repo.Verify(r => r.SaveChangesAsync(), Times.Never);
            result.Should().BeOfType<PageResult>();
        }

        // ---------------- Happy Path Edición ----------------
        [Fact]
        public async Task OnPostEditarAsync_DeberiaActualizarProducto_HappyPath()
        {
            var repo = new Mock<IGenericRepository<Producto>>();
            repo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Producto { Id = 1, Nombre = "Pizza", Descripcion = "Pizza italiana" });

            var page = new ProductosModel(repo.Object)
            {
                Producto = new Producto { Id = 1, Nombre = "Pizza Grande", Descripcion = "Pizza grande italiana" }
            };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
            result.Should().BeOfType<RedirectToPageResult>();
        }


        // ---------------- Unhappy Path Edición (Producto no existe) ----------------
        [Fact]
        public async Task OnPostEditarAsync_ProductoNoExiste_DeberiaRetornarNotFound()
        {
            var repo = new Mock<IGenericRepository<Producto>>();
            repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Producto?)null);

            var page = new ProductosModel(repo.Object)
            {
                Producto = new Producto { Id = 99, Nombre = "ProductoX", Descripcion = "Descripcion X" }
            };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.Update(It.IsAny<Producto>()), Times.Never);
            repo.Verify(r => r.SaveChangesAsync(), Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }

        // ---------------- Unhappy Path Edición (ModelState inválido) ----------------
        [Fact]
        public async Task OnPostEditarAsync_ModelStateInvalido_NoDeberiaActualizar()
        {
            var repo = new Mock<IGenericRepository<Producto>>();
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Producto { Id = 1, Nombre = "Pizza" });

            var page = new ProductosModel(repo.Object);
            page.ModelState.AddModelError("Nombre", "Requerido");
            page.Producto = new Producto { Id = 1, Nombre = "" };

            var result = await page.OnPostAsync();

            repo.Verify(r => r.Update(It.IsAny<Producto>()), Times.Never);
            repo.Verify(r => r.SaveChangesAsync(), Times.Never);
            result.Should().BeOfType<PageResult>();
        }
    }
}
