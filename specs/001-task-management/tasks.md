# Tasks: Task Management

**Input**: Design documents from `/specs/001-task-management/`

**Prerequisites**: [spec.md](./spec.md), [plan.md](./plan.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/openapi.yaml](./contracts/openapi.yaml), [quickstart.md](./quickstart.md)

**Tests**: Required for core business rules and persistence behavior from the spec: task creation, required title, completion, and processing log registration.

**Organization**: Tasks are small, ordered, verifiable, and grouped by requested implementation phase. `[P]` marks tasks that can run in parallel after their dependencies are complete.

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Can run in parallel because it touches different files and does not depend on incomplete tasks.
- **[Story]**: Used where a task maps directly to a spec user story.
- Dependencies are listed inline as `(depends on T###)`.

## Phase 1: Setup do Projeto

**Purpose**: Create the solution skeleton, frontend skeleton, and local database infrastructure.

- [X] T001 Create backend solution `TaskFlow.sln` in `backend/TaskFlow.sln`
- [X] T002 Create ASP.NET Core Web API project `TaskFlow.Api` in `backend/src/TaskFlow.Api/` (depends on T001)
- [X] T003 [P] Create class library project `TaskFlow.Application` in `backend/src/TaskFlow.Application/` (depends on T001)
- [X] T004 [P] Create class library project `TaskFlow.Domain` in `backend/src/TaskFlow.Domain/` (depends on T001)
- [X] T005 [P] Create class library project `TaskFlow.Infrastructure` in `backend/src/TaskFlow.Infrastructure/` (depends on T001)
- [X] T006 [P] Create test project `TaskFlow.Tests` in `backend/tests/TaskFlow.Tests/` (depends on T001)
- [X] T007 Add all backend projects to `backend/TaskFlow.sln` (depends on T002, T003, T004, T005, T006)
- [X] T008 Configure project reference from `backend/src/TaskFlow.Application/TaskFlow.Application.csproj` to `backend/src/TaskFlow.Domain/TaskFlow.Domain.csproj` (depends on T003, T004)
- [X] T009 Configure project references from `backend/src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj` to `backend/src/TaskFlow.Application/TaskFlow.Application.csproj` and `backend/src/TaskFlow.Domain/TaskFlow.Domain.csproj` (depends on T003, T004, T005)
- [X] T010 Configure project references from `backend/src/TaskFlow.Api/TaskFlow.Api.csproj` to `backend/src/TaskFlow.Application/TaskFlow.Application.csproj` and `backend/src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj` (depends on T002, T003, T005)
- [X] T011 Configure project references from `backend/tests/TaskFlow.Tests/TaskFlow.Tests.csproj` to all backend source projects (depends on T002, T003, T004, T005, T006)
- [X] T012 Add backend package dependencies for ASP.NET Core, EF Core, Npgsql provider, Swagger/OpenAPI, and test tooling in the relevant `.csproj` files under `backend/src/` and `backend/tests/`
- [X] T013 Create React + Vite frontend project structure in `frontend/` with TypeScript enabled
- [X] T014 [P] Create frontend source folders `frontend/src/components/`, `frontend/src/pages/`, `frontend/src/services/`, `frontend/src/types/`, and `frontend/src/tests/` (depends on T013)
- [X] T015 Create Docker Compose PostgreSQL service in `docker-compose.yml`
- [X] T016 Create local backend configuration placeholders for PostgreSQL connection in `backend/src/TaskFlow.Api/appsettings.Development.json` (depends on T015)

**Checkpoint**: Solution, frontend skeleton, and PostgreSQL local infrastructure are ready for domain and persistence work.

---

## Phase 2: Domínio

**Purpose**: Define core entities and business rules without HTTP or database dependencies.

