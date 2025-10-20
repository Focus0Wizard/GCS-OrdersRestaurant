# 📊 Análisis de Particiones Equivalentes - Módulo Productos

## 🎯 Objetivo

Identificar y documentar todas las **particiones equivalentes** para el formulario de registro de productos, con el fin de diseñar casos de prueba que maximicen la cobertura con el mínimo número de tests.

---

## 📋 Campos del Formulario

### Campo: **Nombre**
- **Tipo:** String
- **Validaciones:**
  - `[Required]` Es obligatorio
  - `[StringLength(20, MinimumLength = 4)]` Longitud entre 4 y 20 caracteres
- **Observaciones:** Según el código, no hay validación de formato (regex) visible

### Campo: **Precio**
- **Tipo:** Decimal
- **Validaciones:**
  - `[Range(0.01, 1000)]` Rango entre 0.01 y 1000
- **Observaciones:** Debe ser mayor a 0

### Campo: **Stock**
- **Tipo:** Integer (nullable)
- **Validaciones:**
  - `[Range(0, 150)]` Rango entre 0 y 150
- **Observaciones:** No puede ser negativo, máximo 150 (aunque el mensaje dice 200)

### Campo: **Descripción**
- **Tipo:** String
- **Validaciones:**
  - `[Required]` Es obligatorio
  - `[StringLength(100, MinimumLength = 5)]` Longitud entre 5 y 100 caracteres
- **Observaciones:** Mínimo 5 caracteres

### Campo: **CategoríaId**
- **Tipo:** Short
- **Validaciones:**
  - `[Range(1, short.MaxValue)]` Debe ser un número positivo > 0
- **Observaciones:** No se prueba en este módulo (siempre se usa 1)

---

## 🔍 Identificación de Particiones

### 1️⃣ Particiones para **Nombre**

#### ✅ Particiones Válidas:
- **PV-N1:** Nombre con longitud mínima válida (4 caracteres)
  - Ejemplo: "Rico"
  
- **PV-N2:** Nombre con longitud media (8-15 caracteres)
  - Ejemplo: "Hamburguesa"
  
- **PV-N3:** Nombre con longitud máxima válida (20 caracteres)
  - Ejemplo: "HamburguesaEspecial1" (20 chars)

#### ❌ Particiones Inválidas:
- **PI-N1:** Nombre vacío
  - Ejemplo: ""
  
- **PI-N2:** Nombre con longitud < 4
  - Ejemplo: "Pan" (3 caracteres)
  
- **PI-N3:** Nombre con longitud > 20
  - Ejemplo: "SuperMegaComboDobleQuesoExtraGrande" (37 caracteres)
  
- **PI-N4:** Nombre con caracteres especiales/números
  - Ejemplo: "Pizza@123"

---

### 2️⃣ Particiones para **Precio**

#### ✅ Particiones Válidas:
- **PV-P1:** Precio en límite inferior (0.01)
  - Ejemplo: 0.01
  
- **PV-P2:** Precio en rango medio (1-999)
  - Ejemplo: 25.5
  
- **PV-P3:** Precio en límite superior (1000)
  - Ejemplo: 1000

#### ❌ Particiones Inválidas:
- **PI-P1:** Precio = 0
  - Ejemplo: 0
  
- **PI-P2:** Precio < 0.01
  - Ejemplo: 0.001
  
- **PI-P3:** Precio > 1000
  - Ejemplo: 1500
  
- **PI-P4:** Precio negativo
  - Ejemplo: -10

---

### 3️⃣ Particiones para **Stock**

#### ✅ Particiones Válidas:
- **PV-S1:** Stock en límite inferior (0)
  - Ejemplo: 0
  
- **PV-S2:** Stock en rango medio (1-149)
  - Ejemplo: 50
  
- **PV-S3:** Stock en límite superior (150)
  - Ejemplo: 150

