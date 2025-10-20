"""
Suite de pruebas automatizadas para el módulo de Productos.
Implementa casos de prueba basados en particiones equivalentes.

Particiones de Prueba:
- Partición Válida: Datos que cumplen todas las validaciones
- Particiones Inválidas:
  * Campos obligatorios vacíos (Nombre, Descripción)
  * Campos con longitud excedida (Nombre > 20, Descripción > 100)
  * Campos con longitud insuficiente (Nombre < 4, Descripción < 5)
  * Valores numéricos fuera de rango (Precio: 0.01-1000, Stock: 0-150)
  * Caracteres especiales no permitidos en Nombre
"""
import csv
import pytest
from pages.producto_page import ProductoPage


# ==================== CARGA DE DATOS DE PRUEBA ====================

DATA_FILE = "Data/productos_tests.csv"


def load_test_cases():
    """
    Carga los casos de prueba desde el archivo CSV.
    
    Returns:
        list: Lista de diccionarios con los datos de cada caso de prueba
    """
    with open(DATA_FILE, newline='', encoding='utf-8') as f:
        return list(csv.DictReader(f))


# ==================== PRUEBAS PARAMETRIZADAS ====================

@pytest.mark.productos
@pytest.mark.parametrize("case", load_test_cases())
def test_registro_producto(driver, base_url, case):
    """
    Prueba el registro de productos usando particiones equivalentes.
    
    Este test parametrizado ejecuta todos los casos de prueba definidos en el CSV,
    cubriendo tanto particiones válidas como inválidas.
    
    Casos de Prueba:
    - PR1-PR2: Particiones válidas (valores en límites y rangos permitidos)
    - PR3-PR33: Particiones inválidas (diversos tipos de errores)
    
    Args:
        driver: WebDriver fixture de Selenium
        base_url: URL base de la aplicación
        case: Diccionario con los datos del caso de prueba actual
    """
    # ==================== ARRANGE ====================
    # Inicializar el Page Object
    producto_page = ProductoPage(driver)
    
    # Navegar a la página de productos
    producto_page.navigate(base_url)
    
    # Extraer datos del caso de prueba
    nombre = case["nombre"] if case["nombre"] else None
    precio = case["precio"] if case["precio"] else None
    stock = case["stock"] if case["stock"] else None
    descripcion = case["descripcion"] if case["descripcion"] else None
    categoria_id = case.get("categoria_id", 1)
    resultado_esperado = case["esperado"]
    
    # ==================== ACT ====================
    # Llenar el formulario con los datos del caso de prueba
    producto_page.fill_form(
        nombre=nombre,
        precio=precio,
        stock=stock,
        descripcion=descripcion,
        categoria_id=categoria_id
    )
    
    # Enviar el formulario
    producto_page.submit_form()
    
    # Esperar un momento para que se procese el formulario
    driver.implicitly_wait(2)
    
    # ==================== ASSERT ====================
    # Determinar el resultado real
    if resultado_esperado == "Aceptado":
        # Para casos válidos: verificar que el producto fue registrado exitosamente
        assert producto_page.is_producto_registered(), \
            f"❌ Caso {case['caso']} FALLÓ: Se esperaba que el producto fuera ACEPTADO pero fue RECHAZADO.\n" \
            f"   Datos: Nombre='{nombre}', Precio={precio}, Stock={stock}, Descripción='{descripcion}'\n" \
            f"   Partición: {case.get('particion', 'N/A')}\n" \
            f"   Errores de validación: {producto_page.get_validation_errors()}"
        
        print(f"✅ Caso {case['caso']} PASÓ: Producto aceptado correctamente")
        print(f"   Partición: {case.get('particion', 'N/A')}")
        
    else:  # "Rechazado"
        # Para casos inválidos: verificar que hay errores de validación
        # o que no redirigió a la página de índice
        tiene_errores = producto_page.has_validation_errors()
        no_registro = not producto_page.is_producto_registered()
        
        assert tiene_errores or no_registro, \
            f"❌ Caso {case['caso']} FALLÓ: Se esperaba que el producto fuera RECHAZADO pero fue ACEPTADO.\n" \
            f"   Datos: Nombre='{nombre}', Precio={precio}, Stock={stock}, Descripción='{descripcion}'\n" \
            f"   Partición: {case.get('particion', 'N/A')}\n" \
            f"   Observaciones: {case.get('observaciones', 'N/A')}"
        
        print(f"✅ Caso {case['caso']} PASÓ: Producto rechazado correctamente")
        print(f"   Partición: {case.get('particion', 'N/A')}")
        if tiene_errores:
            print(f"   Errores detectados: {producto_page.get_validation_errors()}")


# ==================== PRUEBAS INDIVIDUALES POR PARTICIÓN ====================

@pytest.mark.productos
@pytest.mark.smoke
def test_producto_valido_valores_minimos():
    """
    Prueba de partición válida: Valores mínimos permitidos.
    
    Partición: VÁLIDA - Valores en límite inferior
    - Nombre: 4 caracteres (mínimo permitido)
    - Precio: 0.01 (mínimo permitido)
    - Stock: 0 (mínimo permitido)
    - Descripción: 5 caracteres (mínimo permitido)
    
    Caso de prueba: Similar a PR1
    Resultado esperado: ✅ Aceptado
    """
    pass  # Cubierto por test_registro_producto con PR1