- [ ] T017 [P] [US1] Create `TaskStatus` enum with `Pending` and `Completed` in `backend/src/TaskFlow.Domain/Tasks/TaskStatus.cs`
- [ ] T018 [P] [US4] Create `ProcessingLogEventType` value definition for `TaskCompleted` in `backend/src/TaskFlow.Domain/ProcessingLogs/ProcessingLogEventType.cs`
- [ ] T019 [US1] Create `TaskItem` entity with Id, Title, Description, Status, CreatedAt, UpdatedAt, and CompletedAt in `backend/src/TaskFlow.Domain/Tasks/TaskItem.cs` (depends on T017)
- [ ] T020 [US1] Add task creation factory or constructor rules that require non-whitespace Title and set Status to Pending in `backend/src/TaskFlow.Domain/Tasks/TaskItem.cs` (depends on T019)
- [ ] T021 [US3] Add title/description update behavior that rejects non-whitespace Title violations in `backend/src/TaskFlow.Domain/Tasks/TaskItem.cs` (depends on T019)
- [ ] T022 [US4] Add task completion behavior for Pending -> Completed and idempotent Completed handling in `backend/src/TaskFlow.Domain/Tasks/TaskItem.cs` (depends on T019)
- [ ] T023 [US4] Create `ProcessingLog` entity with Id, TaskId, EventType, Message, and CreatedAt in `backend/src/TaskFlow.Domain/ProcessingLogs/ProcessingLog.cs` (depends on T018)
- [ ] T024 [P] Create domain exception or result type for validation failures in `backend/src/TaskFlow.Domain/Common/DomainValidationException.cs`

**Checkpoint**: Domain can represent tasks, statuses, processing logs, and core rules without controllers or EF Core.

---

## Phase 3: Banco de Dados

**Purpose**: Configure PostgreSQL persistence, EF Core mapping, migrations, repositories, and optional seed.

- [ ] T025 Add EF Core and Npgsql package references to `backend/src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj` (depends on T012)
- [ ] T026 Create `TaskFlowDbContext` with `Tasks` and `ProcessingLogs` sets in `backend/src/TaskFlow.Infrastructure/Persistence/TaskFlowDbContext.cs` (depends on T019, T023, T025)
- [ ] T027 [P] Configure `TaskItem` EF Core mapping for `tasks` table in `backend/src/TaskFlow.Infrastructure/Persistence/Configurations/TaskItemConfiguration.cs` (depends on T026)
- [ ] T028 [P] Configure `ProcessingLog` EF Core mapping for `task_processing_logs` table in `backend/src/TaskFlow.Infrastructure/Persistence/Configurations/ProcessingLogConfiguration.cs` (depends on T026)
- [ ] T029 Configure one-to-many relationship from task to processing logs and uniqueness for one `TaskCompleted` log per task in `backend/src/TaskFlow.Infrastructure/Persistence/Configurations/ProcessingLogConfiguration.cs` (depends on T027, T028)
- [ ] T030 Register Infrastructure persistence dependencies in `backend/src/TaskFlow.Infrastructure/DependencyInjection.cs` (depends on T026)
- [ ] T031 Create repository implementation for task persistence in `backend/src/TaskFlow.Infrastructure/Repositories/TaskRepository.cs` (depends on T026)
- [ ] T032 Create repository implementation for processing log persistence in `backend/src/TaskFlow.Infrastructure/Repositories/ProcessingLogRepository.cs` (depends on T026)
- [ ] T033 Create initial EF Core migration for `tasks` and `task_processing_logs` in `backend/src/TaskFlow.Infrastructure/Persistence/Migrations/` (depends on T027, T028, T029)
- [ ] T034 [P] Create optional development seed hook in `backend/src/TaskFlow.Infrastructure/Persistence/DevelopmentSeed.cs` without enabling production seed by default (depends on T026)

**Checkpoint**: Persistence model can store tasks and processing logs with repeatable migrations.

---

## Phase 4: Application

**Purpose**: Define DTOs, use cases, validation, repository contracts, and business workflow coordination.

