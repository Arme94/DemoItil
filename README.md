# DemoItil

## Descripción

DemoItil es una aplicación web ASP.NET Core 8.0 que implementa una API REST para gestionar prácticas ITIL. La aplicación ha sido desarrollada como parte de un proyecto académico para el curso de Gobierno de TI, centrándose en las mejores prácticas de ITIL y DevOps.

## Características

- API REST para gestión de prácticas ITIL
- Dashboard con KPIs de ITIL
- Gestión de incidentes, servicios y cambios
- Documentación de API mediante Swagger/OpenAPI
- Diseño minimalista con Minimal APIs de .NET 8
- Pipeline de CI/CD automatizado
- Despliegue containerizado con Docker

## Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Git](https://git-scm.com/)

## Instalación y ejecución local

1. Clonar el repositorio
2. Navegar al directorio del proyecto
3. Ejecutar la aplicación:

```bash
dotnet run
```

La aplicación estará disponible en:

- http://localhost:80 - Cuando se ejecuta localmente
- https://localhost:443 - Para conexiones HTTPS

## Flujo de CI/CD

### Integración Continua (CI)

Nuestro pipeline de CI se ejecuta automáticamente en cada push al repositorio y realiza las siguientes tareas:

1. **Validación de código**

   - Análisis estático de código
   - Verificación de estilo de código
   - Detección de vulnerabilidades

2. **Pruebas automatizadas**

   - Ejecución de pruebas unitarias
   - Pruebas de integración
   - Generación de reportes de cobertura

3. **Construcción del artefacto**
   - Compilación del proyecto
   - Generación de imagen Docker
   - Escaneo de seguridad de la imagen

### Despliegue Continuo (CD)

El proceso de CD se activa automáticamente después de un CI exitoso:

1. **Ambiente de Desarrollo**

   - Despliegue automático al aprobar los tests
   - Validación de la API con pruebas de humo

2. **Ambiente de Staging**

   - Despliegue manual con aprobación
   - Pruebas de integración completas
   - Validación de performance

3. **Ambiente de Producción**
   - Despliegue manual con doble aprobación
   - Monitoreo de métricas post-despliegue
   - Rollback automatizado en caso de fallo

## Trabajando con el Pipeline

### Triggers del Pipeline

- **Push a main**: Activa el pipeline completo
- **Pull Request**: Ejecuta validaciones y pruebas
- **Release Tag**: Inicia el proceso de despliegue a producción

### Comandos útiles

```bash
# Construir la imagen localmente
docker build -t demoitil .

# Ejecutar contenedor local
docker run -p 8080:80 demoitil

# Ejecutar pruebas
dotnet test

# Validar el código
dotnet format --verify-no-changes
```

## Monitoreo y Métricas

El pipeline incluye monitoreo automático de:

- Tiempo de construcción y despliegue
- Cobertura de código
- Vulnerabilidades detectadas
- Performance de la aplicación
- Disponibilidad del servicio

## Endpoints de la API

- `GET /itil/dashboard` - Dashboard general con KPIs
- `GET /itil/gestion-servicios` - Prácticas de gestión de servicios
- `GET /itil/gestion-tecnica` - Prácticas de gestión técnica
- `GET /itil/gestion-general` - Prácticas de gestión general

## Documentación

La documentación completa de la API está disponible en:

- Swagger UI: `/swagger` (en modo desarrollo)
- OpenAPI JSON: `/swagger/v1/swagger.json`

## Tecnologías utilizadas

- ASP.NET Core 8.0
- GitHub Actions (CI/CD)
- Docker
- Swagger/OpenAPI
- SonarCloud (Análisis de código)
- Azure Container Registry
- Kubernetes (Orquestación de contenedores)

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
dotnet new webapi -n DemoItil
cd DemoItil
```

### En `Program.cs`, agrega:

```csharp
builder.WebHost.UseUrls("http://0.0.0.0:80");
```

Esto permite que el contenedor escuche desde fuera.

---

## 2️⃣ Crear el Dockerfile

En la raíz del proyecto (`DemoItil`), crea un archivo llamado `Dockerfile`:

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
ENTRYPOINT ["dotnet", "DemoItil.dll"]
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

## 4️⃣ Configurar GitHub Actions con Self-Hosted Runner

### 🔐 A. Crear token en Docker Hub

1. Ve a https://hub.docker.com/settings/security
2. Crea un Access Token

### 🔐 B. Agregar secretos en GitHub

Repositorio → Settings → Secrets → Actions → New repository secret:

- `DOCKERHUB_USERNAME`
- `DOCKERHUB_TOKEN`

### 🤖 C. Configurar Self-Hosted Runner

1. En tu repositorio de GitHub, ve a Settings → Actions → Runners
2. Haz clic en "New self-hosted runner"
3. Selecciona tu sistema operativo (Linux/Windows/macOS)
4. Sigue las instrucciones para:
   - Descargar el runner
   - Configurar el servicio
   - Verificar la conexión

Ejemplo para Linux:

```bash
# Crear directorio para el runner
mkdir actions-runner && cd actions-runner

