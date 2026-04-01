# Decisões Arquiteturais — FinTrack

Este documento registra as principais decisões técnicas tomadas ao longo do desenvolvimento do projeto, bem como suas motivações, trade-offs e evolução.

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

* Introduzir eventos de domínio de forma incremental (ex: `TransactionCreatedEvent`), conforme necessidade real surgir

---

### 📱 Integração com mobile (offline-first)

**Decisão:**
Projetar a API considerando futura sincronização com aplicação mobile.

**Motivação:**

* Cenários reais de instabilidade de rede
* Evolução para sistema distribuído
* Alinhamento com proposta de TCC

---

## 📅 2026-03-31

### 🧪 Estratégia de testes: Integração com banco real

**Decisão:**
Utilizar testes de integração com banco PostgreSQL real, ao invés de mocks ou banco em memória.

**Motivação:**

* Maior confiabilidade dos testes
* Cobertura real do comportamento da aplicação
* Detecção de problemas de persistência

**Alternativas consideradas:**

* InMemory Database (rejeitado por não refletir comportamento real do banco)
* Mocks de repositório (rejeitado para testes de integração)

---

### 🐳 Uso de Testcontainers

**Decisão:**
Utilizar Testcontainers para provisionar o banco PostgreSQL durante os testes.

**Motivação:**

* Isolamento completo do ambiente de teste
* Independência de banco local
* Reprodutibilidade em qualquer ambiente (incluindo CI)

**Trade-offs:**

* Necessidade de Docker em execução
* Aumento leve no tempo de inicialização dos testes

---

### 🔄 Reset de dados com Respawn

**Decisão:**
Utilizar Respawn para limpar os dados entre os testes.

**Motivação:**

* Evitar recriação completa do banco
* Melhor performance dos testes
* Garantia de isolamento entre cenários

**Alternativas consideradas:**

* `EnsureDeleted + Migrate` (rejeitado por ser lento e não escalável)

---

### 🔁 Controle de migrations

**Decisão:**
Executar migrations uma única vez durante a inicialização do container de testes.

**Motivação:**

* Evitar conflitos de schema
* Garantir consistência estrutural
* Melhor controle do ciclo de vida do banco

---

### 🧪 Infraestrutura de testes centralizada

**Decisão:**
Centralizar a infraestrutura de testes (Factory, Fixture, Reset) e criar uma base comum (`IntegrationTestBase`).

**Motivação:**

* Reduzir duplicação de código
* Padronizar setup de testes
* Facilitar manutenção

---

### 📦 Gerenciamento centralizado de pacotes

**Decisão:**
Utilizar `Directory.Packages.props` para centralizar versões de dependências.

**Motivação:**

* Evitar conflitos de versões entre projetos
* Facilitar upgrades
* Garantir consistência na solução

**Trade-offs:**

* Necessidade de maior rigor na definição de versões
* Erros mais explícitos durante configuração inicial

---

## 📌 Diretriz geral

As decisões arquiteturais devem priorizar:

* Simplicidade inicial
* Evolução progressiva
* Clareza estrutural
* Aderência a problemas reais

Evitar overengineering é um princípio central do projeto.

---

## 🔮 Evolução planejada

* Introdução de `Category` e relacionamento com `Transaction`
* Padronização de responses (Result Pattern)
* Introdução gradual de eventos de domínio
* Evolução para cenários distribuídos e offline-first

---

## 📎 Observação

Este documento é evolutivo e será atualizado conforme novas decisões forem tomadas ao longo do desenvolvimento do projeto.
