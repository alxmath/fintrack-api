# 🔄 Fluxo de execução da Application

---

## 📌 Fluxo

```text
Controller → Dispatcher → HandlerExecutor → Pipeline → Handler
```

---

## 🧱 Etapas

### Controller
Entrada HTTP

### Dispatcher
Resolve handler dinamicamente

### HandlerExecutor
Monta pipeline dinamicamente

### Pipeline Steps

Validation  
- Interrompe execução em erro  

Logging  
- Registra entrada/saída  

Observability  
- Cria spans  

Exception  
- Captura exceções  

### Handler
Executa regra de negócio

---

## 🎯 Benefícios

- Separação clara
- Extensibilidade
- Testabilidade

---
