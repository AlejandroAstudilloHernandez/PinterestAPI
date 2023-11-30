# Utiliza la imagen ASP.NET como base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copia los archivos publicados de tu aplicación
COPY ./output .

# Configura el comando de inicio
ENTRYPOINT ["dotnet", "PinterestAPI.dll"]
