# =====================================================================
#  DOCKERFILE - PremierProformas (ASP.NET Core MVC + Playwright)
#  Un Dockerfile es una "receta": Docker la lee de arriba hacia abajo
#  y con ella construye una IMAGEN (una plantilla de la app ya lista).
#  Después, con esa imagen se crean los CONTENEDORES que corren la app.
#
#  Este archivo usa "multi-stage build" (varias etapas):
#    Etapa 1 -> compila el código
#    Etapa 2 -> toma solo lo compilado y lo deja listo para ejecutar
#  Así, el código fuente y los archivos temporales de la compilación
#  no quedan dentro de la imagen final.
# =====================================================================


# ---------------------------------------------------------------------
#  ETAPA 1: COMPILAR
# ---------------------------------------------------------------------

# FROM = imagen base de la que partimos. Es como elegir el sistema
# operativo ya preparado. "sdk" trae todo para COMPILAR .NET.
# "10.0" debe coincidir con <TargetFramework>net10.0</TargetFramework>
# de tu .csproj.
# "AS build" le pone nombre a esta etapa para usarla más abajo.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# WORKDIR = carpeta de trabajo DENTRO del contenedor (si no existe, la crea).
# Todos los comandos siguientes se ejecutan en /src.
WORKDIR /src

# COPY <origen en tu proyecto> <destino en el contenedor>
# Primero copiamos SOLO el .csproj. ¿Por qué? Por el "caché de capas":
# Docker guarda el resultado de cada paso. Si el .csproj no cambió,
# reutiliza el "restore" anterior y la construcción es mucho más rápida.
COPY *.csproj .

# RUN = ejecuta un comando mientras se construye la imagen.
# "dotnet restore" descarga los paquetes NuGet (Npgsql, Playwright, EF...).
RUN dotnet restore

# Ahora sí copiamos todo el resto del código (Controllers, Views, wwwroot...).
# El .dockerignore evita que se copien bin/, obj/, .git, .env, etc.
COPY . .

# Compila en modo Release (optimizado) y deja el resultado en /app/publish.
# Ahí quedan PremierProformas.dll, wwwroot y el script playwright.ps1.
RUN dotnet publish -c Release -o /app/publish


# ---------------------------------------------------------------------
#  ETAPA 2: EJECUTAR
# ---------------------------------------------------------------------

# Empezamos de nuevo desde una imagen limpia. Todo lo de la etapa 1
# se descarta, excepto lo que copiemos explícitamente con --from=build.
# Usamos "sdk" (y no "aspnet", que es más liviana) porque trae PowerShell,
# y Playwright necesita PowerShell para instalar Chromium.
FROM mcr.microsoft.com/dotnet/sdk:10.0

# Carpeta donde vivirá la app ya compilada.
WORKDIR /app

# ENV = variables de entorno disponibles dentro del contenedor.
#  TZ                       -> zona horaria, para que DateTime.Now dé la hora de Honduras.
#  ASPNETCORE_URLS          -> la app escucha en el puerto 8080 en todas las interfaces (+).
#                              Es el puerto INTERNO; el del servidor (8001) se define en el compose.
#  ASPNETCORE_ENVIRONMENT   -> "Production": oculta las páginas de error detalladas y usa
#                              appsettings.Production.json si existe.
#  PLAYWRIGHT_BROWSERS_PATH -> carpeta fija donde Playwright instala Chromium,
#                              para que la app lo encuentre siempre en el mismo lugar.
ENV TZ=America/Tegucigalpa \
    ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

# Copiamos SOLO el resultado compilado de la etapa "build" a /app.
COPY --from=build /app/publish .

# Instala Chromium (el navegador sin ventana que convierte el HTML en PDF).
#  pwsh            -> ejecuta el script con PowerShell
#  --with-deps     -> instala también las librerías de Linux que Chromium necesita
#                     (incluye fuentes como Liberation Serif, parecida a Times New Roman)
# Este paso tarda unos minutos solo la primera vez; después queda en caché.
RUN pwsh ./playwright.ps1 install --with-deps chromium

# EXPOSE documenta en qué puerto escucha la app dentro del contenedor.
# No abre nada en el servidor; eso lo hace "ports:" en docker-compose.yml.
EXPOSE 8080

# ENTRYPOINT = el comando que se ejecuta al iniciar el contenedor.
# Equivale a escribir "dotnet PremierProformas.dll" en una terminal.
# El nombre del .dll debe ser igual al de tu .csproj.
ENTRYPOINT ["dotnet", "PremierProformas.dll"]