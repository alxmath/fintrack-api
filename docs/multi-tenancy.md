# 🧠 Multi-Tenancy — FinTrack API

Este documento descreve a estratégia de isolamento de dados adotada pelo FinTrack API.

---

## 📌 Visão geral

A API utiliza **multi-tenancy lógico baseado em UserId**, garantindo que cada usuário acesse apenas seus próprios dados.

---

## 🎯 Objetivos

- Garantir isolamento de dados entre usuários
- Preparar o sistema para cenários SaaS
- Manter simplicidade na arquitetura
- Evitar complexidade prematura

---

## 🧱 Estratégia adotada

### ✔ Abordagem: Shared Database + Shared Schema

Todos os usuários compartilham:

- Mesmo banco de dados
- Mesmo schema

O isolamento é feito via:

- Coluna `UserId` em todas as entidades

---

## 🧩 Implementação

### 🔹 Entidades

Todas as entidades possuem:

public Guid UserId { get; private set; }

---

### 🔹 Application

O usuário autenticado é obtido via:

IUserContext

---

### 🔹 Uso nos handlers

Exemplo:

var userId = userContext.UserId;

var category = new Category(name, userId);

---

### 🔹 Repositórios

Todas as queries devem obrigatoriamente filtrar por usuário:

.Where(x => x.UserId == userId)

---

### 🔹 Banco de dados

Índices compostos são utilizados para garantir integridade:

(UserId, Name) UNIQUE

---

## 🔐 Segurança

### ✔ Garantias

- Um usuário não acessa dados de outro
- Isolamento aplicado na camada de persistência
- UserId não é exposto ao cliente

---

### ⚠️ Riscos mitigados

- Vazamento de dados por erro de query
- Conflito entre usuários
- Acesso indevido

---

## ⚖️ Trade-offs

| Estratégia | Benefício | Trade-off |
|----------|----------|----------|
| Shared DB | Simples | Requer disciplina |
| UserId filter | Fácil implementação | Dependência de consistência |
| Índice composto | Integridade | Overhead mínimo |

---

## 🔄 Alternativas consideradas

### ❌ Database por tenant

- Alto custo operacional
- Complexidade elevada

---

### ❌ Schema por tenant

- Complexidade de migrations
- Dificuldade de manutenção

---

## 🚧 Evolução futura

- Introdução de TenantId (organizações)
- Suporte a múltiplos usuários por tenant
- Controle de permissões (RBAC)
- Auditoria por usuário

---

## 📊 Impacto na arquitetura

- Handlers passam a depender de IUserContext
- Repositórios devem garantir filtragem obrigatória
- Testes devem validar isolamento

---

## 🧪 Testes

Os testes devem garantir:

- Isolamento entre usuários
- Não vazamento de dados entre tenants
- Consistência dos filtros por UserId

---

## 📌 Considerações finais

A estratégia adotada equilibra:

- Simplicidade
- Segurança
- Escalabilidade

Sendo ideal para:

- Aplicações SaaS iniciais
- Evolução incremental
- Crescimento controlado
