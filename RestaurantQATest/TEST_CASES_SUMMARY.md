# Resumen de Escenarios BDD - Test Cases

## Clientes (9 Escenarios)

### INSERT
1. ✅ **Happy Path 1**: Registrar un nuevo cliente con datos válidos (Juan Pérez)
2. ✅ **Happy Path 2**: Registrar un segundo cliente correctamente (María González)
3. ❌ **Unhappy Path**: Fallar al registrar cliente con nombre inválido (123invalid)

### UPDATE
4. ✅ **Happy Path 1**: Actualizar el teléfono de un cliente existente
5. ✅ **Happy Path 2**: Actualizar el correo de un cliente existente
6. ❌ **Unhappy Path**: Fallar al actualizar cliente con correo inválido

### DELETE
7. ✅ **Happy Path**: Eliminar un cliente con soft delete

### SELECT
8. ✅ **Happy Path**: Listar todos los clientes activos

---

## Pedidos (10 Escenarios)

### PROCESO PRINCIPAL
1. ✅ **Happy Path**: Crear un pedido completo con cliente, productos y repartidor

### INSERT
2. ✅ **Happy Path 1**: Registrar un nuevo pedido correctamente (Total 75.50)
3. ✅ **Happy Path 2**: Registrar un segundo pedido correctamente (Total 100.00)
4. ❌ **Unhappy Path**: Fallar al registrar pedido con cliente inexistente

### UPDATE
5. ✅ **Happy Path 1**: Actualizar el estado de un pedido existente
6. ✅ **Happy Path 2**: Actualizar el total de un pedido existente
7. ❌ **Unhappy Path**: Fallar al actualizar pedido con total negativo

### DELETE
8. ✅ **Happy Path**: Eliminar un pedido con soft delete

### SELECT
9. ✅ **Happy Path**: Listar todos los pedidos activos

---

## Productos (9 Escenarios)

### INSERT
1. ✅ **Happy Path 1**: Registrar un nuevo producto correctamente (Pizza Margarita)
2. ✅ **Happy Path 2**: Registrar un segundo producto correctamente (Hamburguesa)
3. ❌ **Unhappy Path**: Fallar al registrar producto con precio inválido (1500.00 fuera de rango)

### UPDATE
4. ✅ **Happy Path 1**: Actualizar el precio de un producto existente
5. ✅ **Happy Path 2**: Actualizar el stock de un producto existente
6. ❌ **Unhappy Path**: Fallar al actualizar producto con stock negativo

### DELETE
7. ✅ **Happy Path**: Eliminar un producto con soft delete

### SELECT
8. ✅ **Happy Path**: Listar todos los productos activos

---

## Repartidores (9 Escenarios)

### INSERT
1. ✅ **Happy Path 1**: Registrar un nuevo repartidor correctamente (Carlos Rodríguez)
2. ✅ **Happy Path 2**: Registrar un segundo repartidor correctamente (Laura Fernández)
3. ❌ **Unhappy Path**: Fallar al registrar repartidor con teléfono inválido (1234)

### UPDATE
4. ✅ **Happy Path 1**: Actualizar el estado de entrega de un repartidor existente
5. ✅ **Happy Path 2**: Actualizar el tipo de transporte de un repartidor existente
6. ❌ **Unhappy Path**: Fallar al actualizar repartidor con nombre inválido (123Invalid)

### DELETE
7. ✅ **Happy Path**: Eliminar un repartidor con soft delete

### SELECT
8. ✅ **Happy Path**: Listar todos los repartidores activos

---

## Resumen Total

- **Total de Escenarios**: 37
- **Happy Path**: 29 escenarios
- **Unhappy Path**: 8 escenarios

### Distribución por Operación

| Operación | Clientes | Pedidos | Productos | Repartidores | Total |
|-----------|----------|---------|-----------|--------------|-------|
| INSERT    | 3        | 3       | 3         | 3            | 12    |
| UPDATE    | 3        | 3       | 3         | 3            | 12    |
| DELETE    | 1        | 1       | 1         | 1            | 4     |
| SELECT    | 1        | 1       | 1         | 1            | 4     |
| Proceso   | -        | 1       | -         | -            | 1     |
| **Total** | **8**    | **9**   | **8**     | **8**        | **37**|

---

## Validaciones Cubiertas

### Clientes
- ❌ Nombre con números (unhappy)
- ❌ Correo inválido (unhappy)
- ✅ Teléfono válido 7-8 dígitos
- ✅ Soft delete
- ✅ Listado de activos

### Productos
- ❌ Precio fuera de rango >1000 (unhappy)
- ❌ Stock negativo (unhappy)
- ✅ Creación con categoría
- ✅ Actualización de precio y stock
- ✅ Soft delete
- ✅ Listado de activos

### Repartidores
- ❌ Teléfono inválido <7 dígitos (unhappy)
- ❌ Nombre con números (unhappy)
- ✅ Actualización de estado de entrega
- ✅ Actualización de tipo de transporte
- ✅ Soft delete
- ✅ Listado de activos

### Pedidos
- ❌ Cliente inexistente (unhappy)
- ❌ Total negativo (unhappy)
- ✅ Proceso completo con relaciones
- ✅ Actualización de estado
- ✅ Actualización de total
- ✅ Soft delete
- ✅ Listado de activos

---

## Cumplimiento de Requisitos

✅ **Proceso Principal**: 1 Happy Path implementado para Pedidos
✅ **INSERT**: 2 Happy Path + 1 Unhappy Path para cada tabla
✅ **UPDATE**: 2 Happy Path + 1 Unhappy Path para cada tabla
✅ **DELETE**: 1 Happy Path (soft delete) para cada tabla
✅ **SELECT**: 1 Happy Path para cada tabla

**Todos los requisitos han sido cumplidos correctamente.**
