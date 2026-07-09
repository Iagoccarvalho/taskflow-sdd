# Quickstart: Task Management

This guide defines the local validation flow expected after implementation. It
does not implement code.

## Prerequisites

- .NET 10 SDK installed.
- Node.js installed for the React + Vite frontend.
- Docker and Docker Compose available.
- PostgreSQL will run through Docker Compose.

## Expected Local Setup

1. Start PostgreSQL:

   ```bash
   docker compose up -d postgres
   ```

2. Restore backend dependencies:

   ```bash
   dotnet restore backend/TaskFlow.sln
   ```

3. Apply database migrations:

   ```bash
   dotnet ef database update --project backend/src/TaskFlow.Infrastructure --startup-project backend/src/TaskFlow.Api
   ```

4. Install frontend dependencies:

   ```bash
   npm install --prefix frontend
   ```

## Expected Run Commands

1. Run backend API:

   ```bash
   dotnet run --project backend/src/TaskFlow.Api
   ```

2. Run frontend:

   ```bash
   npm run dev --prefix frontend
   ```

## Expected Test Commands

1. Run backend tests:

   ```bash
   dotnet test backend/TaskFlow.sln
   ```

2. Run frontend tests:

   ```bash
   npm test --prefix frontend
   ```

## Manual Validation Scenarios

### Scenario 1: Create and list a task

1. Create a task with title "Study SDD" and no description.
2. View the task list.
3. Expected result: the task appears with status Pending.

### Scenario 2: Reject invalid task title

1. Try to create a task with an empty title.
2. Expected result: the operation is rejected and no task is added.

### Scenario 3: Update a task

1. Create a task.
2. Update the title and remove the description.
3. View the task list.
4. Expected result: the updated title appears and the task remains valid without
   a description.

### Scenario 4: Complete a task and inspect logs

1. Create a task.
2. Complete the task.
3. View the task list.
4. View processing logs for the task.
5. Expected result: the task status is Completed and exactly one TaskCompleted
   processing log exists for that task.

### Scenario 5: Prevent duplicate completion logs

1. Complete a task that is already Completed.
2. View processing logs for the task.
3. Expected result: no duplicate TaskCompleted log is created.

## Contract References

- REST API contract: [contracts/openapi.yaml](./contracts/openapi.yaml)
- Data model: [data-model.md](./data-model.md)
