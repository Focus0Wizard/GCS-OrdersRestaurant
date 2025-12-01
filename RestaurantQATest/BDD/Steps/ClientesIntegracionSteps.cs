using TechTalk.SpecFlow;
using FluentAssertions;
using Restaurant.Entities;
using RestaurantQATest.BDD.Support;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RestaurantQATest.BDD.Steps
{
    [Binding]
    public class ClientesIntegracionSteps
    {
        private readonly IntegracionTestContext _context;

        public ClientesIntegracionSteps(IntegracionTestContext context)
        {
            _context = context;
        }

        [Given(@"que la base de datos está disponible")]
        public void GivenQueLaBaseDeDatosEstaDisponible()
        {
            _context.DbContext.Should().NotBeNull();
        }

        [Given(@"que se tiene un nuevo cliente con Nombre ""(.*)"", Apellido ""(.*)"", Teléfono ""(.*)"" y Correo ""(.*)""")]
        public void GivenQueSeTieneUnNuevoClienteConDatos(string nombre, string apellido, string telefono, string correo)
        {
            _context.ClienteActual = new Cliente
            {
                Nombre = nombre,
                Apellido = apellido,
                Telefono = telefono,
                Correo = correo,
                Estado = 1,
                FechaCreacion = DateTime.Now
            };
        }

        [When(@"guardo el cliente en la base de datos")]
        public async Task WhenGuardoElClienteEnLaBaseDeDatos()
        {
            try
            {
                // Validar el cliente
                var validationContext = new ValidationContext(_context.ClienteActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ClienteActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                await _context.ClienteRepository.AddAsync(_context.ClienteActual);
                await _context.ClienteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [When(@"intento guardar el cliente en la base de datos")]
        public async Task WhenIntentoGuardarElClienteEnLaBaseDeDatos()
        {
            try
            {
                var validationContext = new ValidationContext(_context.ClienteActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ClienteActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                await _context.ClienteRepository.AddAsync(_context.ClienteActual);
                await _context.ClienteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe devolver un Id válido")]
        public void ThenElSistemaDebeDevoverUnIdValido()
        {
            _context.ClienteActual.Should().NotBeNull();
            _context.ClienteActual!.Id.Should().BeGreaterThan((short)0);
        }

        [Then(@"el cliente con Correo ""(.*)"" debe existir en la base de datos")]
        public async Task ThenElClienteConCorreoDebeExistirEnLaBaseDeDatos(string correo)
        {
            var clientes = await _context.ClienteRepository.FindAsync(c => c.Correo == correo);
            clientes.Should().NotBeEmpty();
            clientes.First().Correo.Should().Be(correo);
        }

        [Then(@"el sistema debe rechazar el cliente por validación")]
        public void ThenElSistemaDebeRechazarElClientePorValidacion()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [Given(@"que existe un cliente con Id ""(.*)"", Nombre ""(.*)"", Apellido ""(.*)"", Teléfono ""(.*)"" y Correo ""(.*)""")]
        public async Task GivenQueExisteUnClienteConDatos(string id, string nombre, string apellido, string telefono, string correo)
        {
            var cliente = new Cliente
            {
                Id = short.Parse(id),
                Nombre = nombre,
                Apellido = apellido,
                Telefono = telefono,
                Correo = correo,
                Estado = 1,
                FechaCreacion = DateTime.Now
            };

            _context.DbContext.Clientes.Add(cliente);
            await _context.DbContext.SaveChangesAsync();
            _context.ClienteActual = cliente;
        }

        [When(@"actualizo el Teléfono del cliente a ""(.*)""")]
        public async Task WhenActualizoElTelefonoDelClienteA(string nuevoTelefono)
        {
            try
            {
                _context.ClienteActual!.Telefono = nuevoTelefono;
                _context.ClienteActual.UltimaActualizacion = DateTime.Now;

                var validationContext = new ValidationContext(_context.ClienteActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ClienteActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.ClienteRepository.Update(_context.ClienteActual);
                await _context.ClienteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el cliente debe tener el nuevo Teléfono ""(.*)""")]
        public void ThenElClienteDebeTenerElNuevoTelefono(string telefono)
        {
            _context.ClienteActual!.Telefono.Should().Be(telefono);
        }

        [When(@"actualizo el Correo del cliente a ""(.*)""")]
        public async Task WhenActualizoElCorreoDelClienteA(string nuevoCorreo)
        {
            try
            {
                _context.ClienteActual!.Correo = nuevoCorreo;
                _context.ClienteActual.UltimaActualizacion = DateTime.Now;

                var validationContext = new ValidationContext(_context.ClienteActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ClienteActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.ClienteRepository.Update(_context.ClienteActual);
                await _context.ClienteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el cliente debe tener el nuevo Correo ""(.*)""")]
        public void ThenElClienteDebeTenerElNuevoCorreo(string correo)
        {
            _context.ClienteActual!.Correo.Should().Be(correo);
        }

        [When(@"intento actualizar el Correo del cliente a ""(.*)""")]
        public async Task WhenIntentoActualizarElCorreoDelClienteA(string nuevoCorreo)
        {
            try
            {
                _context.ClienteActual!.Correo = nuevoCorreo;

                var validationContext = new ValidationContext(_context.ClienteActual);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(_context.ClienteActual, validationContext, validationResults, true);

                if (!isValid)
                {
                    _context.ValidationFailed = true;
                    return;
                }

                _context.ClienteRepository.Update(_context.ClienteActual);
                await _context.ClienteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.LastException = ex;
                _context.ValidationFailed = true;
            }
        }

        [Then(@"el sistema debe rechazar la actualización por validación")]
        public void ThenElSistemaDebeRechazarLaActualizacionPorValidacion()
        {
            _context.ValidationFailed.Should().BeTrue();
        }

        [When(@"elimino el cliente usando soft delete")]
        public async Task WhenEliminoElClienteUsandoSoftDelete()
        {
            await _context.ClienteRepository.SoftDeleteAsync(_context.ClienteActual);
        }

        [Then(@"el cliente debe tener Estado igual a ""(.*)""")]
        public void ThenElClienteDebeTenerEstadoIgualA(string estado)
        {
            _context.ClienteActual!.Estado.Should().Be(sbyte.Parse(estado));
        }

        [Then(@"el cliente no debe aparecer en consultas de clientes activos")]
        public async Task ThenElClienteNoDebeAparecerEnConsultasDeClientesActivos()
        {
            var clientesActivos = await _context.ClienteRepository.GetAllAsync();
            clientesActivos.Should().NotContain(c => c.Id == _context.ClienteActual!.Id && c.Estado == 0);
        }

        [Given(@"que existen varios clientes registrados con estado activo")]
        public async Task GivenQueExistenVariosClientesRegistradosConEstadoActivo()
        {
            var clientes = new[]
            {
                new Cliente { Id = 100, Nombre = "Test1", Apellido = "Cliente1", Correo = "test1@mail.com", Estado = 1, FechaCreacion = DateTime.Now },
                new Cliente { Id = 101, Nombre = "Test2", Apellido = "Cliente2", Correo = "test2@mail.com", Estado = 1, FechaCreacion = DateTime.Now },
                new Cliente { Id = 102, Nombre = "Test3", Apellido = "Cliente3", Correo = "test3@mail.com", Estado = 1, FechaCreacion = DateTime.Now }
            };

            _context.DbContext.Clientes.AddRange(clientes);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"solicito el listado de todos los clientes")]
        public async Task WhenSolicitoElListadoDeTodosLosClientes()
        {
            var clientes = await _context.ClienteRepository.GetAllAsync();
            // Guardamos temporalmente en el contexto para validar
            _context.DbContext.ChangeTracker.Clear();
        }

        [Then(@"el sistema debe devolver al menos un registro")]
        public async Task ThenElSistemaDebeDevoverAlMenosUnRegistro()
        {
            var clientes = await _context.ClienteRepository.GetAllAsync();
            clientes.Should().NotBeEmpty();
        }

        [Then(@"todos los clientes deben tener Estado igual a ""(.*)""")]
        public async Task ThenTodosLosClientesDebenTenerEstadoIgualA(string estado)
        {
            var clientes = await _context.ClienteRepository.GetAllAsync();
            clientes.Should().OnlyContain(c => c.Estado == sbyte.Parse(estado));
        }
    }
}
