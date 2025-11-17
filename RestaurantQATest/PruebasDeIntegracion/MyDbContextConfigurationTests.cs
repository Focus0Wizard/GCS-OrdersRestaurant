using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using Restaurant.Entities;

namespace RestaurantTests.Configuration
{
    public class MyDbContextConfigurationTests
    {
        [Fact]
        public void DbContext_DeberiaConfigurarseCorrectamente_EnMemoria()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            // Act
            var context = new MyDbContext(options);

            // Assert
            context.Should().NotBeNull();
        }

        [Fact]
        public void DbSet_Clientes_DeberiaExistirEnElContexto()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("TestDB2")
                .Options;

            var context = new MyDbContext(options);

            context.Clientes.Should().NotBeNull();
        }

        [Fact]
        public void DbSet_Productos_DeberiaExistirEnElContexto()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("TestDB3")
                .Options;

            var context = new MyDbContext(options);

            context.Productos.Should().NotBeNull();
        }

        [Fact]
        public void DbSet_Repartidores_DeberiaExistirEnElContexto()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("TestDB4")
                .Options;

            var context = new MyDbContext(options);

            context.Repartidores.Should().NotBeNull();
        }
    }
}