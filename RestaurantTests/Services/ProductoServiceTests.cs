using Xunit;
using FluentAssertions;
using Moq;
using Restaurant.Entities;
using Restaurant.Repositories;
using Restaurant.Services;
using RestaurantTests.Helpers;

namespace RestaurantTests.Services;

/// <summary>
/// Pruebas unitarias para ProductoService
/// Valida la lógica de negocio de productos sin dependencias externas
/// </summary>
public class ProductoServiceTests
{
    private readonly Mock<IGenericRepository<Producto>> _mockRepository;
    private readonly ProductoService _productoService;

    public ProductoServiceTests()
    {
        _mockRepository = new Mock<IGenericRepository<Producto>>();
        _productoService = new ProductoService(_mockRepository.Object);
    }

    #region GetAllProductosAsync Tests

    [Fact]
    public async Task GetAllProductosAsync_DeberiaRetornarTodosLosProductos_CuandoExistenProductos()
    {
        // Arrange
        var productosEsperados = TestDataFactory.CreateValidProductos(3);
        _mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(productosEsperados);

        // Act
        var resultado = await _productoService.GetAllProductosAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(3);
        resultado.Should().BeEquivalentTo(productosEsperados);
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllProductosAsync_DeberiaRetornarListaVacia_CuandoNoExistenProductos()
    {
        // Arrange
        var productosVacios = new List<Producto>();
        _mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(productosVacios);

        // Act
        var resultado = await _productoService.GetAllProductosAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEmpty();
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    #endregion

    #region GetProductoByIdAsync Tests

    [Fact]
    public async Task GetProductoByIdAsync_DeberiaRetornarProducto_CuandoExisteElId()
    {
        // Arrange
        short idProducto = 1;
        var productoEsperado = TestDataFactory.CreateValidProducto(idProducto);
        _mockRepository.Setup(repo => repo.GetByIdAsync(idProducto))
                      .ReturnsAsync(productoEsperado);

        // Act
        var resultado = await _productoService.GetProductoByIdAsync(idProducto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEquivalentTo(productoEsperado);
        resultado!.Id.Should().Be(idProducto);
        _mockRepository.Verify(repo => repo.GetByIdAsync(idProducto), Times.Once);
    }

    [Fact]
    public async Task GetProductoByIdAsync_DeberiaRetornarNull_CuandoNoExisteElId()
    {
        // Arrange
        short idProductoInexistente = 999;
        _mockRepository.Setup(repo => repo.GetByIdAsync(idProductoInexistente))
                      .ReturnsAsync((Producto?)null);

        // Act
        var resultado = await _productoService.GetProductoByIdAsync(idProductoInexistente);

        // Assert
        resultado.Should().BeNull();
        _mockRepository.Verify(repo => repo.GetByIdAsync(idProductoInexistente), Times.Once);
    }

    #endregion

    #region AddProductoAsync Tests

    [Fact]
    public async Task AddProductoAsync_DeberiaAgregarProductoYGuardarCambios_CuandoProductoEsValido()
    {
        // Arrange
        var nuevoProducto = TestDataFactory.CreateValidProducto();
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Producto>()))
                      .Returns(Task.CompletedTask);
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _productoService.AddProductoAsync(nuevoProducto);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(nuevoProducto), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region UpdateProductoAsync Tests

    [Fact]
    public async Task UpdateProductoAsync_DeberiaActualizarProductoYGuardarCambios_CuandoProductoEsValido()
    {
        // Arrange
        var productoActualizado = TestDataFactory.CreateValidProducto(1, "Producto Actualizado", 25.75m);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Producto>()));
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _productoService.UpdateProductoAsync(productoActualizado);

        // Assert
        _mockRepository.Verify(repo => repo.Update(productoActualizado), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region DeleteProductoAsync Tests

    [Fact]
    public async Task DeleteProductoAsync_DeberiaEliminarProductoYGuardarCambios_CuandoProductoExiste()
    {
        // Arrange
        short idProducto = 1;
        var productoExistente = TestDataFactory.CreateValidProducto(idProducto);
        _mockRepository.Setup(repo => repo.GetByIdAsync(idProducto))
                      .ReturnsAsync(productoExistente);
        _mockRepository.Setup(repo => repo.Delete(It.IsAny<Producto>()));
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _productoService.DeleteProductoAsync(idProducto);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(idProducto), Times.Once);
        _mockRepository.Verify(repo => repo.Delete(productoExistente), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteProductoAsync_NoDeberiaEliminarNiGuardar_CuandoProductoNoExiste()
    {
        // Arrange
        short idProductoInexistente = 999;
        _mockRepository.Setup(repo => repo.GetByIdAsync(idProductoInexistente))
                      .ReturnsAsync((Producto?)null);

        // Act
        await _productoService.DeleteProductoAsync(idProductoInexistente);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(idProductoInexistente), Times.Once);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<Producto>()), Times.Never);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    #endregion
}