# API de Lista de Tarefas (CRUD em .NET)

## Links do Projeto

Esta API está publicada no Render e alimenta um front-end Vue.js publicado no Vercel.

* **API "Live" (Swagger UI):**
    **[https://isaac-api-tarefas.onrender.com/swagger](https://isaac-api-tarefas.onrender.com/swagger)**

* **Aplicação Front-End "Live" (Vue.js):**
    **[https://vue-consumindo-api.vercel.app](https://vue-consumindo-api.vercel.app)**

---

Este é um projeto de portfólio que demonstra a construção de uma API RESTful completa usando C# e .NET 7. O objetivo desta API é fornecer os endpoints de **CRUD** (Create, Read, Update, Delete) para uma aplicação de Lista de Tarefas.

* **Repositório do Front-End (Vue.js):** [github.com/isaacmmendes312/vue-consumindo-api](https://github.com/isaacmmendes312/vue-consumindo-api)

## Funcionalidades (Endpoints)

* `GET /api/tarefas`: Retorna todas as tarefas.
* `POST /api/tarefas`: Cria uma nova tarefa.
* `PUT /api/tarefas/{id}`: Atualiza uma tarefa (alterna o status "concluída").
* `DELETE /api/tarefas/{id}`: Deleta uma tarefa.

## Tecnologias Utilizadas

* **.NET 7** (Minimal APIs)
* **C#**
* **Entity Framework Core 7**
* **PostgreSQL** (Banco de dados hospedado no Render)
* **Docker** (Usado para o deploy no Render)
* **Render** para deploy e hosting da API.

## Como Rodar Localmente

### 1. Pré-requisitos

* [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
* A ferramenta de linha de comando do EF Core (Instale com: `dotnet tool install --global dotnet-ef --version 7.0.10`)
* Um banco de dados PostgreSQL rodando localmente (ou um container Docker).

### 2. Configuração

1.  Clone o repositório.
2.  Renomeie o arquivo `appsettings.Development.json` (que está no `.gitignore`) e configure sua string de conexão local do PostgreSQL.
3.  No terminal, rode `dotnet ef database update` para aplicar as migrações no seu banco local.
4.  Rode `dotnet run` para iniciar a API.