- [ ] T035 [P] Create task DTOs `CreateTaskRequest`, `UpdateTaskRequest`, and `TaskResponse` in `backend/src/TaskFlow.Application/Tasks/TaskDtos.cs`
- [ ] T036 [P] Create processing log DTOs `ProcessingLogResponse` and `CompleteTaskResponse` in `backend/src/TaskFlow.Application/ProcessingLogs/ProcessingLogDtos.cs`
- [ ] T037 [P] Create task repository contract in `backend/src/TaskFlow.Application/Abstractions/ITaskRepository.cs`
- [ ] T038 [P] Create processing log repository contract in `backend/src/TaskFlow.Application/Abstractions/IProcessingLogRepository.cs`
- [ ] T039 [P] Create unit of work or transaction contract in `backend/src/TaskFlow.Application/Abstractions/IUnitOfWork.cs`
- [ ] T040 [US1] Create task creation use case/service in `backend/src/TaskFlow.Application/Tasks/CreateTaskUseCase.cs` (depends on T020, T035, T037)
- [ ] T041 [US2] Create task listing use case/service in `backend/src/TaskFlow.Application/Tasks/ListTasksUseCase.cs` (depends on T035, T037)
- [ ] T042 [US2] Create get-task-by-id use case/service in `backend/src/TaskFlow.Application/Tasks/GetTaskUseCase.cs` (depends on T035, T037)
- [ ] T043 [US3] Create task update use case/service in `backend/src/TaskFlow.Application/Tasks/UpdateTaskUseCase.cs` (depends on T021, T035, T037)
- [ ] T044 [US4] Create task completion use case/service that updates status and creates one processing log in `backend/src/TaskFlow.Application/Tasks/CompleteTaskUseCase.cs` (depends on T022, T023, T036, T037, T038, T039)
- [ ] T045 [US4] Create task processing log listing use case/service in `backend/src/TaskFlow.Application/ProcessingLogs/ListTaskProcessingLogsUseCase.cs` (depends on T036, T038)
- [ ] T046 [US4] Create global processing log listing use case/service in `backend/src/TaskFlow.Application/ProcessingLogs/ListProcessingLogsUseCase.cs` (depends on T036, T038)
- [ ] T047 Create application validation/result model for not found and validation errors in `backend/src/TaskFlow.Application/Common/ApplicationResult.cs`
- [ ] T048 Register Application services in `backend/src/TaskFlow.Application/DependencyInjection.cs` (depends on T040, T041, T042, T043, T044, T045, T046)

**Checkpoint**: Application layer exposes all task and log workflows without depending on controllers.

---

## Phase 5: API

**Purpose**: Expose REST endpoints, Swagger, CORS, dependency injection, and error handling.

- [ ] T049 Configure API dependency injection for Application and Infrastructure in `backend/src/TaskFlow.Api/Program.cs` (depends on T030, T048)
- [ ] T050 Configure Swagger/OpenAPI for development in `backend/src/TaskFlow.Api/Program.cs` (depends on T049)
- [ ] T051 Configure CORS policy for local Vite frontend in `backend/src/TaskFlow.Api/Program.cs` (depends on T049)
- [ ] T052 Configure centralized error handling middleware in `backend/src/TaskFlow.Api/Middleware/ErrorHandlingMiddleware.cs` (depends on T047)
- [ ] T053 Register centralized error handling middleware in `backend/src/TaskFlow.Api/Program.cs` (depends on T052)
- [ ] T054 [P] Create API request/response contracts matching OpenAPI task schemas in `backend/src/TaskFlow.Api/Contracts/TasksContracts.cs` (depends on T035)
- [ ] T055 [P] Create API response contracts matching OpenAPI log schemas in `backend/src/TaskFlow.Api/Contracts/ProcessingLogContracts.cs` (depends on T036)
- [ ] T056 [US1] Create `POST /api/tasks` endpoint in `backend/src/TaskFlow.Api/Controllers/TasksController.cs` (depends on T040, T054)
- [ ] T057 [US2] Create `GET /api/tasks` and `GET /api/tasks/{taskId}` endpoints in `backend/src/TaskFlow.Api/Controllers/TasksController.cs` (depends on T041, T042, T054)
- [ ] T058 [US3] Create `PUT /api/tasks/{taskId}` endpoint in `backend/src/TaskFlow.Api/Controllers/TasksController.cs` (depends on T043, T054)
- [ ] T059 [US4] Create `POST /api/tasks/{taskId}/complete` endpoint in `backend/src/TaskFlow.Api/Controllers/TasksController.cs` (depends on T044, T054, T055)
- [ ] T060 [US4] Create `GET /api/tasks/{taskId}/processing-logs` endpoint in `backend/src/TaskFlow.Api/Controllers/ProcessingLogsController.cs` (depends on T045, T055)
- [ ] T061 [US4] Create `GET /api/processing-logs` endpoint in `backend/src/TaskFlow.Api/Controllers/ProcessingLogsController.cs` (depends on T046, T055)
- [ ] T062 Verify controllers contain no business rules and delegate to Application use cases in `backend/src/TaskFlow.Api/Controllers/`

**Checkpoint**: API matches `specs/001-task-management/contracts/openapi.yaml` and exposes tasks/logs through thin controllers.

---

## Phase 6: Frontend

**Purpose**: Implement simple React + Vite task management UI.

