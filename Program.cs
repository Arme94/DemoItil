var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("http://0.0.0.0:80");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var practicasGestionGeneral = new Dictionary<string, string>
{
    { "Gestión Estratégica", "Definir la dirección general de la organización de TI y cómo entregará valor" },
    { "Gestión de Portafolio", "Asegurar que la organización tenga la combinación correcta de programas, proyectos y servicios" },
    { "Gestión Financiera", "Administrar el presupuesto, la contabilidad y la facturación de los servicios de TI" }
};

var practicasGestionServicios = new Dictionary<string, string>
{
    { "Gestión de Incidentes", "Restaurar el servicio normal lo más rápido posible" },
    { "Gestión de Problemas", "Analizar y resolver las causas de los incidentes" },
    { "Gestión de Nivel de Servicio", "Acordar y garantizar los niveles de servicio" },
    { "Gestión de Disponibilidad", "Asegurar que los servicios estén disponibles cuando se necesiten" }
};

var practicasGestionTecnica = new Dictionary<string, string>
{
    { "Gestión de Cambios", "Asegurar que los cambios se implementen de manera controlada" },
    { "Gestión de Configuración", "Mantener información sobre los componentes de infraestructura" },
    { "Gestión de Despliegue", "Planificar y controlar la implementación de software y hardware" },
    { "Gestión de Seguridad", "Mantener la confidencialidad, integridad y disponibilidad de la información" }
};

var incidents = new List<Incident>
{
    new Incident(1, "Servidor caído", "Alta", "En progreso", "El servidor principal no responde", DateTime.Now.AddHours(-2)),
    new Incident(2, "Error en aplicación web", "Media", "Abierto", "Los usuarios reciben error 500 al iniciar sesión", DateTime.Now.AddHours(-12)),
    new Incident(3, "Lentitud en la red", "Baja", "Resuelto", "Los usuarios reportan lentitud en la conexión", DateTime.Now.AddDays(-1))
};

var services = new List<Service>
{
    new Service(1, "Correo electrónico", "Activo", "Servicio de correo para toda la organización", 99.9m),
    new Service(2, "ERP corporativo", "Activo", "Sistema de planificación de recursos empresariales", 99.5m),
    new Service(3, "Sistema de tickets", "Mantenimiento", "Sistema para gestión de incidentes y peticiones", 98.0m)
};

var changes = new List<Change>
{
    new Change(1, "Actualización SO", "Normal", "Aprobado", "Actualizar el sistema operativo a la última versión", "IT Operations", DateTime.Now.AddDays(7)),
    new Change(2, "Implementación Firewall", "Mayor", "Pendiente", "Instalación de nuevo firewall en la red corporativa", "Seguridad", DateTime.Now.AddDays(14)),
    new Change(3, "Migración Base de Datos", "Emergencia", "Implementado", "Migración urgente por fallo en servidor principal", "Infraestructura", DateTime.Now.AddDays(-1))
};

var configurationItems = new List<ConfigurationItem>
{
    new ConfigurationItem(1, "SRV-DB-001", "Servidor", "Producción", "Servidor de base de datos principal", "Dell PowerEdge R740", "IT Infrastructure"),
    new ConfigurationItem(2, "SRV-APP-001", "Servidor", "Producción", "Servidor de aplicaciones principal", "HP ProLiant DL380", "IT Infrastructure"),
    new ConfigurationItem(3, "FW-001", "Firewall", "Producción", "Firewall principal de la red", "Cisco ASA 5500", "IT Security"),
    new ConfigurationItem(4, "SW-CORE-001", "Switch", "Producción", "Switch core principal", "Cisco Catalyst 9300", "IT Network"),
    new ConfigurationItem(5, "WS-DEV-001", "Estación de trabajo", "Desarrollo", "Estación de trabajo para desarrollo", "Dell Precision 5570", "IT Development")
};

