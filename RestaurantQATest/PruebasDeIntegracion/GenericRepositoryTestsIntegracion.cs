using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using Restaurant.Entities;
using Restaurant.Repositories;
using System;
using System.Threading.Tasks;

namespace RestaurantTests.Repositories
{
    public class GenericRepositoryTestsIntegracion
    {
        // Método para crear contexto InMemory limpio para cada test
        private MyDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging() // útil para debugging
                .Options;

            return new MyDbContext(options);
        }

        [Fact]
        public async Task AddAsync_DeberiaAgregarEntidad_HappyPath()
        {
            var context = GetInMemoryDbContext();
            var repo = new GenericRepository<Cliente>(context);

            var cliente = new Cliente
            {
                Id = 1,
                Nombre = "Juan",
                Apellido = "Perez",
                Correo = "ejemplo@gmail.com",
                Estado = 1
            };

            await repo.AddAsync(cliente);
            await repo.SaveChangesAsync();

            (await context.Clientes.CountAsync()).Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_DeberiaRetornarNull_CuandoEstadoNoEs1_Unhappy()
        {
            var context = GetInMemoryDbContext();
            var repo = new GenericRepository<Cliente>(context);

            var cliente = new Cliente
            {
                Id = 2,
                Nombre = "Pedro",
                Apellido = "Perez",
                Correo = "ejemplo@gmail.com",
                Estado = 0
            };

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(2);

            result.Should().BeNull();
        }

        [Fact]
        public async Task SoftDeleteAsync_DeberiaCambiarEstadoACero()
        {
            var context = GetInMemoryDbContext();
            var repo = new GenericRepository<Cliente>(context);

            var cliente = new Cliente
            {
                Id = 5,
                Nombre = "Maria",
                Apellido = "Perez",
                Correo = "ejemplo@gmail.com",
                Estado = 1
            };

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            await repo.SoftDeleteAsync(cliente);

            cliente.Estado.Should().Be(0);
        }

        [Fact]
        public async Task FindAsync_DeberiaFiltrarSoloActivos()
        {
            var context = GetInMemoryDbContext();
            var repo = new GenericRepository<Cliente>(context);

            context.Clientes.Add(new Cliente { Id = 1, Nombre = "Luis", Apellido = "Barahona", Correo = "nose@gmail.com", Estado = 1 });
            context.Clientes.Add(new Cliente { Id = 2, Nombre = "Lucas", Apellido = "Cadima", Correo = "nose@gmail.com", Estado = 0 });
            await context.SaveChangesAsync();

            var result = await repo.FindAsync(c => c.Nombre.Contains("Lu"));

            result.Should().HaveCount(1);
        }
    }
}
