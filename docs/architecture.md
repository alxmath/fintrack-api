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
* Result Pattern (em evolução)

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

* Introdução de Categories (relacionamento com Transactions)
* Padronização de responses (Result Pattern)
* Evolução do domínio
* Possível suporte a offline-first (futuro)

---

## 📌 Considerações finais

O projeto foi construído com foco em:

* Evolução incremental
* Qualidade de código
* Práticas utilizadas em ambientes reais

Ele serve como base para aprofundamento em backend, arquitetura e testes automatizados.
