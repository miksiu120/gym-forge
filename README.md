# GymForge

[![CI](https://github.com/miksiu120/gym-forge/actions/workflows/ci.yml/badge.svg)](https://github.com/miksiu120/gym-forge/actions/workflows/ci.yml)

GymForge is a full-stack workout planning application. Users can schedule training
plans, record results for individual sets and review exercise-specific progress.
The project demonstrates a production-oriented Angular and ASP.NET Core stack
without hiding the application behind framework-heavy abstractions.

## What the application does

- account registration and JWT-based authentication,
- multi-step training plan creation,
- repetitive and timed exercises with tempo, set and target configuration,
- dashboard for today's and overdue sessions,
- workout completion with weight, repetitions, duration and RPE,
- exercise progress charts and recent training statistics,
- editable athlete profile with metric and imperial preferences,
- working Wilks and one-rep-max calculators,
- responsive desktop, tablet and mobile layouts.

## Technology

| Area | Stack |
| --- | --- |
| Frontend | Angular 19, TypeScript, RxJS, SCSS, Chart.js |
| Backend | ASP.NET Core 9, EF Core, FluentValidation |
| Data | PostgreSQL 17 |
| Authentication | JWT bearer tokens, ASP.NET Core password hashing |
| Delivery | Docker Compose, Nginx, health checks, GitHub Actions |
| Tests | Jasmine/Karma, xUnit |

## Architecture

The backend uses a lightweight layered structure inside one project:

```text
HTTP request
  -> controller
  -> FluentValidation filter
  -> application use case
  -> domain-specific repository
  -> EF Core / PostgreSQL
  -> explicit response mapper
```

- `backend/Application` contains focused account and training use cases.
- `backend/Infrastructure` encapsulates EF Core queries and current-user access.
- `backend/Api` provides validation and RFC-compliant `ProblemDetails`.
- `frontend/src/app/pages` contains standalone Angular page components.
- `frontend/src/app/services` owns API and authentication integration.

Unexpected server errors are logged with a trace identifier without exposing stack
traces to clients. Read-only EF Core queries use `AsNoTracking`, larger graphs use
split queries, and asynchronous operations propagate `CancellationToken`.

## Run with Docker

Requirements: Docker Desktop with Docker Compose.

```bash
cp .env.example .env
docker compose up --build
```

Open [http://localhost:4200](http://localhost:4200).

The first start seeds an idempotent demo account:

```text
nickname: demo
password: GymForge123!
```

The demo includes scheduled and completed sessions, set results and chart data.
Set `SEED_DATA=false` to disable it.

To stop the stack:

```bash
docker compose down
```

Add `-v` only when you intentionally want to delete the PostgreSQL volume.

## Local development

Start PostgreSQL on port `9090`, then run the API and frontend in separate terminals:

```bash
dotnet run --project backend/WorkPlanner.csproj
```

```bash
cd frontend
npm ci
npm start
```

The Angular development server runs at `http://localhost:4200` and uses the API at
`http://localhost:5163/api`.

## Tests and quality checks

```bash
dotnet build backend/WorkPlanner.sln --configuration Release
dotnet test backend/WorkPlanner.sln --configuration Release
```

```bash
cd frontend
npm ci
npm test -- --watch=false --browsers=ChromeHeadless
npm run build -- --configuration production
```

The same checks run in GitHub Actions for every push and pull request.

## Production deployment

Production configuration intentionally requires explicit database credentials,
public origin and JWT secrets:

```bash
cp .env.production.example .env.production
docker compose --env-file .env.production -f docker-compose.production.yml up -d --build
```

See [RELEASE.md](./RELEASE.md) for TLS, health-check and backup guidance.

## Deliberate trade-offs and next steps

- JWTs currently use browser storage; a public multi-user deployment should prefer
  secure HttpOnly cookies and a refresh-token revocation strategy.
- Account uniqueness is checked in the application layer; database-level unique
  indexes and a migration are planned as additional defence against race conditions.
- Unit tests cover validation, statistics, business rules and error mapping.
  PostgreSQL-backed integration tests are the next testing layer.
