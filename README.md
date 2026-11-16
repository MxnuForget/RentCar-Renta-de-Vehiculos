# 🚗 RentCar — Sistema de Gestión y Renta de Vehículos

**Sistema de escritorio para la administración y gestión integral de servicios de alquiler de vehículos.**

RentCar es una aplicación desarrollada en C# y Windows Forms, respaldada por una base de datos SQL Server y estructurada mediante una arquitectura de múltiples capas. Su propósito es centralizar y optimizar los procesos relacionados con la gestión de clientes, vehículos, alquileres, empleados y operaciones administrativas.

El proyecto integra lógica de negocio, acceso a datos, validaciones y mecanismos de autenticación, buscando ofrecer una solución organizada, funcional y mantenible para la gestión de una empresa de renta de vehículos.

---

## 📌 Descripción del proyecto

RentCar nace como un proyecto académico de Ingeniería de Sistemas, con el objetivo de aplicar conocimientos de programación orientada a objetos, arquitectura de software, bases de datos relacionales y desarrollo de aplicaciones de escritorio.

La aplicación contempla diferentes módulos que permiten administrar las operaciones principales del negocio, proporcionando una interfaz gráfica para interactuar con la información y ejecutar los distintos procesos del sistema.

## ✨ Características principales

- **Gestión de clientes:** registro, consulta y administración de información de clientes.
- **Gestión de empleados:** administración de empleados y sus respectivos registros.
- **Administración de vehículos:** gestión del catálogo de vehículos disponibles.
- **Gestión de alquileres:** administración de operaciones de renta de vehículos.
- **Autenticación de usuarios:** inicio de sesión con mecanismos de protección de credenciales.
- **Control de roles y permisos:** diferenciación de accesos para Administrador, Operador y Cliente.
- **Gestión de pagos:** administración de métodos de pago y operaciones relacionadas.
- **Inspecciones y daños:** registro y gestión de información relacionada con las inspecciones de vehículos.
- **Sistema de reportes:** generación de informes mediante archivos RDLC.
- **Validaciones:** controles implementados en las diferentes capas para mantener la integridad de las operaciones.

## 🏗️ Arquitectura del sistema

El proyecto utiliza una arquitectura de cuatro capas, separando las responsabilidades para facilitar el mantenimiento, la organización y la escalabilidad del código.

```text
RentCar/
│
├── RentCarPresentacion/
│   ├── Formularios Windows Forms
│   ├── Controles e interfaces
│   └── Reportes RDLC
│
├── RentCar_Datos/
│   ├── Acceso a datos
│   ├── Conexiones
│   └── Consultas SQL
│
├── RentCar_Entidades/
│   ├── Clases del sistema
│   └── Modelos de datos
│
├── RentCar_Negocio/
│   ├── Lógica de negocio
│   ├── Autenticación
│   ├── Seguridad
│   └── Validaciones
│
├── BaseDeDatos/
│   └── Scripts SQL
│
├── README.md
└── RentCar.sln
```

### Responsabilidad de cada capa

| Capa | Responsabilidad |
|---|---|
| Presentación | Interfaz gráfica e interacción con el usuario. |
| Datos | Comunicación con SQL Server y operaciones de persistencia. |
| Entidades | Representación de las entidades y estructuras del sistema. |
| Negocio | Reglas, procesos, seguridad y validaciones. |

Esta separación permite mantener una estructura organizada y evita concentrar todas las responsabilidades en un único componente.

## 🔐 Seguridad y autenticación

Uno de los componentes importantes del proyecto es la implementación de mecanismos de autenticación y control de acceso.

Entre sus características se encuentran:

- Protección de contraseñas mediante PBKDF2 con SHA-256.
- Derivación de contraseñas con 100.000 iteraciones.
- Diferenciación de roles de usuario.
- Control de permisos según el tipo de usuario.
- Validaciones durante los procesos de autenticación.

El objetivo es fortalecer la protección de las credenciales y organizar el acceso a las funcionalidades del sistema.

## 🗄️ Base de datos

RentCar utiliza Microsoft SQL Server como sistema de gestión de bases de datos.

El proyecto incluye scripts SQL para la creación, configuración, actualización y carga de información de prueba.

Los archivos relacionados con la base de datos se encuentran en el directorio `BaseDeDatos`.

## 🛠️ Tecnologías utilizadas

| Tecnología | Aplicación |
|---|---|
| C# | Lenguaje de programación principal. |
| .NET / Windows Forms | Desarrollo de la aplicación de escritorio. |
| Microsoft SQL Server | Gestión de la base de datos. |
| SQL | Consultas y administración de información. |
| Visual Studio | Entorno de desarrollo. |
| RDLC | Diseño y generación de reportes. |
| Git | Control de versiones. |
| GitHub | Almacenamiento y gestión del repositorio. |

## ⚙️ Instalación y ejecución

Para ejecutar el proyecto en un entorno local, sigue estos pasos:

**1. Clonar el repositorio**

```bash
git clone https://github.com/MxnuForget/RentCar-Renta-de-Vehiculos.git
```

**2. Abrir el proyecto**

- Iniciar Visual Studio.
- Abrir el archivo `RentCar.sln`.
- Esperar a que se carguen los proyectos y sus dependencias.

**3. Configurar SQL Server**

- Tener una instancia de Microsoft SQL Server disponible.
- Revisar los scripts ubicados en `BaseDeDatos`.
- Crear y configurar la base de datos siguiendo los scripts correspondientes.
- Verificar la cadena de conexión utilizada por el proyecto.

**4. Ejecutar la aplicación**

- Establecer `RentCarPresentacion` como proyecto de inicio.
- Compilar la solución.
- Ejecutar la aplicación desde Visual Studio.

> **Nota:** La configuración de la conexión a SQL Server debe adaptarse a la instancia disponible en cada equipo.

## 📂 Estructura del repositorio

El repositorio contiene los siguientes componentes:

- `RentCarPresentacion`: interfaz gráfica y formularios.
- `RentCar_Datos`: acceso y comunicación con la base de datos.
- `RentCar_Entidades`: clases y entidades.
- `RentCar_Negocio`: lógica y reglas del sistema.
- `BaseDeDatos`: scripts SQL y archivos relacionados con la base de datos.
- `RentCar.sln`: solución principal de Visual Studio.

## 🎯 Objetivo académico

Este proyecto representa la aplicación práctica de conocimientos adquiridos durante la formación en Ingeniería de Sistemas, integrando diferentes áreas del desarrollo de software en una solución funcional.

Su desarrollo permitió trabajar con programación orientada a objetos, separación de responsabilidades, gestión de bases de datos, seguridad, validaciones, interfaces gráficas y control de versiones.

## 👨‍💻 Autor

**Emanuel Flores Torrico**

Estudiante de Ingeniería de Sistemas.

- GitHub: [@MxnuForget](https://github.com/MxnuForget)
- Proyecto: [RentCar – Sistema de Renta de Vehículos](https://github.com/MxnuForget/RentCar-Renta-de-Vehiculos)

---

<p align="center">

**RENTCAR — Sistema de Gestión y Renta de Vehículos**

*Desarrollado con dedicación, aprendizaje y pasión por la tecnología.*

</p>
