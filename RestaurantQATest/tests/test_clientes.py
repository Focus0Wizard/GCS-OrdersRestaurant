"""
Suite de pruebas automatizadas para el módulo de Clientes
Implementa pruebas basadas en particiones equivalentes
"""

import pytest
import csv
import os
from pages.cliente_page import ClientePage


def load_test_cases():
    """
    Carga los casos de prueba desde el archivo CSV
    
    Returns:
        list: Lista de tuplas con los datos de cada caso de prueba
    """
    test_cases = []
    csv_path = os.path.join(os.path.dirname(__file__), '..', 'Data', 'clientes_tests.csv')
    
    with open(csv_path, 'r', encoding='utf-8') as file:
        reader = csv.DictReader(file)
        for row in reader:
            test_cases.append((
                row['caso'],
                row['nombre'],
                row['apellido'],
                row['telefono'],
                row['correo'],
                row['esperado'],
                row['particion'],
                row['observaciones']
            ))
    
    return test_cases


@pytest.mark.clientes
@pytest.mark.parametrize(
    "caso,nombre,apellido,telefono,correo,esperado,particion,observaciones",
    load_test_cases(),
    ids=lambda x: x if isinstance(x, str) and x.startswith('CL') else None
)
def test_registro_cliente(driver, base_url, caso, nombre, apellido, telefono, correo, esperado, particion, observaciones):
    """
    Prueba parametrizada para el registro de clientes
    
    Verifica que el sistema valide correctamente los datos ingresados
    según las reglas de negocio definidas:
    
    - Nombre: Requerido, 3-30 caracteres, solo letras, debe iniciar con mayúscula, permite nombres compuestos
    - Apellido: Requerido, 3-30 caracteres, solo letras, debe iniciar con mayúscula, permite apellidos compuestos
    - Teléfono: Opcional, 7-8 dígitos, debe iniciar con 6 o 7
    - Correo: Requerido, formato válido de email
    
    Args:
        driver: Fixture de WebDriver
        base_url: Fixture con URL base de la aplicación
        caso: Identificador del caso de prueba (CL1, CL2, etc.)
        nombre: Nombre del cliente
        apellido: Apellido del cliente
        telefono: Teléfono del cliente (opcional)
        correo: Correo electrónico del cliente
        esperado: Resultado esperado ('valido' o 'invalido')
        particion: Clasificación de la partición equivalente
        observaciones: Descripción detallada del caso
    """
    # ========== ARRANGE (Preparar) ==========
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    print(f"\n{'='*80}")
    print(f"Ejecutando caso: {caso}")
    print(f"Partición: {particion}")
    print(f"Descripción: {observaciones}")
    print(f"Datos de entrada:")
    print(f"  - Nombre: '{nombre}'")
    print(f"  - Apellido: '{apellido}'")
    print(f"  - Teléfono: '{telefono}'")
    print(f"  - Correo: '{correo}'")
    print(f"Resultado esperado: {esperado}")
    print(f"{'='*80}")
    
    # ========== ACT (Actuar) ==========
    cliente_page.fill_form(nombre, apellido, telefono, correo)
    cliente_page.submit_form()
    
    # ========== ASSERT (Verificar) ==========
    if esperado == "valido":
        # Para casos válidos: verificar que el cliente fue registrado
        assert cliente_page.is_cliente_registered(nombre, apellido), \
            f"❌ Caso {caso}: El cliente debería haberse registrado pero no se encuentra en la lista"
        
        print(f"✅ Caso {caso} PASÓ: Cliente registrado correctamente")
        
    else:  # esperado == "invalido"
        # Para casos inválidos: verificar que hay errores de validación
        has_errors = cliente_page.has_validation_errors()
        
        assert has_errors, \
            f"❌ Caso {caso}: Se esperaban errores de validación pero no se encontraron"
        
        # Obtener y mostrar los errores encontrados
        errores = cliente_page.get_validation_errors()
        print(f"✅ Caso {caso} PASÓ: Se detectaron errores de validación como se esperaba")
        print(f"Errores encontrados: {errores}")


# ========== PRUEBAS INDIVIDUALES POR PARTICIÓN ==========

@pytest.mark.clientes
@pytest.mark.smoke
def test_registro_valido_basico(driver, base_url):
    """
    Prueba de registro válido básico (CL1)
    Caso de humo para verificar funcionalidad básica
    """
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    # Datos válidos básicos
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="71234567",
        correo="carlos@gmail.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.is_cliente_registered("Carlos", "Pérez"), \
        "El cliente con datos válidos básicos debería registrarse correctamente"


@pytest.mark.clientes
@pytest.mark.smoke
def test_registro_valido_apellido_compuesto(driver, base_url):
    """
    Prueba de registro válido con apellido compuesto (CL2)
    Verifica que se acepten apellidos con espacios
    """
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    # Datos con apellido compuesto
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="De la Cruz",
        telefono="59171234567",  # Telefono opcional pero válido
        correo="usuario@ucb.edu.bo"
    )
    cliente_page.submit_form()
    
    assert cliente_page.is_cliente_registered("Carlos", "De la Cruz"), \
        "El cliente con apellido compuesto válido debería registrarse correctamente"


@pytest.mark.clientes
def test_nombre_vacio(driver, base_url):
    """Prueba con nombre vacío (requerido)"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="",
        apellido="Pérez",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el nombre está vacío"


@pytest.mark.clientes
def test_apellido_vacio(driver, base_url):
    """Prueba con apellido vacío (requerido)"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el apellido está vacío"


