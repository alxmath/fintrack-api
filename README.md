# 💰 FinTrack API

## 📌 Visão Geral

O **FinTrack API** é um backend para gerenciamento financeiro pessoal, com foco em organização de transações, categorias e usuários.

O projeto foi concebido com ênfase em:

* Arquitetura limpa e escalável
* Separação de responsabilidades
* Testes de integração realistas
* Segurança e isolamento de dados por usuário
* Preparação para cenários **offline-first**

---

## 🎯 Objetivo

Servir como base para:

* Estudo avançado de **arquitetura de software**
* Evolução para **Trabalho de Conclusão de Curso (TCC)**
* Demonstração prática de:

  * Modelagem de domínio
  * Design de APIs REST
  * Autenticação com JWT
  * Multi-tenant (isolamento por usuário)
  * Testes com banco real

---

## 🧱 Arquitetura

O projeto adota:

* Clean Architecture
* Vertical Slice Architecture
* CQRS (leve)
* Result Pattern

Estrutura:

API → Application → Domain → Infrastructure

---

## 🔁 Pipeline de execução

Controller → Dispatcher → HandlerExecutor → Handler

### Responsabilidades

Controller
- Recebe requisição HTTP
- Define resposta HTTP

Dispatcher
- Resolve handler via DI
- Resolve validator automaticamente

HandlerExecutor
- Executa validação
- Logging estruturado
- Controle de fluxo

Handler
- Contém regra de negócio

---

## 🔐 Autenticação

A API utiliza JWT (Bearer Token).

Fluxo:

1. POST /api/v1/auth/login
2. Recebe access_token
3. Envia no header:

Authorization: Bearer {token}

Observações:

- Rotas protegidas exigem autenticação
- Usuário acessado via IUserContext

---

## 🧠 Multi-tenant

Estratégia:

- Cada entidade possui UserId
- Queries filtram por usuário

Benefícios:

- Isolamento de dados
- Segurança
- Base para SaaS

---

## 🛠️ Stack Tecnológica

- .NET
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Serilog
- Testcontainers
- Respawn
- xUnit + FluentAssertions

---

## 🧪 Testes

- Banco real com Testcontainers
- Reset com Respawn
- Testes autenticados com JWT

---

## 📡 Padrão de respostas

### Sucesso

```json
{
  "id": "...",
  "name": "Mercado"
}
```

### Erro
```json
{
  "type": "https://httpstatuses.com/400",
  "title": "Validation error",
  "status": 400,
  "errors": {
    "name": ["Name is required"]
  },
  "traceId": "..."
}
```

---

## ⚙️ Execução

```
dotnet restore  
dotnet build  
dotnet run  
```

### Testes

```
dotnet test  
```

⚠️ Requer Docker

---

## 📦 Estrutura

src/
  Api
  Application
  Domain
  Infrastructure

tests/
  IntegrationTests
  UnitTests

---

## 🚀 Roadmap

✔ Implementado

- Arquitetura base
- CRUD
- Result Pattern
- ProblemDetails estruturado
- JWT
- Multi-tenant
- Testes de integração

🚧 Em evolução

- Filtros avançados
- Observabilidade
- Refresh token

---

## 📄 Documentação

- architecture.md
- application-flow.md
- decisions.md

---

## 📌 Status

🚧 Em evolução com foco em qualidade arquitetural
