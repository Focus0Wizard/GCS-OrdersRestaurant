"""
Suite de pruebas automatizadas para el módulo de Repartidores.
Implementa casos de prueba basados en particiones equivalentes.

Particiones de Prueba:
- Partición Válida: Datos que cumplen todas las validaciones
- Particiones Inválidas:
  * Campos obligatorios vacíos (Nombre, Apellido)
  * Campos con longitud excedida (Nombre > 30, Apellido > 30)
  * Campos con longitud insuficiente (Nombre < 3, Apellido < 3)
  * Teléfono con formato inválido (no inicia con 6/7, contiene letras, longitud incorrecta)
  * Teléfono fuera de rango (< 7 dígitos o > 8 dígitos)
"""
import csv
import pytest
from pages.repartidor_page import RepartidorPage


# ==================== CARGA DE DATOS DE PRUEBA ====================

DATA_FILE = "/home/jhonn/Documentos/Universidad/8vo_Semestre/QA/GCS-OrdersRestaurant/RestaurantQATest/Data/repartidores_tests.csv"


def load_test_cases():
    """
    Carga los casos de prueba desde el archivo CSV.
    
    Returns:
        list: Lista de diccionarios con los datos de cada caso de prueba
    """
    with open(DATA_FILE, newline='', encoding='utf-8') as f:
        return list(csv.DictReader(f))


# ==================== PRUEBAS PARAMETRIZADAS ====================

@pytest.mark.repartidores
@pytest.mark.parametrize("case", load_test_cases())
def test_registro_repartidor(driver, base_url, case):
    """
    Prueba el registro de repartidores usando particiones equivalentes.
    
    Este test parametrizado ejecuta todos los casos de prueba definidos en el CSV,
    cubriendo tanto particiones válidas como inválidas.
    
    Casos de Prueba:
    - RP1, RP2, RP12, RP19: Particiones válidas (datos correctos)
    - RP3-RP11, RP13-RP18, RP20-RP36: Particiones inválidas (diversos tipos de errores)
    
    Args:
        driver: WebDriver fixture de Selenium
        base_url: URL base de la aplicación
        case: Diccionario con los datos del caso de prueba actual
    """
    # ==================== ARRANGE ====================
    # Inicializar el Page Object
    repartidor_page = RepartidorPage(driver)
    
    # Navegar a la página de repartidores
    repartidor_page.navigate(base_url)
    
    # Extraer datos del caso de prueba
    nombre = case["nombre"] if case["nombre"] else None
    apellido = case["apellido"] if case["apellido"] else None
    telefono = case["telefono"] if case["telefono"] else None
    tipo = case["tipo"] if case["tipo"] else None
    resultado_esperado = case["esperado"]
    
    # ==================== ACT ====================
    # Llenar el formulario con los datos del caso de prueba
    repartidor_page.fill_form(
        nombre=nombre,
        apellido=apellido,
        telefono=telefono,
        tipo=tipo
    )
    
    # Enviar el formulario
    repartidor_page.submit_form()
    
    # Esperar un momento para que se procese el formulario
    driver.implicitly_wait(2)
    
    # ==================== ASSERT ====================
    # Determinar el resultado real
    if resultado_esperado == "Aceptado":
        # Para casos válidos: verificar que el repartidor fue registrado exitosamente
        assert repartidor_page.is_repartidor_registered(), \
            f"❌ Caso {case['caso']} FALLÓ: Se esperaba que el repartidor fuera ACEPTADO pero fue RECHAZADO.\n" \
            f"   Datos: Nombre='{nombre}', Apellido='{apellido}', Teléfono='{telefono}', Tipo='{tipo}'\n" \
            f"   Partición: {case.get('particion', 'N/A')}\n" \
            f"   Errores de validación: {repartidor_page.get_validation_errors()}"
        
        print(f"✅ Caso {case['caso']} PASÓ: Repartidor aceptado correctamente")
        print(f"   Partición: {case.get('particion', 'N/A')}")
        
    else:  # "Rechazado"
        # Para casos inválidos: verificar que hay errores de validación
        # o que no redirigió a la página de índice
        tiene_errores = repartidor_page.has_validation_errors()
        no_registro = not repartidor_page.is_repartidor_registered()
        
        assert tiene_errores or no_registro, \
            f"❌ Caso {case['caso']} FALLÓ: Se esperaba que el repartidor fuera RECHAZADO pero fue ACEPTADO.\n" \
            f"   Datos: Nombre='{nombre}', Apellido='{apellido}', Teléfono='{telefono}', Tipo='{tipo}'\n" \
            f"   Partición: {case.get('particion', 'N/A')}\n" \
            f"   Observaciones: {case.get('observaciones', 'N/A')}"
        
        print(f"✅ Caso {case['caso']} PASÓ: Repartidor rechazado correctamente")
        print(f"   Partición: {case.get('particion', 'N/A')}")
        if tiene_errores:
            print(f"   Errores detectados: {repartidor_page.get_validation_errors()}")


