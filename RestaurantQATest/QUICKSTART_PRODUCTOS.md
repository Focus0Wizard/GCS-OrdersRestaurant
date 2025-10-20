# 🚀 Guía Rápida - Pruebas de Productos

## ⚡ Inicio Rápido (5 minutos)

### 1. Instalar dependencias
```bash
cd RestaurantQATest
pip install -r requirements.txt
```

### 2. Verificar que la app esté corriendo
```bash
# La aplicación debe estar en: http://localhost:5020
# Verificar abriendo en navegador
```

### 3. Ejecutar pruebas
```bash
pytest tests/test_productos.py -v
```

---

## 📊 Casos de Prueba - Productos

### ✅ Casos Válidos (Deben PASAR)

| Caso | Nombre | Precio | Stock | Descripción | Resultado |
|------|--------|--------|-------|-------------|-----------|
| PR1 | Hamburguesa | 0.01 | 0 | Rico | ✅ Aceptado |
| PR2 | Hamburguesa | 1000 | 150 | Producto gourmet... (100 chars) | ✅ Aceptado |
| PR5 | Hamburguesa | 25.5 | 50 | Rico | ✅ Aceptado |

### ❌ Casos Inválidos (Deben FALLAR)

**Nombre Vacío:**
- PR11-PR16, PR29-PR33

**Nombre > 20 caracteres:**
- PR17-PR22

**Nombre con caracteres especiales:**
- PR23-PR28

**Precio = 0 o fuera de rango:**
- PR3, PR8, PR12, PR17, PR28, PR32

**Stock negativo:**
- PR3, PR7, PR16, PR20, PR24, PR33

**Stock > 150:**
- PR4, PR6, PR8, PR11, PR15, PR19, PR23, PR25, PR29, PR32

**Descripción corta (<5 chars):**
- PR6, PR14, PR17, PR19, PR26, PR28-PR30, PR32

**Descripción larga (>100 chars):**
- PR10, PR13, PR22, PR25, PR33

---

## 🎯 Particiones Equivalentes

### Válidas
- ✅ Nombre: 4-20 caracteres, solo letras
- ✅ Precio: 0.01 - 1000
- ✅ Stock: 0 - 150
- ✅ Descripción: 5-100 caracteres

### Inválidas
- ❌ Nombre: vacío, <4, >20, caracteres especiales
- ❌ Precio: 0, <0.01, >1000
- ❌ Stock: <0, >150
- ❌ Descripción: vacío, <5, >100

---

## 📝 Comandos Útiles

```bash
# Ejecutar solo productos
pytest -m productos

# Ejecutar solo smoke tests
pytest -m smoke

# Generar reporte HTML
pytest --html=reports/report.html

# Ver detalles completos
pytest -vv -s

# Detener al primer fallo
pytest -x

# Ejecutar en paralelo (rápido)
pytest -n auto
```

---

## 🔍 Verificar un Caso Específico

```bash
# Ejecutar solo PR1
pytest tests/test_productos.py -k "PR1"

# Ejecutar casos PR1 a PR5
pytest tests/test_productos.py::test_registro_producto
```

---

## ✨ Estructura del Test

```python
# 1. ARRANGE: Preparar datos
producto_page = ProductoPage(driver)
producto_page.navigate(base_url)

# 2. ACT: Ejecutar acción
producto_page.fill_form(nombre="Hamburguesa", precio=25.5, ...)
producto_page.submit_form()

# 3. ASSERT: Verificar resultado
assert producto_page.is_producto_registered()
```

---

## 📦 Archivos Importantes

```
RestaurantQATest/
├── Data/productos_tests.csv        ← Casos de prueba
├── pages/producto_page.py          ← Page Object
├── tests/test_productos.py         ← Tests
├── conftest.py                     ← Configuración
├── pytest.ini                      ← Opciones pytest
└── README_PRODUCTOS.md             ← Documentación completa
```

---

## 🐛 Problemas Comunes

### ❌ Error: No module named 'selenium'
```bash
pip install -r requirements.txt
```

### ❌ Error: Connection refused
- Verificar que la app esté corriendo en localhost:5020

### ❌ Tests lentos
```bash
pytest -n auto  # Ejecutar en paralelo
```

---

## 📞 Ayuda

- Ver README completo: `README_PRODUCTOS.md`
- Ver logs: `reports/pytest.log`
- Ver reporte HTML: `reports/report.html`