- [ ] T063 [P] Create frontend task and log types in `frontend/src/types/task.ts`
- [ ] T064 [P] Create task API service for create, list, get, update, complete, and log endpoints in `frontend/src/services/tasksApi.ts`
- [ ] T065 [US2] Create task list page in `frontend/src/pages/TaskListPage.tsx` (depends on T063, T064)
- [ ] T066 [US1] Create task creation form component in `frontend/src/components/TaskForm.tsx` (depends on T063)
- [ ] T067 [US1] Wire task creation form into task list page in `frontend/src/pages/TaskListPage.tsx` (depends on T065, T066)
- [ ] T068 [US3] Create task edit behavior in `frontend/src/components/TaskForm.tsx` (depends on T066)
- [ ] T069 [US3] Wire task update action into task list page in `frontend/src/pages/TaskListPage.tsx` (depends on T068)
- [ ] T070 [US4] Create complete-task action component in `frontend/src/components/CompleteTaskButton.tsx` (depends on T063, T064)
- [ ] T071 [US4] Wire complete-task action into task list page in `frontend/src/pages/TaskListPage.tsx` (depends on T065, T070)
- [ ] T072 [US4] Create processing logs section in `frontend/src/components/ProcessingLogsSection.tsx` (depends on T063, T064)
- [ ] T073 [US4] Wire processing logs section into task list page in `frontend/src/pages/TaskListPage.tsx` (depends on T072)
- [ ] T074 Configure frontend app entry to render task management page in `frontend/src/App.tsx` (depends on T065)

**Checkpoint**: Frontend supports listing, creation, update, completion, and log inspection with simple reusable components.

---

## Phase 7: Testes

**Purpose**: Verify required rules, API behavior, persistence, and core UI flows.

- [ ] T075 [P] [US1] Test task creation sets status Pending in `backend/tests/TaskFlow.Tests/Domain/TaskItemTests.cs` (depends on T020)
- [ ] T076 [P] [US1] Test required title rejects null, empty, and whitespace in `backend/tests/TaskFlow.Tests/Domain/TaskItemTests.cs` (depends on T020)
- [ ] T077 [P] [US4] Test task completion sets status Completed and CompletedAt in `backend/tests/TaskFlow.Tests/Domain/TaskItemTests.cs` (depends on T022)
- [ ] T078 [P] [US4] Test repeated completion does not create duplicate completion intent in `backend/tests/TaskFlow.Tests/Domain/TaskItemTests.cs` (depends on T022)
- [ ] T079 [US1] Test create task use case persists Pending task in `backend/tests/TaskFlow.Tests/Application/CreateTaskUseCaseTests.cs` (depends on T040)
- [ ] T080 [US3] Test update task use case rejects empty title in `backend/tests/TaskFlow.Tests/Application/UpdateTaskUseCaseTests.cs` (depends on T043)
- [ ] T081 [US4] Test complete task use case persists task status and processing log in `backend/tests/TaskFlow.Tests/Application/CompleteTaskUseCaseTests.cs` (depends on T044)
- [ ] T082 [US4] Test complete task use case does not create duplicate log for already Completed task in `backend/tests/TaskFlow.Tests/Application/CompleteTaskUseCaseTests.cs` (depends on T044)
- [ ] T083 [US1] Test `POST /api/tasks` returns 201 and Pending status in `backend/tests/TaskFlow.Tests/Api/TasksApiTests.cs` (depends on T056)
- [ ] T084 [US1] Test `POST /api/tasks` returns 400 for invalid title in `backend/tests/TaskFlow.Tests/Api/TasksApiTests.cs` (depends on T056)
- [ ] T085 [US2] Test `GET /api/tasks` returns created tasks in `backend/tests/TaskFlow.Tests/Api/TasksApiTests.cs` (depends on T057)
- [ ] T086 [US3] Test `PUT /api/tasks/{taskId}` updates title and description in `backend/tests/TaskFlow.Tests/Api/TasksApiTests.cs` (depends on T058)
- [ ] T087 [US4] Test `POST /api/tasks/{taskId}/complete` completes task and creates log in `backend/tests/TaskFlow.Tests/Api/TasksApiTests.cs` (depends on T059)
- [ ] T088 [US4] Test processing log endpoints return completion logs in `backend/tests/TaskFlow.Tests/Api/ProcessingLogsApiTests.cs` (depends on T060, T061)
- [ ] T089 [US1] Test frontend creation form validation in `frontend/src/tests/TaskForm.test.tsx` (depends on T066)
- [ ] T090 [US2] Test frontend task list rendering in `frontend/src/tests/TaskListPage.test.tsx` (depends on T065)
- [ ] T091 [US4] Test frontend complete action updates displayed status in `frontend/src/tests/CompleteTaskButton.test.tsx` (depends on T070)
- [ ] T092 [US4] Test frontend processing logs section renders logs in `frontend/src/tests/ProcessingLogsSection.test.tsx` (depends on T072)

