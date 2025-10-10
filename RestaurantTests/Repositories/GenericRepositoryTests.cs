using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Restaurant.Context;
using Restaurant.Entities;
using Restaurant.Repositories;
using RestaurantTests.Helpers;

namespace RestaurantTests.Repositories;

/// <summary>
/// Contexto de base de datos específico para pruebas unitarias
/// Sobrescribe la configuración del contexto principal para usar InMemory
/// </summary>
public class TestDbContext : MyDbContext
{
    public TestDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // No llamamos al método base para evitar la configuración de MySQL
        // El contexto ya está configurado con InMemory desde las opciones del constructor
    }
}

/// <summary>
/// Pruebas unitarias para GenericRepository
/// Valida el patrón de repositorio genérico con base de datos en memoria
/// </summary>
public class GenericRepositoryTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly GenericRepository<Producto> _productoRepository;

    public GenericRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _productoRepository = new GenericRepository<Producto>(_context);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DeberiaRetornarTodosLosElementos_CuandoExistenDatos()
    {
        // Arrange
        var productos = TestDataFactory.CreateValidProductos(3);
        await _context.Productos.AddRangeAsync(productos);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _productoRepository.GetAllAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(3);
        resultado.Should().BeEquivalentTo(productos, options => options.Excluding(p => p.Categoria));
    }

    [Fact]
    public async Task GetAllAsync_DeberiaRetornarListaVacia_CuandoNoExistenDatos()
    {
        // Act
        var resultado = await _productoRepository.GetAllAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_DeberiaRetornarElemento_CuandoExisteElId()
    {
        // Arrange
        var producto = TestDataFactory.CreateValidProducto(1, "Producto Test", 15.75m);
        await _context.Productos.AddAsync(producto);
        await _context.SaveChangesAsync();

        // Act
        var resultado = await _productoRepository.GetByIdAsync(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(1);
        resultado.Nombre.Should().Be("Producto Test");
        resultado.Precio.Should().Be(15.75m);
    }

    [Fact]
    public async Task GetByIdAsync_DeberiaRetornarNull_CuandoNoExisteElId()
    {
        // Act
        var resultado = await _productoRepository.GetByIdAsync(999);

        // Assert
        resultado.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task GetByIdAsync_DeberiaRetornarNull_CuandoIdEsInvalido(short idInvalido)
    {
        // Act
        var resultado = await _productoRepository.GetByIdAsync(idInvalido);

        // Assert
        resultado.Should().BeNull();
    }

    #endregion

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_DeberiaAgregarElemento_CuandoElementoEsValido()
    {
        // Arrange
        var producto = TestDataFactory.CreateValidProducto(1, "Nuevo Producto", 20.50m);

        // Act
        await _productoRepository.AddAsync(producto);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productosEnBD = await _context.Productos.ToListAsync();
        productosEnBD.Should().HaveCount(1);
        productosEnBD[0].Should().BeEquivalentTo(producto, options => options.Excluding(p => p.Categoria));
    }

    [Fact]
    public async Task AddAsync_DeberiaAgregarMultiplesElementos_CuandoTodosValidos()
    {
        // Arrange
        var producto1 = TestDataFactory.CreateValidProducto(1, "Producto 1", 10.00m);
        var producto2 = TestDataFactory.CreateValidProducto(2, "Producto 2", 15.00m);

        // Act
        await _productoRepository.AddAsync(producto1);
        await _productoRepository.AddAsync(producto2);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productosEnBD = await _context.Productos.ToListAsync();
        productosEnBD.Should().HaveCount(2);
        productosEnBD.Should().Contain(p => p.Nombre == "Producto 1");
        productosEnBD.Should().Contain(p => p.Nombre == "Producto 2");
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_DeberiaActualizarElemento_CuandoElementoExiste()
    {
        // Arrange
        var producto = TestDataFactory.CreateValidProducto(1, "Producto Original", 10.00m);
        await _context.Productos.AddAsync(producto);
        await _context.SaveChangesAsync();

        // Modificar el producto
        producto.Nombre = "Producto Actualizado";
        producto.Precio = 25.50m;

        // Act
        _productoRepository.Update(producto);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productoActualizado = await _context.Productos.FindAsync((short)1);
        productoActualizado.Should().NotBeNull();
        productoActualizado!.Nombre.Should().Be("Producto Actualizado");
        productoActualizado.Precio.Should().Be(25.50m);
    }

    [Fact]
    public async Task Update_DeberiaActualizarSoloCamposModificados_CuandoSeActualizaParcialmente()
    {
        // Arrange
        var producto = TestDataFactory.CreateValidProducto(1, "Producto Test", 10.00m);
        producto.Stock = 50;
        await _context.Productos.AddAsync(producto);
        await _context.SaveChangesAsync();

        // Modificar solo el precio
        producto.Precio = 12.75m;

        // Act
        _productoRepository.Update(producto);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productoActualizado = await _context.Productos.FindAsync((short)1);
        productoActualizado.Should().NotBeNull();
        productoActualizado!.Nombre.Should().Be("Producto Test"); // Sin cambios
        productoActualizado.Precio.Should().Be(12.75m); // Actualizado
        productoActualizado.Stock.Should().Be(50); // Sin cambios
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_DeberiaEliminarElemento_CuandoElementoExiste()
    {
        // Arrange
        var producto = TestDataFactory.CreateValidProducto(1, "Producto a Eliminar", 15.00m);
        await _context.Productos.AddAsync(producto);
        await _context.SaveChangesAsync();

        // Act
        _productoRepository.Delete(producto);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productosEnBD = await _context.Productos.ToListAsync();
        productosEnBD.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_DeberiaEliminarSoloElementoEspecificado_CuandoHayMultiplesElementos()
    {
        // Arrange
        var producto1 = TestDataFactory.CreateValidProducto(1, "Producto 1", 10.00m);
        var producto2 = TestDataFactory.CreateValidProducto(2, "Producto 2", 15.00m);
        var producto3 = TestDataFactory.CreateValidProducto(3, "Producto 3", 20.00m);
        
        await _context.Productos.AddRangeAsync(producto1, producto2, producto3);
        await _context.SaveChangesAsync();

        // Act - Eliminar solo el producto2
        _productoRepository.Delete(producto2);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productosEnBD = await _context.Productos.ToListAsync();
        productosEnBD.Should().HaveCount(2);
        productosEnBD.Should().Contain(p => p.Id == 1);
        productosEnBD.Should().Contain(p => p.Id == 3);
        productosEnBD.Should().NotContain(p => p.Id == 2);
    }

    #endregion

    #region SaveChangesAsync Tests

    [Fact]
    public async Task SaveChangesAsync_DeberiaGuardarCambios_CuandoHayOperacionesPendientes()
    {
        // Arrange
        var producto = TestDataFactory.CreateValidProducto(1, "Producto Test", 10.00m);

        // Act
        await _productoRepository.AddAsync(producto);
        
        // Verificar que no está guardado antes de SaveChangesAsync
        var productosAntes = await _context.Productos.AsNoTracking().ToListAsync();
        productosAntes.Should().BeEmpty();

        await _productoRepository.SaveChangesAsync();

        // Assert
        var productosDespues = await _context.Productos.AsNoTracking().ToListAsync();
        productosDespues.Should().HaveCount(1);
        productosDespues[0].Nombre.Should().Be("Producto Test");
    }

    [Fact]
    public async Task SaveChangesAsync_DeberiaGuardarOperacionesMultiples_EnUnaTransaccion()
    {
        // Arrange
        var producto1 = TestDataFactory.CreateValidProducto(1, "Producto Nuevo", 10.00m);
        var producto2 = TestDataFactory.CreateValidProducto(2, "Producto Existente", 15.00m);
        
        // Agregar producto2 primero
        await _context.Productos.AddAsync(producto2);
        await _context.SaveChangesAsync();

        // Act - Agregar producto1 y actualizar producto2 en la misma transacción
        await _productoRepository.AddAsync(producto1);
        producto2.Precio = 18.75m;
        _productoRepository.Update(producto2);
        await _productoRepository.SaveChangesAsync();

        // Assert
        var productos = await _context.Productos.ToListAsync();
        productos.Should().HaveCount(2);
        
        var productoNuevo = productos.FirstOrDefault(p => p.Id == 1);
        var productoActualizado = productos.FirstOrDefault(p => p.Id == 2);
        
        productoNuevo.Should().NotBeNull();
        productoNuevo!.Nombre.Should().Be("Producto Nuevo");
        
        productoActualizado.Should().NotBeNull();
        productoActualizado!.Precio.Should().Be(18.75m);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task CRUDCompleto_DeberiaFuncionarCorrectamente_ConTodaLaFuncionalidad()
    {
        // CREATE
        var producto = TestDataFactory.CreateValidProducto(1, "Producto CRUD", 25.00m);
        await _productoRepository.AddAsync(producto);
        await _productoRepository.SaveChangesAsync();

        // READ - GetAll
        var productos = await _productoRepository.GetAllAsync();
        productos.Should().HaveCount(1);

        // READ - GetById
        var productoEncontrado = await _productoRepository.GetByIdAsync(1);
        productoEncontrado.Should().NotBeNull();
        productoEncontrado!.Nombre.Should().Be("Producto CRUD");

        // UPDATE
        productoEncontrado.Nombre = "Producto CRUD Actualizado";
        productoEncontrado.Precio = 30.00m;
        _productoRepository.Update(productoEncontrado);
        await _productoRepository.SaveChangesAsync();

        var productoActualizado = await _productoRepository.GetByIdAsync(1);
        productoActualizado!.Nombre.Should().Be("Producto CRUD Actualizado");
        productoActualizado.Precio.Should().Be(30.00m);

        // DELETE
        _productoRepository.Delete(productoActualizado);
        await _productoRepository.SaveChangesAsync();

        var productosFinales = await _productoRepository.GetAllAsync();
        productosFinales.Should().BeEmpty();
    }

    #endregion

    public void Dispose()
    {
        _context.Dispose();
    }
}