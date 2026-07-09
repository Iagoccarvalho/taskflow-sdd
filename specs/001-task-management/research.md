# Research: Task Management

## Decision: Use .NET 10 for the backend

**Rationale**: The user allowed .NET 9 or .NET 10. .NET 10 is the LTS release in
the current Microsoft support policy and has support through November 14, 2028,
while .NET 9 is STS. For a project simulating productive work, the LTS line is a
better default.

**Alternatives considered**:

- .NET 9: acceptable technically, but shorter STS lifecycle.
- .NET 10: selected because it aligns with long-term support.

## Decision: Use ASP.NET Core Web API with controllers

**Rationale**: The project asks for ASP.NET Core Web API and controller rules.
Controllers provide a familiar REST boundary for the study project while keeping
business rules outside the HTTP layer.

**Alternatives considered**:

- Minimal APIs: simpler for very small APIs, but less aligned with the explicit
  controller rule in this feature.
- Full MVC with views: unnecessary because the frontend is React + Vite.

## Decision: Split backend into API, Application, Domain and Infrastructure

**Rationale**: This matches the constitution and requested architecture. API
handles transport, Application coordinates use cases, Domain owns business
state/rules, and Infrastructure owns EF Core persistence.

**Alternatives considered**:

- Single backend project: faster initial setup but weaker learning value and
  easier to mix controllers with business rules.
- Additional layers such as CQRS packages or mediator libraries: unnecessary
  for this MVP and would add overengineering.

## Decision: Use EF Core with PostgreSQL migrations

**Rationale**: The project requires PostgreSQL and EF Core. Migrations provide a
reviewable, repeatable way to evolve `tasks` and `task_processing_logs`.

**Alternatives considered**:

- Raw SQL only: simple but loses the EF Core learning goal and migration model.
- Alternative database: violates the project stack.

## Decision: Persist processing log synchronously when completing a task

**Rationale**: The spec requires a database log when a task is completed, and the
constitution excludes RabbitMQ, MassTransit, Hangfire and Redis for this feature.
The Application completion use case will update the task and create exactly one
processing log in the same persistence flow.

**Alternatives considered**:

- Queue or background worker: explicitly out of scope.
- External log sink only: does not satisfy the requirement to register a log in
  the database.

## Decision: Provide REST endpoints for tasks and processing logs

**Rationale**: The user requested REST endpoints for tasks and logs. The API will
include task CRUD-like operations limited to create/list/get/update/complete and
read-only log endpoints for inspection.

**Alternatives considered**:

- GraphQL: unnecessary for this simple MVP.
- No log endpoint: would make validating persisted processing logs harder.

## Decision: Use simple React + Vite frontend structure

**Rationale**: The constitution requires React + Vite with simple reusable
components. The feature can be handled with local component state and small
service modules.

**Alternatives considered**:

- Global state libraries: unnecessary for the MVP.
- Server-rendered UI: not aligned with the selected frontend stack.

## Decision: Test primary business rules before broad UI coverage

**Rationale**: The highest-risk behavior is task state and processing-log
correctness. Domain/Application tests should prove these rules independently of
HTTP and UI. API integration and frontend tests then verify the user flows.

**Alternatives considered**:

- Only end-to-end tests: slower feedback and weaker rule isolation.
- Only unit tests: insufficient to prove REST and persistence behavior.
