/* =====================================================================
   RentCar - BASE DE DATOS DESDE CERO  (compatible con RentCar_Proyecto_v2)
   !!! ATENCION !!!  Este script BORRA la base RentCar actual (tablas y datos)
   y la vuelve a crear con 16 tablas y datos de prueba.
   Úsenlo solo si nadie del grupo necesita lo que hay en esa base.
   Usuarios de prueba:  admin / 123456   y   operador / 123456
   ===================================================================== */
USE master;
GO
IF DB_ID('RentCar') IS NOT NULL
BEGIN
    ALTER DATABASE RentCar SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE RentCar;
END
GO
CREATE DATABASE RentCar;
GO
USE RentCar;
GO

/* ------------------------- PERSONAS ------------------------- */
CREATE TABLE dbo.Persona (
    id_persona       INT IDENTITY(1,1) PRIMARY KEY,
    nombres          VARCHAR(80) NOT NULL,
    ap_paterno       VARCHAR(50) NOT NULL,
    ap_materno       VARCHAR(50) NULL,
    ci               VARCHAR(20) NOT NULL UNIQUE,
    fecha_nacimiento DATE        NOT NULL,
    email            VARCHAR(100) NULL
);
GO
CREATE TABLE dbo.TelefonoPersona (
    id_persona INT         NOT NULL REFERENCES dbo.Persona(id_persona),
    telefono   VARCHAR(20) NOT NULL,
    PRIMARY KEY (id_persona, telefono)
);
GO
CREATE TABLE dbo.Cliente (
    id_persona          INT         NOT NULL PRIMARY KEY REFERENCES dbo.Persona(id_persona),
    licencia_conducir   VARCHAR(30) NOT NULL UNIQUE,
    fecha_venc_licencia DATE        NOT NULL
);
GO
CREATE TABLE dbo.Cargo (
    id_cargo     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_cargo VARCHAR(50) NOT NULL UNIQUE
);
GO
CREATE TABLE dbo.Sucursal (
    id_sucursal     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_sucursal VARCHAR(80) NOT NULL UNIQUE,
    ciudad          VARCHAR(60) NULL
);
GO
CREATE TABLE dbo.Empleado (
    id_persona         INT           NOT NULL PRIMARY KEY REFERENCES dbo.Persona(id_persona),
    fecha_contratacion DATE          NOT NULL DEFAULT GETDATE(),
    salario            DECIMAL(10,2) NOT NULL CHECK (salario >= 0),
    usuario            VARCHAR(50)   NOT NULL UNIQUE,
    contrasena         VARCHAR(255)  NOT NULL,     -- HASH (iteraciones.sal.hash), nunca texto plano
    id_cargo           INT           NOT NULL REFERENCES dbo.Cargo(id_cargo),
    id_sucursal        INT           NOT NULL REFERENCES dbo.Sucursal(id_sucursal)
);
GO

/* ------------------------- VEHICULOS ------------------------- */
CREATE TABLE dbo.Marca (
    id_marca     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_marca VARCHAR(50) NOT NULL UNIQUE
);
GO
CREATE TABLE dbo.Modelo (                                   -- entidad débil de Marca
    id_marca         INT         NOT NULL REFERENCES dbo.Marca(id_marca),
    nombre_modelo    VARCHAR(60) NOT NULL,
    anio_lanzamiento INT         NULL,
    PRIMARY KEY (id_marca, nombre_modelo)
);
GO
CREATE TABLE dbo.CategoriaVehiculo (
    id_categoria     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_categoria VARCHAR(50) NOT NULL UNIQUE
);
GO
CREATE TABLE dbo.Vehiculo (
    id_vehiculo      INT IDENTITY(1,1) PRIMARY KEY,
    placa            VARCHAR(15)   NOT NULL UNIQUE,
    color            VARCHAR(30)   NULL,
    anio_fabricacion INT           NULL,
    kilometraje      INT           NOT NULL DEFAULT 0 CHECK (kilometraje >= 0),
    estado           VARCHAR(20)   NOT NULL DEFAULT 'Disponible'
                     CHECK (estado IN ('Disponible','Alquilado','Mantenimiento')),
    tarifa_diaria    DECIMAL(10,2) NOT NULL CHECK (tarifa_diaria > 0),
    id_marca         INT           NOT NULL,
    nombre_modelo    VARCHAR(60)   NOT NULL,
    id_categoria     INT           NOT NULL REFERENCES dbo.CategoriaVehiculo(id_categoria),
    id_sucursal      INT           NOT NULL REFERENCES dbo.Sucursal(id_sucursal),
    FOREIGN KEY (id_marca, nombre_modelo) REFERENCES dbo.Modelo(id_marca, nombre_modelo)
);
GO
CREATE TABLE dbo.Mantenimiento (
    id_mantenimiento INT IDENTITY(1,1) PRIMARY KEY,
    id_vehiculo      INT           NOT NULL REFERENCES dbo.Vehiculo(id_vehiculo),
    fecha            DATE          NOT NULL,
    descripcion      VARCHAR(200)  NOT NULL,
    costo            DECIMAL(10,2) NOT NULL CHECK (costo >= 0)
);
GO
CREATE TABLE dbo.MetodoPago (
    id_metodo_pago INT IDENTITY(1,1) PRIMARY KEY,
    nombre_metodo  VARCHAR(50) NOT NULL UNIQUE
);
GO