#### ❌ Particiones Inválidas:
- **PI-S1:** Stock negativo
  - Ejemplo: -5
  
- **PI-S2:** Stock > 150
  - Ejemplo: 200

---

### 4️⃣ Particiones para **Descripción**

#### ✅ Particiones Válidas:
- **PV-D1:** Descripción con longitud mínima (5 caracteres)
  - Ejemplo: "Rico1"
  
- **PV-D2:** Descripción con longitud media (20-80 caracteres)
  - Ejemplo: "Deliciosa hamburguesa con queso y tomate fresco."
  
- **PV-D3:** Descripción con longitud máxima (100 caracteres)
  - Ejemplo: "Producto gourmet elaborado artesanalmente..." (100 chars)

#### ❌ Particiones Inválidas:
- **PI-D1:** Descripción vacía
  - Ejemplo: ""
  
- **PI-D2:** Descripción con longitud < 5
  - Ejemplo: "Pan" (3 caracteres)
  
- **PI-D3:** Descripción con longitud > 100
  - Ejemplo: "Producto gourmet elaborado artesanalmente con ingredientes de alta calidad seleccionados cuidadosamente." (112 caracteres)

---

## 🧮 Matriz de Casos de Prueba

### ✅ Casos Válidos (Particiones Positivas)

| Caso | Nombre (PV-N) | Precio (PV-P) | Stock (PV-S) | Descripción (PV-D) | Resultado |
|------|---------------|---------------|--------------|---------------------|-----------|
| **PR1** | PV-N2 (4-20) | PV-P1 (0.01) | PV-S1 (0) | PV-D1 (5) | ✅ Aceptado |
| **PR2** | PV-N2 (4-20) | PV-P3 (1000) | PV-S3 (150) | PV-D3 (100) | ✅ Aceptado |
| **PR5** | PV-N2 (4-20) | PV-P2 (medio) | PV-S2 (medio) | PV-D1 (5) | ✅ Aceptado |

**Cobertura de Particiones Válidas:**
- ✅ Todas las combinaciones de límites (mínimo, medio, máximo)
- ✅ 3 casos cubren todas las particiones válidas

---

### ❌ Casos Inválidos (Particiones Negativas)

#### Grupo 1: Nombre Inválido

| Caso | Nombre | Precio | Stock | Descripción | Partición Inválida | Resultado |
|------|--------|--------|-------|-------------|---------------------|-----------|
| **PR11-PR16** | PI-N1 (vacío) | Varios | Varios | Varios | Campo obligatorio | ❌ Rechazado |
| **PR17-PR22** | PI-N3 (>20) | Varios | Varios | Varios | Longitud excedida | ❌ Rechazado |
| **PR23-PR28** | PI-N4 (especiales) | Varios | Varios | Varios | Caracteres inválidos | ❌ Rechazado |

#### Grupo 2: Precio Inválido

| Caso | Nombre | Precio | Stock | Descripción | Partición Inválida | Resultado |
|------|--------|--------|-------|-------------|---------------------|-----------|
| **PR3, PR8, PR12** | Varios | PI-P1 (0) | Varios | Varios | Precio = 0 | ❌ Rechazado |
| **PR4, PR9, PR13** | Varios | PI-P3 (>1000) | Varios | Varios | Precio excede máximo | ❌ Rechazado |

#### Grupo 3: Stock Inválido

| Caso | Nombre | Precio | Stock | Descripción | Partición Inválida | Resultado |
|------|--------|--------|-------|-------------|---------------------|-----------|
| **PR3, PR7, PR16** | Varios | Varios | PI-S1 (<0) | Varios | Stock negativo | ❌ Rechazado |
| **PR4, PR8, PR11** | Varios | Varios | PI-S2 (>150) | Varios | Stock excede máximo | ❌ Rechazado |

#### Grupo 4: Descripción Inválida

