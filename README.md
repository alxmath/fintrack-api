# 💰 FinTrack API

## 📌 Visão Geral

O **FinTrack API** é um backend para gerenciamento financeiro pessoal, projetado com foco em:

- Clareza arquitetural
- Evolução incremental
- Observabilidade
- Preparação para cenários distribuídos (offline-first)

O projeto também serve como base para estudo avançado de arquitetura de software e potencial Trabalho de Conclusão de Curso (TCC).

---

## 🎯 Objetivo

- Demonstrar boas práticas de arquitetura
- Servir como base evolutiva de produto
- Explorar padrões como:
  - Clean Architecture
  - Vertical Slice
  - CQRS (leve)
  - Pipeline Pattern

---

## 🚀 Stack

- .NET / ASP.NET Core
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Serilog
- OpenTelemetry
- Testcontainers
- xUnit

---

## ▶️ Execução

```bash
dotnet restore
dotnet build
dotnet run
```

### Testes

```bash
dotnet test
```

> Requer Docker para execução dos testes de integração

---

## 📚 Documentação

Toda a documentação detalhada está na pasta de docs (ou raiz do projeto):

### 🏗 Arquitetura
- [architecture.md](docs/architecture.md)

### 🔄 Fluxo da aplicação
- [application-flow.md](docs/application-flow.md)

### 📡 Contrato da API
- [api-contract.md](docs/api-contract.md)

### 🧠 Multi-tenancy
- [multitenancy.md](docs/multitenancy.md)

### 📖 Decisões arquiteturais
- [decisions.md](docs/decisions.md)

### 📊 Observabilidade
- [observability.md](docs/observability.md)

---

## 🧭 Como navegar na documentação

Sugestão de leitura:

1. `architecture.md`
2. `application-flow.md`
3. `decisions.md`
4. `observability.md`
5. `api-contract.md`

---

## 🧠 Filosofia do projeto

> Simplicidade inicial com capacidade de evolução controlada.

---

## 📌 Status

🚧 Em evolução contínua com foco em qualidade arquitetural
