# url-shorten
A .NET 6.0 API and React TypeScript client for URL shortening.

## Requirements
* .NET 6.0
* Node v14.0.0
* Docker or Postgres database

## Database Setup
1. Run the following docker command (from project root) to create and start a containerised Postgres database and automatically apply migrations:
```
docker-compose up
```

OR

1. Create/start your own Postgres database.
2. Run the script stored at `SQL/initialCreate.sql` on your PostgresSQL server to setup required entities
3. Replace the Postgres connection string in `api/url-shorten/UrlShorten/appSettings.json` with the connection string for your PostgresSQL server.

## Starting API and client
1. Start the API using the following command (from project root):
```
dotnet run --project api/url-shorten/UrlShorten
```
2. Install packages for client with the following command (from within `client/url-shorten`):
```
npm install
```
3. Start the client with the following command (from within `client/url-shorten`)
```
npm start
```
