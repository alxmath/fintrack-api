# 🔄 Fluxo de execução da Application

## 📌 Visão geral

Controller → Dispatcher → HandlerExecutor → Handler

---

## 🧱 Etapas

### Controller

- Recebe requisição HTTP
- Chama Dispatcher

---

### Dispatcher

- Resolve handler via DI
- Resolve validator automaticamente

---

### HandlerExecutor

- Executa validação
- Interrompe fluxo em erro
- Logging estruturado
- Mede tempo de execução

---

### Handler

- Executa regra de negócio
- Acessa repositórios
- Retorna Result<T>

---

## 🎯 Benefícios

- Separação de responsabilidades
- Pipeline reutilizável
- Código limpo
- Testabilidade

---

## 🔐 Integração com autenticação

- IUserContext fornece UserId
- Sem acoplamento com HTTP
