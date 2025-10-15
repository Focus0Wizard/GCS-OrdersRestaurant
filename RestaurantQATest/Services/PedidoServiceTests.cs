using Xunit;
using FluentAssertions;
using Moq;
using Restaurant.Entities;
using Restaurant.Repositories;
using Restaurant.Services;
using RestaurantTests.Helpers;

namespace RestaurantTests.Services;

/// <summary>
/// Pruebas unitarias para PedidoService
/// Valida la lógica de negocio más compleja que involucra múltiples entidades y cálculos
/// </summary>
public class PedidoServiceTests
{
    private readonly Mock<IGenericRepository<Pedido>> _mockPedidoRepository;
    private readonly Mock<IGenericRepository<DetallePedido>> _mockDetalleRepository;
    private readonly PedidoService _pedidoService;

    public PedidoServiceTests()
    {
        _mockPedidoRepository = new Mock<IGenericRepository<Pedido>>();
        _mockDetalleRepository = new Mock<IGenericRepository<DetallePedido>>();
        _pedidoService = new PedidoService(_mockPedidoRepository.Object, _mockDetalleRepository.Object);
    }

    #region GetAllPedidosAsync Tests

    [Fact]
    public async Task GetAllPedidosAsync_DeberiaRetornarTodosLosPedidos_CuandoExistenPedidos()
    {
        // Arrange
        var pedidosEsperados = new List<Pedido>
        {
            TestDataFactory.CreateValidPedido(1, 1),
            TestDataFactory.CreateValidPedido(2, 2),
            TestDataFactory.CreateValidPedido(3, 1)
        };
        _mockPedidoRepository.Setup(repo => repo.GetAllAsync())
                            .ReturnsAsync(pedidosEsperados);

        // Act
        var resultado = await _pedidoService.GetAllPedidosAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(3);
        resultado.Should().BeEquivalentTo(pedidosEsperados);
        _mockPedidoRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllPedidosAsync_DeberiaRetornarListaVacia_CuandoNoExistenPedidos()
    {
        // Arrange
        var pedidosVacios = new List<Pedido>();
        _mockPedidoRepository.Setup(repo => repo.GetAllAsync())
                            .ReturnsAsync(pedidosVacios);

        // Act
        var resultado = await _pedidoService.GetAllPedidosAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEmpty();
        _mockPedidoRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    #endregion

    #region GetPedidoByIdAsync Tests

    [Fact]
    public async Task GetPedidoByIdAsync_DeberiaRetornarPedido_CuandoExisteElId()
    {
        // Arrange
        short idPedido = 1;
        var pedidoEsperado = TestDataFactory.CreateValidPedido(idPedido, 1);
        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedido))
                            .ReturnsAsync(pedidoEsperado);

        // Act
        var resultado = await _pedidoService.GetPedidoByIdAsync(idPedido);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEquivalentTo(pedidoEsperado);
        resultado!.Id.Should().Be(idPedido);
        _mockPedidoRepository.Verify(repo => repo.GetByIdAsync(idPedido), Times.Once);
    }

