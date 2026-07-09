<!--
Sync Impact Report
Version change: 1.0.0 -> 1.1.0
Modified principles:
- I. Spec-Driven Flow First -> I. Spec-Driven Development
- II. MVP Scope Discipline -> II. Simplicity in the MVP
- III. Simple, Organized Architecture -> III. Backend Architecture
- IV. Testable User-Value Slices -> VI. Quality and Maintainability
- V. Local Reproducibility -> VII. Local Execution
Added principles:
- IV. Frontend Architecture
- V. Data Persistence
Added sections:
- Project Scope and Stack
- Required Development Flow
Removed sections:
- Technology and Scope Constraints
- Development Workflow and Quality Gates
Templates requiring updates:
- updated .specify/templates/plan-template.md
- updated .specify/templates/spec-template.md
- updated .specify/templates/tasks-template.md
- reviewed .specify/templates/checklist-template.md
Follow-up TODOs:
- None
-->
# TaskFlow SDD Constitution

## Core Principles

### I. Spec-Driven Development
Every feature MUST follow the order constitution -> spec -> plan -> tasks ->
implementation. No implementation work MAY begin until the feature has an
approved `spec.md`, `plan.md`, and `tasks.md`. Every feature artifact MUST live
under `/specs/[###-feature-name]/`. Any implementation discovery that changes
scope, behavior, architecture, or verification MUST update the relevant spec,
plan, or tasks before code changes continue.

Rationale: TaskFlow SDD is a study project that must simulate a real productive
project, so the SDD artifacts are the source of truth for delivery decisions.

### II. Simplicity in the MVP
The initial MVP MUST focus on task management only. The design MUST avoid
overengineering, speculative abstractions, and infrastructure that does not serve
the current feature. RabbitMQ, MassTransit, Hangfire, and Redis MUST remain out
of the MVP and MAY be introduced only by future features with their own spec,
plan, tasks, and explicit justification.

Rationale: the first milestone must prove the product workflow and learning
goals before adding distributed systems concerns.

### III. Backend Architecture
The backend MUST be built with .NET and organized into API, Application, Domain,
and Infrastructure responsibilities. Controllers MUST stay thin: they MAY handle
HTTP concerns, request binding, response mapping, and orchestration, but business
rules MUST live outside controllers. Domain behavior and application use cases
MUST be testable without depending on HTTP controllers.

Rationale: this keeps the project simple while teaching maintainable backend
boundaries used in real systems.

### IV. Frontend Architecture
The frontend MUST use React + Vite. Components MUST be simple, readable, and
reusable where reuse removes duplication or clarifies intent. UI code MUST keep
business-facing flows understandable and MUST avoid unnecessary state management,
frameworks, or abstractions until a feature plan justifies them.

Rationale: a small React application is easier to evolve when components express
clear responsibilities instead of hiding behavior behind premature abstraction.

### V. Data Persistence
PostgreSQL MUST be the application database for persisted data. The backend MUST
use EF Core for data access and migrations. Schema changes MUST be represented
by migrations and tied to the feature tasks that require them. Direct database
changes outside migrations are not acceptable for project state that belongs to
the application.

Rationale: migrations make database evolution reviewable, repeatable, and
aligned with the SDD artifacts.

### VI. Quality and Maintainability
Code MUST be readable, cohesive, and named according to its responsibility.
Primary business rules MUST have tests or a documented verification rationale in
the feature tasks. README documentation MUST be updated when setup, execution,
or user-facing behavior changes. Commits MUST be small enough to review and use
descriptive messages that state the intent of the change.

Rationale: a study project only teaches good engineering if it uses the same
quality habits expected in productive work.

### VII. Local Execution
The project MUST run locally with documented commands. Docker Compose MUST be
used for PostgreSQL in local development. Required environment variables,
database setup, migrations, and run/test commands MUST be documented before a
feature is considered complete. Secrets MUST NOT be committed; examples or
templates MAY document required configuration names.

Rationale: reproducible local execution prevents hidden machine-specific
assumptions and keeps the project easy to evaluate.

## Project Scope and Stack

- Project purpose: study SDD, .NET, React, Docker, PostgreSQL, and development
  practices while simulating a real productive project.
- Initial MVP: task management only, including task creation, listing, editing,
  completion/reopening, and deletion as defined by the first feature spec.
- Backend: .NET.
- Backend structure: API, Application, Domain, Infrastructure.
- Frontend: React + Vite.
- Database: PostgreSQL.
- Data access: EF Core with migrations.
- Local infrastructure: Docker Compose for PostgreSQL.
- Deferred technologies for future features: RabbitMQ, MassTransit, Hangfire,
  Redis, distributed messaging, background jobs, and caching infrastructure.

## Required Development Flow

1. Constitution review MUST happen before creating or changing feature specs.
2. Feature specs MUST be created under `/specs/[###-feature-name]/` and define
   user stories, acceptance scenarios, functional requirements, key entities
   when data exists, success criteria, assumptions, and out-of-scope boundaries.
3. Implementation plans MUST pass the Constitution Check before Phase 0 research
   and again after Phase 1 design.
4. Task lists MUST be dependency-ordered, grouped by user story, and explicit
   about which tasks form the MVP.
5. Implementation MUST follow the generated tasks. If task order, scope,
   architecture, or verification changes, update `tasks.md` before changing
   code.
6. Verification evidence MUST be captured in the relevant plan, tasks, README,
   or implementation notes before a feature is considered complete.

## Governance

This constitution supersedes conflicting project conventions, templates, and
feature-level decisions. If a spec, plan, or task conflicts with this document,
the artifact MUST be revised before implementation continues.

Amendments require a documented reason, an updated Sync Impact Report, and review
of dependent templates under `.specify/templates/`. Versioning follows semantic
versioning:

- MAJOR: removes or redefines a core principle in a way that changes governance.
- MINOR: adds a principle, section, quality gate, or materially expands scope.
- PATCH: clarifies wording without changing required behavior.

Compliance review is required at each feature transition: before spec creation,
before plan approval, before task generation, and before implementation begins.
The Constitution Check in `plan.md` is the formal gate for recording compliance
and any justified deviations.

**Version**: 1.1.0 | **Ratified**: 2026-07-08 | **Last Amended**: 2026-07-08