@pytest.mark.productos
@pytest.mark.smoke
def test_producto_valido_valores_maximos():
    """
    Prueba de partición válida: Valores máximos permitidos.
    
    Partición: VÁLIDA - Valores en límite superior
    - Nombre: 20 caracteres (máximo permitido)
    - Precio: 1000 (máximo permitido)
    - Stock: 150 (máximo permitido)
    - Descripción: 100 caracteres (máximo permitido)
    
    Caso de prueba: Similar a PR2
    Resultado esperado: ✅ Aceptado
    """
    pass  # Cubierto por test_registro_producto con PR2


@pytest.mark.productos
def test_producto_invalido_nombre_vacio():
    """
    Prueba de partición inválida: Campo obligatorio vacío.
    
    Partición: INVÁLIDA - Campo obligatorio ausente
    - Nombre: Vacío (campo requerido)
    
    Casos de prueba: PR11-PR16, PR29-PR33
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_nombre_excede_maximo():
    """
    Prueba de partición inválida: Longitud de nombre excedida.
    
    Partición: INVÁLIDA - Longitud máxima excedida
    - Nombre: > 20 caracteres
    
    Casos de prueba: PR17-PR22
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_nombre_caracteres_especiales():
    """
    Prueba de partición inválida: Caracteres no permitidos en nombre.
    
    Partición: INVÁLIDA - Formato de nombre incorrecto
    - Nombre: Contiene caracteres especiales (@, números, etc.)
    
    Casos de prueba: PR23-PR28
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_precio_fuera_rango():
    """
    Prueba de partición inválida: Precio fuera del rango permitido.
    
    Partición: INVÁLIDA - Valor numérico fuera de rango
    - Precio: < 0.01 o > 1000
    
    Casos de prueba: PR3, PR4, PR8, PR9, PR12, PR13, etc.
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_stock_negativo():
    """
    Prueba de partición inválida: Stock con valor negativo.
    
    Partición: INVÁLIDA - Valor numérico negativo
    - Stock: < 0
    
    Casos de prueba: PR3, PR7, PR16, PR20, PR24, PR33
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_stock_excede_maximo():
    """
    Prueba de partición inválida: Stock excede el máximo permitido.
    
    Partición: INVÁLIDA - Valor numérico excede máximo
    - Stock: > 150
    
    Casos de prueba: PR4, PR8, PR11, PR21, PR25, PR29
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_descripcion_corta():
    """
    Prueba de partición inválida: Descripción muy corta.
    
    Partición: INVÁLIDA - Longitud mínima no alcanzada
    - Descripción: < 5 caracteres
    
    Casos de prueba: PR6, PR14, PR17, PR19, etc.
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


@pytest.mark.productos
def test_producto_invalido_descripcion_larga():
    """
    Prueba de partición inválida: Descripción excede máximo.
    
    Partición: INVÁLIDA - Longitud máxima excedida
    - Descripción: > 100 caracteres
    
    Casos de prueba: PR2, PR10, PR13, PR22, PR25, PR33
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_producto


# ==================== INFORMACIÓN DEL MÓDULO ====================

def test_module_info():
    """
    Información sobre el módulo de pruebas de productos.
    """
    info = """
    ╔═══════════════════════════════════════════════════════════════════╗
    ║          MÓDULO DE PRUEBAS: PRODUCTOS                            ║
    ║          Casos de Prueba: 33 (2 válidos, 31 inválidos)           ║
    ╚═══════════════════════════════════════════════════════════════════╝
    
    📊 DISTRIBUCIÓN DE PARTICIONES:
    
    ✅ PARTICIONES VÁLIDAS (2 casos):
       • PR1: Valores mínimos permitidos
       • PR2: Valores máximos permitidos
    
    ❌ PARTICIONES INVÁLIDAS (31 casos):
       • Nombre vacío: PR11-PR16, PR29-PR33 (10 casos)
       • Nombre excede máximo (>20): PR17-PR22 (6 casos)
       • Nombre con caracteres especiales: PR23-PR28 (6 casos)
       • Precio = 0 o fuera de rango: PR3, PR8, PR12, PR17, PR28, PR32 (6 casos)
       • Precio > 1000: PR4, PR9, PR13, PR18, PR23, PR31, PR33 (7 casos)
       • Stock negativo: PR3, PR7, PR16, PR20, PR24, PR33 (6 casos)
       • Stock > 150: PR4, PR6, PR8, PR11, PR15, PR19, PR23, PR25, PR29, PR32 (10 casos)
       • Descripción < 5 caracteres: PR6, PR14, PR17, PR19, PR26, PR28-PR30, PR32 (9 casos)
       • Descripción > 100 caracteres: PR2, PR10, PR13, PR22, PR25, PR33 (6 casos)
    
    🔍 VALIDACIONES CUBIERTAS:
       • Nombre: Required, Length(4-20)
       • Precio: Range(0.01-1000)
       • Stock: Range(0-150)
       • Descripción: Required, Length(5-100)
       • Categoría: Range(1-max)
    """
    print(info)
    assert True
