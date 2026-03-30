# FinTrack API

## 📌 Visão Geral

O FinTrack API é um backend para gerenciamento financeiro pessoal, com foco em organização de transações, categorias e usuários.

O projeto foi concebido com ênfase em:

* Arquitetura limpa e escalável
* Separação de responsabilidades
* Preparação para cenários offline-first (integração com app mobile)

## 🎯 Objetivo

Servir como base para estudo avançado de arquitetura de software, podendo evoluir para Trabalho de Conclusão de Curso (TCC), com foco em backend, design de sistemas e integração entre aplicações.

## 🧱 Arquitetura

O projeto adota uma abordagem híbrida baseada em:

* Clean Architecture (separação de camadas)
* Vertical Slice na camada Application
* Princípios de baixo acoplamento e alta coesão

### Estrutura de alto nível:

API → Application → Domain → Infrastructure

## 🛠️ Stack Tecnológica

* .NET Web API
* Entity Framework Core
* PostgreSQL
* Docker (planejado)
* JWT (autenticação)

## 📦 Estrutura do Projeto

src/

* FinTrack.Api → Camada de entrada (HTTP)
* FinTrack.Application → Casos de uso (Vertical Slice)
* FinTrack.Domain → Regras de negócio
* FinTrack.Infrastructure → Persistência e serviços externos

## 🚀 Roadmap Inicial

* [ ] Setup da solução e projetos
* [ ] Configuração de DI
* [ ] Primeiro caso de uso: CreateTransaction
* [ ] Persistência com EF Core
* [ ] Autenticação básica
* [ ] Deploy inicial

## 🔍 Diretrizes

* Controllers devem ser finos (sem regra de negócio)
* Regras devem estar no Domain
* Application orquestra os casos de uso
* Infrastructure contém apenas implementações técnicas

## 📱 Integração futura (Mobile)

O projeto prevê integração com aplicação Android (Kotlin), incluindo:

* Sincronização offline-first
* Estratégias de resolução de conflito
* Evolução para comunicação assíncrona

## 📚 Uso acadêmico (TCC)

Este projeto poderá ser utilizado como base para TCC, com foco em:

* Arquitetura de software
* Integração entre sistemas
* Estratégias de sincronização de dados
* Design de APIs escaláveis

## 📌 Status

Em desenvolvimento inicial (setup e definição arquitetural).
