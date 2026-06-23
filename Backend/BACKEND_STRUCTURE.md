# 🏗️ Struktura plików — Backend

## Drzewo katalogów

```
Backend/
├── SubsAPP.sln                         # Solution .NET
│
├── Backend.API/                        # Główny projekt Web API
│   ├── Program.cs                      # Punkt wejścia aplikacji
│   ├── appsettings.json                # Konfiguracja (Jwt, Logging)
│   ├── appsettings.Development.json    # Konfiguracja środowiska dev
│   ├── .env                            # Zmienne środowiskowe (ignorowane przez git)
│   ├── Backend.API.csproj              # Definicja projektu (.NET)
│   │
│   ├── Controllers/                    # Kontrolery HTTP (REST API)
│   │   ├── AuthController.cs           # POST /api/Auth/login
│   │   └── GroupsController.cs         # CRUD /api/Groups/...
│   │
│   ├── Services/                       # Logika biznesowa
│   │   ├── Auth/
│   │   │   ├── IAuthService.cs         # Interfejs serwisu autentykacji
│   │   │   └── AuthService.cs          # Implementacja logowania
│   │   ├── Groups/
│   │   │   ├── IGroupsServices.cs      # Interfejs serwisu grup
│   │   │   └── GroupService.cs         # Implementacja CRUD grup
│   │   └── Token/
│   │       ├── ITokenService.cs        # Interfejs generowania JWT
│   │       └── TokenService.cs         # Implementacja JWT
│   │
│   ├── Models/                         # Modele danych
│   │   ├── Entities/                   # Encje bazodanowe (EF Core)
│   │   │   ├── User.cs                 # Użytkownik (dziedziczy IdentityUser)
│   │   │   ├── ApplicationUser.cs      # Pomocniczy model użytkownika
│   │   │   ├── Group.cs                # Grupa
│   │   │   ├── UserGroup.cs            # Relacja User ↔ Group
│   │   │   ├── Schedule.cs             # Harmonogram
│   │   │   ├── Substitution.cs         # Zastępstwo
│   │   │   ├── SubstitutionGroup.cs    # Relacja Substitution ↔ Group
│   │   │   ├── SubstitutionNotification.cs # Powiadomienie o zastępstwie
│   │   │   ├── SubstitutionTaken.cs    # Podjęte zastępstwo
│   │   │   └── Overtime.cs             # Nadgodziny
│   │   │
│   │   ├── DTOs/                       # Obiekty transferu danych
│   │   │   ├── Auth/
│   │   │   │   ├── LoginDTO.cs         # Dane logowania (Email, Password)
│   │   │   │   └── AuthResponseDTO.cs  # Odpowiedź (Token, User, IsSuccess)
│   │   │   ├── Groups/
│   │   │   │   ├── CreateGroupDTO.cs   # Dane do tworzenia grupy
│   │   │   │   └── UpdateGroupDTO.cs   # Dane do aktualizacji grupy
│   │   │   └── User/
│   │   │       ├── CreateUserDTO.cs    # Dane do tworzenia użytkownika
│   │   │       ├── UpdateUserDTO.cs    # Dane do aktualizacji użytkownika
│   │   │       └── UserResponseDTO.cs  # Odpowiedź z danymi użytkownika
│   │   │
│   │   └── Configurations/             # Konfiguracje EF (Fluent API)
│   │       └── DataSeeder.cs           # Seed danych inicjalnych
│   │
│   ├── Data/
│   │   └── AppDbContext.cs             # Kontekst bazy danych (EF Core)
│   │
│   ├── Extensions/                     # Metody rozszerzające dla DI
│   │   ├── ServiceCollectionExtensions.cs  # Rejestracja serwisów, JWT, CORS, Swagger
│   │   ├── WebApplicationExtensions.cs     # Middleware pipeline, seed
│   │   └── EnvironmentExtensions.cs        # Wczytywanie zmiennych .env
│   │
│   ├── Logging/                        # (zarezerwowane na konfigurację logowania)
│   │
│   ├── Migrations/                     # Migracje Entity Framework Core
│   └── Properties/
│       └── launchSettings.json         # Profile uruchamiania (dev/https)
│
└── Backend.Tests/                      # Projekt testów
    ├── Backend.Tests.csproj
    ├── ExampleTestTemplate.txt         # Szablon testów
    └── ServicesTests/
        └── AuthServieTests.cs          # Testy serwisu autentykacji
```

## 📐 Architektura

```
Request → Controller → Service → DbContext → SQL Server
                ↓
           DTO mapping
                ↓
Response ← Controller ← Service
```

### Wzorzec DI (Dependency Injection)

Wszystkie serwisy są rejestrowane jako **Scoped** w `ServiceCollectionExtensions.cs`:
```csharp
services.AddScoped<IGroupsService, GroupService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IAuthService, AuthService>();
```

### Konfiguracja środowisk

Plik `.env` w katalogu `Backend.API/` przechowuje sekrety:
- `DB_CONNECTION_STRING` — połączenie do SQL Server
- `JWT_SECRET` — klucz podpisywania tokenów JWT

Wczytywane przez `EnvironmentExtensions.cs` z biblioteki `DotNetEnv`.
