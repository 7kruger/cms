FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CourseWork/CourseWork.csproj", "CourseWork/"]
COPY ["CourseWork.DAL/CourseWork.DAL.csproj", "CourseWork.DAL/"]
COPY ["CourseWork.Domain/CourseWork.Domain.csproj", "CourseWork.Domain/"]
COPY ["CourseWork.Service/CourseWork.Service.csproj", "CourseWork.Service/"]
RUN dotnet restore "CourseWork/CourseWork.csproj"
COPY . .
WORKDIR "/src/CourseWork"
RUN dotnet build "CourseWork.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseWork.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseWork.dll"]