using Restaurant.Entities;

namespace RestaurantTests.Helpers;

public static class TestDataFactory
{
    public static Producto CreateValidProducto(short id = 1, string nombre = "Producto Test", decimal precio = 10.50m)
    {
        return new Producto
        {
            Id = id,
            Nombre = nombre,
            Precio = precio,
            Stock = 10,
            CategoriaId = 1,
            FechaCreacion = DateTime.Now,
            UltimaActualizacion = DateTime.Now,
            Estado = 1,
            CreadoPor = 1
        };
    }

    public static List<Producto> CreateValidProductos(int count = 3)
    {
        var productos = new List<Producto>();
        for (int i = 1; i <= count; i++)
        {
            productos.Add(CreateValidProducto((short)i, $"Producto {i}", 10.50m * i));
        }
        return productos;
    }

    public static Cliente CreateValidCliente(short id = 1, string nombre = "Cliente Test")
    {
        return new Cliente
        {
            Id = id,
            Nombre = nombre,
            Apellido = "Apellido Test",
            Correo = $"cliente{id}@test.com",
            Telefono = "123456789",
            FechaCreacion = DateTime.Now,
            UltimaActualizacion = DateTime.Now,
            Estado = 1,
            CreadoPor = 1
        };
    }

    public static List<Cliente> CreateValidClientes(int count = 3)
    {
        var clientes = new List<Cliente>();
        for (int i = 1; i <= count; i++)
        {
            clientes.Add(CreateValidCliente((short)i, $"Cliente {i}"));
        }
        return clientes;
    }

    public static Pedido CreateValidPedido(short id = 1, short clienteId = 1)
    {
        return new Pedido
        {
            Id = id,
            ClienteId = clienteId,
            UsuarioId = 1,
            EstadoPedido = 1,
            NombreCliente = "Cliente Test",
            ApellidoCliente = "Apellido Test",
            Total = 100.00m,
            FechaCreacion = DateTime.Now,
            UltimaActualizacion = DateTime.Now,
            CreadoPor = 1
        };
    }

    public static DetallePedido CreateValidDetallePedido(short pedidoId = 1, short productoId = 1)
    {
        return new DetallePedido
        {
            PedidoId = pedidoId,
            ProductoId = productoId,
            Cantidad = 2,
            PrecioUnitario = 10.50m,
            Subtotal = 21.00m
        };
    }
}