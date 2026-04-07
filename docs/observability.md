# 📊 Observabilidade — FinTrack API

---

## 📌 1. Contexto

Em sistemas modernos, especialmente aqueles que evoluem em complexidade ao longo do tempo, a capacidade de **entender o comportamento interno da aplicação em tempo de execução** torna-se essencial.

Abordagens tradicionais baseadas apenas em logs são insuficientes para:

- rastrear fluxos completos de execução
- identificar gargalos de performance
- diagnosticar falhas em múltiplas camadas

Diante desse cenário, a observabilidade foi tratada no FinTrack API como um **requisito arquitetural**, e não como uma funcionalidade adicional.

---

## 🎯 2. Problema

Antes da introdução de observabilidade estruturada, os principais desafios eram:

- Dificuldade em rastrear uma requisição ponta a ponta
- Falta de visibilidade sobre tempo de execução
- Diagnóstico limitado a logs isolados
- Baixa capacidade de análise em cenários de erro

Esses problemas tendem a se agravar conforme:

- novas features são adicionadas
- integrações externas surgem
- volume de dados cresce

---

## 🧠 3. Fundamentação

Observabilidade é tradicionalmente composta por três pilares:

### 📄 Logs
Registros textuais de eventos ocorridos no sistema.

### 📈 Métricas
Dados agregados ao longo do tempo (latência, throughput, etc.).

### 🔍 Traces
Representação do fluxo de execução de uma requisição através do sistema.

---

## 🎯 Decisão adotada

O projeto prioriza inicialmente o uso de **tracing distribuído**.

### Motivo:

- Permite visualizar o fluxo completo de execução
- Facilita identificação de gargalos
- Oferece maior valor diagnóstico imediato

Logs e métricas são considerados complementares e evolutivos.

---

## 🧱 4. Arquitetura de Observabilidade

A observabilidade está integrada diretamente ao pipeline de execução da aplicação.

```text
Controller → Dispatcher → HandlerExecutor → ObservabilityStep → Handler
```

### Ponto chave

O `ObservabilityStep` atua como um **interceptor centralizado**, garantindo que todas as execuções passem por instrumentação.

---

## ⚙️ 5. Implementação

A implementação utiliza:

- OpenTelemetry
- System.Diagnostics.Activity
- Jaeger (visualização de traces)

---

### 🔹 Criação de spans

Cada requisição gera um span associado ao handler executado.

Exemplo conceitual:

```csharp
using var activity = _activitySource.StartActivity("GetTransactions");
```

---

### 🔹 Tags adicionadas

Os spans são enriquecidos com metadados relevantes:

- `request.name`
- `execution.time`
- `result.success`

Essas informações permitem:

- identificar operações
- medir performance
- analisar sucesso/falha

---

### 🔹 Tratamento de exceções

Quando ocorre erro:

- o span é marcado como erro
- informações da exceção são adicionadas
- o erro é propagado (não é engolido)

---

### 🔹 Visualização

Os traces são enviados para o Jaeger, permitindo:

- visualização hierárquica das chamadas
- identificação de falhas
- análise de tempo de execução

---

## 📊 6. Exemplo de fluxo observado

### ✔ Execução normal

```text
HTTP GET /transactions
 └── GetTransactions
      └── Database Query
```

---

### ❌ Execução com erro

```text
HTTP (ERROR)
 └── Handler (ERROR)
```

---

## 🧠 7. Análise crítica

---

### ✔ Benefícios

- Visibilidade completa do fluxo de execução
- Diagnóstico mais rápido de problemas
- Base para monitoramento em produção
- Redução do tempo de debugging

---

### ⚖️ Trade-offs

| Aspecto | Impacto |
|--------|--------|
| Complexidade inicial | Moderada |
| Overhead de execução | Baixo |
| Curva de aprendizado | Média |

---

### ⚠️ Limitações atuais

- Ausência de métricas (Prometheus/Grafana)
- Falta de enrichment com contexto de negócio (ex: userId)
- Correlação entre serviços ainda não implementada

---

## 🚀 8. Evolução planejada

---

### 🥇 Enriquecimento de spans

Adicionar:

- `user.id`
- `correlation.id`
- parâmetros de filtros

---

### 🥈 Métricas

Integração com:

- Prometheus
- Grafana

Métricas planejadas:

- latência (p50, p95)
- taxa de erro
- throughput

---

### 🥉 Correlação distribuída

Propagação de:

- correlationId entre serviços
- integração com logs estruturados

---

## 🧭 9. Papel na arquitetura

A observabilidade no FinTrack API não é um componente isolado.

Ela atua como:

- ferramenta de diagnóstico
- suporte à tomada de decisão
- base para evolução arquitetural

---

## 💬 10. Conclusão

A introdução de observabilidade baseada em tracing representou uma evolução significativa na arquitetura do sistema.

O projeto passa a ter:

- maior previsibilidade
- melhor capacidade de diagnóstico
- base sólida para operação em produção

Essa abordagem permite que o sistema evolua com segurança, mantendo visibilidade sobre seu comportamento interno.
