# 💰 FinTrack API

## 📌 Visão Geral

O **FinTrack API** é um backend para gerenciamento financeiro pessoal, projetado com foco em **clareza arquitetural, extensibilidade e controle explícito do fluxo de execução**.

O projeto evoluiu de uma arquitetura simples para um modelo baseado em **pipeline de execução**, permitindo maior previsibilidade, observabilidade e desacoplamento.

---

## 🎯 Objetivo

Este projeto serve como:

- Base para estudo avançado de arquitetura de software
- Candidato a Trabalho de Conclusão de Curso (TCC)
- Demonstração prática de:
  - Clean Architecture
  - Pipeline Pattern
  - CQRS (leve)
  - Observabilidade com tracing distribuído
  - Multi-tenancy
  - Testes de integração com banco real

---

## 🧱 Arquitetura

A aplicação combina:

- Clean Architecture
- Vertical Slice Architecture
- Pipeline Pattern (Chain of Responsibility)
- Result Pattern

Estrutura:

API → Application → Domain → Infrastructure

---

## 🔁 Pipeline de execução

```text
Controller → Dispatcher → HandlerExecutor → Pipeline → Handler
```

### Responsabilidades

**Controller**
- Entrada HTTP
- Conversão para resposta HTTP

**Dispatcher**
- Resolução dinâmica de handlers

**HandlerExecutor**
- Orquestra pipeline
- Não contém regra de negócio

**Pipeline (Steps)**
- Validation
- Logging
- Observability
- Exception

**Handler**
- Regra de negócio

---

## 🧠 Decisão arquitetural chave

Foi adotado um pipeline explícito ao invés de frameworks como MediatR.

**Motivo:**

- Maior controle do fluxo
- Debug mais simples
- Evitar dependência externa

**Trade-off:**

- Mais código manual
- Maior responsabilidade arquitetural

---

## 🔐 Autenticação

- JWT (Bearer Token)
- IUserContext abstrai acesso ao usuário

---

## 🧠 Multi-tenant

- Isolamento por UserId
- Filtro obrigatório nas queries

---

## 📊 Observabilidade

- OpenTelemetry
- Jaeger
- Tracing por request

---

## 🛠️ Stack

- .NET / ASP.NET Core
- EF Core / PostgreSQL
- FluentValidation
- Serilog
- Testcontainers
- xUnit

---

## 🧪 Testes

- Banco real (PostgreSQL)
- Reset com Respawn
- Testes autenticados

---

## 🚀 Roadmap

✔ Pipeline estruturado  
✔ Observabilidade básica  

🚧 Em evolução  
- Enriquecimento de tracing  
- Métricas (Prometheus/Grafana)  
- Refresh token  

---

## 📄 Documentação

- [architecture.md](docs/architecture.md)
- [application-flow.md](docs/application-flow.md)
- [decisions.md](docs/decisions.md)
- [multitenancy.md](docs/multitenancy.md)
- [api-contract.md](docs/api-contract.md)

---
