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

- Backend: .NET
- Frontend: React + Vite
- Banco: PostgreSQL
- Infra local: Docker Compose
