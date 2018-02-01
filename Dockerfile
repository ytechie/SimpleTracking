FROM microsoft/aspnetcore
WORKDIR /app
COPY Simpletracking/bin/Debug/netcoreapp2.0/publish/ .
ENTRYPOINT ["dotnet", "Simpletracking.dll"]
EXPOSE 5010