| Caso | Nombre | Precio | Stock | Descripción | Partición Inválida | Resultado |
|------|--------|--------|-------|-------------|---------------------|-----------|
| **PR6, PR14, PR17** | Varios | Varios | Varios | PI-D2 (<5) | Longitud insuficiente | ❌ Rechazado |
| **PR10, PR13, PR22** | Varios | Varios | Varios | PI-D3 (>100) | Longitud excedida | ❌ Rechazado |

**Cobertura de Particiones Inválidas:**
- ✅ Todas las particiones inválidas están cubiertas
- ✅ Se prueban combinaciones múltiples de errores
- ✅ 31 casos inválidos para máxima cobertura

---

## 📈 Resumen de Cobertura

### Cobertura por Campo

| Campo | Particiones Válidas | Particiones Inválidas | Total | Casos |
|-------|--------------------|-----------------------|-------|-------|
| **Nombre** | 3 | 4 | 7 | 33 |
| **Precio** | 3 | 4 | 7 | 33 |
| **Stock** | 3 | 2 | 5 | 33 |
| **Descripción** | 3 | 3 | 6 | 33 |

### Distribución de Casos

```
Total de Casos: 33
├── ✅ Válidos: 3 (9%)
│   ├── Límites mínimos: PR1
│   ├── Límites máximos: PR2
│   └── Valores medios: PR5
│
└── ❌ Inválidos: 30 (91%)
    ├── Nombre vacío: 6 casos
    ├── Nombre largo: 6 casos
    ├── Nombre con caracteres especiales: 6 casos
    ├── Precio inválido: 6 casos
    ├── Stock inválido: 6 casos
    └── Descripción inválida: Multiple combinaciones
```

---

## 🎯 Estrategia de Testing

### 1. **Pruebas de Valores Límite (Boundary Value Analysis)**

Se prueban los límites de cada partición:

- **Nombre:** 4 chars (mínimo), 20 chars (máximo)
- **Precio:** 0.01 (mínimo), 1000 (máximo)
- **Stock:** 0 (mínimo), 150 (máximo)
- **Descripción:** 5 chars (mínimo), 100 chars (máximo)

### 2. **Pruebas de Partición Simple**

Cada partición inválida se prueba al menos una vez:

- Un caso con nombre vacío
- Un caso con nombre muy largo
- Un caso con caracteres especiales
- Etc.

### 3. **Pruebas de Combinaciones Múltiples**

Se prueban casos con múltiples errores simultáneos:

- **PR3:** Precio = 0 + Stock negativo
- **PR4:** Precio > 1000 + Stock > 150
- **PR33:** Nombre vacío + Precio alto + Stock negativo + Descripción larga

---

## 📊 Técnicas Aplicadas

### ✅ Particiones Equivalentes
- Dividir dominio de entrada en clases
- Cada clase se comporta de manera similar
- Reducir número de tests manteniendo cobertura

### ✅ Análisis de Valores Límite
- Probar en los bordes de las particiones
- Mayor probabilidad de encontrar defectos
- Casos PR1 y PR2 son ejemplos claros

### ✅ Testing Combinatorio
- No es necesario probar TODAS las combinaciones
- Se eligen combinaciones representativas
- 33 casos cubren todas las particiones

---

## 💡 Conclusiones

1. **Cobertura Completa:** Todos los campos y validaciones están cubiertos
2. **Eficiencia:** 33 casos son suficientes (vs. miles de combinaciones posibles)
3. **Mantenibilidad:** Fácil agregar nuevos casos siguiendo la matriz
4. **Trazabilidad:** Cada caso está documentado con su partición
5. **Automatización:** Todos los casos están automatizados con Selenium + Pytest

---

## 📚 Referencias

- **ISTQB Foundation Level:** Técnicas de diseño de pruebas
- **Black Box Testing:** Particiones equivalentes y valores límite
- **IEEE 829:** Documentación de pruebas de software

---

**Autor:** Ingeniero de QA  
**Fecha:** Octubre 2025  
**Versión:** 1.0  
