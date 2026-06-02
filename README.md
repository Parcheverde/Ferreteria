# Ferretería API - ASP.NET Core 8 + MySQL

API REST desarrollada con ASP.NET Core 8 y MySQL para la gestión de una ferretería.

Permite administrar categorías, productos y movimientos de inventario.

## Tecnologías Utilizadas

* ASP.NET Core 8
* Entity Framework Core
* MySQL / MariaDB
* Swagger / OpenAPI
* C#

---

## Características

### Categorías

* Listar categorías
* Agregar categorías
* Eliminar categorías

### Productos

* Listar productos
* Agregar productos
* Actualizar productos
* Eliminar productos

### Movimientos

* Consultar historial de movimientos
* Filtrar movimientos por tipo

---

## Estructura del Proyecto

```text
Ferreteri/
│
├── Controllers/
├── Models/
├── Services/
├── appsettings.json
├── Program.cs
├── BDScript.sql
└── README.md
```

---

## Requisitos

* .NET 8 SDK
* MySQL o MariaDB
* XAMPP (opcional)
* Visual Studio 2022 o VS Code

---

## Configuración de Base de Datos

Crear la base de datos:

```sql
CREATE DATABASE ferreteri;
```

Ejecutar el script incluido en el repositorio:

```text
BDScript.sql
```

 ayuda : https://youtu.be/zTU8DfDLxoI
---

## Configuración de la Cadena de Conexión

Editar el archivo:

```text
appsettings.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "Connection": "server=localhost;port=3306;database=ferreteri;user=root;password=;"
  }
}
```

---

## Ejecución

Restaurar dependencias:

```bash
dotnet restore
```

Compilar:

```bash
dotnet build
```

Ejecutar:

```bash
dotnet run
```

---

## Swagger

Una vez iniciada la aplicación acceder a:

```text
http://localhost:5016/swagger
```

o al puerto configurado en `launchSettings.json`.

---

## Endpoints Disponibles

### Categorías

| Método | Endpoint                    |
| ------ | --------------------------- |
| GET    | /api/Categorias/listar      |
| GET    | /api/Categorias/count       |
| POST   | /api/Categorias/add         |
| DELETE | /api/Categorias/remove/{id} |

### Productos

| Método | Endpoint                   |
| ------ | -------------------------- |
| GET    | /api/Productos/listar      |
| GET    | /api/Productos/count       |
| POST   | /api/Productos/add         |
| PUT    | /api/Productos/update      |
| DELETE | /api/Productos/remove/{id} |

### Movimientos

| Método | Endpoint                     |
| ------ | ---------------------------- |
| GET    | /api/Movimientos             |
| GET    | /api/Movimientos/count       |
| GET    | /api/Movimientos/tipo/{tipo} |

---

## Registro de Movimientos

La API mantiene un historial de movimientos relacionados con los productos para llevar el control del inventario.

---

## Autor

Proyecto académico desarrollado con ASP.NET Core 8, Entity Framework Core y MySQL.
