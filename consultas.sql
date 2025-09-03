USE PruebaFullStack
GO

-- (a) Porcentaje de ejecución por proyecto
/*
    El porcentaje de ejecución de fondos para cada proyecto registrado. La ejecución se
    calcula como el monto total ejecutado (gastado) dentro del total de fondos recibidos por
    medio de donaciones.
*/
WITH don
  AS (
       SELECT d.ProyectoId,
              SUM(d.Monto) AS TotalDonado
         FROM dbo.Donaciones d
        GROUP BY d.ProyectoId
     ),
     eje
  AS (
       SELECT ocd.ProyectoId,
              SUM(ocd.Monto) AS TotalEjecutado
         FROM dbo.OrdenesCompraDetalle ocd
        GROUP BY ocd.ProyectoId
     )

SELECT p.Id, CONCAT('P-', RIGHT('0000' + CAST(p.Id AS VARCHAR(10)), 4)) AS Codigo,
       p.Nombre,
       ISNULL(don.TotalDonado,0) AS TotalDonado,
       ISNULL(eje.TotalEjecutado,0) AS TotalEjecutado,
       CASE
           WHEN ISNULL(don.TotalDonado,0)=0
               THEN 0
           ELSE (ISNULL(eje.TotalEjecutado,0)*100.0/ISNULL(don.TotalDonado,0))
       END AS PorcentajeEjecucion
  FROM dbo.Proyectos p
       LEFT JOIN don ON don.ProyectoId = p.Id
       LEFT JOIN eje ON eje.ProyectoId = p.Id
 ORDER BY p.Id;

GO

-- (b) Disponibilidad por rubro del proyecto X (todos los rubros)
/*
    La disponibilidad de fondos en cada rubro del proyecto “X”, de modo que se muestren
    todos los rubros del proyecto (incluyendo los que pueden no tener ninguna donación
    recibida o ninguna orden de compra emitida)
*/
DECLARE @ProyectoId INT = 1;

WITH don
  AS (
       SELECT RubroId, 
              SUM(Monto) AS Donado
         FROM dbo.Donaciones
        WHERE ProyectoId = @ProyectoId
        GROUP BY RubroId
     ),
     eje
  AS (
       SELECT RubroId,
              SUM(Monto) AS Ejecutado
         FROM dbo.OrdenesCompraDetalle
        WHERE ProyectoId = @ProyectoId
        GROUP BY RubroId
     )

SELECT r.Id AS RubroId, r.Nombre,
       ISNULL(don.Donado,0)     AS TotalDonado,
       ISNULL(eje.Ejecutado,0)  AS TotalEjecutado,
       ISNULL(don.Donado,0) - ISNULL(eje.Ejecutado,0) AS Disponibilidad
  FROM dbo.Rubros r
       LEFT JOIN don ON don.RubroId = r.Id
       LEFT JOIN eje ON eje.RubroId = r.Id
 WHERE r.ProyectoId = @ProyectoId
 ORDER BY r.Nombre;