app.MapGet("/itil", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetItil")
.WithOpenApi();

app.MapGet("/itil/gestion-general", () =>
{
    return practicasGestionGeneral;
})
.WithName("GetPracticasGestionGeneral")
.WithOpenApi()
.WithDescription("Obtiene las principales prácticas de gestión general de ITIL");

app.MapGet("/itil/gestion-general/portfolio", () =>
{
    var proyectos = new[]
    {
        new { Id = 1, Nombre = "Migración ERP", Estado = "En progreso", Prioridad = "Alta", FechaInicio = DateTime.Now.AddMonths(-2), FechaFin = DateTime.Now.AddMonths(4) },
        new { Id = 2, Nombre = "Implementación CRM", Estado = "Planificado", Prioridad = "Media", FechaInicio = DateTime.Now.AddMonths(1), FechaFin = DateTime.Now.AddMonths(6) },
        new { Id = 3, Nombre = "Renovación Infraestructura", Estado = "Aprobado", Prioridad = "Alta", FechaInicio = DateTime.Now.AddMonths(2), FechaFin = DateTime.Now.AddMonths(5) }
    };
    return proyectos;
})
.WithName("GetPortfolio")
.WithOpenApi()
.WithDescription("Obtiene la información del portafolio de proyectos");

app.MapGet("/itil/gestion-servicios", () =>
{
    return practicasGestionServicios;
})
.WithName("GetPracticasGestionServicios")
.WithOpenApi()
.WithDescription("Obtiene las principales prácticas de gestión de servicios de ITIL");

app.MapGet("/itil/gestion-servicios/incidentes", () =>
{
    return incidents;
})
.WithName("GetIncidentes")
.WithOpenApi()
.WithDescription("Obtiene todos los incidentes registrados");

app.MapGet("/itil/gestion-servicios/catalogo", () =>
{
    return services;
})
.WithName("GetCatalogoServicios")
.WithOpenApi()
.WithDescription("Obtiene el catálogo de servicios disponibles");

app.MapGet("/itil/gestion-tecnica", () =>
{
    return practicasGestionTecnica;
})
.WithName("GetPracticasGestionTecnica")
.WithOpenApi()
.WithDescription("Obtiene las principales prácticas de gestión técnica de ITIL");

app.MapGet("/itil/gestion-tecnica/cambios", () =>
{
    return changes;
})
.WithName("GetCambios")
.WithOpenApi()
.WithDescription("Obtiene todos los cambios registrados");

app.MapGet("/itil/gestion-tecnica/configuracion", () =>
{
    return configurationItems;
})
.WithName("GetElementosConfiguracion")
.WithOpenApi()
.WithDescription("Obtiene todos los elementos de configuración");

app.MapGet("/itil/dashboard", () =>
{
    var now = DateTime.Now;

    var incidentesPendientes = incidents.Count(i => i.Status != "Resuelto");
    var incidentesUrgentes = incidents.Count(i => i.Priority == "Alta");
    var serviciosActivos = services.Count(s => s.Status == "Activo");
    var cambiosProgramados = changes.Count(c => c.ScheduledDate > now && c.Status == "Aprobado");
    var slaPromedio = services.Average(s => s.SLA);

    var dashboard = new
    {
        GestionGeneral = new
        {
            ProyectosActivos = 3,
            PresupuestoAnual = 1250000,
            RiesgosCriticos = 2
        },
        GestionServicios = new
        {
            IncidentesPendientes = incidentesPendientes,
            IncidentesUrgentes = incidentesUrgentes,
            ServiciosActivos = serviciosActivos,
            SLAPromedio = slaPromedio
        },
        GestionTecnica = new
        {
            CambiosProgramados = cambiosProgramados,
            ElementosConfiguracion = configurationItems.Count,
            VulnerabilidadesDetectadas = 5
        },
        FechaActualizacion = now
    };

    return dashboard;
})
.WithName("GetDashboard")
.WithOpenApi()
.WithDescription("Obtiene un dashboard integral con KPIs de ITIL por categoría");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Incident(int Id, string Title, string Priority, string Status, string Description, DateTime CreatedAt);

record Service(int Id, string Name, string Status, string Description, decimal SLA);

record Change(int Id, string Title, string Category, string Status, string Description, string Team, DateTime ScheduledDate);

record ConfigurationItem(int Id, string Name, string Type, string Environment, string Description, string Model, string Owner);
