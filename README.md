# 💰 FinTrack API

## 📌 Visão Geral

O **FinTrack API** é um backend para gerenciamento financeiro pessoal, com foco em organização de transações, categorias e usuários.

O projeto foi concebido com ênfase em:

* Arquitetura limpa e escalável
* Separação de responsabilidades
* Testes de integração realistas
* Preparação para cenários **offline-first** (integração com app mobile)

---

## 🎯 Objetivo

Servir como base para:

* Estudo avançado de **arquitetura de software**
* Evolução para **Trabalho de Conclusão de Curso (TCC)**
* Demonstração prática de:

  * Modelagem de domínio
  * Design de APIs
  * Testes de integração com banco real
  * Boas práticas de engenharia backend

---

## 🧱 Arquitetura

O projeto adota uma abordagem híbrida baseada em:

* Clean Architecture (separação de camadas)
* Vertical Slice na camada Application
* CQRS (leve)
* Result Pattern (padronização de responses)

### Estrutura de alto nível:

```
API → Application → Domain → Infrastructure
```

---

## 🛠️ Stack Tecnológica

* .NET 10 (ASP.NET Core)
* Entity Framework Core
* PostgreSQL
* Testcontainers (testes de integração)
* Respawn (reset de dados em testes)
* FluentValidation
* xUnit + FluentAssertions
* Docker (requisito para testes)

---

## 🧪 Testes

O projeto utiliza **testes de integração com banco real**, com foco em confiabilidade e reprodutibilidade.

### Estratégia:

* PostgreSQL isolado via Testcontainers
* Reset de dados com Respawn
* Migrations executadas de forma controlada

---

## 📦 Estrutura do Projeto

```
src/

FinTrack.Api → Camada de entrada (HTTP)
FinTrack.Application → Casos de uso (Vertical Slice)
FinTrack.Domain → Regras de negócio
FinTrack.Infrastructure → Persistência e integrações

tests/

FinTrack.Api.IntegrationTests → Testes de integração
FinTrack.Application.Tests → Testes unitários
```

---

## ⚙️ Como executar

```bash
dotnet restore
dotnet build
dotnet run
```
🧪 Como executar os testes

```bash
dotnet test
```

⚠️ Requisito: Docker em execução (necessário para Testcontainers)

---

🔍 Diretrizes
* Controllers devem ser finos (sem regra de negócio)
* Application orquestra os casos de uso
* Domain contém regras de negócio
* Infrastructure contém apenas implementações técnicas
* Todos endpoints retornam Result<T> como contrato padrão

---

## 📱 Integração futura (Mobile)

O projeto prevê integração com aplicação Android (Kotlin), incluindo:

* Sincronização offline-first
* Estratégias de resolução de conflito
* Evolução para comunicação assíncrona

---

## 🚀 Roadmap
✔ Já implementado
* Estrutura base do projeto
* Arquitetura (Clean + Vertical Slice)
* CRUD de transações
* Entidade Category e relacionamento com Transaction
* Padronização de responses com Result<T>
* Testes de integração com banco real

---

### 🚧 Em evolução
* Filtros e paginação em Transactions
* Autenticação (JWT)

---

## 📄 Documentação adicional
* [Arquitetura](./docs/architecture.md)

---

## 📌 Status

🚧 Em desenvolvimento ativo, com foco em evolução *incremental e qualidade arquitetural.
