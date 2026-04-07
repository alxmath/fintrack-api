# 🧠 Arquitetura do FinTrack API

---

## 🏗 Visão geral

A arquitetura foi projetada para equilibrar:

- Simplicidade inicial
- Evolução incremental
- Baixo acoplamento
- Alta observabilidade

---

## 🧱 Camadas

Domain → regras de negócio  
Application → casos de uso  
Infrastructure → persistência  
API → interface HTTP  

---

## 🔀 Vertical Slice

Organização por feature para aumentar coesão e reduzir dependências transversais.

---

## ⚙️ CQRS (leve)

Separação lógica:

- Commands → escrita
- Queries → leitura

Sem segregação física.

---

## 🔁 Pipeline

```text
Controller → Dispatcher → HandlerExecutor → Steps → Handler
```

---

## 🧠 Motivação do Pipeline

Resolver problemas de:

- Código duplicado (validação/log)
- Baixa observabilidade
- Crescimento desorganizado

---

## ⚖️ Trade-offs

| Decisão | Benefício | Trade-off |
|--------|----------|----------|
| Pipeline próprio | Controle total | Mais código |
| Dispatcher dinâmico | Flexibilidade | Reflection |
| Result Pattern | Consistência | Verbosidade |

---

## 📊 Observabilidade

- Tracing distribuído
- Integração com OpenTelemetry

---

## 🚧 Evolução futura

- Métricas
- Cache
- Refinamento do pipeline

---
