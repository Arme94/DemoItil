# DemoItil

## Descripción

DemoItil es una aplicación web ASP.NET Core 8.0 que ofrece un servicio de pronóstico del tiempo mediante una API REST. La aplicación ha sido desarrollada como parte de un proyecto académico para el curso de Gobierno de TI, centrado en prácticas ITIL.

## Características

- API REST para consulta de pronóstico del tiempo
- Documentación de API mediante Swagger/OpenAPI
- Diseño minimalista con Minimal APIs de .NET 8
- Preparada para contenedores Docker

## Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (opcional, para ejecución en contenedores)

## Instalación y ejecución local

1. Clonar el repositorio
2. Navegar al directorio del proyecto
3. Ejecutar la aplicación:

```
dotnet run
```

La aplicación estará disponible en:

- http://localhost:80 - Cuando se ejecuta localmente
- https://localhost:443 - Para conexiones HTTPS

## Endpoints de la API

- `GET /weatherforecast` - Devuelve un pronóstico del tiempo para los próximos 5 días

## Ejecución con Docker

El proyecto incluye un Dockerfile que permite construir y ejecutar la aplicación en un contenedor:

```
# Construir la imagen
docker build -t demoitil .

# Ejecutar el contenedor
docker run -p 8080:80 demoitil
```

La aplicación estará disponible en http://localhost:8080

## Documentación de la API

La documentación Swagger estará disponible en `/swagger` cuando la aplicación se ejecute en modo desarrollo.

## Tecnologías utilizadas

- ASP.NET Core 8.0
- Minimal APIs
- Swagger/OpenAPI
- Docker