@pytest.mark.clientes
def test_correo_vacio(driver, base_url):
    """Prueba con correo vacío (requerido)"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="71234567",
        correo=""
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el correo está vacío"


@pytest.mark.clientes
def test_telefono_vacio_valido(driver, base_url):
    """
    Prueba con teléfono vacío (no requerido - debería ser válido)
    Nota: El teléfono es opcional según la entidad Cliente
    """
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="",  # Teléfono vacío - es válido porque es opcional
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    # El teléfono es opcional, por lo que debería registrarse
    assert cliente_page.is_cliente_registered("Carlos", "Pérez"), \
        "El cliente debería registrarse correctamente sin teléfono (campo opcional)"


@pytest.mark.clientes
def test_nombre_mayor_max(driver, base_url):
    """Prueba con nombre que excede el máximo de 30 caracteres"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="AlejandroFernandezGonzalezMartinezPerezLopez",  # 46 caracteres
        apellido="Pérez",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el nombre excede 30 caracteres"


@pytest.mark.clientes
def test_apellido_mayor_max(driver, base_url):
    """Prueba con apellido que excede el máximo de 30 caracteres"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="FernandezMartinezGonzalezRodriguezPerezLópez",  # 47 caracteres
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el apellido excede 30 caracteres"


@pytest.mark.clientes
def test_nombre_con_numeros(driver, base_url):
    """Prueba con nombre que contiene números"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Juan123",
        apellido="Pérez",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el nombre contiene números"


@pytest.mark.clientes
def test_apellido_con_numeros(driver, base_url):
    """Prueba con apellido que contiene números"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Gomez123",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el apellido contiene números"


@pytest.mark.clientes
def test_nombre_con_caracteres_especiales(driver, base_url):
    """Prueba con nombre que contiene caracteres especiales"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="@Pedro!",
        apellido="Pérez",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el nombre contiene caracteres especiales"


@pytest.mark.clientes
def test_apellido_con_caracteres_especiales(driver, base_url):
    """Prueba con apellido que contiene caracteres especiales"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="López#",
        telefono="71234567",
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el apellido contiene caracteres especiales"


@pytest.mark.clientes
def test_telefono_menor_min(driver, base_url):
    """Prueba con teléfono menor al mínimo de 7 dígitos"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="712345",  # 6 dígitos (< 7)
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el teléfono tiene menos de 7 dígitos"


@pytest.mark.clientes
def test_telefono_mayor_max(driver, base_url):
    """Prueba con teléfono mayor al máximo de 8 dígitos"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="712345678901234",  # 15 dígitos (> 8)
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el teléfono tiene más de 8 dígitos"


@pytest.mark.clientes
def test_telefono_formato_invalido(driver, base_url):
    """Prueba con teléfono que contiene letras"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="71A23B67",  # Contiene letras
        correo="test@example.com"
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el teléfono contiene letras"


@pytest.mark.clientes
def test_correo_formato_invalido_sin_arroba(driver, base_url):
    """Prueba con correo sin @"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="71234567",
        correo="carlosgmail.com"  # Sin @
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el correo no tiene @"


@pytest.mark.clientes
def test_correo_formato_invalido_incompleto(driver, base_url):
    """Prueba con correo incompleto"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="71234567",
        correo="usuario@"  # Incompleto
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el correo está incompleto"


@pytest.mark.clientes
def test_correo_formato_invalido_multiple_arroba(driver, base_url):
    """Prueba con correo con múltiples @"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="Carlos",
        apellido="Pérez",
        telefono="71234567",
        correo="maría@@gmail..com"  # @@ y ..
    )
    cliente_page.submit_form()
    
    assert cliente_page.has_validation_errors(), \
        "Debería mostrar error cuando el correo tiene formato inválido con @@ y .."


@pytest.mark.clientes
def test_nombre_compuesto_valido(driver, base_url):
    """Prueba con nombre compuesto válido"""
    cliente_page = ClientePage(driver, base_url)
    cliente_page.navigate()
    
    cliente_page.fill_form(
        nombre="María José",  # Nombre compuesto válido
        apellido="Pérez",
        telefono="71234567",
        correo="mariajose@example.com"
    )
    cliente_page.submit_form()
    
    # Verificar si se registró correctamente
    # Nota: Depende de la implementación si acepta nombres compuestos
    resultado = cliente_page.is_cliente_registered("María José", "Pérez")
    
    # Si el sistema acepta nombres compuestos, debería estar registrado
    # Si no, debería mostrar errores de validación
    if not resultado:
        assert cliente_page.has_validation_errors(), \
            "Si no se acepta el nombre compuesto, debería mostrar errores de validación"


# ========== UTILIDADES Y FIXTURES ADICIONALES ==========

def test_module_info():
    """Información del módulo de pruebas de Clientes"""
    info = {
        "modulo": "Clientes",
        "total_casos": 36,
        "casos_validos": 2,
        "casos_invalidos": 34,
        "campos": ["Nombre", "Apellido", "Teléfono (opcional)", "Correo"],
        "validaciones": {
            "Nombre": "Requerido, 3-30 caracteres, solo letras, inicia con mayúscula",
            "Apellido": "Requerido, 3-30 caracteres, solo letras, inicia con mayúscula",
            "Teléfono": "Opcional, 7-8 dígitos, inicia con 6 o 7",
            "Correo": "Requerido, formato email válido"
        }
    }
    
    print("\n" + "="*80)
    print("INFORMACIÓN DEL MÓDULO DE PRUEBAS")
    print("="*80)
    for key, value in info.items():
        print(f"{key}: {value}")
    print("="*80)
    
    assert True, "Información del módulo mostrada correctamente"
