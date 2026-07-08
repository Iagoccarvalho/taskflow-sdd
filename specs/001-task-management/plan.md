# Implementation Plan: Task Management

**Branch**: `001-task-management` | **Date**: 2026-07-08 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/001-task-management/spec.md`

## Summary

Implementar o MVP de gerenciamento de tarefas com criação, listagem,
atualização e conclusão de tarefas. Toda tarefa inicia como Pending, pode ser
concluída como Completed, e cada conclusão bem-sucedida deve registrar um log de
processamento persistente. A solução terá backend .NET, API REST, PostgreSQL com
EF Core, frontend React + Vite e infraestrutura local com Docker Compose.

## Technical Context

**Language/Version**: .NET 10 para backend; TypeScript com React + Vite para frontend

**Primary Dependencies**: ASP.NET Core Web API, EF Core, Npgsql EF Core provider,
React, Vite

**Storage**: PostgreSQL para tarefas e logs de processamento

**Testing**: xUnit para testes de Domain/Application; testes de integração da API
com banco PostgreSQL em container; Vitest/React Testing Library para fluxos
principais do frontend

**Target Platform**: Desenvolvimento local via Docker Compose para PostgreSQL,
backend HTTP local e frontend em browser

**Project Type**: Web application com backend API + frontend SPA

**Performance Goals**: Operações principais percebidas pelo usuário como
imediatas em cenário local; fluxo criar -> listar -> atualizar -> concluir em
até 2 minutos conforme spec

**Constraints**: Sem autenticação; sem RabbitMQ, MassTransit, Hangfire, Redis,
background workers, filas ou cache distribuído; controllers finos; regras de
negócio em Application/Domain; Infrastructure contém EF Core, DbContext,
migrations e repositórios

**Scale/Scope**: MVP local para estudo com um único usuário, sem colaboração,
notificações, deleção, reabertura ou filtros avançados

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-driven sequence**: PASS. A spec existe em
  `/specs/001-task-management/spec.md`; este plano antecede tasks e
  implementação.
- **MVP scope discipline**: PASS. O plano cobre apenas gerenciamento básico de
  tarefas e logs de conclusão. RabbitMQ, MassTransit, Hangfire, Redis, workers,
  filas, cache, autenticação, colaboração, notificações e CI/CD ficam fora.
- **Backend architecture**: PASS. O backend será dividido em `TaskFlow.Api`,
  `TaskFlow.Application`, `TaskFlow.Domain` e `TaskFlow.Infrastructure`.
  Controllers apenas recebem requisições, chamam Application e retornam
  respostas.
- **Frontend architecture**: PASS. O frontend usará React + Vite com componentes
  simples para lista, formulário e ações de tarefa.
- **Data persistence**: PASS. PostgreSQL com EF Core e migrations. Logs de
  processamento serão persistidos no banco.
- **Testable user-value slices**: PASS. As histórias de criar, listar, atualizar
  e concluir são independentes e verificáveis.
- **Quality and documentation**: PASS. O plano prevê testes para regras
  principais, contratos REST, quickstart e atualização de README durante a
  implementação.
- **Local reproducibility**: PASS. PostgreSQL será executado via Docker Compose;
  comandos de setup, migration, run e test serão documentados.

## Technical Decisions

See [research.md](./research.md) for decision records.

Key decisions:

- Use .NET 10 because it is the current LTS line in the Microsoft support policy.
- Use ASP.NET Core controllers for a simple REST API.
- Keep use cases in Application and entity invariants/state transitions in
  Domain.
- Use EF Core migrations against PostgreSQL for schema evolution.
- Store task completion processing logs synchronously in the same persistence
  flow as task completion for this MVP.
- Use simple React component state and service modules, without global state
  libraries.

## Project Structure

### Documentation (this feature)

```text
specs/001-task-management/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── openapi.yaml
├── checklists/
│   └── requirements.md
└── spec.md
```

### Source Code (repository root)

```text
backend/
├── TaskFlow.sln
├── src/
│   ├── TaskFlow.Api/
│   │   ├── Controllers/
│   │   ├── Contracts/
│   │   ├── Program.cs
│   │   └── appsettings*.json
│   ├── TaskFlow.Application/
│   │   ├── Abstractions/
│   │   ├── Tasks/
│   │   └── ProcessingLogs/
│   ├── TaskFlow.Domain/
│   │   ├── Tasks/
│   │   └── ProcessingLogs/
│   └── TaskFlow.Infrastructure/
│       ├── Persistence/
│       │   ├── TaskFlowDbContext.cs
│       │   ├── Configurations/
│       │   └── Migrations/
│       └── Repositories/
└── tests/
    └── TaskFlow.Tests/
        ├── Domain/
        ├── Application/
        └── Api/

