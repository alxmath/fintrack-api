# 🧠 Arquitetura do FinTrack API

---

## 🏗 Visão geral

O projeto utiliza:

- Clean Architecture
- Vertical Slice Architecture
- CQRS leve

---

## 🧱 Camadas

Domain → regras de negócio  
Application → casos de uso  
Infrastructure → persistência  
API → HTTP  

---

## 🔀 Vertical Slice

Organização por feature:

Features/
  Transactions
  Categories

---

## ⚙️ CQRS

Commands → escrita  
Queries → leitura  

---

## 🔁 Pipeline

Controller → Dispatcher → HandlerExecutor → Handler

---

## 🔐 Autenticação

JWT com Bearer Token

Componentes:

- JwtTokenService
- IUserContext
- Middleware de autenticação

---

## 🧠 Multi-tenant

Estratégia:

- UserId nas entidades
- Filtro por usuário nos repositórios

Benefícios:

- Isolamento de dados
- Segurança
- Base para SaaS

---

## 🧪 Testes

- Testcontainers
- PostgreSQL real
- Respawn

---

## 🧩 Result Pattern

- IsSuccess
- Errors (Dictionary)
- Value

---

## 🔌 Injeção de Dependência

Application:
- Handlers
- Validators

Infrastructure:
- Repositories
- DbContext

API:
- Configuração

---

## ⚖️ Trade-offs

Dispatcher custom → mais controle / mais complexidade  
Result Pattern → padronização / mais código  
Multi-tenant → simples / exige disciplina  

---

## 🚧 Próximos passos

- Observabilidade
- Refresh token
- Evolução de queries
