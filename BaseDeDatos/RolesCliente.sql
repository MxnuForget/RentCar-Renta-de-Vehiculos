/* =====================================================================
   RentCar - ROLES: cuentas de CLIENTE + usuario interno de reservas
   Ejecutar UNA vez (F5) sobre la base RentCar y leer la pestaña MENSAJES.
   NO borra nada: solo agrega una tabla y filas.
   Cuentas de prueba de clientes:  cliente1 / 123456   y   cliente2 / 123456
   ===================================================================== */
USE RentCar;
GO
SET NOCOUNT ON;

/* 1) Tabla de cuentas de cliente (la contraseña se guarda como HASH) */
IF OBJECT_ID('dbo.CuentaCliente','U') IS NULL
    CREATE TABLE dbo.CuentaCliente (
        id_persona INT          NOT NULL PRIMARY KEY REFERENCES dbo.Cliente(id_persona),
        usuario    VARCHAR(50)  NOT NULL UNIQUE,
        contrasena VARCHAR(255) NOT NULL
    );
GO

/* 2) Empleado interno: a su nombre se registran las reservas hechas por clientes.
      Su contraseña es inválida a propósito, así que nadie puede iniciar sesión con él. */
BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM dbo.Empleado WHERE usuario = 'reservas_web')
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE ci = '0000000')
            INSERT INTO dbo.Persona (nombres, ap_paterno, ap_materno, ci, fecha_nacimiento)
            VALUES ('Reserva', 'En línea', NULL, '0000000', '2000-01-01');

        INSERT INTO dbo.Empleado (id_persona, fecha_contratacion, salario, usuario, contrasena, id_cargo, id_sucursal)
        SELECT p.id_persona, CAST(GETDATE() AS DATE), 0, 'reservas_web', 'CUENTA-DESHABILITADA',
               (SELECT TOP 1 id_cargo FROM dbo.Cargo ORDER BY id_cargo),
               (SELECT TOP 1 id_sucursal FROM dbo.Sucursal ORDER BY id_sucursal)
        FROM dbo.Persona p WHERE p.ci = '0000000';
    END
    PRINT 'OK: usuario interno de reservas';
END TRY
BEGIN CATCH
    PRINT 'ERROR en usuario interno: ' + ERROR_MESSAGE();
END CATCH;

/* 3) Cuentas de prueba para los clientes que ya existen */
BEGIN TRY
    INSERT INTO dbo.CuentaCliente (id_persona, usuario, contrasena)
    SELECT p.id_persona, v.usuario, v.clave
    FROM (VALUES ('1000001', 'cliente1', '100000.qf1E66wNqUKYIdEXcftmmQ==.hz8CuTr3lZefsNTPcR/8SVacA8znZs0glnpA6ZXdDkw='),
                 ('1000002', 'cliente2', '100000.Ti0ZvzyqP2xRrnW8nW5jIw==.nFxME0mqpn3erjQ3weIg5+H1YpjnIloGf+Yal7cd6WA=')) v(ci, usuario, clave)
    JOIN dbo.Persona p ON p.ci = v.ci
    JOIN dbo.Cliente c ON c.id_persona = p.id_persona
    WHERE NOT EXISTS (SELECT 1 FROM dbo.CuentaCliente x WHERE x.id_persona = p.id_persona OR x.usuario = v.usuario);
    PRINT 'OK: cuentas cliente1 y cliente2';
END TRY
BEGIN CATCH
    PRINT 'ERROR en cuentas de cliente: ' + ERROR_MESSAGE();
END CATCH;

SELECT 'CuentaCliente' AS tabla, COUNT(*) AS filas FROM dbo.CuentaCliente
UNION ALL SELECT 'Empleado (incluye reservas_web)', COUNT(*) FROM dbo.Empleado;
GO