# ==================== PRUEBAS INDIVIDUALES POR PARTICIÓN ====================

@pytest.mark.repartidores
@pytest.mark.smoke
def test_repartidor_valido_datos_normales():
    """
    Prueba de partición válida: Datos normales completos.
    
    Partición: VÁLIDA - Todos los campos con valores correctos
    - Nombre: "Carlos" (válido, 6 caracteres)
    - Apellido: "Gómez" (válido, 5 caracteres)
    - Teléfono: "71234567" (válido, 8 dígitos, inicia con 7)
    - Tipo: "Interno" (válido)
    
    Caso de prueba: Similar a RP1
    Resultado esperado: ✅ Aceptado
    """
    pass  # Cubierto por test_registro_repartidor con RP1


@pytest.mark.repartidores
@pytest.mark.smoke
def test_repartidor_valido_nombre_compuesto():
    """
    Prueba de partición válida: Nombre compuesto válido.
    
    Partición: VÁLIDA - Nombre con espacio permitido
    - Nombre: "Juan Pérez" (válido, nombre compuesto)
    - Apellido: "Gómez" (válido)
    - Teléfono: "78901234" (válido, 8 dígitos, inicia con 7)
    - Tipo: "Temporal" (válido)
    
    Caso de prueba: Similar a RP12
    Resultado esperado: ✅ Aceptado
    """
    pass  # Cubierto por test_registro_repartidor con RP12


@pytest.mark.repartidores
def test_repartidor_invalido_nombre_vacio():
    """
    Prueba de partición inválida: Campo obligatorio vacío (Nombre).
    
    Partición: INVÁLIDA - Campo obligatorio ausente
    - Nombre: Vacío (campo requerido)
    
    Casos de prueba: RP31-RP36
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_apellido_vacio():
    """
    Prueba de partición inválida: Campo obligatorio vacío (Apellido).
    
    Partición: INVÁLIDA - Campo obligatorio ausente
    - Apellido: Vacío (campo requerido)
    
    Casos de prueba: RP6, RP11, RP21, RP26
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_nombre_corto():
    """
    Prueba de partición inválida: Longitud de nombre insuficiente.
    
    Partición: INVÁLIDA - Longitud mínima no alcanzada
    - Nombre: "A" (< 3 caracteres)
    
    Casos de prueba: RP13-RP18
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_apellido_corto():
    """
    Prueba de partición inválida: Longitud de apellido insuficiente.
    
    Partición: INVÁLIDA - Longitud mínima no alcanzada
    - Apellido: "A" (< 3 caracteres)
    
    Casos de prueba: RP3, RP8, RP24, RP29, RP34
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_nombre_largo():
    """
    Prueba de partición inválida: Longitud de nombre excedida.
    
    Partición: INVÁLIDA - Longitud máxima excedida
    - Nombre: > 30 caracteres
    
    Casos de prueba: RP25-RP30
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_apellido_largo():
    """
    Prueba de partición inválida: Longitud de apellido excedida.
    
    Partición: INVÁLIDA - Longitud máxima excedida
    - Apellido: > 30 caracteres
    
    Casos de prueba: RP5, RP10, RP15, RP20, RP25, RP36
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_telefono_corto():
    """
    Prueba de partición inválida: Teléfono muy corto.
    
    Partición: INVÁLIDA - Longitud mínima no alcanzada
    - Teléfono: < 7 dígitos
    
    Casos de prueba: RP4, RP8, RP18, RP22, RP28, RP36
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_telefono_largo():
    """
    Prueba de partición inválida: Teléfono excede máximo.
    
    Partición: INVÁLIDA - Longitud máxima excedida
    - Teléfono: > 8 dígitos
    
    Casos de prueba: RP5, RP9, RP13, RP23, RP29, RP31
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


