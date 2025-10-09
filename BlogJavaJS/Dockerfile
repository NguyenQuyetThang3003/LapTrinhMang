# Dùng .NET SDK chính thức để build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy toàn bộ code và build
COPY . ./
RUN dotnet publish -c Release -o out

# Dùng image nhỏ gọn để chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "BlogJavaJS.dll"]