    [Fact]
    public async Task GetPedidoByIdAsync_DeberiaRetornarNull_CuandoNoExisteElId()
    {
        // Arrange
        short idPedidoInexistente = 999;
        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedidoInexistente))
                            .ReturnsAsync((Pedido?)null);

        // Act
        var resultado = await _pedidoService.GetPedidoByIdAsync(idPedidoInexistente);

        // Assert
        resultado.Should().BeNull();
        _mockPedidoRepository.Verify(repo => repo.GetByIdAsync(idPedidoInexistente), Times.Once);
    }

    #endregion

    #region AddPedidoAsync Tests

    [Fact]
    public async Task AddPedidoAsync_DeberiaCrearPedidoConDetallesYCalcularTotal_CuandoDatosValidos()
    {
        // Arrange
        var pedido = TestDataFactory.CreateValidPedido(1, 1);
        pedido.Total = 0; // Será calculado por el servicio

        var producto1 = TestDataFactory.CreateValidProducto(1, "Pizza", 15.50m);
        var producto2 = TestDataFactory.CreateValidProducto(2, "Refresco", 3.25m);
        
        var detalles = new List<DetallePedido>
        {
            new DetallePedido { ProductoId = 1, Cantidad = 2, PrecioUnitario = 15.50m, Subtotal = 31.00m, Producto = producto1 },
            new DetallePedido { ProductoId = 2, Cantidad = 1, PrecioUnitario = 3.25m, Subtotal = 3.25m, Producto = producto2 }
        };

        _mockPedidoRepository.Setup(repo => repo.AddAsync(It.IsAny<Pedido>()))
                            .Returns(Task.CompletedTask);
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);
        _mockDetalleRepository.Setup(repo => repo.AddAsync(It.IsAny<DetallePedido>()))
                             .Returns(Task.CompletedTask);
        _mockDetalleRepository.Setup(repo => repo.SaveChangesAsync())
                             .Returns(Task.CompletedTask);

        // Act
        var resultado = await _pedidoService.AddPedidoAsync(pedido, detalles);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Total.Should().Be(34.25m); // 31.00 + 3.25
        resultado.FechaCreacion.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        resultado.UltimaActualizacion.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

        // Verificar que el stock se actualizó
        producto1.Stock.Should().Be(8); // 10 - 2
        producto2.Stock.Should().Be(9); // 10 - 1

        _mockPedidoRepository.Verify(repo => repo.AddAsync(pedido), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _mockDetalleRepository.Verify(repo => repo.AddAsync(It.IsAny<DetallePedido>()), Times.Exactly(2));
        _mockDetalleRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPedidoAsync_DeberiaAsignarPedidoIdADetalles_CuandoSeCreaCorrectamente()
    {
        // Arrange
        var pedido = TestDataFactory.CreateValidPedido(5, 1);
        var detalles = new List<DetallePedido>
        {
            new DetallePedido { ProductoId = 1, Cantidad = 1, PrecioUnitario = 10.00m, Subtotal = 10.00m }
        };

        _mockPedidoRepository.Setup(repo => repo.AddAsync(It.IsAny<Pedido>()))
                            .Returns(Task.CompletedTask);
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);
        _mockDetalleRepository.Setup(repo => repo.AddAsync(It.IsAny<DetallePedido>()))
                             .Returns(Task.CompletedTask);
        _mockDetalleRepository.Setup(repo => repo.SaveChangesAsync())
                             .Returns(Task.CompletedTask);

        // Act
        await _pedidoService.AddPedidoAsync(pedido, detalles);

        // Assert
        detalles[0].PedidoId.Should().Be(5);
        _mockDetalleRepository.Verify(repo => repo.AddAsync(It.Is<DetallePedido>(d => d.PedidoId == 5)), Times.Once);
    }

    [Fact]
    public async Task AddPedidoAsync_DeberiaCalcularTotalCorrectamente_ConMultiplesDetalles()
    {
        // Arrange
        var pedido = TestDataFactory.CreateValidPedido(1, 1);
        var detalles = new List<DetallePedido>
        {
            new DetallePedido { ProductoId = 1, Cantidad = 3, PrecioUnitario = 12.50m, Subtotal = 37.50m },
            new DetallePedido { ProductoId = 2, Cantidad = 2, PrecioUnitario = 8.75m, Subtotal = 17.50m },
            new DetallePedido { ProductoId = 3, Cantidad = 1, PrecioUnitario = 25.00m, Subtotal = 25.00m }
        };

        _mockPedidoRepository.Setup(repo => repo.AddAsync(It.IsAny<Pedido>()))
                            .Returns(Task.CompletedTask);
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);
        _mockDetalleRepository.Setup(repo => repo.AddAsync(It.IsAny<DetallePedido>()))
                             .Returns(Task.CompletedTask);
        _mockDetalleRepository.Setup(repo => repo.SaveChangesAsync())
                             .Returns(Task.CompletedTask);

        // Act
        var resultado = await _pedidoService.AddPedidoAsync(pedido, detalles);

        // Assert
        resultado.Total.Should().Be(80.00m); // 37.50 + 17.50 + 25.00
    }

    #endregion

    #region UpdatePedidoAsync Tests

    [Fact]
    public async Task UpdatePedidoAsync_DeberiaActualizarPedidoYFechaModificacion_CuandoPedidoEsValido()
    {
        // Arrange
        var pedidoActualizado = TestDataFactory.CreateValidPedido(1, 1);
        var fechaOriginal = pedidoActualizado.UltimaActualizacion;
        Thread.Sleep(100); // Asegurar que hay diferencia de tiempo

        _mockPedidoRepository.Setup(repo => repo.Update(It.IsAny<Pedido>()));
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        // Act
        await _pedidoService.UpdatePedidoAsync(pedidoActualizado);

        // Assert
        pedidoActualizado.UltimaActualizacion.Should().BeAfter(fechaOriginal!.Value);
        pedidoActualizado.UltimaActualizacion.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        _mockPedidoRepository.Verify(repo => repo.Update(pedidoActualizado), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region DeletePedidoAsync Tests

    [Fact]
    public async Task DeletePedidoAsync_DeberiaEliminarPedidoYGuardarCambios_CuandoPedidoExiste()
    {
        // Arrange
        short idPedido = 1;
        var pedidoExistente = TestDataFactory.CreateValidPedido(idPedido, 1);
        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedido))
                            .ReturnsAsync(pedidoExistente);
        _mockPedidoRepository.Setup(repo => repo.Delete(It.IsAny<Pedido>()));
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        // Act
        await _pedidoService.DeletePedidoAsync(idPedido);

        // Assert
        _mockPedidoRepository.Verify(repo => repo.GetByIdAsync(idPedido), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.Delete(pedidoExistente), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeletePedidoAsync_NoDeberiaEliminarNiGuardar_CuandoPedidoNoExiste()
    {
        // Arrange
        short idPedidoInexistente = 999;
        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedidoInexistente))
                            .ReturnsAsync((Pedido?)null);

        // Act
        await _pedidoService.DeletePedidoAsync(idPedidoInexistente);

        // Assert
        _mockPedidoRepository.Verify(repo => repo.GetByIdAsync(idPedidoInexistente), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.Delete(It.IsAny<Pedido>()), Times.Never);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    #endregion

    #region UpdateEstadoPedidoAsync Tests

    [Fact]
    public async Task UpdateEstadoPedidoAsync_DeberiaActualizarEstadoYFecha_CuandoPedidoExiste()
    {
        // Arrange
        short idPedido = 1;
        sbyte nuevoEstado = 2; // Ej: En preparación
        var pedidoExistente = TestDataFactory.CreateValidPedido(idPedido, 1);
        pedidoExistente.EstadoPedido = 1; // Estado original
        var fechaOriginal = pedidoExistente.UltimaActualizacion;
        Thread.Sleep(100);

        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedido))
                            .ReturnsAsync(pedidoExistente);
        _mockPedidoRepository.Setup(repo => repo.Update(It.IsAny<Pedido>()));
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        // Act
        await _pedidoService.UpdateEstadoPedidoAsync(idPedido, nuevoEstado);

        // Assert
        pedidoExistente.EstadoPedido.Should().Be(nuevoEstado);
        pedidoExistente.UltimaActualizacion.Should().BeAfter(fechaOriginal!.Value);
        pedidoExistente.UltimaActualizacion.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        
        _mockPedidoRepository.Verify(repo => repo.GetByIdAsync(idPedido), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.Update(pedidoExistente), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateEstadoPedidoAsync_NoDeberiaHacerNada_CuandoPedidoNoExiste()
    {
        // Arrange
        short idPedidoInexistente = 999;
        sbyte nuevoEstado = 2;
        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedidoInexistente))
                            .ReturnsAsync((Pedido?)null);

        // Act
        await _pedidoService.UpdateEstadoPedidoAsync(idPedidoInexistente, nuevoEstado);

        // Assert
        _mockPedidoRepository.Verify(repo => repo.GetByIdAsync(idPedidoInexistente), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.Update(It.IsAny<Pedido>()), Times.Never);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData((sbyte)0)] // Pendiente
    [InlineData((sbyte)1)] // Confirmado
    [InlineData((sbyte)2)] // En preparación
    [InlineData((sbyte)3)] // Listo
    [InlineData((sbyte)4)] // Entregado
    [InlineData((sbyte)-1)] // Cancelado
    public async Task UpdateEstadoPedidoAsync_DeberiaPermitirDiferentesEstados_CuandoPedidoExiste(sbyte estado)
    {
        // Arrange
        short idPedido = 1;
        var pedidoExistente = TestDataFactory.CreateValidPedido(idPedido, 1);
        
        _mockPedidoRepository.Setup(repo => repo.GetByIdAsync(idPedido))
                            .ReturnsAsync(pedidoExistente);
        _mockPedidoRepository.Setup(repo => repo.Update(It.IsAny<Pedido>()));
        _mockPedidoRepository.Setup(repo => repo.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        // Act
        await _pedidoService.UpdateEstadoPedidoAsync(idPedido, estado);

        // Assert
        pedidoExistente.EstadoPedido.Should().Be(estado);
        _mockPedidoRepository.Verify(repo => repo.Update(pedidoExistente), Times.Once);
        _mockPedidoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    #endregion
}