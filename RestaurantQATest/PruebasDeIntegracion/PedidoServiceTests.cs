using Xunit;
using FluentAssertions;
using Moq;
using Restaurant.Entities;
using Restaurant.Repositories;
using Restaurant.Services;

public class PedidoServiceTests
{
    private readonly Mock<IGenericRepository<Pedido>> _mockPedidoRepo;
    private readonly Mock<IGenericRepository<DetallePedido>> _mockDetalleRepo;
    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        _mockPedidoRepo = new Mock<IGenericRepository<Pedido>>();
        _mockDetalleRepo = new Mock<IGenericRepository<DetallePedido>>();

        _service = new PedidoService(
            _mockPedidoRepo.Object,
            _mockDetalleRepo.Object
        );
    }

    [Fact]
    public async Task GetAllPedidosAsync_DeberiaRetornarPedidos()
    {
        var pedidos = new List<Pedido>
        {
            new Pedido { Id = 1, ClienteId = 1 },
            new Pedido { Id = 2, ClienteId = 2 }
        };

        _mockPedidoRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(pedidos);

        var result = await _service.GetAllPedidosAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetPedidoByIdAsync_DeberiaRetornarPedido()
    {
        var pedido = new Pedido { Id = 1, ClienteId = 1 };

        _mockPedidoRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pedido);

        var result = await _service.GetPedidoByIdAsync(1);

        result.Should().BeEquivalentTo(pedido);
    }

    [Fact]
    public async Task GetPedidoByIdAsync_DeberiaRetornarNull_SiNoExiste()
    {
        _mockPedidoRepo.Setup(r => r.GetByIdAsync(99))
                       .ReturnsAsync((Pedido?)null);

        var result = await _service.GetPedidoByIdAsync(99);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddPedidoAsync_DeberiaCrearPedidoYDetalles()
    {
        var pedido = new Pedido { Id = 1, ClienteId = 1 };
        var detalles = new List<DetallePedido>
        {
            new DetallePedido { ProductoId = 1, Cantidad = 2 }
        };

        await _service.AddPedidoAsync(pedido, detalles);

        _mockPedidoRepo.Verify(r => r.AddAsync(pedido), Times.Once);
        _mockDetalleRepo.Verify(r => r.AddAsync(It.IsAny<DetallePedido>()), Times.Exactly(1));
        _mockPedidoRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPedidoAsync_Unhappy_NoDebePermitirDetallesVacios()
    {
        var pedido = new Pedido { Id = 1, ClienteId = 1 };

        Func<Task> act = async () => await _service.AddPedidoAsync(pedido, new List<DetallePedido>());

        await act.Should().ThrowAsync<ArgumentException>()
                 .WithMessage("El pedido debe contener al menos un detalle*");
    }

}
