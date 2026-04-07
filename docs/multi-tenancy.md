# 🧠 Multi-Tenancy — FinTrack API

---

## 📌 Estratégia adotada

Shared Database + isolamento lógico por UserId.

---

## 🎯 Motivação

- Simplicidade operacional
- Facilidade de implementação
- Preparação para evolução futura

---

## ⚖️ Trade-offs

| Estratégia | Benefício | Trade-off |
|----------|----------|----------|
| Shared DB | Simples | Requer disciplina |

---

## 🧠 Análise crítica

Essa abordagem é ideal para:

- aplicações iniciais
- validação de produto
- sistemas com baixa complexidade organizacional

Não é ideal para:

- isolamento físico rigoroso
- compliance extremo

---

## 🔮 Evolução

- TenantId
- RBAC
- isolamento híbrido

---
