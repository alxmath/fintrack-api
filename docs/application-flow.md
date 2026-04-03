# 🔄 Fluxo de execução da Application

## Visão geral

O fluxo de execução de uma requisição segue o padrão:

Controller → Executor → Handler

---

## Etapas

### 1. Controller

Recebe requisição HTTP e delega execução.

### 2. Executor

Responsável por:

* Executar validação (FluentValidation)
* Interromper fluxo em caso de erro

### 3. Handler

Responsável por:

* Executar regra de negócio
* Persistência via repositórios

---

## Benefícios

* Separação clara de responsabilidades
* Redução de boilerplate
* Maior previsibilidade

---

## Evolução futura

Este fluxo será evoluído para:

Controller → Dispatcher → Executor → Handler
