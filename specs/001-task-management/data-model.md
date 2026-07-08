# Data Model: Task Management

## Overview

This feature persists tasks and processing logs. A task starts as Pending and can
transition to Completed once. Completing a task creates one processing log
associated with that task.

## Entity: Task

Represents a task managed by the local user.

### Fields

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| Id | UUID | Yes | Stable identifier generated when the task is created |
| Title | string | Yes | Must not be empty or whitespace |
| Description | string | No | Optional text; may be absent or empty |
| Status | enum | Yes | Pending or Completed |
| CreatedAt | timestamp | Yes | Creation time |
| UpdatedAt | timestamp | Yes | Last task content/status update time |
| CompletedAt | timestamp | No | Set when status changes to Completed |

### Validation Rules

- Title is required.
- Title must contain at least one non-whitespace character.
- Description is optional.
- Status is required.
- New tasks always start with Pending.
- CompletedAt is empty while status is Pending.
- CompletedAt is set when status becomes Completed.

### State Transitions

```text
Pending -> Completed
Completed -> Completed (no state change; no duplicate processing log)
```

Disallowed in this feature:

- Completed -> Pending
- Pending or Completed -> Deleted
- Any transition driven by background processing

## Entity: TaskProcessingLog

Represents a persistent processing record generated when a task is completed.

### Fields

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| Id | UUID | Yes | Stable identifier generated when the log is created |
| TaskId | UUID | Yes | References the completed task |
| EventType | string | Yes | Use `TaskCompleted` for this feature |
| Message | string | Yes | Human-readable processing description |
| CreatedAt | timestamp | Yes | Log creation time |

### Validation Rules

- TaskId is required and must reference an existing task.
- EventType is required.
- Message is required.
- A task must not receive more than one completion processing log.

## Relationships

- Task has zero or more TaskProcessingLog records.
- TaskProcessingLog belongs to exactly one Task.
- For this feature, a Task can have at most one log with EventType
  `TaskCompleted`.

## Persistence Notes

- Tables: `tasks`, `task_processing_logs`.
- The schema must be created and evolved with migrations.
- Completing a task must persist the task status update and processing log
  together so users do not see a Completed task without its completion log.

## Query Needs

- List all tasks with current status.
- Retrieve one task by Id.
- Retrieve processing logs for a task.
- Retrieve processing logs globally for local inspection.
