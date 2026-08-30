# ClimbSort API

Catalogue, store and sort through rock climbs.

## Overview

Current climb database applications are all either region-specific, behind a paywall and or don't store or let you search on relevant information. If you want to find what climbs to do in an area, you have to do hours of research.

With this api, I introduce a system for storing crag, climb, topography, location data and more while also being capable of filtering using a multi-component filter, for a precise and customisable list.

## Features

- CRUD operations for climbs, crags.
- GET operations for grades, grading systems and other static values.
- Detailed, multi-component filter GET operation for climbs.
- Migration tool for populating database with climbs from the Fontainebleau region.

## Technology

- .NET 10
- ASP.NET Core
- Entity Framework Core
- SQL Server
- SQLite
- Azure App Configuration
- Azure Key Vault
- Docker

## Getting Started

### Prerequisites

.NET 10 SDK
Docker (optional)

### Clone

git clone https://github.com/MellyD/ClimbSort.git

### Local Configuration

(CURRENTLY NOT IMPLEMENTED)
For local use, an SQLite database will be used, to ensure that anyone can run this application on their machine.

### Database

(CURRENTLY NOT IMPLEMENTED)
Database creation and migration console application will need to be ran with this command while being in the BoolderDataMigration directory:
`dotnet run`

### Run

Make sure you are in the FontRecommender directory:
`dotnet run`

### Swagger

https://localhost:7080/swagger/index.html

## Configuration

Explain Development vs Production configuration.

### Development

SQLite database, local logging.

### Production

(CURRENTLY NOT IMPLEMENTED)
The live system is currently deployed to a container app, available to target following the information below.

The basic framework for the live version:
Azure App Configuration + Azure SQL + MSSQL Server

## API

There are 4 endpoints for this api:
- Climb (Full CRUD)
- Crag (FULL CRUD)
- Grade (GET only)
- Statics (GET only)

(Link to swagger documentation will be provided upon launch of the api)

## Architecture

<img width="2360" height="2924" alt="ClimbSort-SQL-Diagram" src="https://github.com/user-attachments/assets/950ae9bc-5839-48b9-bef4-63766ee2676c" />
This is the structure of the system, modularity ensured for future development. Coordinates are treated as both location coordinates and as x y information for topographies of climbs.

## Data

Climb data is created on the application, however data has been collected using the public [Boolder database](https://github.com/boolder-org/boolder-data), which contains information on the Fontainebleau area climbs. 
This, in combination with the [bleau.info](https://bleau.info/) website, was used to create a base data layer.

## Contributing

Feel free to fork and clone the api and use as you see fit. Please document any development and email me at: mellingdave@hotmail.com.
