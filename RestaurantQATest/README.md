# 🧪 Suite de Pruebas Automatizadas - RestaurantQA

> **Autor:** Ingeniero de Automatización de Pruebas  
> **Framework:** Python + Selenium + Pytest  
> **Patrón:** Page Object Model (POM)  
> **Fecha:** Octubre 2025

---

## 📋 Tabla de Contenidos

1. [Inicio Rápido](#-inicio-rápido)
2. [Arquitectura del Proyecto](#-arquitectura-del-proyecto)
3. [Instalación y Configuración](#-instalación-y-configuración)
4. [Ejecución de Pruebas](#-ejecución-de-pruebas)
5. [Módulos Implementados](#-módulos-implementados)
   - [Productos](#módulo-productos)
   - [Repartidores](#módulo-repartidores)
   - [Clientes](#módulo-clientes)
6. [Análisis de Particiones Equivalentes](#-análisis-de-particiones-equivalentes)
7. [Estructura de Archivos](#-estructura-de-archivos)
8. [Reportes y Resultados](#-reportes-y-resultados)
9. [Solución de Problemas](#-solución-de-problemas)
10. [Contribuir](#-contribuir)

---

## 🚀 Inicio Rápido

### Pre-requisitos
- **Python 3.8+** instalado
- **Google Chrome** instalado
- **Git** para clonar el repositorio
- **Aplicación RestaurantQA** ejecutándose en `http://localhost:5020`

### Instalación en 3 Pasos

```bash
# 1. Navegar al directorio de pruebas
cd RestaurantQATest

# 2. Instalar dependencias
pip install -r requirements.txt

# 3. Ejecutar todas las pruebas
pytest -v
```

### Ejecución Rápida por Módulo

```bash
# Pruebas de Productos (33 casos)
pytest -m productos -v

# Pruebas de Repartidores (36 casos)
pytest -m repartidores -v

# Pruebas de Clientes (36 casos)
pytest -m clientes -v

# Pruebas críticas (smoke tests)
pytest -m smoke -v

# Generar reporte HTML
pytest --html=reports/report.html --self-contained-html
```

---

## 🏗️ Arquitectura del Proyecto

### Patrón Page Object Model (POM)

```
RestaurantQATest/
│
├── pages/                    # 📄 Page Objects
│   ├── base_page.py         # Clase base con métodos reutilizables
│   ├── producto_page.py     # POM para módulo Productos
│   └── repartidor_page.py   # POM para módulo Repartidores
│
├── tests/                    # 🧪 Archivos de pruebas
│   ├── test_productos.py    # 33 casos de prueba
│   └── test_repartidores.py # 36 casos de prueba
│
├── Data/                     # 📊 Datos de prueba (CSV)
│   ├── productos_tests.csv
│   └── repartidores_tests.csv
│
├── reports/                  # 📈 Reportes HTML
│
├── conftest.py              # ⚙️ Configuración de pytest
├── pytest.ini               # 🔧 Configuración de marcadores
└── requirements.txt         # 📦 Dependencias
```

### Principios de Diseño

✅ **Separación de Responsabilidades**: Page Objects separados de los tests  
✅ **Reutilización**: Clase `BasePage` con métodos comunes  
✅ **Mantenibilidad**: Localizadores centralizados en cada POM  
✅ **Data-Driven**: Casos de prueba en CSV para fácil modificación  
✅ **AAA Pattern**: Arrange-Act-Assert en cada test  

---

## 🛠️ Instalación y Configuración

### Opción 1: Instalación Manual

```bash
# Crear entorno virtual (recomendado)
python -m venv venv

# Activar entorno virtual
# En Linux/Mac:
source venv/bin/activate
# En Windows:
venv\Scripts\activate

# Instalar dependencias
pip install -r requirements.txt
```

### Opción 2: Instalación Automática

```bash
# Ejecutar script de instalación
chmod +x setup.sh
./setup.sh
```

### Dependencias Principales

```
selenium==4.18.1          # Automatización del navegador
pytest==8.1.1             # Framework de testing
pytest-html==4.1.1        # Reportes HTML
webdriver-manager==4.0.1  # Gestión automática de drivers
```

### Configuración de la Aplicación

Asegúrate de que la aplicación **RestaurantQA** esté ejecutándose:

```bash
# En el directorio RestaurantQA/
dotnet run
```

La aplicación debe estar disponible en: `http://localhost:5020`

---

## 🧪 Ejecución de Pruebas

### Comandos Básicos

```bash
# Todas las pruebas
pytest

# Modo verbose (detalles)
pytest -v

# Modo muy verbose (más información)
pytest -vv

# Mostrar prints
pytest -s

# Detener en primer fallo
pytest -x

# Ejecutar último fallo
pytest --lf
```

### Filtrado por Marcadores

```bash
# Por módulo
pytest -m productos
pytest -m repartidores

# Por tipo
pytest -m smoke          # Pruebas críticas
pytest -m "not slow"     # Excluir pruebas lentas

# Combinar marcadores
pytest -m "productos and smoke"
```

### Filtrado por Casos Específicos

```bash
# Un archivo específico
pytest tests/test_productos.py

# Una función específica
pytest tests/test_productos.py::test_registro_producto

# Por patrón en el nombre
pytest -k "valido"       # Solo casos con "valido" en el nombre
pytest -k "PR1 or PR2"   # Casos específicos
```

### Reportes HTML

```bash
# Generar reporte HTML completo
pytest --html=reports/report.html --self-contained-html

# Con capturas de pantalla (si está configurado)
pytest --html=reports/report.html --self-contained-html -v
```

### Ejecución en Paralelo (Opcional)

```bash
# Instalar plugin
pip install pytest-xdist

# Ejecutar con 4 workers
pytest -n 4
```

### Modo Headless (Sin GUI)

El modo headless ya está configurado por defecto en `conftest.py`.  
Para ejecutar con navegador visible, edita `conftest.py`:

```python
options.add_argument('--headless')  # Comentar esta línea
```

---

## 📦 Módulos Implementados

### Módulo: Productos

#### Descripción
Validación del formulario de registro de productos en `/Productos/Index`.

#### Campos Validados

| Campo | Validación | Rango |
|-------|-----------|-------|
| **Nombre** | Requerido, StringLength | 4-20 caracteres |
| **Precio** | Requerido, Range | 0.01 - 1000.00 |
| **Stock** | Requerido, Range | 0 - 150 |
| **Descripción** | StringLength | 5-100 caracteres |
| **Categoría** | Requerido, ForeignKey | ID válido |

#### Estadísticas
- **Total de casos:** 33
- **Casos válidos:** 2 (PR1, PR2)
- **Casos inválidos:** 31
- **Particiones:** 15 únicas

#### Particiones Implementadas

##### **Nombre (4-20 caracteres)**
- `NOMBRE_VACIO`: Cadena vacía → ❌ Error
- `NOMBRE_MENOR_MIN`: 1-3 caracteres → ❌ Error
- `NOMBRE_VALIDO_MIN`: 4 caracteres → ✅ Válido
- `NOMBRE_VALIDO_MEDIO`: 5-19 caracteres → ✅ Válido
- `NOMBRE_VALIDO_MAX`: 20 caracteres → ✅ Válido
- `NOMBRE_MAYOR_MAX`: 21+ caracteres → ❌ Error

##### **Precio (0.01 - 1000.00)**
- `PRECIO_VACIO`: Campo vacío → ❌ Error
- `PRECIO_NEGATIVO`: < 0 → ❌ Error
- `PRECIO_CERO`: 0.00 → ❌ Error
- `PRECIO_VALIDO_MIN`: 0.01 → ✅ Válido
- `PRECIO_VALIDO_MEDIO`: 0.02-999.99 → ✅ Válido
- `PRECIO_VALIDO_MAX`: 1000.00 → ✅ Válido
- `PRECIO_MAYOR_MAX`: > 1000.00 → ❌ Error

##### **Stock (0 - 150)**
- `STOCK_VACIO`: Campo vacío → ❌ Error
- `STOCK_NEGATIVO`: < 0 → ❌ Error
- `STOCK_VALIDO_MIN`: 0 → ✅ Válido
- `STOCK_VALIDO_MEDIO`: 1-149 → ✅ Válido
- `STOCK_VALIDO_MAX`: 150 → ✅ Válido
- `STOCK_MAYOR_MAX`: > 150 → ❌ Error

##### **Descripción (5-100 caracteres)**
- `DESCRIPCION_VACIA`: Cadena vacía → ❌ Error (opcional pero si se llena debe cumplir)
- `DESCRIPCION_MENOR_MIN`: 1-4 caracteres → ❌ Error
- `DESCRIPCION_VALIDA_MIN`: 5 caracteres → ✅ Válido
- `DESCRIPCION_VALIDA_MEDIO`: 6-99 caracteres → ✅ Válido
- `DESCRIPCION_VALIDA_MAX`: 100 caracteres → ✅ Válido
- `DESCRIPCION_MAYOR_MAX`: 101+ caracteres → ❌ Error

#### Casos de Prueba Destacados

**PR1 - Registro Válido Básico:**
```csv
PR1, Pizza, 15.50, 10, Pizza deliciosa, 1, valido, VALIDO_BASICO
```

**PR8 - Nombre Vacío:**
```csv
PR8, , 15.50, 10, Pizza deliciosa, 1, invalido, NOMBRE_VACIO
```

**PR15 - Precio Negativo:**
```csv
PR15, Pizza, -5.00, 10, Pizza deliciosa, 1, invalido, PRECIO_NEGATIVO
```

#### Ejecutar Pruebas

```bash
# Todas las pruebas de productos
pytest -m productos -v

# Solo casos válidos
pytest tests/test_productos.py -k "PR1 or PR2"

# Solo validaciones de nombre
pytest tests/test_productos.py -k "nombre"
```

---

### Módulo: Repartidores

#### Descripción
Validación del formulario de creación de repartidores en `/Repartidores/Create`.

#### Campos Validados

| Campo | Validación | Rango/Formato |
|-------|-----------|---------------|
| **Nombre** | Requerido, StringLength | 3-30 caracteres |
| **Apellido** | Requerido, StringLength | 3-30 caracteres |
| **Teléfono** | Requerido, RegularExpression, Phone | 7-8 dígitos, inicia 6 o 7 |
| **Tipo** | Requerido, Select | Bicicleta, Moto, Auto |

#### Estadísticas
- **Total de casos:** 36
- **Casos válidos:** 4 (RP1, RP2, RP12, RP19)
- **Casos inválidos:** 32
- **Particiones:** 16 únicas

#### Particiones Implementadas

##### **Nombre (3-30 caracteres)**
- `NOMBRE_VACIO`: Cadena vacía → ❌ Error
- `NOMBRE_MENOR_MIN`: 1-2 caracteres → ❌ Error
- `NOMBRE_VALIDO_MIN`: 3 caracteres → ✅ Válido
- `NOMBRE_VALIDO_MEDIO`: 4-29 caracteres → ✅ Válido
- `NOMBRE_VALIDO_MAX`: 30 caracteres → ✅ Válido
- `NOMBRE_MAYOR_MAX`: 31+ caracteres → ❌ Error

##### **Apellido (3-30 caracteres)**
- `APELLIDO_VACIO`: Cadena vacía → ❌ Error
- `APELLIDO_MENOR_MIN`: 1-2 caracteres → ❌ Error
- `APELLIDO_VALIDO_MIN`: 3 caracteres → ✅ Válido
- `APELLIDO_VALIDO_MEDIO`: 4-29 caracteres → ✅ Válido
- `APELLIDO_VALIDO_MAX`: 30 caracteres → ✅ Válido
- `APELLIDO_MAYOR_MAX`: 31+ caracteres → ❌ Error

##### **Teléfono (7-8 dígitos, inicia 6 o 7)**
- `TELEFONO_VACIO`: Cadena vacía → ❌ Error
- `TELEFONO_FORMATO_INVALIDO`: Letras, caracteres especiales → ❌ Error
- `TELEFONO_MENOR_MIN`: 1-6 dígitos → ❌ Error
- `TELEFONO_VALIDO_7_DIGITOS`: 7 dígitos iniciando 6 o 7 → ✅ Válido
- `TELEFONO_VALIDO_8_DIGITOS`: 8 dígitos iniciando 6 o 7 → ✅ Válido
- `TELEFONO_MAYOR_MAX`: 9+ dígitos → ❌ Error
- `TELEFONO_INICIO_INVALIDO`: Inicia con 1-5, 8, 9 → ❌ Error

##### **Tipo (Select - Bicicleta, Moto, Auto)**
- `TIPO_VACIO`: Sin selección → ❌ Error
- `TIPO_VALIDO`: Bicicleta, Moto o Auto → ✅ Válido

#### Mapeo de Tipos

El Page Object incluye un mapeo para traducir valores del CSV:

```python
tipo_mapping = {
    'Interno': 'Bicicleta',
    'Externo': 'Moto',
    'Temporal': 'Auto'
}
```

#### Casos de Prueba Destacados

**RP1 - Registro Válido con Bicicleta:**
```csv
RP1, Juan, Pérez, 71234567, Interno, valido, VALIDO_BASICO_INTERNO
```

**RP9 - Nombre Vacío:**
```csv
RP9, , Pérez, 71234567, Interno, invalido, NOMBRE_VACIO
```

**RP25 - Teléfono con Letras:**
```csv
RP25, Juan, Pérez, ABC12345, Interno, invalido, TELEFONO_FORMATO_INVALIDO
```

#### Ejecutar Pruebas

```bash
# Todas las pruebas de repartidores
pytest -m repartidores -v

# Solo casos válidos
pytest tests/test_repartidores.py -k "RP1 or RP2 or RP12 or RP19"

# Solo validaciones de teléfono
pytest tests/test_repartidores.py -k "telefono"
```

---

### Módulo: Clientes

#### Descripción
Validación del formulario de registro de clientes en `/Clientes/Create`.

#### Campos Validados

| Campo | Validación | Rango/Formato |
|-------|-----------|---------------|
| **Nombre** | Requerido, RegularExpression, StringLength | 3-30 caracteres, solo letras, inicia mayúscula |
| **Apellido** | Requerido, RegularExpression, StringLength | 3-30 caracteres, solo letras, inicia mayúscula |
| **Teléfono** | Opcional, RegularExpression, Phone | 7-8 dígitos, inicia 6 o 7 |
| **Correo** | Requerido, EmailAddress | Formato email válido |

#### Estadísticas
- **Total de casos:** 36
- **Casos válidos:** 2 (CL1, CL2)
- **Casos inválidos:** 34
- **Particiones:** 20 únicas

#### Particiones Implementadas

##### **Nombre (3-30 caracteres, solo letras)**
- `NOMBRE_VACIO`: Cadena vacía → ❌ Error
- `NOMBRE_MENOR_MIN`: 1-2 caracteres → ❌ Error
- `NOMBRE_VALIDO_MIN`: 3 caracteres → ✅ Válido
- `NOMBRE_VALIDO_MEDIO`: 4-29 caracteres → ✅ Válido
- `NOMBRE_VALIDO_MAX`: 30 caracteres → ✅ Válido
- `NOMBRE_MAYOR_MAX`: 31+ caracteres → ❌ Error
- `NOMBRE_CONTIENE_NUMEROS`: Con dígitos (Juan123) → ❌ Error
- `NOMBRE_CARACTERES_ESPECIALES`: Con @, !, # → ❌ Error
- `NOMBRE_VALIDO_COMPUESTO`: Con espacio (María José) → ✅ Válido

##### **Apellido (3-30 caracteres, solo letras)**
- `APELLIDO_VACIO`: Cadena vacía → ❌ Error
- `APELLIDO_MENOR_MIN`: 1-2 caracteres → ❌ Error
- `APELLIDO_VALIDO_MIN`: 3 caracteres → ✅ Válido
- `APELLIDO_VALIDO_MEDIO`: 4-29 caracteres → ✅ Válido
- `APELLIDO_VALIDO_MAX`: 30 caracteres → ✅ Válido
- `APELLIDO_MAYOR_MAX`: 31+ caracteres → ❌ Error
- `APELLIDO_CONTIENE_NUMEROS`: Con dígitos (Gomez123) → ❌ Error
- `APELLIDO_CARACTERES_ESPECIALES`: Con # → ❌ Error
- `APELLIDO_VALIDO_COMPUESTO`: Con espacio (De la Cruz) → ✅ Válido

##### **Teléfono (7-8 dígitos, inicia 6 o 7, OPCIONAL)**
- `TELEFONO_VACIO`: Campo vacío → ✅ Válido (es opcional)
- `TELEFONO_MENOR_MIN`: 1-6 dígitos → ❌ Error
- `TELEFONO_VALIDO_7_DIGITOS`: 7 dígitos iniciando 6 o 7 → ✅ Válido
- `TELEFONO_VALIDO_8_DIGITOS`: 8 dígitos iniciando 6 o 7 → ✅ Válido
- `TELEFONO_MAYOR_MAX`: 9+ dígitos → ❌ Error
- `TELEFONO_FORMATO_INVALIDO`: Contiene letras (71A23B67) → ❌ Error

##### **Correo (formato email)**
- `CORREO_VACIO`: Campo vacío → ❌ Error
- `CORREO_VALIDO`: Formato correcto (carlos@gmail.com) → ✅ Válido
- `CORREO_FORMATO_INVALIDO`: 
  - Sin @ (carlosgmail.com) → ❌ Error
  - Incompleto (usuario@) → ❌ Error
  - Con @@ o .. (maría@@gmail..com) → ❌ Error

#### Casos de Prueba Destacados

**CL1 - Registro Válido Básico:**
```csv
CL1, Carlos, Pérez, 71234567, carlos@gmail.com, valido, VALIDO_BASICO
```

**CL2 - Registro Válido con Apellido Compuesto:**
```csv
CL2, Carlos, De la Cruz, 59171234567, usuario@ucb.edu.bo, valido, VALIDO_APELLIDO_COMPUESTO
```

**CL3 - Campos Requeridos Vacíos:**
```csv
CL3, Carlos, , , , invalido, APELLIDO_VACIO_TELEFONO_VACIO_CORREO_VACIO
```

**CL7 - Nombre Compuesto Válido con Correo Inválido:**
```csv
CL7, María José, De la Cruz, , carlosgmail.com, invalido, NOMBRE_VALIDO_COMPUESTO_CORREO_FORMATO_INVALIDO
```

**CL25 - Nombre y Apellido con Números:**
```csv
CL25, Juan123, Gomez123, , carlos@gmail.com, invalido, NOMBRE_CONTIENE_NUMEROS_APELLIDO_CONTIENE_NUMEROS
```

**CL31 - Caracteres Especiales en Nombre y Apellido:**
```csv
CL31, @Pedro!, López#, 712345678901234, carlosgmail.com, invalido, NOMBRE_CARACTERES_ESPECIALES_APELLIDO_CARACTERES_ESPECIALES
```

#### Características Especiales

**Nombres y Apellidos Compuestos:**
El sistema acepta nombres y apellidos compuestos con espacios:
- Nombres válidos: "María José", "Juan Carlos", "Ana María"
- Apellidos válidos: "De la Cruz", "Del Valle", "De los Santos"

**Teléfono Opcional:**
A diferencia de otros campos, el teléfono NO es requerido:
- Puede dejarse vacío y el registro será exitoso
- Si se llena, debe cumplir con las validaciones (7-8 dígitos, inicia 6 o 7)

#### Ejecutar Pruebas

```bash
# Todas las pruebas de clientes
pytest -m clientes -v

# Solo casos válidos
pytest tests/test_clientes.py -k "CL1 or CL2"

# Solo validaciones de nombre
pytest tests/test_clientes.py -k "nombre"

# Solo validaciones de correo
pytest tests/test_clientes.py -k "correo"

# Casos con nombres compuestos
pytest tests/test_clientes.py -k "compuesto"
```

---

## 📊 Análisis de Particiones Equivalentes

### ¿Qué son las Particiones Equivalentes?

Es una técnica de **caja negra** que divide el dominio de entrada en clases de equivalencia donde:
- Todos los valores de una partición deberían comportarse de la misma manera
- Se selecciona un representante de cada partición para reducir el número de casos

### Metodología Aplicada

#### 1. Identificación de Campos
- Analizar atributos de validación en las entidades C#
- Documentar rangos, tipos y restricciones

#### 2. Definición de Particiones
- **Particiones Válidas:** Valores que deberían ser aceptados
- **Particiones Inválidas:** Valores que deberían ser rechazados

#### 3. Casos de Prueba
- Mínimo un caso por partición
- Casos de frontera (boundary values)
- Casos combinados cuando hay dependencias

### Ejemplo Completo: Campo Teléfono

```
Regla: 7-8 dígitos numéricos, debe iniciar con 6 o 7

Particiones Válidas:
├── TELEFONO_VALIDO_7_DIGITOS: 71234567 (7 dígitos, inicia 7)
└── TELEFONO_VALIDO_8_DIGITOS: 67123456 (8 dígitos, inicia 6)

Particiones Inválidas:
├── TELEFONO_VACIO: "" (campo vacío)
├── TELEFONO_FORMATO_INVALIDO: "ABC12345" (contiene letras)
├── TELEFONO_MENOR_MIN: "612345" (6 dígitos, < mínimo)
├── TELEFONO_MAYOR_MAX: "671234567" (9 dígitos, > máximo)
└── TELEFONO_INICIO_INVALIDO: "81234567" (inicia con 8)
```

### Cobertura de Pruebas

**Productos:**
- 15 particiones únicas identificadas
- 33 casos de prueba generados
- 100% cobertura de particiones

**Repartidores:**
- 16 particiones únicas identificadas
- 36 casos de prueba generados
- 100% cobertura de particiones

**Clientes:**
- 20 particiones únicas identificadas
- 36 casos de prueba generados
- 100% cobertura de particiones

### Beneficios del Enfoque

✅ **Cobertura Sistemática:** Todas las condiciones están probadas  
✅ **Optimización:** Evita casos redundantes  
✅ **Documentación:** CSV describe claramente cada partición  
✅ **Mantenibilidad:** Fácil agregar/modificar particiones  
✅ **Trazabilidad:** Relación clara entre requisitos y pruebas  

---

## 📁 Estructura de Archivos

### Directorio `pages/`

#### `base_page.py` (184 líneas)
Clase base con métodos reutilizables para todos los Page Objects.

**Métodos principales:**
- `find_element(locator)`: Localiza elementos con espera implícita
- `click(locator)`: Click con manejo de errores
- `enter_text(locator, text)`: Ingresa texto y limpia campo
- `is_element_visible(locator)`: Verifica visibilidad
- `wait_for_element(locator, timeout=10)`: Espera explícita
- `get_validation_message(locator)`: Obtiene mensaje de error HTML5

#### `producto_page.py` (223 líneas)
Page Object para el formulario de productos.

**Localizadores:**
```python
NOMBRE_INPUT = (By.ID, "Producto_Nombre")
PRECIO_INPUT = (By.ID, "Producto_Precio")
STOCK_INPUT = (By.ID, "Producto_Stock")
DESCRIPCION_INPUT = (By.ID, "Producto_Descripcion")
CATEGORIA_SELECT = (By.ID, "Producto_CategoriaId")
SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")
```

**Métodos principales:**
- `fill_form(nombre, precio, stock, descripcion, categoria_id)`: Completa formulario
- `submit_form()`: Envía formulario
- `is_producto_registered(nombre)`: Verifica registro exitoso
- `has_validation_errors()`: Detecta errores de validación
- `get_validation_errors()`: Obtiene lista de errores

#### `repartidor_page.py` (262 líneas)
Page Object para el formulario de repartidores.

**Localizadores:**
```python
NOMBRE_INPUT = (By.ID, "Repartidore_Nombre")
APELLIDO_INPUT = (By.ID, "Repartidore_Apellido")
TELEFONO_INPUT = (By.ID, "Repartidore_Telefono")
TIPO_SELECT = (By.ID, "Repartidore_Tipo")
SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")
```

**Características especiales:**
- Manejo de `Select` para campo Tipo
- Mapeo de valores CSV a opciones del dropdown
- Validación de teléfono con regex

### Directorio `tests/`

#### `test_productos.py` (304 líneas)
Suite de pruebas para productos usando parametrización.

**Estructura:**
```python
@pytest.mark.productos
@pytest.mark.parametrize("caso,nombre,precio,stock,descripcion,categoria_id,esperado,particion,observaciones",
                         load_test_cases())
def test_registro_producto(driver, base_url, caso, nombre, precio, ...):
    # Arrange
    producto_page = ProductoPage(driver, base_url)
    
    # Act
    producto_page.fill_form(nombre, precio, stock, descripcion, categoria_id)
    producto_page.submit_form()
    
    # Assert
    if esperado == "valido":
        assert producto_page.is_producto_registered(nombre)
    else:
        assert producto_page.has_validation_errors()
```

#### `test_repartidores.py` (273 líneas)
Suite de pruebas para repartidores.

**Características:**
- 36 casos parametrizados desde CSV
- Validación de mensajes de error específicos
- Verificación de registro exitoso

### Archivos de Configuración

#### `conftest.py` (80+ líneas)
Configuración global de pytest.

**Fixtures:**
```python
@pytest.fixture(scope="function")
def driver():
    """Inicializa WebDriver con Chrome headless"""
    
@pytest.fixture(scope="session")
def base_url():
    """URL base de la aplicación"""
    
@pytest.fixture(autouse=True)
def setup_teardown(driver):
    """Limpieza antes/después de cada test"""
```

**Marcadores registrados:**
- `productos`: Pruebas del módulo productos
- `repartidores`: Pruebas del módulo repartidores
- `clientes`: Pruebas del módulo clientes (futuro)
- `smoke`: Pruebas críticas de humo

#### `pytest.ini` (56 líneas)
Configuración de pytest.

```ini
[pytest]
markers =
    productos: Pruebas del módulo de productos
    repartidores: Pruebas del módulo de repartidores
    smoke: Pruebas críticas de humo

testpaths = tests
python_files = test_*.py
python_classes = Test*
python_functions = test_*
```

#### `requirements.txt`
```
selenium==4.18.1
pytest==8.1.1
pytest-html==4.1.1
webdriver-manager==4.0.1
```

---

## 📈 Reportes y Resultados

### Generación de Reportes HTML

```bash
# Reporte estándar
pytest --html=reports/report.html --self-contained-html

# Reporte con más detalles
pytest -vv --html=reports/report_detallado.html --self-contained-html

# Reporte solo de fallos
pytest --html=reports/fallos.html --self-contained-html --tb=short
```

### Contenido del Reporte

El reporte HTML incluye:
- ✅ **Resumen:** Total, Pasados, Fallados, Saltados
- 📊 **Estadísticas:** Duración total y por test
- 📝 **Detalles:** Logs de cada test
- ❌ **Trazas:** Stack traces de errores
- 🏷️ **Marcadores:** Filtrado por categoría

### Interpretar Resultados

#### Salida en Terminal

```
tests/test_productos.py::test_registro_producto[PR1-...] PASSED [ 3%]
tests/test_productos.py::test_registro_producto[PR2-...] PASSED [ 6%]
tests/test_productos.py::test_registro_producto[PR8-...] PASSED [ 9%]
...
===================== 33 passed in 45.23s =====================
```

#### Códigos de Estado

- **PASSED** ✅: Test exitoso
- **FAILED** ❌: Test falló (error en assertion)
- **ERROR** 🔴: Error en el test (excepción)
- **SKIPPED** ⏭️: Test omitido
- **XFAIL** 🔶: Fallo esperado
- **XPASS** 🎉: Pasó cuando se esperaba fallo

### Logs Detallados

```bash
# Mostrar prints de los tests
pytest -s

# Mostrar logs de nivel DEBUG
pytest --log-cli-level=DEBUG

# Capturar output en archivo
pytest --capture=no > output.log 2>&1
```

### Análisis de Cobertura

```bash
# Instalar coverage
pip install pytest-cov

# Ejecutar con cobertura
pytest --cov=pages --cov-report=html

# Ver reporte
open htmlcov/index.html
```

---

## 🔧 Solución de Problemas

### Problema 1: ChromeDriver no encontrado

**Síntoma:**
```
selenium.common.exceptions.WebDriverException: 'chromedriver' executable needs to be in PATH
```

**Solución:**
```bash
# Verificar instalación de Chrome
google-chrome --version

# Reinstalar webdriver-manager
pip uninstall webdriver-manager
pip install webdriver-manager==4.0.1

# Ejecutar de nuevo
pytest
```

### Problema 2: La aplicación no está corriendo

**Síntoma:**
```
selenium.common.exceptions.WebDriverException: Message: unknown error: net::ERR_CONNECTION_REFUSED
```

**Solución:**
```bash
# Iniciar la aplicación en otra terminal
cd RestaurantQA
dotnet run

# Verificar que esté en http://localhost:5020
curl http://localhost:5020
```

### Problema 3: Timeout al esperar elementos

**Síntoma:**
```
selenium.common.exceptions.TimeoutException: Message: 
```

**Solución:**
1. Aumentar timeout en `base_page.py`:
```python
def wait_for_element(self, locator, timeout=20):  # Aumentar a 20 segundos
```

2. Verificar que el localizador sea correcto:
```python
# Imprimir source de la página
print(driver.page_source)
```

### Problema 4: Tests pasan pero no deberían

**Diagnóstico:**
```bash
# Ejecutar un test específico en modo verbose
pytest tests/test_productos.py::test_registro_producto[PR8-...] -vv -s
```

**Posibles causas:**
- Assertions incorrectos
- Datos del CSV mal formateados
- Lógica de validación en el POM incorrecta

### Problema 5: ImportError

**Síntoma:**
```
ImportError: cannot import name 'ProductoPage' from 'pages'
```

**Solución:**
```bash
# Verificar que exista __init__.py
ls pages/__init__.py

# Reinstalar en modo desarrollo
pip install -e .

# Agregar PYTHONPATH
export PYTHONPATH="${PYTHONPATH}:$(pwd)"
```

### Problema 6: Tests intermitentes

**Solución:**
```python
# En conftest.py, agregar esperas implícitas
driver.implicitly_wait(10)

# Usar esperas explícitas en lugar de sleep
from selenium.webdriver.support.ui import WebDriverWait
wait = WebDriverWait(driver, 10)
```

### Verificación de Entorno

```bash
# Script de diagnóstico
python --version          # Debe ser 3.8+
pip list | grep selenium  # Debe mostrar 4.18.1
google-chrome --version   # Verificar Chrome instalado
pytest --version          # Verificar pytest instalado
```

---

## 🤝 Contribuir

### Agregar Nuevos Módulos

#### 1. Crear CSV de Datos
```bash
# Crear archivo en Data/
touch Data/nuevo_modulo_tests.csv
```

Formato del CSV:
```csv
caso,campo1,campo2,campo3,esperado,particion,observaciones
NM1,valor1,valor2,valor3,valido,PARTICION_VALIDA,Descripción
```

#### 2. Crear Page Object
```python
# pages/nuevo_modulo_page.py
from pages.base_page import BasePage
from selenium.webdriver.common.by import By

class NuevoModuloPage(BasePage):
    # Localizadores
    CAMPO1_INPUT = (By.ID, "NuevoModulo_Campo1")
    SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")
    
    def __init__(self, driver, base_url):
        super().__init__(driver)
        self.base_url = base_url
        
    def navigate(self):
        self.driver.get(f"{self.base_url}/NuevoModulo/Index")
        
    def fill_form(self, campo1, campo2):
        self.enter_text(self.CAMPO1_INPUT, campo1)
        # ...
```

#### 3. Crear Test Suite
```python
# tests/test_nuevo_modulo.py
import pytest
import csv
from pages.nuevo_modulo_page import NuevoModuloPage

def load_test_cases():
    with open('Data/nuevo_modulo_tests.csv', encoding='utf-8') as f:
        reader = csv.DictReader(f)
        return [(row['caso'], row['campo1'], ...) for row in reader]

@pytest.mark.nuevo_modulo
@pytest.mark.parametrize("caso,campo1,campo2,esperado,particion,observaciones",
                         load_test_cases())
def test_nuevo_modulo(driver, base_url, caso, campo1, ...):
    # Arrange
    page = NuevoModuloPage(driver, base_url)
    page.navigate()
    
    # Act
    page.fill_form(campo1, campo2)
    page.submit_form()
    
    # Assert
    if esperado == "valido":
        assert page.is_registered()
    else:
        assert page.has_validation_errors()
```

#### 4. Registrar Marcador
```python
# conftest.py
def pytest_configure(config):
    config.addinivalue_line(
        "markers", "nuevo_modulo: Pruebas del módulo nuevo"
    )
```

```ini
# pytest.ini
[pytest]
markers =
    nuevo_modulo: Pruebas del módulo nuevo
```

#### 5. Documentar
Agregar sección al README con:
- Descripción del módulo
- Campos validados
- Particiones implementadas
- Comandos de ejecución

### Mejores Prácticas

✅ **Nomenclatura:**
- Archivos: `snake_case`
- Clases: `PascalCase`
- Métodos: `snake_case`
- Constantes: `UPPER_CASE`

✅ **Documentación:**
- Docstrings en cada método
- Comentarios para lógica compleja
- README actualizado

✅ **Testing:**
- Probar el nuevo módulo antes de commit
- Verificar que no rompe tests existentes
- Generar reporte HTML

✅ **Git:**
```bash
# Crear rama para nueva feature
git checkout -b feature/nuevo-modulo

# Commits descriptivos
git commit -m "feat: Agregar pruebas para módulo X"

# Push y Pull Request
git push origin feature/nuevo-modulo
```

---

## 📞 Soporte

### Recursos Adicionales

- 📚 **Selenium Docs:** https://www.selenium.dev/documentation/
- 🧪 **Pytest Docs:** https://docs.pytest.org/
- 🐍 **Python Docs:** https://docs.python.org/3/

### Contacto

Para preguntas o problemas:
1. Revisar esta documentación
2. Consultar logs de ejecución
3. Verificar issues en el repositorio
4. Contactar al equipo de QA

---

## 📝 Notas de Versión

### v1.0.0 - Octubre 2025

**Módulos Implementados:**
- ✅ Productos (33 casos)
- ✅ Repartidores (36 casos)

**Características:**
- Page Object Model completo
- Pruebas parametrizadas con CSV
- Reportes HTML
- Modo headless
- Gestión automática de drivers
- Documentación completa

**Próximos Módulos:**
- 🔜 Clientes
- 🔜 Pedidos
- 🔜 Pagos

---

## 🎯 Resumen Ejecutivo

### Métricas del Proyecto

| Métrica | Valor |
|---------|-------|
| **Módulos Automatizados** | 3 |
| **Total Casos de Prueba** | 105 |
| **Particiones Únicas** | 51 |
| **Cobertura de Particiones** | 100% |
| **Archivos Python** | 11 |
| **Líneas de Código** | ~2,400 |
| **Tiempo Ejecución Promedio** | ~90 segundos |

### Tecnologías Utilizadas

- **Lenguaje:** Python 3.8+
- **Framework de Testing:** Pytest 8.1.1
- **Automatización Web:** Selenium 4.18.1
- **Patrón de Diseño:** Page Object Model
- **Gestión de Drivers:** WebDriver Manager 4.0.1
- **Reportes:** Pytest-HTML 4.1.1
- **Datos de Prueba:** CSV
- **Control de Versiones:** Git

### Próximos Pasos

1. ✅ Ejecutar todas las pruebas: `pytest -v`
2. ✅ Generar reporte HTML: `pytest --html=reports/report.html`
3. ✅ Implementar módulo Clientes
4. 🔜 Implementar módulo Pedidos
5. 🔜 Integrar con CI/CD
6. 🔜 Agregar capturas de pantalla en fallos

---

**🎉 ¡Suite de Pruebas Lista para Usar!**

