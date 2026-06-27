# GymForge 1.0 deployment

The production release runs as three Docker services: Nginx/Angular, ASP.NET Core,
and PostgreSQL. Use a Linux host with Docker Engine and Docker Compose.

## Configure

Copy `.env.production.example` to `.env.production` and replace every example
domain, password, and JWT key. Keep `SEED_DATA=false` for a public deployment.

The public origin must include the final protocol and hostname, for example:

```text
PUBLIC_ORIGIN=https://gymforge.example.com
JWT_ISSUER=https://gymforge.example.com
```

## Start

```bash
docker compose --env-file .env.production -f docker-compose.production.yml up -d --build
```

Check that every service is healthy:

```bash
docker compose --env-file .env.production -f docker-compose.production.yml ps
```

## TLS and domain

Expose the configured `APP_PORT` through a TLS reverse proxy such as Caddy,
Traefik, or the hosting provider's HTTPS proxy. Point the domain at the server
and forward requests to that port.

## Update

Pull the new release, back up the PostgreSQL volume, and run the start command
again. Database migrations are applied automatically by the backend.

## Back up the database

```bash
docker compose --env-file .env.production -f docker-compose.production.yml exec -T database \
  pg_dump -U "$POSTGRES_USER" "$POSTGRES_DB" > gymforge-backup.sql
```
