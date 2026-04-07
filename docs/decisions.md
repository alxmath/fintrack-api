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

## 📅 2026-04-01

### 🔄 Padronização de responses com Result<T>

**Decisão:**
Utilizar `Result<T>` como contrato padrão para todos os endpoints da API.

**Motivação:**

* Padronizar respostas da API
* Evitar uso de exceptions como fluxo de controle
* Melhorar previsibilidade em testes
* Simplificar controllers

**Alternativas consideradas:**

* Retorno direto de DTOs (rejeitado por inconsistência)
* Uso de exceptions para controle de fluxo (rejeitado por acoplamento e menor clareza)

**Consequências:**

* Controllers mais simples
* Testes mais consistentes
* Possibilidade de evoluir para tratamento mais avançado (ex: ProblemDetails)

---

### 🔌 Separação da Injeção de Dependência por camada

**Decisão:**
Separar configuração de DI entre Application e Infrastructure.

**Motivação:**

* Respeitar Clean Architecture (Dependency Rule)
* Evitar acoplamento indevido entre camadas
* Melhor organização do projeto

**Estrutura adotada:**

* Application → Handlers + Validators
* Infrastructure → Repositories + DbContext + Providers
* API → composição das dependências

**Consequências:**

* Maior clareza arquitetural
* Melhor testabilidade
* Facilidade de evolução futura

---

### 📂 Introdução de Categories com relacionamento

**Decisão:**
Adicionar entidade Category com relacionamento obrigatório em Transaction.

**Motivação:**

* Tornar o domínio mais realista
* Garantir integridade referencial
* Evoluir modelo de dados

**Consequências:**

* Ajustes nos testes de integração
* Maior consistência do domínio

---

## 📅 2026-04-03
### 🌐 Padronização de erros com ProblemDetails

Decisão:
Utilizar ProblemDetails (RFC 7807) como formato padrão de erro HTTP.

Motivação:

Padronização do contrato da API
Melhor integração com clientes
Facilitar debugging e observabilidade

Consequências:

Controllers passaram a mapear Result<T> → HTTP
Testes de integração passaram a validar estrutura de erro

---

### 🔁 Pipeline de validação (HandlerExecutor)

Decisão:
Centralizar validação em um executor ao invés de executar nos handlers.

Motivação:

Remover duplicação
Garantir execução obrigatória da validação
Melhor separação de responsabilidades

Consequências:

Handlers passaram a conter apenas regra de negócio
Testes de validação migraram para o executor

---

### 🧹 Remoção de validação dos handlers

Decisão:
Handlers não devem mais executar validação.

Motivação:

* Reduzir acoplamento
* Tornar handlers mais previsíveis
* Melhor aderência ao princípio de responsabilidade única

---

## 📅 2026-04-05

### 🔐 Autenticação com JWT

Decisão:
Utilizar JWT para autenticação.

Motivação:
- Stateless
- Escalável
- Padrão de mercado

Consequências:
- Introdução de IUserContext
- Testes autenticados

---

### 🧠 Multi-tenant por UserId

Decisão:
Isolar dados por usuário.

Motivação:
- Segurança
- Preparação para SaaS

Consequências:
- Alteração nas entidades
- Filtros nos repositórios

---

### 🔁 Dispatcher custom

Decisão:
Introduzir Dispatcher.

Motivação:
- Desacoplamento
- Pipeline centralizado

Consequências:
- Uso de reflection
- Possível otimização futura

---

## 📅 2026-04-07

### 🔁 Introdução do Pipeline

**Problema:**

- Lógica duplicada
- Baixa observabilidade

**Solução:**

Pipeline baseado em steps

**Resultado:**

- Fluxo controlado
- Melhor rastreabilidade

---

### 📊 Observabilidade distribuída

**Motivação:**

- Diagnóstico em produção
- Monitoramento de performance

**Impacto:**

- Integração com Jaeger
- Base para métricas

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
* Introdução gradual de eventos de domínio
* Evolução para cenários distribuídos e offline-first

---

## 📎 Observação

Este documento é evolutivo e será atualizado conforme novas decisões forem tomadas ao longo do desenvolvimento do projeto.
