using Xunit;
using FluentAssertions;
using Moq;
using Restaurant.Entities;
using Restaurant.Repositories;
using Restaurant.Services;
using RestaurantTests.Helpers;

namespace RestaurantTests.Services;

/// <summary>
/// Pruebas unitarias para ClienteService
/// Valida la lógica de negocio de clientes sin dependencias externas
/// </summary>
public class ClienteServiceTests
{
    private readonly Mock<IGenericRepository<Cliente>> _mockRepository;
    private readonly ClienteService _clienteService;

    public ClienteServiceTests()
    {
        _mockRepository = new Mock<IGenericRepository<Cliente>>();
        _clienteService = new ClienteService(_mockRepository.Object);
    }

    #region GetAllClientesAsync Tests

    [Fact]
    public async Task GetAllClientesAsync_DeberiaRetornarTodosLosClientes_CuandoExistenClientes()
    {
        // Arrange
        var clientesEsperados = TestDataFactory.CreateValidClientes(3);
        _mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(clientesEsperados);

        // Act
        var resultado = await _clienteService.GetAllClientesAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(3);
        resultado.Should().BeEquivalentTo(clientesEsperados);
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllClientesAsync_DeberiaRetornarListaVacia_CuandoNoExistenClientes()
    {
        // Arrange
        var clientesVacios = new List<Cliente>();
        _mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(clientesVacios);

        // Act
        var resultado = await _clienteService.GetAllClientesAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEmpty();
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    #endregion

    #region GetClienteByIdAsync Tests

    [Fact]
    public async Task GetClienteByIdAsync_DeberiaRetornarCliente_CuandoExisteElId()
    {
        // Arrange
        short idCliente = 1;
        var clienteEsperado = TestDataFactory.CreateValidCliente(idCliente, "Juan Pérez");
        _mockRepository.Setup(repo => repo.GetByIdAsync(idCliente))
                      .ReturnsAsync(clienteEsperado);

        // Act
        var resultado = await _clienteService.GetClienteByIdAsync(idCliente);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().BeEquivalentTo(clienteEsperado);
        resultado!.Id.Should().Be(idCliente);
        resultado.Nombre.Should().Be("Juan Pérez");
        _mockRepository.Verify(repo => repo.GetByIdAsync(idCliente), Times.Once);
    }

    [Fact]
    public async Task GetClienteByIdAsync_DeberiaRetornarNull_CuandoNoExisteElId()
    {
        // Arrange
        short idClienteInexistente = 999;
        _mockRepository.Setup(repo => repo.GetByIdAsync(idClienteInexistente))
                      .ReturnsAsync((Cliente?)null);

        // Act
        var resultado = await _clienteService.GetClienteByIdAsync(idClienteInexistente);

        // Assert
        resultado.Should().BeNull();
        _mockRepository.Verify(repo => repo.GetByIdAsync(idClienteInexistente), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task GetClienteByIdAsync_DeberiaRetornarNull_CuandoIdEsInvalido(short idInvalido)
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(idInvalido))
                      .ReturnsAsync((Cliente?)null);

        // Act
        var resultado = await _clienteService.GetClienteByIdAsync(idInvalido);

        // Assert
        resultado.Should().BeNull();
        _mockRepository.Verify(repo => repo.GetByIdAsync(idInvalido), Times.Once);
    }

    #endregion

    #region AddClienteAsync Tests

    [Fact]
    public async Task AddClienteAsync_DeberiaAgregarClienteYGuardarCambios_CuandoClienteEsValido()
    {
        // Arrange
        var nuevoCliente = TestDataFactory.CreateValidCliente(1, "Maria Rodriguez");
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Cliente>()))
                      .Returns(Task.CompletedTask);
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _clienteService.AddClienteAsync(nuevoCliente);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(nuevoCliente), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddClienteAsync_DeberiaValidarDatosMinimos_CuandoClienteTieneDatosCompletos()
    {
        // Arrange
        var clienteCompleto = new Cliente
        {
            Id = 1,
            Nombre = "Ana",
            Apellido = "García",
            Correo = "ana.garcia@test.com",
            Telefono = "987654321",
            FechaCreacion = DateTime.Now,
            Estado = 1
        };

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Cliente>()))
                      .Returns(Task.CompletedTask);
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _clienteService.AddClienteAsync(clienteCompleto);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(It.Is<Cliente>(c => 
            c.Nombre == "Ana" && 
            c.Apellido == "García" && 
            c.Correo == "ana.garcia@test.com")), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region UpdateClienteAsync Tests

    [Fact]
    public async Task UpdateClienteAsync_DeberiaActualizarClienteYGuardarCambios_CuandoClienteEsValido()
    {
        // Arrange
        var clienteActualizado = TestDataFactory.CreateValidCliente(1, "Pedro Actualizado");
        clienteActualizado.Correo = "pedro.actualizado@nuevo.com";
        
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Cliente>()));
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _clienteService.UpdateClienteAsync(clienteActualizado);

        // Assert
        _mockRepository.Verify(repo => repo.Update(clienteActualizado), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateClienteAsync_DeberiaPermitirActualizacionParcial_CuandoSoloSeModificanAlgunosCampos()
    {
        // Arrange
        var clienteExistente = TestDataFactory.CreateValidCliente(5, "Cliente Original");
        // Simular actualización solo del teléfono
        clienteExistente.Telefono = "111-222-3333";

        _mockRepository.Setup(repo => repo.Update(It.IsAny<Cliente>()));
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _clienteService.UpdateClienteAsync(clienteExistente);

        // Assert
        _mockRepository.Verify(repo => repo.Update(It.Is<Cliente>(c => 
            c.Id == 5 && 
            c.Telefono == "111-222-3333")), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    #endregion

    #region DeleteClienteAsync Tests

    [Fact]
    public async Task DeleteClienteAsync_DeberiaEliminarClienteYGuardarCambios_CuandoClienteExiste()
    {
        // Arrange
        short idCliente = 1;
        var clienteExistente = TestDataFactory.CreateValidCliente(idCliente, "Cliente a Eliminar");
        _mockRepository.Setup(repo => repo.GetByIdAsync(idCliente))
                      .ReturnsAsync(clienteExistente);
        _mockRepository.Setup(repo => repo.Delete(It.IsAny<Cliente>()));
        _mockRepository.Setup(repo => repo.SaveChangesAsync())
                      .Returns(Task.CompletedTask);

        // Act
        await _clienteService.DeleteClienteAsync(idCliente);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(idCliente), Times.Once);
        _mockRepository.Verify(repo => repo.Delete(clienteExistente), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteClienteAsync_NoDeberiaEliminarNiGuardar_CuandoClienteNoExiste()
    {
        // Arrange
        short idClienteInexistente = 999;
        _mockRepository.Setup(repo => repo.GetByIdAsync(idClienteInexistente))
                      .ReturnsAsync((Cliente?)null);

        // Act
        await _clienteService.DeleteClienteAsync(idClienteInexistente);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(idClienteInexistente), Times.Once);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<Cliente>()), Times.Never);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteClienteAsync_NoDeberiaEliminar_CuandoIdEsInvalido(short idInvalido)
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(idInvalido))
                      .ReturnsAsync((Cliente?)null);

        // Act
        await _clienteService.DeleteClienteAsync(idInvalido);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(idInvalido), Times.Once);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<Cliente>()), Times.Never);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    #endregion
}