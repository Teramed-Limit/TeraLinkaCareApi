# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Core Development Commands

### Building and Running
- **Build**: `dotnet build --configuration Release`
- **Publish**: `.\build-and-publish.ps1` (PowerShell script for complete build and publish)
- **Run locally**: `dotnet run` (uses launchSettings.json configuration)

### Database Operations
- **Install EF Tools**: `dotnet tool install --global dotnet-ef`
- **Scaffold Database**: Use the predefined commands in README.md for CRS database scaffolding
- **Update Database**: `dotnet ef database update`

### Testing and Debugging
- **Debug Mode**: Set `"SQLDebug": true` in appsettings.json to enable SQL query logging
- **Logs Location**: `./Logs/` directory (auto-generated with date-based naming)

## Project Architecture

### High-Level Structure
This is a **Clean Architecture** ASP.NET Core 8.0 Web API for a medical care system (TeraLinkaCare) that handles DICOM medical imaging data, patient cases, and clinical workflows.

**Key Architecture Patterns:**
- **Clean Architecture** with Domain, Application, Infrastructure, and API layers
- **CQRS (Command Query Responsibility Segregation)** using MediatR
- **Repository Pattern** with Unit of Work
- **Database-First** approach with Entity Framework Core scaffolding

### Core Layers

#### 1. API Layer (`src/API/`)
- **Controllers**: RESTful API endpoints organized by domain (Cases, Patients, DICOM, Auth)
- **Base Controllers**: `BaseController` provides common functionality (user context, response formatting)
- **CRUD Controller**: Generic `CRUDController<T>` for standard operations

#### 2. Application Layer (`src/Application/`)
- **Use Cases**: CQRS commands and queries organized by domain
  - Commands: Write operations (Create, Update, Delete)  
  - Queries: Read operations with complex business logic
- **DTOs**: Data transfer objects for API communication
- **Services**: Domain services for complex business logic
- **Extensions**: Dependency injection configuration

#### 3. Core Layer (`src/Core/`)
- **Domain Entities**: Database-first entities (auto-generated from CRS database)
- **Repository Interfaces**: Abstraction layer for data access
- **Value Objects**: Domain value types

#### 4. Infrastructure Layer (`src/Infrastructure/`)
- **Persistence**: Entity Framework DbContext and Unit of Work implementation
- **Authentication**: JWT Bearer and API Key authentication handlers
- **Mappings**: AutoMapper profiles for entity-DTO mapping

### Key Technologies

#### Medical Imaging
- **fo-dicom**: DICOM file processing and medical imaging
- **DICOM Entities**: Comprehensive support for patients, studies, series, images
- **Image Markers**: Image annotation and marker system

#### Data & Persistence  
- **Entity Framework Core 8.0**: Database access with SQL Server
- **Database**: CRS (Clinical Records System) - complex medical database schema
- **AutoMapper**: Object-to-object mapping
- **Repository Pattern**: Generic repositories with Unit of Work

#### Authentication & Authorization
- **JWT Bearer**: Primary authentication mechanism
- **API Key Authentication**: Secondary auth for system integrations
- **Role-based Authorization**: Function-role mapping system

#### Logging & Monitoring
- **Serilog**: Structured logging with multiple sinks
- **Log Separation**: Auth logs, request logs, and general application logs
- **File-based Logging**: Daily rolling logs in `./Logs/` directory

## Domain Models

### Primary Entities
- **PtCase**: Patient case management (wound care cases)
- **PtCaseRecord**: Individual case records and observations
- **PtPatient**: Patient demographics and basic information
- **DicomPatient/Study/Series/Image**: Complete DICOM medical imaging hierarchy
- **CfgBodyLocation/CfgCaseType**: Configuration for case locations and types

### Key Business Logic
- **Case Management**: Multi-location wound care cases with status tracking
- **DICOM Integration**: Full medical imaging workflow support
- **Clinical Units**: Hospital unit and shift management
- **Role-based Access**: Comprehensive permission system

## Configuration

### Connection Strings
- **DefaultConnection**: Primary CRS database (SQL Server)
- Database scaffolding supports multiple databases (CRSCoreDB, CRSPatientDataDB1)

### Key Settings
- **ImageVirtualPath**: Virtual path for medical images
- **ImageMarkerVirtualPath**: Path for image annotations
- **DicomPdfDirPath**: DICOM PDF storage location
- **IsDevelopment**: Environment flag for development features
- **AllowedOrigins**: CORS configuration for frontend applications

## Development Patterns

### Request/Response Flow
1. Controller receives HTTP request
2. Maps to MediatR Command/Query via handler
3. Handler executes business logic using repositories
4. AutoMapper converts entities to DTOs
5. Returns structured Result<T> response

### Error Handling
- **Result Pattern**: All use cases return `Result<T>` for consistent error handling
- **Base Controller**: Provides standardized error response methods
- **Logging**: Comprehensive error logging with Serilog

### Database Patterns
- **Generic Repository**: `IRepository<TEntity, TKey>` with common CRUD operations
- **Unit of Work**: Transaction management across multiple repositories
- **Database-First**: Entity scaffolding from existing CRS database schema

## Important Notes

### Security
- JWT tokens issued by "teramed" issuer/audience
- API keys supported for system integrations
- CORS configured for specific frontend origins
- Connection strings should be moved to user secrets in production

### Medical Data Compliance
- DICOM-compliant medical imaging handling
- Patient data privacy considerations built into entity design
- Audit trails maintained for medical records

### Development Environment
- SQL Server required (DESKTOP-DSL3QH8\MSSQLSERVER01 in development)
- fo-dicom library configured for DICOM processing
- Serilog configured for development debugging