# TaskFlow SDD

Projeto de estudo para aprender Spec-Driven Development usando .NET, React, Docker e boas práticas de desenvolvimento.

## Objetivo

Construir um gerenciador de tarefas simples, começando por um MVP pequeno para estudar o fluxo SDD completo antes de expandir a arquitetura.

## Fluxo SDD

O projeto segue obrigatoriamente esta ordem:

1. Constitution
2. Spec
3. Plan
4. Tasks
5. Implementation

Nenhum código de aplicação deve ser implementado antes de existir spec, plan e tasks para a feature.

## MVP inicial

O primeiro MVP terá apenas gerenciamento de tarefas.

Ficam fora da primeira feature: RabbitMQ, MassTransit, Hangfire, Redis, background workers, cache, autenticação, colaboração, notificações e CI/CD.

## Stack planejada

- Backend: .NET 10
- Frontend: React + Vite
- Banco: PostgreSQL
- Infra local: Docker Compose

## Estrutura inicial

```text
backend/
├── TaskFlow.sln
├── src/
│   ├── TaskFlow.Api/
│   ├── TaskFlow.Application/
│   ├── TaskFlow.Domain/
│   └── TaskFlow.Infrastructure/
└── tests/
    └── TaskFlow.Tests/

frontend/
└── src/
```

## Comandos iniciais

Subir PostgreSQL local:

```bash
docker compose up -d postgres
```

Restaurar e validar o backend:

```bash
dotnet restore backend/TaskFlow.sln
dotnet build backend/TaskFlow.sln
dotnet test backend/TaskFlow.sln
```

Rodar a API:

```bash
dotnet run --project backend/src/TaskFlow.Api
```

Instalar dependências do frontend:

```bash
npm install --prefix frontend
```

Rodar o frontend:

```bash
npm run dev --prefix frontend
```

Validar build do frontend:

```bash
npm run build --prefix frontend
```
