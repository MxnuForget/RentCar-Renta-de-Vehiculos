/* =====================================================================
   DATOS DE PRUEBA para la base RentCar que YA EXISTE (no crea ni borra tablas)
   Ejecutar completo (F5) y leer la pestaña MENSAJES.
   Crea usuarios:  admin / 123456   y   operador / 123456
   (la contraseña se guarda como HASH, no en texto plano)
   ===================================================================== */
USE RentCar;
SET NOCOUNT ON;

/* ---------- 1) Personas y clientes ---------- */
BEGIN TRY
    INSERT INTO dbo.Persona (ci, nombres, ap_paterno, ap_materno, fecha_nacimiento)
    SELECT v.ci, v.nombres, v.ap_pat, v.ap_mat, v.nac
    FROM (VALUES
        ('1000001', 'Carlos', 'Mamani',    'Quispe',  CAST('1995-04-12' AS DATE)),
        ('1000002', 'Lucía',  'Rojas',     'Vargas',  CAST('1998-09-30' AS DATE)),
        ('1000003', 'Marco',  'Flores',    'Choque',  CAST('1990-01-20' AS DATE)),
        ('1000004', 'Ana',    'Gutiérrez', 'Salazar', CAST('1992-07-08' AS DATE)),
        ('1000005', 'Diego',  'Camacho',   'Rivero',  CAST('1996-11-15' AS DATE))
    ) v(ci, nombres, ap_pat, ap_mat, nac)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Persona p WHERE p.ci = v.ci);

    INSERT INTO dbo.Cliente (id_persona, licencia_conducir, fecha_venc_licencia)
    SELECT p.id_persona, v.lic, CAST('2030-12-31' AS DATE)
    FROM (VALUES ('1000001','LIC-0001'), ('1000002','LIC-0002'), ('1000003','LIC-0003')) v(ci, lic)
    JOIN dbo.Persona p ON p.ci = v.ci
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Cliente c WHERE c.id_persona = p.id_persona);

    PRINT 'OK: personas y clientes';
END TRY
BEGIN CATCH
    PRINT 'ERROR en personas/clientes: ' + ERROR_MESSAGE();
END CATCH;

/* ---------- 2) Empleados con usuario (admin / operador) ---------- */
IF EXISTS (SELECT 1 FROM dbo.Cargo) AND EXISTS (SELECT 1 FROM dbo.Sucursal)
BEGIN
    BEGIN TRY
        INSERT INTO dbo.Empleado (id_persona, fecha_contratacion, salario, usuario, contrasena, id_cargo, id_sucursal)
        SELECT p.id_persona, CAST(GETDATE() AS DATE), 3000, v.usuario, v.clave,
               (SELECT TOP 1 id_cargo FROM dbo.Cargo ORDER BY id_cargo),
               (SELECT TOP 1 id_sucursal FROM dbo.Sucursal ORDER BY id_sucursal)
        FROM (VALUES ('1000004', 'admin',    '100000.aQUPl5FHMraF6zjY3PQMTQ==.uiuBA1b/xtomadoZvHrYvPkEydK8f+sljvF+eFz7hGw='),
                     ('1000005', 'operador', '100000.3XndiAbJHxaenRLb4UUPIg==.MEepgNJMZ9ofniwLhRtbz1Ax03KGc1MfmrRq6rr+8RI=')) v(ci, usuario, clave)
        JOIN dbo.Persona p ON p.ci = v.ci
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.usuario = v.usuario OR e.id_persona = p.id_persona);

        UPDATE dbo.Empleado SET contrasena = '100000.aQUPl5FHMraF6zjY3PQMTQ==.uiuBA1b/xtomadoZvHrYvPkEydK8f+sljvF+eFz7hGw=' WHERE usuario = 'admin';
        UPDATE dbo.Empleado SET contrasena = '100000.3XndiAbJHxaenRLb4UUPIg==.MEepgNJMZ9ofniwLhRtbz1Ax03KGc1MfmrRq6rr+8RI=' WHERE usuario = 'operador';
        PRINT 'OK: empleados (admin y operador)';
    END TRY
    BEGIN CATCH
        PRINT 'ERROR en empleados: ' + ERROR_MESSAGE();
    END CATCH;
END
ELSE
    PRINT 'FALTA: registre al menos 1 fila en Cargo y 1 en Sucursal (clic derecho en la tabla > Editar las 200 primeras filas) y vuelva a ejecutar este script.';

/* ---------- 3) Métodos de pago ---------- */
BEGIN TRY
    INSERT INTO dbo.MetodoPago (nombre_metodo)
    SELECT v.n FROM (VALUES ('Efectivo'), ('Tarjeta'), ('Transferencia')) v(n)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.MetodoPago m WHERE m.nombre_metodo = v.n);
    PRINT 'OK: metodos de pago';
END TRY
BEGIN CATCH
    PRINT 'ERROR en metodos de pago: ' + ERROR_MESSAGE();
END CATCH;

/* ---------- 4) Modelos y vehiculos ---------- */
IF EXISTS (SELECT 1 FROM dbo.Marca) AND EXISTS (SELECT 1 FROM dbo.CategoriaVehiculo) AND EXISTS (SELECT 1 FROM dbo.Sucursal)
BEGIN
    BEGIN TRY
        DECLARE @marca INT = (SELECT TOP 1 id_marca FROM dbo.Marca ORDER BY id_marca);
        DECLARE @cat   INT = (SELECT TOP 1 id_categoria FROM dbo.CategoriaVehiculo ORDER BY id_categoria);
        DECLARE @suc   INT = (SELECT TOP 1 id_sucursal FROM dbo.Sucursal ORDER BY id_sucursal);

        INSERT INTO dbo.Modelo (id_marca, nombre_modelo, anio_lanzamiento)
        SELECT @marca, v.m, 2020 FROM (VALUES ('Corolla'), ('Vitara')) v(m)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Modelo x WHERE x.id_marca = @marca AND x.nombre_modelo = v.m);

        INSERT INTO dbo.Vehiculo (placa, color, anio_fabricacion, kilometraje, estado, tarifa_diaria,
                                  id_marca, nombre_modelo, id_categoria, id_sucursal)
        SELECT v.placa, v.color, 2022, 15000, 'Disponible', v.tarifa, @marca, v.modelo, @cat, @suc
        FROM (VALUES ('1234-ABC', 'Blanco', 'Corolla', 250.00),
                     ('2345-BCD', 'Gris',   'Vitara',  400.00),
                     ('3456-CDE', 'Rojo',   'Corolla', 260.00)) v(placa, color, modelo, tarifa)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Vehiculo x WHERE x.placa = v.placa);
        PRINT 'OK: modelos y vehiculos';
    END TRY
    BEGIN CATCH
        PRINT 'ERROR en vehiculos: ' + ERROR_MESSAGE();
    END CATCH;
END
ELSE
    PRINT 'FALTA: registre al menos 1 fila en Marca, CategoriaVehiculo y Sucursal (Editar las 200 primeras filas) y vuelva a ejecutar este script.';

/* ---------- Resumen ---------- */
SELECT 'Persona' AS tabla, COUNT(*) AS filas FROM dbo.Persona
UNION ALL SELECT 'Cliente',    COUNT(*) FROM dbo.Cliente
UNION ALL SELECT 'Empleado',   COUNT(*) FROM dbo.Empleado
UNION ALL SELECT 'MetodoPago', COUNT(*) FROM dbo.MetodoPago
UNION ALL SELECT 'Vehiculo',   COUNT(*) FROM dbo.Vehiculo;
