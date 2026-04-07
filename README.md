# 💰 FinTrack API

## 📌 Visão Geral

O **FinTrack API** é um backend para gerenciamento financeiro pessoal, projetado com foco em:

- Clareza arquitetural
- Escalabilidade incremental
- Observabilidade nativa
- Preparação para cenários distribuídos (offline-first)

O projeto vai além de um CRUD tradicional, sendo construído como um **estudo prático de arquitetura de software aplicada**, com decisões explícitas e evolução controlada.

---

## 🎯 Problema

Aplicações de controle financeiro frequentemente sofrem com:

- Crescimento desorganizado da lógica de negócio
- Forte acoplamento entre camadas
- Baixa observabilidade
- Dificuldade de evolução (mobile, offline, integrações)

---

## 💡 Proposta

O FinTrack API resolve esses problemas através de:

- Pipeline explícito de execução
- Separação rigorosa de responsabilidades
- Observabilidade integrada desde o início
- Estratégia de multi-tenancy simples e evolutiva

---

## 🧱 Arquitetura

A aplicação combina:

- Clean Architecture  
- Vertical Slice Architecture  
- CQRS (leve)  
- Pipeline Pattern (Chain of Responsibility)  
- Result Pattern  

Estrutura:

```
API → Application → Domain → Infrastructure
```

---

## 🔁 Pipeline de execução

```
Controller → Dispatcher → HandlerExecutor → Pipeline → Handler
```

---

## 🧠 Decisão arquitetural central

Substituição do fluxo direto:

```
Controller → Handler
```

Por um pipeline explícito:

```
Controller → Dispatcher → HandlerExecutor → Pipeline → Handler
```

### Impacto

| Aspecto | Antes | Depois |
|--------|------|--------|
| Acoplamento | Alto | Baixo |
| Observabilidade | Limitada | Completa |
| Testabilidade | Média | Alta |
| Extensibilidade | Baixa | Alta |

---

## 📊 Observabilidade

- OpenTelemetry  
- Jaeger  
- Tracing distribuído por requisição  

---

## 🧠 Multi-tenancy

- Isolamento por `UserId`
- Base para evolução SaaS

---

## 📚 Documentação

### 🏗 Arquitetura

- [architecture.md](./architecture.md)  
  Visão arquitetural completa, decisões e trade-offs  

- [application-flow.md](./application-flow.md)  
  Fluxo detalhado de execução da aplicação  

---

### 📡 API

- [api-contract.md](./api-contract.md)  
  Contrato HTTP, padrões de resposta e erros  

---

### 🧠 Domínio e Estratégias

- [multitenancy.md](./multitenancy.md)  
  Estratégia de isolamento de dados e evolução  

---

### 📖 Decisões Arquiteturais

- [decisions.md](./decisions.md)  
  Registro histórico de decisões técnicas, motivações e impactos  

---

## 🧭 Como ler esta documentação

Para melhor compreensão, recomenda-se a seguinte ordem:

1. `architecture.md` → visão geral  
2. `application-flow.md` → fluxo interno  
3. `decisions.md` → contexto das escolhas  
4. `multitenancy.md` → estratégia de dados  
5. `api-contract.md` → interface externa  

---

## 🚀 Roadmap arquitetural

- Enriquecimento de tracing (userId, correlationId)
- Métricas (Prometheus / Grafana)
- Evolução para offline-first
- Possível distribuição futura

---

## 🧠 Diretriz do projeto

> Evitar complexidade prematura, mas garantir base sólida para evolução.

---

## 📌 Status

🚧 Em evolução com foco em qualidade arquitetural e maturidade de produto
