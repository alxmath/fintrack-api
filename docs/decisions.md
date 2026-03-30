# Decisões Arquiteturais — FinTrack

Este documento registra as principais decisões técnicas tomadas ao longo do desenvolvimento do projeto, bem como suas motivações e alternativas consideradas.

---

## 📅 2026-03-30

### 🧱 Arquitetura: Clean Architecture (adaptada)

**Decisão:**
Utilizar Clean Architecture com separação em camadas (API, Application, Domain, Infrastructure).

**Motivação:**

* Melhor organização do código
* Separação clara de responsabilidades
* Facilidade de manutenção e testes

**Alternativas consideradas:**

* Arquitetura monolítica simples (rejeitada por baixa escalabilidade)
* Arquitetura em camadas tradicional (rejeitada por menor flexibilidade)

---

### 📦 Organização: Vertical Slice na Application

**Decisão:**
Organizar a camada Application por funcionalidade (feature), ao invés de camadas técnicas.

**Motivação:**

* Alta coesão por caso de uso
* Melhor navegabilidade
* Escalabilidade do código

**Alternativas consideradas:**

* Organização por pastas técnicas (DTOs, Services, etc.)

---

### 🌐 API: Uso de Controllers (MVC)

**Decisão:**
Utilizar Controllers ao invés de Minimal APIs.

**Motivação:**

* Melhor organização para projetos maiores
* Maior aderência a padrões de mercado
* Clareza arquitetural

**Alternativas consideradas:**

* Minimal API (rejeitada por dificultar organização em larga escala)

---

### 🧠 Domain: Modelo rico

**Decisão:**
Centralizar regras de negócio na camada Domain.

**Motivação:**

* Evitar lógica espalhada
* Garantir consistência das regras
* Facilitar testes unitários

---

### 🔄 Eventos de domínio

**Decisão:**
Não utilizar eventos de domínio na fase inicial.

**Motivação:**

* Evitar complexidade prematura
* Focar em validação da arquitetura base

**Plano futuro:**
Introduzir eventos de domínio de forma incremental (ex: TransactionCreatedEvent), conforme necessidade real surgir.

---

### 📱 Integração com mobile (offline-first)

**Decisão:**
Projetar a API considerando futura sincronização com aplicação mobile.

**Motivação:**

* Cenários reais de instabilidade de rede
* Evolução para sistema distribuído
* Alinhamento com proposta de TCC

---

## 📌 Diretriz geral

As decisões arquiteturais devem priorizar:

* Simplicidade inicial
* Evolução progressiva
* Clareza estrutural
* Aderência a problemas reais

Evitar overengineering é um princípio central do projeto.
