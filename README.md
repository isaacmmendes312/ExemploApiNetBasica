# API de Lista de Tarefas (CRUD em .NET)

Este é um projeto de portfólio que demonstra a construção de uma API RESTful completa usando C# e .NET 7. O objetivo desta API é fornecer os endpoints de **CRUD** (Create, Read, Update, Delete) para uma aplicação de Lista de Tarefas.

Esta API foi construída para ser consumida pelo seguinte projeto front-end:
**[https://github.com/isaacmmendes312/vue-consumindo-api](https://github.com/isaacmmendes312/vue-consumindo-api)**

---

## Funcionalidades (Endpoints)

* **`GET /api/tarefas`**: Retorna uma lista de todas as tarefas.
* **`POST /api/tarefas`**: Cria uma nova tarefa.
* **`PUT /api/tarefas/{id}`**: Atualiza uma tarefa existente (especificamente, alterna seu status de "concluída").
* **`DELETE /api/tarefas/{id}`**: Deleta uma tarefa específica.

## Tecnologias Utilizadas

* **.NET 7** (utilizando Minimal APIs)
* **C#**
* **Entity Framework Core 7** (EF Core)
* **SQLite**: Banco de dados leve e baseado em arquivo, ideal para desenvolvimento e prototipagem.
* **Swagger/OpenAPI**: Para documentação e teste interativo dos endpoints.

## Como Rodar Localmente

### 1. Pré-requisitos

* [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) (ou a versão 7.x.x que você tiver instalada)
* A ferramenta de linha de comando do EF Core (Instale com: `dotnet tool install --global dotnet-ef --version 7.0.10`)

### 2. Rodando a API

```bash
# 1. Clone o repositório
git clone [https://github.com/isaacmmendes312/ExemploApiNetBasica.git](https://github.com/isaacmmendes312/ExemploApiNetBasica.git)

# 2. Entre na pasta do projeto
cd ExemploApiNetBasica

# 3. Restaure os pacotes (dependências)
dotnet restore

# 4. CRIE O BANCO DE DADOS (Passo Crucial!)
# O Entity Framework Core usará o arquivo de "migration" no projeto
# para criar o banco de dados 'tarefas.db' automaticamente.
dotnet ef database update

# 5. Inicie a API
dotnet run