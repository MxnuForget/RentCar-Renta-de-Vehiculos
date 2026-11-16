/* =====================================================================
   RentCar - MEJORAS DE AUTENTICACIÓN (ejecutar DESPUÉS de RolesCliente.sql)
   - Estado (activo) y fecha de creación en cuentas de empleado y de cliente
   - Rol del empleado: Administrador u Operador
   - Correo único en Persona
   - Contraseñas de prueba nuevas (cumplen la política de 8 caracteres con letra y número)
   Ejecutar UNA vez (F5) y leer la pestaña MENSAJES. No borra datos.
   ===================================================================== */
USE RentCar;
GO
SET NOCOUNT ON;

/* 1) Columnas nuevas */
IF COL_LENGTH('dbo.CuentaCliente','activo') IS NULL
    ALTER TABLE dbo.CuentaCliente ADD activo BIT NOT NULL CONSTRAINT DF_CuentaCliente_activo DEFAULT 1;
IF COL_LENGTH('dbo.CuentaCliente','fecha_creacion') IS NULL
    ALTER TABLE dbo.CuentaCliente ADD fecha_creacion DATETIME2 NOT NULL CONSTRAINT DF_CuentaCliente_fecha DEFAULT SYSDATETIME();
IF COL_LENGTH('dbo.Empleado','activo') IS NULL
    ALTER TABLE dbo.Empleado ADD activo BIT NOT NULL CONSTRAINT DF_Empleado_activo DEFAULT 1;
IF COL_LENGTH('dbo.Empleado','fecha_creacion') IS NULL
    ALTER TABLE dbo.Empleado ADD fecha_creacion DATETIME2 NOT NULL CONSTRAINT DF_Empleado_fecha DEFAULT SYSDATETIME();
IF COL_LENGTH('dbo.Empleado','rol') IS NULL
    ALTER TABLE dbo.Empleado ADD rol VARCHAR(20) NOT NULL CONSTRAINT DF_Empleado_rol DEFAULT 'Operador'
        CONSTRAINT CK_Empleado_rol CHECK (rol IN ('Administrador','Operador'));
GO

/* 2) Roles y estado de las cuentas existentes */
BEGIN TRY
    UPDATE dbo.Empleado SET rol = 'Administrador' WHERE usuario = 'admin';
    UPDATE dbo.Empleado SET rol = 'Operador'      WHERE usuario = 'operador';
    UPDATE dbo.Empleado SET activo = 0            WHERE usuario = 'reservas_web';
    PRINT 'OK: roles y estado de empleados';
END TRY
BEGIN CATCH
    PRINT 'ERROR en roles: ' + ERROR_MESSAGE();
END CATCH;

/* 3) Correo único (los NULL no cuentan) */
BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Persona_email' AND object_id = OBJECT_ID('dbo.Persona'))
        CREATE UNIQUE INDEX UX_Persona_email ON dbo.Persona(email) WHERE email IS NOT NULL;
    PRINT 'OK: correo unico';
END TRY
BEGIN CATCH
    PRINT 'ERROR en correo unico (¿hay correos repetidos?): ' + ERROR_MESSAGE();
END CATCH;

/* 4) Contraseñas de prueba nuevas: RentCar2026 (hash) */
BEGIN TRY
    UPDATE dbo.Empleado      SET contrasena = '100000.VZSqGkQgbK6BXMwoYKeBvQ==.q0TsNjyRky/V26XyVOcZ0OHeBEBL6YnpbiWZwrXXLnc=' WHERE usuario = 'admin';
    UPDATE dbo.Empleado      SET contrasena = '100000.1nuyjXbPCidwBm5dsFkNQA==.sdJOjIl8ACNV/E5IPFHp/ZiQKDumrOs3eR6bfKuELp4=' WHERE usuario = 'operador';
    UPDATE dbo.CuentaCliente SET contrasena = '100000.7nb2gOaXRFQTDBDI0063lA==.QcO0bh/J0cW/NzzqPJQ8EBcJXCFyDr3SoEZBhsp/uWU=' WHERE usuario = 'cliente1';
    UPDATE dbo.CuentaCliente SET contrasena = '100000.A64s173Imsf6B6Uhm5NB5Q==.csG9vzH9hNMpKfCsyEz/mPS68nAOjastQeZYhJKjeo4=' WHERE usuario = 'cliente2';
    PRINT 'OK: contrasenas de prueba = RentCar2026';
END TRY
BEGIN CATCH
    PRINT 'ERROR en contrasenas: ' + ERROR_MESSAGE();
END CATCH;

SELECT usuario, rol, activo FROM dbo.Empleado ORDER BY usuario;
GO
