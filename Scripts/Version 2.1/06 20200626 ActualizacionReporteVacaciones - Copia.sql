-------------------------------------------------
--\\Actualizacion de procedimineto almacenado//--
-------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_ReporteVacaciones]    Script Date: 6/26/2020 11:45:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_ReporteVacaciones]
		@Fecha_Corte varchar(8)
AS
BEGIN

	SET NOCOUNT ON;
	SELECT * FROM (
		SELECT EMPLEADOS.Cod_Empleado, EMPLEADOS.Estado,  Empleado, Cedula,
			SUM(CASE WHEN SubPeriodo = 0 THEN Dias_Disponibles ELSE 0 END) AS Dias_Causados,	
			SUM(CASE WHEN SubPeriodo <> 0 THEN Dias_Tiempo ELSE 0 END) AS Dias_Tiempo,
			SUM(CASE WHEN SubPeriodo <> 0 THEN Dias_Dinero ELSE 0 END) AS Dias_Dinero
		FROM EMPLEADOS
		JOIN VACACIONES on (EMPLEADOS.Cod_Empleado = VACACIONES.Cod_Empleado)
		WHERE EMPLEADOS.Estado <> 'R'
		GROUP BY EMPLEADOS.Cod_Empleado, Empleado, EMPLEADOS.Estado , Cedula
	) V LEFT JOIN (
		SELECT Cod_Empleado as Cod2,
			SUM(CASE WHEN SOLICITUDES.Estado not in ('A', 'AP', 'R', 'D') THEN Cantidad ELSE 0 END) AS Dias_Pendientes,
			SUM(CASE WHEN SOLICITUDES.Estado in ('A') THEN Cantidad ELSE 0 END) AS Dias_Aprobados,
			SUM(CASE WHEN SOLICITUDES.Estado in ('AP') THEN Cantidad ELSE 0 END) AS Dias_Pagado,
			SUM(CASE WHEN SOLICITUDES.Estado in ('A') AND SOLICITUDES.Fec_Salida <= @Fecha_Corte THEN Cantidad  ELSE 0 END) AS Dias_Aprobados_Corte
		from SOLICITUDES
		GROUP BY Cod_Empleado
	) S on V.Cod_Empleado = S.Cod2
	ORDER BY V.Cod_Empleado
END
