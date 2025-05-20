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

# 🚀 Tutorial de Despliegue Automatizado con Docker, GitHub Actions y Podman

Este tutorial guía paso a paso el despliegue de una aplicación .NET Core como ejemplo práctico de la **práctica de Gestión de Despliegue (ITIL 4)**.

---

## 🧱 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) o [Podman](https://podman.io/)
- Cuenta en [Docker Hub](https://hub.docker.com/)
- Cuenta en [GitHub](https://github.com/)

---

## 1️⃣ Crear la aplicación .NET

```bash
dotnet new webapi -n GestionDespliegueApp
cd GestionDespliegueApp
```

### En `Program.cs`, agrega:

```csharp
builder.WebHost.UseUrls("http://0.0.0.0:80");
```

Esto permite que el contenedor escuche desde fuera.

---

## 2️⃣ Crear el Dockerfile

En la raíz del proyecto (`GestionDespliegueApp`), crea un archivo llamado `Dockerfile`:

```Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GestionDespliegueApp.dll"]
```

---

## 3️⃣ Construir y probar localmente con Podman

```bash
podman build -t demoitil .
podman run -d -p 8080:80 --name demoitil demoitil
```

Abre en el navegador:

```
http://localhost:8080/weatherforecast
```

---

## 4️⃣ Subir a Docker Hub con GitHub Actions

### 🔐 A. Crear token en Docker Hub

1. Ve a https://hub.docker.com/settings/security
2. Crea un Access Token

### 🔐 B. Agregar secretos en GitHub

Repositorio → Settings → Secrets → Actions → New repository secret:

- `DOCKERHUB_USERNAME`
- `DOCKERHUB_TOKEN`

### 🛠️ C. Crear workflow `.github/workflows/docker-publish.yml`

```yaml
name: Build and Push to Docker Hub

on:
  push:
    branches: ["main"]

jobs:
  build-and-push:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v3

      - name: Login to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKERHUB_USERNAME }}
          password: ${{ secrets.DOCKERHUB_TOKEN }}

      - name: Build and push Docker image
        uses: docker/build-push-action@v5
        with:
          context: .
          file: ./Dockerfile
          push: true
          tags: ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:latest
```

---

## 5️⃣ Desplegar desde el servidor con Podman Compose

### A. Crear archivo `docker-compose.yml`

```yaml
services:
  demoitil:
    image: tu_usuario/demoitil:latest
    container_name: demoitil
    ports:
      - "8080:80"
    restart: unless-stopped
```

> Cambia `tu_usuario` por tu nombre en Docker Hub.

### B. Ejecutar

```bash
podman-compose up -d
```

### C. Verificar

```bash
curl http://localhost:8080/weatherforecast
```

---

## 🧠 Relación con ITIL 4

| Práctica ITIL               | Aplicación en el tutorial                          |
| --------------------------- | -------------------------------------------------- |
| Gestión de Despliegue       | Imagen Docker desplegada automáticamente           |
| Gestión de Liberación       | Se usa `latest` como versión de producción         |
| Gestión de la Configuración | Dockerfile y Git definen la configuración          |
| Desarrollo y Pruebas        | Uso de GitHub Actions como pipeline de integración |

---

## ✅ Resultado

Tu aplicación estará:

- Construida automáticamente al hacer push en GitHub
- Publicada en Docker Hub
- Desplegada localmente con Podman en tu servidor

```
http://<tu-ip>:8080/weatherforecast
```

---

🎓 Ideal para demostrar automatización y buenas prácticas de despliegue según ITIL 4.
