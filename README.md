# Stock Management System

Sistema de gestión de inventario desarrollado para el challenge técnico de GYF Inteligencia Digital.

DENTRO DEL REPOSITORIO VAN A ENCONTRAR 2 CARPETAS :
- StockManagement (CARPETA DIRIGIDA AL FRONT HECHA EN REACT.JS)
- -StockmanagementBackend ( CARPETA DIRIGIDA AL BACKEND HECHA EN .NET 8 )


## Challenge Completado

- ✅ Backend: API REST con .NET 8
- ✅ Frontend: React 18 con Vite
- ✅ Base de datos: SQL Server / LocalDB
- ✅ Autenticación: JWT Bearer Token
- ✅ CRUD completo de productos
- ✅ Algoritmo de filtrado por presupuesto
- ✅ Tests unitarios (5 tests)
- ✅ Documentación completa

---

## Tecnologías Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core 8** - ORM
- **SQL Server / LocalDB** - Base de datos
- **JWT Bearer Authentication** - Autenticación
- **BCrypt** - Hash de contraseñas
- **Serilog** - Logging
- **Swagger/OpenAPI** - Documentación de API
- **xUnit + Moq** - Testing

### Frontend
- **React 18** - UI Library
- **Vite** - Build tool
- **React Router v6** - Routing
- **Axios** - HTTP Client
- **React Toastify** - Notificaciones

---

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

### Obligatorio
- ✅ **Visual Studio 2022** (Community, Professional o Enterprise)
  - Con workload: "ASP.NET and web development"

- ✅ **Node.js 18+**

### Base de Datos

 SQL Server Management Studio (SSMS):
 (localdb)\MSSQLLocalDB
---

## 📂 Estructura del Proyecto

```
StockManagement/
├── Backend/
│   ├── StockManagement.API/          # API principal
│   │   ├── Controllers/               # Endpoints REST
│   │   ├── Services/                  # Lógica de negocio
│   │   ├── Models/                    # Entidades
│   │   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Data/                      # DbContext
│   │   └── appsettings.json          # Configuración
│   ├── StockManagement.Tests/         # Tests unitarios
│   └── StockManagement.sln           # Solución de Visual Studio
├── Frontend/
│   ├── src/
│   │   ├── components/                # Componentes React
│   │   ├── api.js                     # Cliente API
│   │   └── App.jsx                    # Componente principal
│   └── package.json
├── Database/
│   └── SETUP_DATABASE.sql            # Script completo de base de datos
└── README.md
```

---

## 🔧 Instalación y Configuración

### Paso 1: Clonar o Descargar el Proyecto

```bash
# Si está en GitHub:
git clone <url-del-repositorio>
cd StockManagementChallenge

---

### Paso 2: Configurar la Base de Datos

####  Usando LocalDB**
El proyecto ya viene configurado para LocalDB. Solo necesitas:

1. **Verificar que LocalDB está corriendo:**
   ```bash
   sqllocaldb info
   ```
   Debería mostrar: `MSSQLLocalDB`

2. **Si no está corriendo, iniciarlo:**
   ```bash
   sqllocaldb start MSSQLLocalDB
   ```

3. **Ejecutar el script de base de datos:**
   ```bash
   # Desde PowerShell o CMD
   cd Database
   sqlcmd -S "(localdb)\MSSQLLocalDB" -i SETUP_DATABASE_CHALLENGE.sql
   ```

4. **¡Listo!** El `appsettings.json` ya está configurado para LocalDB.

#### **Opción B: Usando SQL Server Express**

1. **Editar `Backend/StockManagement.API/appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StockManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

2. **Ejecutar el script en SSMS:**
   - Abrir SQL Server Management Studio
   - Conectarse a `(localdb)\MSSQLLocalDB`
   - Authentication : Windows Authentication
   - Abrir archivo `Database/SETUP_DATABASE.sql`
   - Ejecutar (F5)

---

### Paso 3: Ejecutar el Backend

#### **Método 1: Desde Visual Studio 2022** 
1. Abrir `Backend/StockManagement.sln`
2. Presionar **F5** o click en el botón verde ▶
3. Se abrirá el navegador con Swagger: `https://localhost:5253/swagger`

#### **Método 2: Desde Terminal**

```bash
cd \StockManagementBackend\StockManagement.API
dotnet restore
dotnet run
```

**Verificar que funciona:**
- Swagger: https://localhost:5253/swagger
---

### Paso 4: Ejecutar el Frontend

# Instalar dependencias
npm install

# Ejecutar servidor de desarrollo
npm run dev
```

El frontend estará disponible en: **http://localhost:3000**

---

### Paso 5: Ejecutar Tests

#### **Desde Visual Studio:**

#### **Desde Terminal:**
```bash
cd Backend/StockManagement.Tests
dotnet test

**Resultado esperado:**
```
Passed!  - Failed:     0, Passed:     5, Skipped:     0, Total:     5
```

---

## 🔐 Credenciales de Acceso

Para facilitar la revisión, el script de base de datos inyecta automáticamente un usuario con hash de contraseña real compatible con la lógica de la API:

**Usuario:** `admin`  
**Contraseña:** `admin123`

---

## 📡 Endpoints de la API

### **Autenticación**

#### POST /api/auth/login
Autenticarse y obtener token JWT

