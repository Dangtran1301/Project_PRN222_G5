FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Project_PRN222_G5.Web/Project_PRN222_G5.Web.csproj", "Project_PRN222_G5.Web/"]
COPY ["Project_PRN222_G5.BusinessLogic/Project_PRN222_G5.BusinessLogic.csproj", "Project_PRN222_G5.BusinessLogic/"]
COPY ["Project_PRN222_G5.DataAccess/Project_PRN222_G5.DataAccess.csproj", "Project_PRN222_G5.DataAccess/"]
RUN dotnet restore "Project_PRN222_G5.Web/Project_PRN222_G5.Web.csproj"

COPY . .
WORKDIR "/src/Project_PRN222_G5.Web"
RUN dotnet publish "Project_PRN222_G5.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Project_PRN222_G5.Web.dll"]