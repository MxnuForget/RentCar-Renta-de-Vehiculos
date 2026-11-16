/* =====================================================================
   RentCar - OBSERVACIONES DEL DOCENTE (nivel base de datos)
   Ejecutar UNA vez (F5) en SSMS sobre la base RentCar, DESPUES de los otros scripts.
   Leer siempre la pestaña MENSAJES. No borra datos.

   1) El nombre de usuario NO puede repetirse en todo el sistema
      (ni entre empleados, ni entre clientes, ni entre empleado y cliente).
   2) Una persona solo puede tener UNA cuenta (o es empleado o es cliente con cuenta).
   3) El CI debe tener un formato valido (5 a 10 digitos y complemento opcional).
   ===================================================================== */
USE RentCar;
GO
SET NOCOUNT ON;

/* 0) Revisar si ya hay usuarios repetidos entre empleados y clientes (debe salir vacio) */
SELECT e.usuario AS usuario_repetido
FROM dbo.Empleado e JOIN dbo.CuentaCliente c ON c.usuario = e.usuario;
GO

/* 1 y 2) Triggers: bloquean la cuenta si el usuario ya existe en la otra tabla
          o si la persona ya tiene cuenta en la otra tabla */
CREATE OR ALTER TRIGGER dbo.trg_Empleado_CuentaUnica ON dbo.Empleado
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.CuentaCliente c ON c.usuario = i.usuario)
    BEGIN
        RAISERROR('El nombre de usuario no está disponible. Elija otro.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.CuentaCliente c ON c.id_persona = i.id_persona)
    BEGIN
        RAISERROR('Esta persona ya tiene una cuenta de cliente. Una persona solo puede tener un usuario.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

CREATE OR ALTER TRIGGER dbo.trg_CuentaCliente_CuentaUnica ON dbo.CuentaCliente
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.Empleado e ON e.usuario = i.usuario)
    BEGIN
        RAISERROR('El nombre de usuario no está disponible. Elija otro.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.Empleado e ON e.id_persona = i.id_persona)
    BEGIN
        RAISERROR('Esta persona ya tiene una cuenta de empleado. Una persona solo puede tener un usuario.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

/* 3) Formato del CI */
BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Persona_ci_formato')
        ALTER TABLE dbo.Persona WITH CHECK ADD CONSTRAINT CK_Persona_ci_formato
            CHECK (LEN(ci) BETWEEN 5 AND 14 AND ci NOT LIKE '%[^0-9A-Za-z-]%');
    PRINT 'OK: formato de CI';
END TRY
BEGIN CATCH
    PRINT 'ERROR en el CI (¿hay CI con simbolos raros?): ' + ERROR_MESSAGE();
END CATCH;
GO

PRINT 'LISTO: observaciones del docente aplicadas en la base de datos.';
GO
