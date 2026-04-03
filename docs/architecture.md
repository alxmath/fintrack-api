# 🧠 Arquitetura do FinTrack API

Este documento descreve as principais decisões arquiteturais do projeto, seus motivos e trade-offs.

---

## 🏗 Visão geral

O projeto segue uma combinação de:

* Clean Architecture
* Vertical Slice Architecture
* CQRS leve

A estrutura foi pensada para:

* Separar responsabilidades
* Facilitar evolução incremental
* Reduzir acoplamento entre camadas

---

## 🧱 Organização por camadas

```
Domain          → Entidades e regras de negócio
Application     → Casos de uso (commands/queries)
Infrastructure  → Persistência e integrações externas
Api             → Exposição HTTP
```

---

## 🔀 Vertical Slice Architecture

Ao invés de organizar apenas por camadas técnicas, o projeto utiliza **organização por feature**.

Exemplo:

```
Features/
 ├── Transactions
 ├── Categories (futuro)
```

### Motivo

* Reduz acoplamento
* Facilita manutenção
* Permite evolução por feature

---

## ⚙️ CQRS (leve)

Separação entre:

* Commands → escrita
* Queries → leitura

### Motivo

* Clareza de intenção
* Melhor organização de código
* Escalabilidade futura

---

## 🔁 Pipeline de execução (Application)

O fluxo de execução segue o padrão:

Controller → Executor → Handler


### Responsabilidades

* **Controller**
  * Recebe requisição HTTP
  * Delegação da execução

* **Executor (HandlerExecutor)**
  * Executa validação (FluentValidation)
  * Interrompe fluxo em caso de erro

* **Handler**
  * Contém apenas regra de negócio
  * Assume dados válidos

### Motivação

* Remover validação dos handlers
* Reduzir boilerplate
* Centralizar comportamento transversal

### Trade-off

* Leve aumento de abstração

---

## 🧪 Estratégia de testes

Os testes de integração são um ponto central do projeto.

### Tecnologias utilizadas

* Testcontainers
* PostgreSQL real
* Respawn

---

### 🐳 Testcontainers

Utilizado para subir um banco PostgreSQL isolado durante os testes.

#### Motivo

* Evitar dependência de banco local
* Garantir ambiente reproduzível
* Facilitar execução em CI/CD

#### Trade-off

* Necessidade de Docker

---

### 🔄 Respawn

Responsável por resetar os dados entre testes.

#### Motivo

* Evitar recriação do banco
* Melhor performance
* Isolamento entre testes

---

### 🔁 Migrations controladas

As migrations são executadas:

* Uma única vez por execução (fixture)

#### Motivo

* Evitar conflitos de schema
* Melhor controle do ciclo de vida do banco

---

## 🧩 Central Package Management

Uso de `Directory.Packages.props` para centralizar versões de pacotes.

### Motivo

* Evitar conflitos de versão
* Facilitar manutenção
* Garantir consistência entre projetos

---


## 🧠 Padrões utilizados

* Clean Architecture
* Vertical Slice
* CQRS (leve)
* Result Pattern (padronização de responses)

---

## 🔄 Result Pattern

O projeto utiliza um padrão de retorno baseado em `Result<T>` para padronizar respostas da API.

### Estrutura:

* `IsSuccess`
* `Error`
* `Value`

### Motivações:

* Padronização do contrato da API
* Redução de uso de exceptions como fluxo de controle
* Melhor previsibilidade para testes e consumidores

### Integração com HTTP

O Result<T> é convertido na camada API para:

* Sucesso → 200 OK
* Erro → ProblemDetails (RFC 7807)

---

## 🔌 Injeção de Dependência

A configuração de DI segue separação por camada:

### Application

* Handlers (Commands/Queries)
* Validators (FluentValidation)

### Infrastructure

* DbContext (EF Core)
* Repositories
* Providers (ex: DateTime)

### API

* Orquestração das dependências
* Configuração do pipeline HTTP

### Benefícios

* Baixo acoplamento entre camadas
* Melhor testabilidade
* Facilidade de evolução

---

## ⚖️ Trade-offs gerais

| Decisão         | Benefício         | Trade-off                   |
| --------------- | ----------------- | --------------------------- |
| Testcontainers  | Ambiente isolado  | Requer Docker               |
| Respawn         | Testes rápidos    | Complexidade inicial        |
| Vertical Slice  | Baixo acoplamento | Estrutura menos tradicional |
| Central Package | Consistência      | Setup mais rigoroso         |

---

## 🚧 Próximos passos arquiteturais

* Introdução de Dispatcher (remoção de dependência direta de handlers nos controllers)
* Logging estruturado
* Evolução de queries
* Versionamento de API

---

## 📌 Considerações finais

O projeto foi construído com foco em:

* Evolução incremental
* Qualidade de código
* Práticas utilizadas em ambientes reais

Ele serve como base para aprofundamento em backend, arquitetura e testes automatizados.