**Request:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin"
}
```

---

### **Productos**

#### GET /api/products
Obtener todos los productos

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "id": 1,
    "price": 60.00,
    "loadDate": "21/10/2019",
    "category": "PRODUNO"
  }
]
```

#### POST /api/products
Crear un nuevo producto

**Request:**
```json
{
  "price": 50.00,
  "loadDate": "2024-02-12T00:00:00",
  "category": "PRODUNO"
}
```

#### PUT /api/products/{id}
Actualizar un producto

#### DELETE /api/products/{id}
Eliminar un producto

#### POST /api/products/filter
Filtrar productos por presupuesto

**Request:**
```json
{
  "budget": 70
}
```

**Response:**
```json
{
  "productOne": {
    "id": 2,
    "price": 60.00,
    "loadDate": "21/10/2019",
    "category": "PRODUNO"
  },
  "productTwo": {
    "id": 1,
    "price": 10.00,
    "loadDate": "21/10/2019",
    "category": "PRODDOS"
  },
  "total": 70.00,
  "message": "Productos encontrados exitosamente."
}
```

---

## Funcionalidades del Frontend

### 1. **Login Screen**
- Autenticación con JWT
- Validación de credenciales
- Redirección automática al dashboard

### 2. **Dashboard**
- Vista principal con navegación
- Sidebar con opciones
- Logout

### 3. **Gestión de Productos**
- Tabla con todos los productos
- CRUD completo (Crear, Leer, Actualizar, Eliminar)
- Filtros por categoría
- Ordenamiento por columnas
- Modal para crear/editar

### 4. **Búsqueda por Presupuesto**
- Input de presupuesto (1-1,000,000)
- Algoritmo que encuentra la mejor combinación
- Muestra un producto de cada categoría
- Total no excede el presupuesto
- Maximiza la suma más cercana al presupuesto

---

## 🧪 Tests Unitarios

El proyecto incluye **5 tests unitarios** que cubren:

1. **CreateProduct_ShouldAddProductToDatabase**
   - Verifica la creación de productos

2. **GetFilteredProducts_ShouldReturnBestCombination**
   - Verifica el algoritmo de filtrado por presupuesto
   - Caso: presupuesto = 70, debe retornar productos que sumen exactamente 70

3. **GetFilteredProducts_WithInsufficientBudget_ShouldReturnMessage**
   - Verifica el caso cuando no hay combinación válida

4. **DeleteProduct_ShouldRemoveProduct**
   - Verifica la eliminación de productos

5. **UpdateProduct_ShouldModifyExistingProduct**
   - Verifica la actualización de productos

**Cobertura:** Los tests usan **InMemory Database**, por lo que no afectan la base de datos real.

---

## 🔍 Algoritmo de Filtrado por Presupuesto

El algoritmo implementado en `ProductService.GetFilteredProductsAsync()`:

1. Obtiene todos los productos de categoría **PRODUNO** con precio ≤ presupuesto
2. Obtiene todos los productos de categoría **PRODDOS** con precio ≤ presupuesto
3. Itera todas las combinaciones posibles (O(n × m))
4. Encuentra la combinación donde:
   - `precio_produno + precio_proddos ≤ presupuesto`
   - La suma sea **máxima** sin exceder el presupuesto
5. Retorna un producto de cada categoría

**Ejemplo:**
- Presupuesto: **$70**
- Productos PRODUNO: $60, $5
- Productos PRODDOS: $10, $5, $15
- **Resultado:** PRODUNO($60) + PRODDOS($10) = **$70** ✅

---

## 📝 Notas Técnicas

### Seguridad
- ✅ Contraseñas hasheadas con **BCrypt**
- ✅ Autenticación con **JWT Bearer Token**
- ✅ Tokens expiran en 60 minutos
- ✅ HTTPS redirection
- ✅ CORS configurado para React

### Base de Datos
- ✅ **Migrations automáticas** al iniciar la API
- ✅ **Seed de usuario admin** automático
- ✅ Índices en columnas clave (Category, Price)
- ✅ Stored procedures para consultas complejas

### Logging
- ✅ **Serilog** configurado
- ✅ Logs en consola y archivo
- ✅ Archivos rotativos por día en carpeta `/Logs`

---

## 🐛 Solución de Problemas

### Error: "Cannot connect to database"
✅ Verificar que LocalDB está corriendo: `sqllocaldb info`  
✅ Verificar connection string en `appsettings.json`  
✅ Ejecutar script `SETUP_DATABASE.sql`

### Error: "Port 5253 already in use"
✅ Cambiar puerto en `Properties/launchSettings.json`

### Frontend no se conecta al backend
✅ Verificar que el backend está corriendo en `https://localhost:5253`  
✅ Verificar CORS en `Program.cs`  
✅ Verificar URL en `Frontend/src/api.js`

### Tests fallan
✅ Ejecutar: `dotnet restore` en carpeta Tests  
✅ Verificar que todos los paquetes NuGet están instalados

---
# Notas de Implementación 
Concurrencia:Se implementó ROWVERSION  en las tablas para evitar que dos usuarios sobrescriban el mismo producto simultáneamente.
- El script SQL puede ejecutarse múltiples veces sin borrar datos existentes ni generar errores de duplicación.

## Autor

**Franco Ferreyra**  
Desarrollado para el challenge técnico de GYF Inteligencia Digital
