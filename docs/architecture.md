# 🧠 Arquitetura do FinTrack API

---

## 🏗 Visão Arquitetural

A arquitetura foi projetada com base no princípio de:

> "Simplicidade inicial com capacidade de evolução controlada"

O objetivo não foi criar uma arquitetura perfeita desde o início, mas sim uma estrutura que **suporte crescimento sem colapso estrutural**.

---

## 🧱 Camadas

Domain  
- Regras de negócio puras  
- Independente de infraestrutura  

Application  
- Orquestra casos de uso  
- Contém pipeline e handlers  

Infrastructure  
- Persistência  
- Integrações externas  

API  
- Interface HTTP  
- Conversão de contratos  

---

## 🔀 Vertical Slice

A camada Application é organizada por feature, não por tipo técnico.

### Motivação

- Reduzir dispersão de código
- Aumentar coesão
- Facilitar manutenção

---

## ⚙️ CQRS (leve)

Separação lógica entre leitura e escrita.

### Decisão

Não separar fisicamente (microservices), evitando complexidade prematura.

---

## 🔁 Pipeline de execução

### Problema original

Cross-cutting concerns espalhados:

- validação em handlers
- logs inconsistentes
- ausência de tracing

---

### Solução

Pipeline baseado em steps:

```text
Validation → Logging → Observability → Exception → Handler
```

---

## 🧠 Análise crítica

### Benefícios

- Centralização de responsabilidades
- Ordem de execução controlada
- Extensibilidade sem alteração de handlers

### Trade-offs

- Complexidade inicial maior
- Curva de aprendizado
- Necessidade de disciplina arquitetural

---

## ⚖️ Decisões relevantes

### ✔ Não uso de MediatR

**Motivo:**

- Evitar abstração excessiva
- Manter controle explícito

**Trade-off:**

- Implementação manual

---

### ✔ Result Pattern

**Motivo:**

- Evitar exceções como fluxo
- Padronizar respostas

---

## 📊 Observabilidade como pilar

A observabilidade não foi tratada como complemento, mas como parte da arquitetura.

---