frontend/
├── package.json
├── vite.config.ts
├── index.html
└── src/
    ├── components/
    ├── pages/
    ├── services/
    ├── types/
    └── tests/

docker-compose.yml
README.md
```

**Structure Decision**: Use a solution-per-backend layout with four production
projects plus one test project. Keep frontend as a separate Vite app under
`frontend/`. Keep Docker Compose at repository root for local PostgreSQL.

## Backend Design

- `TaskFlow.Api`: HTTP controllers, request/response DTOs, dependency injection,
  error mapping, OpenAPI exposure during development.
- `TaskFlow.Application`: use cases for creating, listing, updating and
  completing tasks; repository interfaces; transaction boundary coordination.
- `TaskFlow.Domain`: `Task`, `TaskStatus`, `TaskProcessingLog` and domain
  behavior such as title validation and Pending -> Completed transition.
- `TaskFlow.Infrastructure`: EF Core DbContext, entity configurations,
  migrations and repository implementations.
- `TaskFlow.Tests`: Domain/Application unit tests and API integration tests.

Controllers must not contain business rules. They validate transport shape at
the boundary, delegate to Application use cases, and translate results to HTTP
responses.

## Data Model

See [data-model.md](./data-model.md) for fields, relationships, validations and
state transitions.

Database tables planned:

- `tasks`
- `task_processing_logs`

## REST Endpoints

Contract source: [contracts/openapi.yaml](./contracts/openapi.yaml)

Planned endpoints:

- `POST /api/tasks`: create task.
- `GET /api/tasks`: list tasks.
- `GET /api/tasks/{taskId}`: retrieve one task for update/detail flows.
- `PUT /api/tasks/{taskId}`: update title and description.
- `POST /api/tasks/{taskId}/complete`: complete a Pending task and create one
  processing log.
- `GET /api/tasks/{taskId}/processing-logs`: list logs for one task.
- `GET /api/processing-logs`: list processing logs across tasks for local
  inspection.

## Testing Strategy

- **Domain tests**: title required; description optional; new task starts
  Pending; Pending task can transition to Completed; Completed task cannot
  produce a duplicate completion transition.
- **Application tests**: create task persists Pending task; update rejects empty
  title; complete task persists status change and one processing log; completing
  nonexistent task returns not found; completing already Completed task is
  idempotent from the user's perspective and does not create duplicate log.
- **API integration tests**: REST status codes and response shapes for create,
  list, update, complete, not found and validation errors; persistence verified
  against PostgreSQL-compatible test database/container.
- **Frontend tests**: task form validation, list rendering, edit flow and
  complete action rendering updated status.
- **Manual validation**: quickstart scenarios in [quickstart.md](./quickstart.md)
  prove the end-to-end MVP flow.

## Risks and Validations

| Risk | Impact | Validation |
|------|--------|------------|
| Business rules drift into controllers | Architecture becomes harder to test | Application/Domain tests must cover rules without HTTP |
| Duplicate processing logs on repeated completion | Audit data becomes incorrect | Test repeated completion creates no second log |
| EF Core migration diverges from model | Local setup becomes unreliable | Quickstart must include migration command and schema validation |
| Scope creep into auth, queues, workers or cache | MVP violates constitution | Constitution Check and tasks must reject those additions |
| Frontend state grows too complex | Study project becomes harder to understand | Keep local component state unless a later spec justifies more |

## Complexity Tracking

No constitution violations. No complexity exceptions requested.

## Post-Design Constitution Check

- **Spec-driven sequence**: PASS. Plan and generated artifacts remain under
  `/specs/001-task-management/`.
- **MVP scope discipline**: PASS. No deferred infrastructure is included.
- **Backend architecture**: PASS. Source layout preserves API, Application,
  Domain and Infrastructure boundaries.
- **Frontend architecture**: PASS. React + Vite remains simple and local.
- **Data persistence**: PASS. PostgreSQL and EF Core migrations are explicitly
  planned.
- **Testable user-value slices**: PASS. Tests map to user stories and rules.
- **Quality and documentation**: PASS. README and quickstart updates are planned
  for implementation.
- **Local reproducibility**: PASS. Docker Compose for PostgreSQL and local
  commands are defined in quickstart.
