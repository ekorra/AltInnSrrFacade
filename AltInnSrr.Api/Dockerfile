FROM microsoft/aspnetcore:1.1
LABEL name "AltInnSrrApi"
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "AltInnSrr.Api.dll"]