/* ------------------------- OPERACIÓN ------------------------- */
CREATE TABLE dbo.Alquiler (
    id_alquiler           INT IDENTITY(1,1) PRIMARY KEY,
    fecha_inicio          DATE        NOT NULL,
    fecha_fin_pactada     DATE        NOT NULL,
    fecha_devolucion_real DATE        NULL,
    estado_alquiler       VARCHAR(20) NOT NULL DEFAULT 'Activo'
                          CHECK (estado_alquiler IN ('Activo','Finalizado','Cancelado')),
    id_cliente            INT         NOT NULL REFERENCES dbo.Cliente(id_persona),
    id_empleado           INT         NOT NULL REFERENCES dbo.Empleado(id_persona),
    id_vehiculo           INT         NOT NULL REFERENCES dbo.Vehiculo(id_vehiculo),
    CHECK (fecha_fin_pactada >= fecha_inicio)
);
GO
CREATE TABLE dbo.Pago (                                     -- entidad débil de Alquiler
    id_alquiler    INT           NOT NULL REFERENCES dbo.Alquiler(id_alquiler),
    num_pago       INT           NOT NULL,
    fecha_pago     DATE          NOT NULL,
    monto          DECIMAL(10,2) NOT NULL CHECK (monto > 0),
    id_metodo_pago INT           NOT NULL REFERENCES dbo.MetodoPago(id_metodo_pago),
    PRIMARY KEY (id_alquiler, num_pago)
);
GO
CREATE TABLE dbo.Inspeccion (
    id_inspeccion    INT IDENTITY(1,1) PRIMARY KEY,
    tipo_inspeccion  VARCHAR(20)  NOT NULL
                     CHECK (tipo_inspeccion IN ('Entrega','Devolucion')),
    fecha_inspeccion DATE         NOT NULL,
    observaciones    VARCHAR(500) NULL,
    id_alquiler      INT          NOT NULL REFERENCES dbo.Alquiler(id_alquiler)
);
GO
CREATE TABLE dbo.DetalleDanio (                             -- entidad débil de Inspeccion
    id_inspeccion     INT           NOT NULL REFERENCES dbo.Inspeccion(id_inspeccion),
    num_item          INT           NOT NULL,
    descripcion_danio VARCHAR(200)  NOT NULL,
    costo_reparacion  DECIMAL(10,2) NOT NULL CHECK (costo_reparacion >= 0),
    PRIMARY KEY (id_inspeccion, num_item)
);
GO

