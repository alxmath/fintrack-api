# 📡 API Contract — FinTrack API

Este documento define o contrato HTTP da API, incluindo formatos de sucesso, erro e convenções utilizadas.

---

## 📌 Princípios

- A API segue padrões REST
- Respostas de erro utilizam **ProblemDetails (RFC 7807)**
- Validações retornam erros estruturados por campo
- Autenticação via **JWT Bearer Token**

---

## 🔐 Autenticação

### Header obrigatório

Authorization: Bearer {token}

---

## ✅ Respostas de sucesso

### 200 OK

Retorna o recurso solicitado.

Exemplo:

```json
{
  "id": "guid",
  "name": "Alimentação"
}
```
---

### 201 Created

Retornado em criações.

Inclui:

- Body com o recurso criado
- Header Location com endpoint do recurso

Exemplo:

```json
{
  "id": "guid",
  "name": "Alimentação"
}
```

---

## ❌ Respostas de erro

A API utiliza ProblemDetails.

---

### 🔴 400 — Validation Error

Quando falha de validação ocorre.

Formato:
```json
{
  "type": "https://httpstatuses.com/400",
  "title": "Validation error",
  "status": 400,
  "errors": {
    "name": [
      "Name is required",
      "Name must be less than 100 characters"
    ]
  },
  "traceId": "..."
}
```
---

### 🔴 404 — Not Found

Quando recurso não é encontrado.

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Resource not found",
  "status": 404,
  "detail": "Categoria não encontrada",
  "traceId": "..."
}
```

---

### 🔴 409 — Conflict

Quando há conflito de dados.

```json
{
  "type": "https://httpstatuses.com/409",
  "title": "Conflict",
  "status": 409,
  "detail": "Categoria já existe",
  "traceId": "..."
}
```

---

### 🔴 500 — Unexpected Error

Erro interno não tratado.

```json
{
  "type": "https://httpstatuses.com/500",
  "title": "Unexpected error",
  "status": 500,
  "detail": "Erro inesperado",
  "traceId": "..."
}
```

---

## 🧠 Convenções adotadas

### ✔ Nomes de propriedades

- JSON utiliza camelCase
- Campos seguem padrão consistente (name, description, etc.)

---

### ✔ Validação

- Executada via FluentValidation
- Centralizada no pipeline (HandlerExecutor)
- Não ocorre nos handlers

---

### ✔ Estrutura de erro

- errors → validação por campo
- detail → erros gerais
- traceId → rastreabilidade

---

### ✔ Status codes

| Status | Uso |
|------|-----|
| 200 | Sucesso |
| 201 | Criado |
| 400 | Validação |
| 404 | Não encontrado |
| 409 | Conflito |
| 500 | Erro interno |

---

## 🔄 Evolução futura

- Internacionalização de mensagens de erro
- Versionamento de contrato (v2)
- Padronização para SDK frontend

---

## 📌 Observações finais

Este contrato foi projetado para:

- Facilitar integração com frontend
- Garantir consistência
- Melhorar debug e observabilidade
