FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet build
CMD ["dotnet", "run", "--project", "AuTan"]
