FROM mcr.microsoft.com/dotnet/sdk:7.0.410-bullseye-slim AS restore
WORKDIR /src
COPY ./*.sln ./
COPY */*.csproj ./
# Take into account using the same name for the folder and the .csproj and only one folder level
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore

FROM restore AS build
COPY . .
RUN dotnet format --verify-no-changes
RUN dotnet build -c Release

FROM build AS test
RUN dotnet test

FROM build AS publish
RUN dotnet publish "./Doppler.CDHelper/Doppler.CDHelper.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0.20-bullseye-slim AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ARG version=unknown
RUN echo $version > /app/wwwroot/version.txt
ENTRYPOINT ["dotnet", "Doppler.CDHelper.dll"]