# Descargar el runner
curl -o actions-runner-linux-x64-2.310.2.tar.gz -L https://github.com/actions/runner/releases/download/v2.310.2/actions-runner-linux-x64-2.310.2.tar.gz

# Extraer el runner
tar xzf ./actions-runner-linux-x64-2.310.2.tar.gz

# Configurar el runner
./config.sh --url https://github.com/TU_USUARIO/TU_REPO --token TU_TOKEN

# Instalar y ejecutar como servicio
sudo ./svc.sh install
sudo ./svc.sh start
```

### 🛠️ D. Crear workflow `.github/workflows/docker-publish.yml`

```yaml
name: CI/CD Pipeline

on:
  push:
    branches: ["main", "develop"]
  pull_request:
    branches: ["main", "develop"]

jobs:
  build:
    name: Build and Test
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: "8.0.x"
      - name: Build
        run: dotnet build --configuration Release

  deploy-to-staging:
    needs: build
    runs-on: self-hosted
    if: github.ref == 'refs/heads/develop'
    environment: staging
    steps:
      - uses: actions/checkout@v3

      - name: Login to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKERHUB_USERNAME }}
          password: ${{ secrets.DOCKERHUB_TOKEN }}

      - name: Build and Push
        run: |
          podman build -t ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:staging .
          podman push ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:staging

      - name: Deploy to Staging
        run: |
          podman stop demoitil-staging || true
          podman rm demoitil-staging || true
          podman pull ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:staging
          podman run -d --name demoitil-staging -p 8081:80 ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:staging

  deploy-to-production:
    needs: build
    runs-on: self-hosted
    if: github.ref == 'refs/heads/main'
    environment: production
    steps:
      - uses: actions/checkout@v3

      - name: Login to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKERHUB_USERNAME }}
          password: ${{ secrets.DOCKERHUB_TOKEN }}

      - name: Build and Push
        run: |
          podman build -t ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:latest .
          podman push ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:latest

      - name: Deploy to Production
        run: |
          podman stop demoitil-prod || true
          podman rm demoitil-prod || true
          podman pull ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:latest
          podman run -d --name demoitil-prod -p 8080:80 ${{ secrets.DOCKERHUB_USERNAME }}/demoitil:latest
```

Este workflow:

- Usa runners self-hosted para los despliegues
- Separa ambientes de staging y producción
- Implementa despliegue continuo con Podman
- Maneja diferentes tags de Docker para cada ambiente

### 📈 E. Monitoreo del Runner

1. Verifica el estado del runner en GitHub:

   - Settings → Actions → Runners
   - Deberías ver tu runner como "Active"

2. Logs del runner (en el servidor):

   ```bash
   cd actions-runner
   tail -f _diag/Runner_*.log
   ```

3. Estado del servicio:
   ```bash
   sudo ./svc.sh status
   ```

---

## 5️⃣ Desplegar manualmente desde el servidor con Podman Compose

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

| Práctica ITIL               | Aplicación en el tutorial                               |
| --------------------------- | ------------------------------------------------------- |
| Gestión de Despliegue       | Automatización con GitHub Actions y runners self-hosted |
| Gestión de Liberación       | Diferentes tags para staging/production                 |
| Gestión de la Configuración | Dockerfile y workflows definen la configuración         |
| Gestión de Disponibilidad   | Monitoreo de runners y servicios desplegados            |
| Gestión de Seguridad        | Secretos en GitHub Actions y tokens de Docker Hub       |

---

## ✅ Beneficios del Self-Hosted Runner

1. **Control Total**

   - Personalización completa del ambiente de ejecución
   - Acceso directo a recursos internos
   - Mayor seguridad al mantener el código dentro de tu infraestructura

2. **Rendimiento**

   - Sin límites de minutos de ejecución de GitHub
   - Mejor velocidad en operaciones de red internas
   - Reutilización de capas Docker/Podman

3. **Seguridad**

   - Los secretos permanecen en tu infraestructura
   - Control total sobre las políticas de seguridad
   - Auditoría completa de las ejecuciones

4. **Costos**
   - Reducción de costos en proyectos grandes
   - Mejor aprovechamiento de recursos existentes
   - Sin cargos por minutos de ejecución

---

🎓 Este tutorial demuestra una implementación práctica de CI/CD siguiendo las mejores prácticas de ITIL 4, utilizando runners self-hosted para mayor control y seguridad.
