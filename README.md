## Payroll
## Lenguaje de programacion I
## Detalle de requerimientos.

Se requiere hacer un programa para manejar la nómina de
los empleados de una empresa, el sistema debe tener un
menú con las opciones para Administrar Empleados,
Administrar Nomina, Generar Reportes:


## Administrar Empleado 15%

# Entidad

{Empleado}
- Id
- Nombre
- Apellido
- Sexo
- Sueldo Bruto

# Funcionalidades

- Pantalla administrativa del CRUD
- Crear Empleado 
- Modificar Empleado 
- Activar e inactivar empleado 

# Gestionar Nomina 

#Entidad

{Nomina}
- Id
- EmpleadoId
- Sueldo Bruto
- Retención AFP (2.87%)
- Retención ARS (3.04%)
- Sueldo Imponible (Sueldo Bruto - (AFP+ARS))
- Retención ISR (ver nota 1)
- Total Retención (AFP+ARS+ISR)
- Sueldo neto (Sueldo bruto - Total Retención)

- Funcionalidades Generar Nomina 
Inactivar Nomina 5%
3 Reportes 20%
Reportes 1 Nomina por mes (Permitir filtra por sexo y en general) 10%
Reportes 2 Empleados Activos 5%
Reportes 3 Empleados Inactivos 5%