**Checkpoint**: Required creation, title validation, completion, and log registration behavior is covered by automated tests.

---

## Phase 8: Documentação

**Purpose**: Document local execution, API usage, endpoints, and validation.

- [ ] T093 Update project overview and feature scope in `README.md`
- [ ] T094 Document prerequisites for .NET 10, Node.js, Docker, and Docker Compose in `README.md`
- [ ] T095 Document PostgreSQL local startup command from `docker-compose.yml` in `README.md` (depends on T015)
- [ ] T096 Document backend restore, migration, run, and test commands in `README.md` (depends on T033)
- [ ] T097 Document frontend install, run, and test commands in `README.md` (depends on T013)
- [ ] T098 Document REST endpoints and link to `specs/001-task-management/contracts/openapi.yaml` in `README.md` (depends on T056, T057, T058, T059, T060, T061)
- [ ] T099 Update validation guide with final local commands in `specs/001-task-management/quickstart.md`
- [ ] T100 Verify README excludes RabbitMQ, MassTransit, Hangfire, Redis, auth, workers, queues, and cache from this feature in `README.md`

**Checkpoint**: A developer can understand the feature, run it locally, inspect endpoints, and validate behavior from documentation.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Domínio**: Depends on backend project setup from Phase 1.
- **Phase 3 Banco de Dados**: Depends on Domain entities and Infrastructure project references.
- **Phase 4 Application**: Depends on Domain and repository contract decisions; can start after Domain basics, while Infrastructure repositories finish.
- **Phase 5 API**: Depends on Application use cases and Infrastructure registration.
- **Phase 6 Frontend**: Can start after API contracts are stable; final integration depends on API endpoints.
- **Phase 7 Testes**: Test files can be created early, but passing tests depend on corresponding Domain/Application/API/Frontend tasks.
- **Phase 8 Documentação**: Can start early; final command and endpoint documentation depends on implementation tasks.

### User Story Dependencies

- **US1 Criar tarefa**: Requires setup, TaskItem domain rules, create use case, `POST /api/tasks`, task form.
- **US2 Listar tarefas**: Requires persistence and list/get use cases; can be delivered after US1 creates data.
- **US3 Atualizar tarefa**: Requires TaskItem update rule and update use case; depends on ability to identify existing tasks.
- **US4 Concluir tarefa**: Requires ProcessingLog, completion domain rule, transactional completion use case, log endpoints, and log UI.

### Parallel Opportunities

- T003, T004, T005, T006 can run in parallel after T001.
- T017 and T018 can run in parallel after project setup.
- T027 and T028 can run in parallel after DbContext exists.
- T035, T036, T037, T038, T039 can run in parallel after Application project setup.
- T054 and T055 can run in parallel after DTOs exist.
- T063 and T064 can run in parallel after frontend setup.
- Domain unit tests T075-T078 can run in parallel once domain behavior exists.
- Frontend tests T089-T092 can run in parallel once corresponding components exist.

## Implementation Strategy

### MVP First

1. Complete Phase 1 Setup.
2. Complete Phase 2 Domain for TaskItem, TaskStatus and creation rule.
3. Complete minimum Phase 3 persistence for tasks.
4. Complete US1 tasks in Application, API, Frontend and Tests.
5. Validate: create task with valid title, reject empty title, list shows Pending task.

### Incremental Delivery

1. Deliver US1 create task.
2. Add US2 listing/get task.
3. Add US3 update task.
4. Add US4 complete task and processing logs.
5. Complete documentation and quickstart validation.

### Scope Guardrails

- Do not add authentication in this feature.
- Do not add RabbitMQ, MassTransit, Hangfire, Redis, background workers, queues, or cache.
- Keep controllers thin and rules in Application/Domain.
- Keep Infrastructure responsible for EF Core, DbContext, migrations and repositories.
