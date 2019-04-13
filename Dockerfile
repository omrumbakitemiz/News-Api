FROM microsoft/dotnet:2.2-aspnetcore-runtime

WORKDIR /app

COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet News.Api.dll