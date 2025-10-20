# 🚀 Guía Rápida - Pruebas de Repartidores

## ⚡ Inicio Rápido (5 minutos)

### 1. Instalar dependencias (si aún no lo hiciste)
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
pytest tests/test_repartidores.py -v
```

---

## 📊 Casos de Prueba - Repartidores

### ✅ Casos Válidos (Deben PASAR)

| Caso | Nombre | Apellido | Teléfono | Tipo | Resultado |
|------|--------|----------|----------|------|-----------|
| RP1 | Carlos | Gómez | 71234567 | Interno | ✅ Aceptado |
| RP2 | Carlos | López | 78901234 | Externo | ✅ Aceptado |
| RP12 | Juan Pérez | Gómez | 78901234 | Temporal | ✅ Aceptado |
| RP19 | María Fernanda | Rodríguez | 71234567 | Interno | ✅ Aceptado |

### ❌ Casos Inválidos (Deben FALLAR)

**Nombre vacío:**
- RP31-RP36

**Nombre muy corto (<3 caracteres):**
- RP13-RP18

**Nombre muy largo (>30 caracteres):**
- RP25-RP30

**Apellido vacío:**
- RP6, RP11, RP21, RP26

**Apellido muy corto (<3 caracteres):**
- RP3, RP8, RP24, RP29, RP34

**Apellido muy largo (>30 caracteres):**
- RP5, RP10, RP15, RP20, RP25, RP36

**Teléfono vacío:**
- RP3, RP7, RP17, RP21, RP27, RP35

**Teléfono muy corto (<7 dígitos):**
- RP4, RP8, RP18, RP22, RP28, RP36

**Teléfono muy largo (>8 dígitos):**
- RP5, RP9, RP13, RP23, RP29, RP31

**Teléfono con letras:**
- RP6, RP10, RP14, RP24, RP30, RP32

---

## 🎯 Particiones Equivalentes

### Válidas
- ✅ Nombre: 3-30 caracteres, solo letras, mayúscula inicial, puede tener espacios
- ✅ Apellido: 3-30 caracteres, solo letras, mayúscula inicial, puede tener espacios
- ✅ Teléfono: 7-8 dígitos, inicia con 6 o 7
- ✅ Tipo: Bicicleta, Moto, Auto (mapeado a Interno, Externo, Temporal)

### Inválidas
- ❌ Nombre: vacío, <3, >30
- ❌ Apellido: vacío, <3, >30
- ❌ Teléfono: vacío, <7, >8, contiene letras, no inicia con 6/7
- ❌ Tipo: valor no válido

---

## 📝 Comandos Útiles

```bash
# Ejecutar solo repartidores
pytest -m repartidores

# Ejecutar solo smoke tests
pytest -m smoke

# Generar reporte HTML
pytest --html=reports/report_repartidores.html

# Ver detalles completos
pytest -vv -s tests/test_repartidores.py

# Detener al primer fallo
pytest -x tests/test_repartidores.py

# Ejecutar en paralelo (rápido)
pytest -n auto -m repartidores
```

---

## 🔍 Verificar un Caso Específico

```bash
# Ejecutar solo RP1
pytest tests/test_repartidores.py -k "RP1"

# Ejecutar casos RP1 a RP5
pytest tests/test_repartidores.py::test_registro_repartidor
```

---

## ✨ Estructura del Test

```python
# 1. ARRANGE: Preparar datos
repartidor_page = RepartidorPage(driver)
repartidor_page.navigate(base_url)

# 2. ACT: Ejecutar acción
repartidor_page.fill_form(
    nombre="Carlos", 
    apellido="Gómez",
    telefono="71234567",
    tipo="Interno"
)
repartidor_page.submit_form()

# 3. ASSERT: Verificar resultado
assert repartidor_page.is_repartidor_registered()
```

---

## 📦 Archivos Importantes

```
RestaurantQATest/
├── Data/repartidores_tests.csv      ← Casos de prueba
├── pages/repartidor_page.py         ← Page Object
├── tests/test_repartidores.py       ← Tests
├── conftest.py                      ← Configuración
└── pytest.ini                       ← Opciones pytest
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

- Ver documentación completa (cuando esté disponible)
- Ver logs: `reports/pytest.log`
- Ver reporte HTML: `reports/report.html`

---

## 📈 Resumen de Casos

```
Total:      36 casos
Válidos:     4 casos (RP1, RP2, RP12, RP19)
Inválidos:  32 casos (RP3-RP11, RP13-RP18, RP20-RP36)
```

---

**¡Listo para comenzar a probar! 🚀**
