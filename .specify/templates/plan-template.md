# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]

**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit-plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: [.NET version for backend, TypeScript/React + Vite version for frontend or NEEDS CLARIFICATION]

**Primary Dependencies**: [e.g., ASP.NET Core, Entity Framework Core, React, Vite or NEEDS CLARIFICATION]

**Storage**: [PostgreSQL for persisted application data or N/A]

**Testing**: [.NET test framework, frontend test tooling, API/integration test approach or NEEDS CLARIFICATION]

**Target Platform**: [local Docker Compose, browser frontend, backend API runtime or NEEDS CLARIFICATION]

**Project Type**: [web application with backend API + frontend SPA or NEEDS CLARIFICATION]

**Performance Goals**: [domain-specific, e.g., 1000 req/s, 10k lines/sec, 60 fps or NEEDS CLARIFICATION]

**Constraints**: [domain-specific, e.g., <200ms p95, <100MB memory, offline-capable or NEEDS CLARIFICATION]

**Scale/Scope**: [domain-specific, e.g., 10k users, 1M LOC, 50 screens or NEEDS CLARIFICATION]

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-driven sequence**: Confirm constitution review is complete and this
  feature has a spec before plan, tasks before implementation.
- **MVP scope discipline**: Confirm the initial MVP stays within task
  management and excludes RabbitMQ, MassTransit, Hangfire, Redis, background
  workers, caching infrastructure, authentication, collaboration,
  notifications, and CI/CD unless a later approved spec changes scope.
- **Simple organized architecture**: Confirm the design uses .NET backend,
  React + Vite frontend, PostgreSQL when persistence is needed, and Docker
  Compose for local infrastructure without unjustified enterprise patterns.
- **Testable user-value slices**: Confirm user stories are independently
  testable, prioritized, and capable of delivering a P1 MVP slice.
- **Local reproducibility**: Confirm runtime dependencies, environment
  variables, and verification commands can be represented locally without
  committed secrets.
- **Verification plan**: Confirm each behavior has tests or a documented
  verification method before implementation tasks are generated.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit-plan command output)
├── research.md          # Phase 0 output (/speckit-plan command)
├── data-model.md        # Phase 1 output (/speckit-plan command)
├── quickstart.md        # Phase 1 output (/speckit-plan command)
├── contracts/           # Phase 1 output (/speckit-plan command)
└── tasks.md             # Phase 2 output (/speckit-tasks command - NOT created by /speckit-plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
# [REMOVE IF UNUSED] Option 1: Single project (DEFAULT)
src/
├── models/
├── services/
├── cli/
└── lib/

tests/
├── contract/
├── integration/
└── unit/

backend/
├── src/
│   ├── Api/
│   ├── Application/
│   ├── Domain/
│   └── Infrastructure/
└── tests/

frontend/
├── src/
│   ├── components/
│   ├── pages/
│   ├── services/
│   └── tests/
└── tests/

# [REMOVE IF UNUSED] Option 3: Mobile + API (when "iOS/Android" detected)
api/
└── [same as backend above]

ios/ or android/
└── [platform-specific structure: feature modules, UI flows, platform tests]
```

**Structure Decision**: [Document the selected structure and reference the real
directories captured above]

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
