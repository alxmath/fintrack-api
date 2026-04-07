# 🔄 Fluxo de execução da Application

---

## 📌 Visão geral

O fluxo foi projetado para ser:

- Explícito
- Encadeado
- Controlável

---

## 🔁 Fluxo completo

```text
HTTP → Controller → Dispatcher → HandlerExecutor → Pipeline → Handler → DB
```

---

## 🧩 Componentes

### Controller

Responsável apenas por HTTP.

---

### Dispatcher

Resolve handlers dinamicamente.

---

### HandlerExecutor

Monta o pipeline em tempo de execução.

---

### Pipeline

#### ValidationStep
Garante consistência de entrada.

#### LoggingStep
Garante rastreabilidade.

#### ObservabilityStep
Gera tracing distribuído.

#### ExceptionStep
Garante captura e propagação correta de erros.

---

### Handler

Executa regra de negócio isolada.

---

## 🧠 Insight arquitetural

O handler deixa de ser o centro da execução.

👉 O pipeline passa a ser o verdadeiro fluxo da aplicação.

---
