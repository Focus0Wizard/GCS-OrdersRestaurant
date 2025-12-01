# Pruebas BDD con SpecFlow + Gherkin + C#

Este documento describe las pruebas de integración implementadas usando BDD (Behavior-Driven Development) con SpecFlow, Gherkin y C#.

## Estructura de Pruebas

Se han creado pruebas de integración para las 4 tablas principales del sistema:

### 1. **Clientes** (`Features/ClientesIntegracion.feature`)
- **INSERT**: 2 escenarios Happy Path + 1 Unhappy Path
- **UPDATE**: 2 escenarios Happy Path + 1 Unhappy Path
- **DELETE**: 1 escenario Happy Path (soft delete)
- **SELECT**: 1 escenario Happy Path

### 2. **Pedidos** (`Features/PedidosIntegracion.feature`)
- **Proceso Principal**: 1 escenario Happy Path (creación completa de pedido con cliente, producto y repartidor)
- **INSERT**: 2 escenarios Happy Path + 1 Unhappy Path
- **UPDATE**: 2 escenarios Happy Path + 1 Unhappy Path
- **DELETE**: 1 escenario Happy Path (soft delete)
- **SELECT**: 1 escenario Happy Path

### 3. **Productos** (`Features/ProductosIntegracion.feature`)
- **INSERT**: 2 escenarios Happy Path + 1 Unhappy Path
- **UPDATE**: 2 escenarios Happy Path + 1 Unhappy Path
- **DELETE**: 1 escenario Happy Path (soft delete)
- **SELECT**: 1 escenario Happy Path

### 4. **Repartidores** (`Features/RepartidoresIntegracion.feature`)
- **INSERT**: 2 escenarios Happy Path + 1 Unhappy Path
- **UPDATE**: 2 escenarios Happy Path + 1 Unhappy Path
- **DELETE**: 1 escenario Happy Path (soft delete)
- **SELECT**: 1 escenario Happy Path

## Estructura de Archivos

```
RestaurantQATest/
├── Features/
│   ├── ClientesIntegracion.feature
│   ├── PedidosIntegracion.feature
│   ├── ProductosIntegracion.feature
│   └── RepartidoresIntegracion.feature
├── BDD/
│   ├── Steps/
│   │   ├── ClientesIntegracionSteps.cs
│   │   ├── PedidosIntegracionSteps.cs
│   │   ├── ProductosIntegracionSteps.cs
│   │   └── RepartidoresIntegracionSteps.cs
│   ├── Support/
│   │   └── IntegracionTestContext.cs
│   └── Hooks/
│       └── IntegracionHooks.cs
├── specflow.json
└── RestaurantQATest.csproj
```

## Tecnologías Utilizadas

- **SpecFlow 4.0.31-beta**: Framework BDD para .NET
- **Gherkin**: Lenguaje de especificación de comportamiento
- **xUnit**: Framework de pruebas unitarias
- **FluentAssertions**: Biblioteca de aserciones fluidas
- **Entity Framework Core InMemory**: Base de datos en memoria para pruebas

## Características de las Pruebas

### Happy Path vs Unhappy Path

- **Happy Path**: Escenarios donde todo funciona correctamente con datos válidos
- **Unhappy Path**: Escenarios donde se prueban validaciones con datos inválidos

### Validaciones Implementadas

#### Clientes
- Nombre y Apellido: Solo letras, iniciar con mayúscula
- Teléfono: 7-8 dígitos, comenzar con 6 o 7
- Correo: Formato de email válido

#### Productos
- Precio: Entre 0.01 y 1000
- Stock: Entre 0 y 150
- Nombre: 4-20 caracteres
- Descripción: 5-100 caracteres

#### Repartidores
- Nombre y Apellido: Solo letras, iniciar con mayúscula
- Teléfono: 7-8 dígitos, comenzar con 6 o 7

#### Pedidos
- Total: No negativo
- ClienteId: Debe existir en la base de datos
- Relaciones válidas con Cliente, Usuario y Repartidor

