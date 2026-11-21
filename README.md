# 📦 Sistema de Gestión de Inventario — ASP.NET Core 10 & Neon PostgreSQL

Aplicación Web Full-Stack y API REST desarrollada en **C# (.NET 10)** con **ASP.NET Core Web API**, **Entity Framework Core** y **PostgreSQL en la nube (Neon)**. Permite administrar productos, categorías, proveedores, autenticación de usuarios por roles y control automatizado de movimientos de inventario mediante *Triggers* en base de datos.

---

## 🛠️ Tecnologías Utilizadas

* **Backend:** C# 13, .NET 10, ASP.NET Core Web API
* **ORM y Base de Datos:** Entity Framework Core 10 (`Npgsql.EntityFrameworkCore.PostgreSQL`) conectada a **Neon (Serverless PostgreSQL)**
* **Documentación de API:** OpenAPI / Swagger UI (`Swashbuckle.AspNetCore`)
* **Frontend:** HTML5, CSS3 y JavaScript Vanilla (`wwwroot/`)

---

## 📂 Estructura del Proyecto

```text
WebApplication1/
├── WebApplication1.slnx                  # Archivo de solución de Visual Studio 2026
├── WebApplication1/
│   ├── Controllers/
│   │   ├── AuthController.cs             # Inicio de sesión y validación de roles
│   │   ├── CategoriasController.cs       # CRUD de categorías
│   │   ├── InventarioController.cs       # Historial y registro de entradas/salidas/ajustes
│   │   ├── ProductosController.cs        # CRUD de productos y consulta de stock bajo
│   │   └── ProveedoresController.cs      # CRUD de proveedores
│   ├── Data/
│   │   └── ApplicationDbContext.cs       # Contexto de Entity Framework Core
│   ├── Models/
│   │   ├── Categoria.cs                  # Entidad tabla 'categorias'
│   │   ├── HistorialInventario.cs        # Entidad tabla 'historial_inventario'
│   │   ├── Producto.cs                   # Entidad tabla 'productos'
│   │   ├── Proveedor.cs                  # Entidad tabla 'proveedores'
│   │   └── Usuario.cs                    # Entidad tabla 'usuarios' y DTO LoginRequest
│   ├── wwwroot/
│   │   ├── index.html                    # Interfaz web principal
│   │   ├── css/style.css                 # Hojas de estilo
│   │   └── JS/app.js                     # Consumo de la API REST desde el navegador
│   ├── Program.cs                        # Configuración de servicios, Swagger y pipeline HTTP
│   └── WebApplication1.csproj            # Dependencias NuGet del proyecto
└── README.md                             # Documentación del proyecto