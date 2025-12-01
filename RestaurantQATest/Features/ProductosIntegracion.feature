Feature: Gestión de Productos con Pruebas de Integración
  Validar el comportamiento de las operaciones CRUD del repositorio ProductoRepository 
  contra la base de datos a través de pruebas de Integración.

  Background:
    Given que la base de datos está disponible

  # -------------------------
  #      INSERT - Happy Path 1
  # -------------------------
  Scenario: Registrar un nuevo producto correctamente
    Given que se tiene un nuevo producto con Nombre "Pizza Margarita", Precio "45.50", Stock "100", Descripcion "Pizza con queso y tomate" y CategoriaId "1"
    When guardo el producto en la base de datos
    Then el sistema debe devolver un Id válido para el producto
    And el producto con Nombre "Pizza Margarita" debe existir en la base de datos

  # -------------------------
  #      INSERT - Happy Path 2
  # -------------------------
  Scenario: Registrar un segundo producto correctamente
    Given que se tiene un nuevo producto con Nombre "Hamburguesa", Precio "35.00", Stock "50", Descripcion "Hamburguesa con carne y vegetales" y CategoriaId "1"
    When guardo el producto en la base de datos
    Then el sistema debe devolver un Id válido para el producto
    And el producto con Nombre "Hamburguesa" debe existir en la base de datos

  # -------------------------
  #      INSERT - Unhappy Path
  # -------------------------
  Scenario: Fallar al registrar producto con precio inválido
    Given que se tiene un nuevo producto con Nombre "Producto", Precio "1500.00", Stock "10", Descripcion "Descripcion del producto" y CategoriaId "1"
    When intento guardar el producto con precio fuera de rango en la base de datos
    Then el sistema debe rechazar el producto por validación de precio

  # -------------------------
  #      UPDATE - Happy Path 1
  # -------------------------
  Scenario: Actualizar el precio de un producto existente
    Given que existe un producto con Id "1", Nombre "Ensalada", Precio "25.00", Stock "30" y Descripcion "Ensalada fresca"
    When actualizo el Precio del producto a "30.00"
    Then el producto debe tener el nuevo Precio "30.00"

  # -------------------------
  #      UPDATE - Happy Path 2
  # -------------------------
  Scenario: Actualizar el stock de un producto existente
    Given que existe un producto con Id "2", Nombre "Refresco", Precio "10.00", Stock "100" y Descripcion "Bebida refrescante"
    When actualizo el Stock del producto a "150"
    Then el producto debe tener el nuevo Stock "150"

  # -------------------------
  #      UPDATE - Unhappy Path
  # -------------------------
  Scenario: Fallar al actualizar producto con stock negativo
    Given que existe un producto con Id "3", Nombre "Postre", Precio "20.00", Stock "50" y Descripcion "Postre delicioso"
    When intento actualizar el Stock del producto a "-10"
    Then el sistema debe rechazar la actualización del producto por validación

  # -------------------------
  #       DELETE - Happy Path
  # -------------------------
  Scenario: Eliminar un producto con soft delete
    Given que existe un producto con Id "4", Nombre "Snack", Precio "15.00", Stock "80" y Descripcion "Snack crujiente"
    When elimino el producto usando soft delete
    Then el producto debe tener Estado igual a "0"
    And el producto no debe aparecer en consultas de productos activos

  # -------------------------
  #       SELECT ALL - Happy Path
  # -------------------------
  Scenario: Listar todos los productos activos
    Given que existen varios productos registrados con estado activo
    When solicito el listado de todos los productos
    Then el sistema debe devolver al menos un producto
    And todos los productos deben tener Estado igual a "1"