### Soft Delete

Todas las entidades implementan soft delete, cambiando el campo `Estado` a `0` en lugar de eliminar físicamente el registro.

## Cómo Ejecutar las Pruebas

### Prerrequisitos

- .NET 9.0 SDK
- Visual Studio 2022 o VS Code

### Ejecutar todas las pruebas

```bash
dotnet test RestaurantQATest/RestaurantQATest.csproj
```

### Ejecutar pruebas específicas por Feature

```bash
# Clientes
dotnet test --filter "FullyQualifiedName~ClientesIntegracion"

# Pedidos
dotnet test --filter "FullyQualifiedName~PedidosIntegracion"

# Productos
dotnet test --filter "FullyQualifiedName~ProductosIntegracion"

# Repartidores
dotnet test --filter "FullyQualifiedName~RepartidoresIntegracion"
```

### Ejecutar con reporte detallado

```bash
dotnet test RestaurantQATest/RestaurantQATest.csproj --logger "console;verbosity=detailed"
```

## Ejemplo de Escenario Gherkin

```gherkin
Feature: Gestión de Clientes con Pruebas de Integración
  Validar el comportamiento de las operaciones CRUD del repositorio ClienteRepository 
  contra la base de datos a través de pruebas de Integración.

  Background:
    Given que la base de datos está disponible

  Scenario: Registrar un nuevo cliente correctamente con datos válidos
    Given que se tiene un nuevo cliente con Nombre "Juan", Apellido "Pérez", Teléfono "70123456" y Correo "juan.perez@mail.com"
    When guardo el cliente en la base de datos
    Then el sistema debe devolver un Id válido
    And el cliente con Correo "juan.perez@mail.com" debe existir en la base de datos
```

## Arquitectura de Pruebas

### IntegracionTestContext

Contexto compartido entre todos los escenarios que contiene:
- Instancia de `MyDbContext` (InMemory)
- Repositorios genéricos para cada entidad
- Entidades actuales en uso durante el escenario
- Flags de validación y excepciones

### Hooks

`IntegracionHooks.cs` maneja el ciclo de vida de los escenarios:
- **BeforeScenario**: Limpia el estado antes de cada escenario
- **AfterScenario**: Libera recursos después de cada escenario

### Step Definitions

Cada archivo de Steps implementa los pasos Given/When/Then usando expresiones regulares para capturar parámetros de los escenarios.

## Beneficios de BDD

1. **Documentación Ejecutable**: Los features sirven como documentación que siempre está actualizada
2. **Lenguaje Común**: Gherkin permite que técnicos y no técnicos entiendan las pruebas
3. **Cobertura Completa**: Se prueban todos los casos de uso importantes
4. **Mantenibilidad**: Los steps se pueden reutilizar entre diferentes escenarios
5. **Validación de Negocio**: Se verifica que el sistema cumple con los requisitos del negocio

## Reglas Aplicadas

Según los requisitos:

- **Proceso Principal**: 1 Happy Path (Pedidos - creación completa)
- **INSERT/UPDATE**: 2 Happy Path + 1 Unhappy Path para Clientes, Productos y Repartidores
- **DELETE/SELECT**: Solo Happy Path para todas las entidades

## Reportes

Los reportes de ejecución se pueden generar usando herramientas como LivingDoc o SpecFlow+ LivingDoc para visualizar los resultados en formato HTML.

## Notas Adicionales

- Todas las pruebas usan Entity Framework Core InMemory para evitar dependencias de base de datos externa
- Las validaciones se aplican usando Data Annotations y ValidationContext
- El contexto de pruebas se crea con un GUID único para cada escenario, garantizando aislamiento
- Se usa FluentAssertions para aserciones más legibles y expresivas

## Contacto y Soporte

Para preguntas o problemas con las pruebas, consulte la documentación de SpecFlow:
- [SpecFlow Documentation](https://docs.specflow.org/)
- [Gherkin Reference](https://cucumber.io/docs/gherkin/reference/)