/* ------------------------- DATOS DE PRUEBA ------------------------- */
INSERT INTO dbo.Cargo (nombre_cargo) VALUES ('Administrador'), ('Operador');
INSERT INTO dbo.Sucursal (nombre_sucursal, ciudad) VALUES ('Sucursal Central','Cochabamba'), ('Sucursal Aeropuerto','Cochabamba');
INSERT INTO dbo.Marca (nombre_marca) VALUES ('Toyota'), ('Nissan'), ('Suzuki');
INSERT INTO dbo.CategoriaVehiculo (nombre_categoria) VALUES ('Económico'), ('SUV'), ('Camioneta');
INSERT INTO dbo.MetodoPago (nombre_metodo) VALUES ('Efectivo'), ('Tarjeta'), ('Transferencia');
GO
INSERT INTO dbo.Modelo (id_marca, nombre_modelo, anio_lanzamiento)
SELECT m.id_marca, v.modelo, v.anio
FROM (VALUES ('Toyota','Corolla',2020), ('Toyota','Hilux',2021), ('Nissan','Sentra',2020), ('Suzuki','Vitara',2022)) v(marca, modelo, anio)
JOIN dbo.Marca m ON m.nombre_marca = v.marca;
GO
INSERT INTO dbo.Persona (nombres, ap_paterno, ap_materno, ci, fecha_nacimiento) VALUES
    ('Carlos', 'Mamani',    'Quispe',  '1000001', '1995-04-12'),
    ('Lucía',  'Rojas',     'Vargas',  '1000002', '1998-09-30'),
    ('Marco',  'Flores',    'Choque',  '1000003', '1990-01-20'),
    ('Ana',    'Gutiérrez', 'Salazar', '1000004', '1992-07-08'),
    ('Diego',  'Camacho',   'Rivero',  '1000005', '1996-11-15');
GO
INSERT INTO dbo.Cliente (id_persona, licencia_conducir, fecha_venc_licencia)
SELECT p.id_persona, v.lic, '2030-12-31'
FROM (VALUES ('1000001','LIC-0001'), ('1000002','LIC-0002'), ('1000003','LIC-0003')) v(ci, lic)
JOIN dbo.Persona p ON p.ci = v.ci;
GO
INSERT INTO dbo.Empleado (id_persona, fecha_contratacion, salario, usuario, contrasena, id_cargo, id_sucursal)
SELECT p.id_persona, CAST(GETDATE() AS DATE), 3000, v.usuario, v.clave, c.id_cargo, s.id_sucursal
FROM (VALUES ('1000004', 'Administrador', 'admin',    '100000.QWbhrPcOiTCBEzsQW2ZSGw==.FHwBKQC49EKtvrlA+syxMIfBra2xCdAgnfYJ3GmZVCE='),
             ('1000005', 'Operador',      'operador', '100000.lhhJayGFfkVYqwbIHEgiNQ==.T2sBctHUAZ+h7sFuMycRyYyaAxWr66IZF41YmwSocsM=')) v(ci, cargo, usuario, clave)
JOIN dbo.Persona p ON p.ci = v.ci
JOIN dbo.Cargo c ON c.nombre_cargo = v.cargo
JOIN dbo.Sucursal s ON s.nombre_sucursal = 'Sucursal Central';
GO
INSERT INTO dbo.Vehiculo (placa, color, anio_fabricacion, kilometraje, estado, tarifa_diaria, id_marca, nombre_modelo, id_categoria, id_sucursal)
SELECT v.placa, v.color, v.anio, 15000, 'Disponible', v.tarifa, mo.id_marca, mo.nombre_modelo, ca.id_categoria, su.id_sucursal
FROM (VALUES
    ('1234-ABC', 'Blanco', 'Corolla', 'Económico', 'Sucursal Central',    2022, 250.00),
    ('2345-BCD', 'Gris',   'Sentra',  'Económico', 'Sucursal Central',    2021, 240.00),
    ('3456-CDE', 'Rojo',   'Vitara',  'SUV',       'Sucursal Aeropuerto', 2023, 400.00),
    ('4567-DEF', 'Negro',  'Hilux',   'Camioneta', 'Sucursal Aeropuerto', 2022, 500.00)
) v(placa, color, modelo, categoria, sucursal, anio, tarifa)
JOIN dbo.Modelo mo ON mo.nombre_modelo = v.modelo
JOIN dbo.CategoriaVehiculo ca ON ca.nombre_categoria = v.categoria
JOIN dbo.Sucursal su ON su.nombre_sucursal = v.sucursal;
GO

/* ------------------------- RESUMEN ------------------------- */
SELECT 'Persona' AS tabla, COUNT(*) AS filas FROM dbo.Persona
UNION ALL SELECT 'Cliente',    COUNT(*) FROM dbo.Cliente
UNION ALL SELECT 'Empleado',   COUNT(*) FROM dbo.Empleado
UNION ALL SELECT 'Marca',      COUNT(*) FROM dbo.Marca
UNION ALL SELECT 'Modelo',     COUNT(*) FROM dbo.Modelo
UNION ALL SELECT 'Vehiculo',   COUNT(*) FROM dbo.Vehiculo
UNION ALL SELECT 'MetodoPago', COUNT(*) FROM dbo.MetodoPago;
GO
