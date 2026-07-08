<!--
Sync Impact Report
Version change: template -> 1.0.0
Modified principles:
- Template principle 1 -> I. Spec-Driven Flow First
- Template principle 2 -> II. MVP Scope Discipline
- Template principle 3 -> III. Simple, Organized Architecture
- Template principle 4 -> IV. Testable User-Value Slices
- Template principle 5 -> V. Local Reproducibility
Added sections:
- Technology and Scope Constraints
- Development Workflow and Quality Gates
Removed sections:
- None
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

### I. Spec-Driven Flow First
Every feature MUST follow the order constitution -> spec -> plan -> tasks ->
implementation. Implementation work MUST NOT begin until the feature has an
approved specification, implementation plan, and task list. Changes discovered
during implementation MUST flow back into the relevant spec, plan, or tasks
before code is changed.

Rationale: the project exists to study Spec-Driven Development, so the workflow
is part of the product quality bar rather than an optional process preference.

### II. MVP Scope Discipline
The initial MVP MUST include only task management capabilities. RabbitMQ,
MassTransit, Hangfire, Redis, background job orchestration, distributed messaging,
and caching infrastructure MUST NOT be introduced in the first feature. Future
features MAY add these technologies only after a new spec and plan justify the
need.

Rationale: the first learning milestone must stay small enough to validate the
SDD flow, .NET, React, PostgreSQL, and Docker Compose without unnecessary
infrastructure.

### III. Simple, Organized Architecture
The system MUST use a simple web application architecture with clear boundaries:
.NET backend, React + Vite frontend, PostgreSQL storage, and Docker Compose for
local infrastructure. The backend MUST keep API, application/business rules, data
access, and configuration responsibilities separated without adding enterprise
patterns unless a feature plan proves they are needed.

Rationale: the architecture should be understandable for study while still
teaching maintainable separation of concerns.

### IV. Testable User-Value Slices
Specs MUST describe independently testable user stories prioritized by value.
Plans and tasks MUST preserve that independence so the P1 story can become a
demonstrable MVP by itself. Each implemented behavior MUST have an explicit
verification path, and tasks for behavior changes MUST include tests or a
documented rationale for a different verification method.

Rationale: SDD is only useful when each slice can be validated against user
outcomes, not merely against implementation activity.

### V. Local Reproducibility
The project MUST remain runnable and verifiable in a local development
environment. Runtime dependencies required by a feature MUST be represented in
Docker Compose or documented local setup instructions before implementation
tasks are marked complete. Secrets MUST NOT be committed; templates or examples
MAY document required environment variables.

Rationale: reproducible local setup keeps the learning project approachable and
prevents hidden machine-specific assumptions.

## Technology and Scope Constraints

- Backend target: .NET.
- Frontend target: React + Vite.
- Database target: PostgreSQL.
- Local infrastructure target: Docker Compose.
- Initial MVP scope: create, view, update, complete/reopen, and delete tasks, as
  defined by the first feature specification.
- Initial MVP exclusions: authentication, multi-user collaboration,
  notifications, RabbitMQ, MassTransit, Hangfire, Redis, background workers,
  caching infrastructure, and CI/CD.
- Any dependency outside the target stack MUST be justified in the implementation
  plan's Constitution Check before tasks are generated.

## Development Workflow and Quality Gates

1. Constitution review MUST happen before creating or changing feature specs.
2. Feature specs MUST define user stories, acceptance scenarios, functional
   requirements, key entities when data exists, success criteria, assumptions,
   and out-of-scope boundaries.
3. Implementation plans MUST pass the Constitution Check before Phase 0 research
   and again after Phase 1 design.
4. Task lists MUST be dependency-ordered, grouped by user story, and explicit
   about which tasks are required for the MVP.
5. Implementation MUST follow the generated tasks. If task order or scope
   changes, update tasks.md before changing code.
6. Verification evidence MUST be captured in the relevant plan, tasks, or
   implementation notes before a feature is considered complete.

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
The Constitution Check in plan.md is the formal gate for recording compliance
and any justified deviations.

**Version**: 1.0.0 | **Ratified**: 2026-07-08 | **Last Amended**: 2026-07-08
