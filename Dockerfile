FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

COPY . .
RUN dotnet restore
CMD ["dotnet", "run", "--project", "AuTan"]