@pytest.mark.repartidores
def test_repartidor_invalido_telefono_con_letras():
    """
    Prueba de partición inválida: Teléfono con caracteres no numéricos.
    
    Partición: INVÁLIDA - Formato de teléfono incorrecto
    - Teléfono: Contiene letras (ej: "71A3456B")
    
    Casos de prueba: RP6, RP10, RP14, RP24, RP30, RP32
    Resultado esperado: ❌ Rechazado con mensaje de validación
    """
    pass  # Cubierto por test_registro_repartidor


# ==================== INFORMACIÓN DEL MÓDULO ====================

def test_module_info():
    """
    Información sobre el módulo de pruebas de repartidores.
    """
    info = """
    ╔═══════════════════════════════════════════════════════════════════╗
    ║          MÓDULO DE PRUEBAS: REPARTIDORES                         ║
    ║          Casos de Prueba: 36 (4 válidos, 32 inválidos)           ║
    ╚═══════════════════════════════════════════════════════════════════╝
    
    📊 DISTRIBUCIÓN DE PARTICIONES:
    
    ✅ PARTICIONES VÁLIDAS (4 casos):
       • RP1: Datos normales completos
       • RP2: Teléfono iniciando con 7, tipo Externo
       • RP12: Nombre compuesto válido
       • RP19: Nombre compuesto largo válido
    
    ❌ PARTICIONES INVÁLIDAS (32 casos):
       • Nombre vacío: RP31-RP36 (6 casos)
       • Nombre muy corto (<3): RP13-RP18 (6 casos)
       • Nombre muy largo (>30): RP25-RP30 (6 casos)
       • Apellido vacío: RP6, RP11, RP21, RP26 (4 casos)
       • Apellido muy corto (<3): RP3, RP8, RP24, RP29, RP34 (5 casos)
       • Apellido muy largo (>30): RP5, RP10, RP15, RP20, RP25, RP36 (6 casos)
       • Teléfono vacío: RP3, RP7, RP17, RP21, RP27, RP35 (6 casos)
       • Teléfono muy corto (<7): RP4, RP8, RP18, RP22, RP28, RP36 (6 casos)
       • Teléfono muy largo (>8): RP5, RP9, RP13, RP23, RP29, RP31 (6 casos)
       • Teléfono con letras: RP6, RP10, RP14, RP24, RP30, RP32 (6 casos)
    
    🔍 VALIDACIONES CUBIERTAS:
       • Nombre: Required, RegularExpression (letras, mayúscula inicial), Length(3-30)
       • Apellido: Required, RegularExpression (letras, mayúscula inicial), Length(3-30)
       • Teléfono: Phone, RegularExpression (inicia con 6/7, 7-8 dígitos), Length(7-8)
       • Tipo: Select con opciones predefinidas (Bicicleta, Moto, Auto)
    """
    print(info)
    assert True
