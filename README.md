# 🚀 Route API

API para gerenciamento de rotas aéreas, incluindo criação, atualização, exclusão e busca da melhor rota entre dois pontos.

---

## 📦 Tecnologias Utilizadas

- [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- ASP.NET Core Web API
- Swagger / OpenAPI (Swashbuckle)
- Hexagonal Architecture
- xUnit (para testes)
- FluentValidation
- AutoMapper

---

## 📂 Estrutura do Projeto

- **Api**: Camada de apresentação (controllers, filtros, startup).
- **Application**: Casos de uso, interfaces e regras de negócio.
- **Domain**: Entidades e contratos de domínio.
- **Infrastructure**: Acesso a dados e configurações externas.

---
## 🛠️ Configurando o Projeto com PostgreSQL

Siga os passos abaixo para configurar e preparar o projeto:

---

### ✅ 1. Ter um banco de dados PostgreSQL existente

Antes de iniciar, certifique-se de que você possui um banco de dados PostgreSQL rodando e acessível com as credenciais corretas.

### 📝 2. Ajustar a *Connection String*

Abra o arquivo `appsettings.Development.json` e altere os valores de acesso ao banco na seção `ConnectionStrings`:

**JSON:**
  {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=NOME-BANCO;Username=USER-BANCO;Password=SENHA-DO-BANCO"
      }
  }

**Substitua:**
  
  * NOME-BANCO pelo nome do seu banco de dados
  * USER-BANCO pelo nome de usuário
  * SENHA-DO-BANCO pela senha correta

### 🧩 3. Aplicar Migrations (EF Core)

Utilize os comandos abaixo no terminal dentro da pasta raiz do projeto para gerar e aplicar as migrations:

* Gerar a migration inicial
  dotnet ef migrations add StartProject --project ./Infrastructure/Infrastructure.csproj --startup-project ./Api/Api.csproj

* Aplicar as migrations ao banco
  dotnet ef database update --project ./Infrastructure/Infrastructure.csproj --startup-project ./Api/Api.csproj

* (Opcional) Remover migration caso necessário
  dotnet ef migrations remove --force --project ./Infrastructure/Infrastructure.csproj --startup-project ./Api/Api.csproj

---
## 🔌 Endpoints Disponíveis

### 🔍 Buscar Melhor Rota

**GET** `/route/searchBestRoutes`

Busca a melhor rota com base na origem e destino.

**Query Parameters:**

| Parâmetro | Tipo   | Obrigatório | Descrição            |
|-----------|--------|-------------|----------------------|
| origin    | string | ✅ Sim      | Cidade de origem     |
| destination | string | ✅ Sim    | Cidade de destino    |

### ✈️ Considerando que as rotas abaixo já estão cadastradas:

| Origem | Destino | Valor |
|--------|---------|-------|
| GRU    | BRC     | 10    |
| BRC    | SCL     | 5     |
| GRU    | CDG     | 75    |
| GRU    | SCL     | 20    |
| GRU    | ORL     | 56    |
| ORL    | CDG     | 5     |
| SCL    | ORL     | 20    |

📦 **Exemplo:**  
Se consultarmos a rota abaixo, o resultado abaixo deve retornar:
`GET /route/searchBestRoutes?origin=GRU&destination=CDG`

**Resposta:**

    {
        "data": {
            "resultado": "GRU => BRC => SCL => ORL => CDG ao custo de R$ 40"
        }
    }
---

### ➕ Criar Nova Rota

    POST /route/create

Cria uma nova rota entre dois aeroportos.

**Body (JSON):**

    {
        "origin": "GRU",
        "destination": "BRC",
        "price": 10
    }

**Resposta:**

    {
        "data": {
            "id": 1,
            "origin": "GRU",
            "destination": "BRC",
            "price": 10
        }
    }
---

### ✏️ Atualizar Rota

    PUT /route/update

Atualiza os dados de uma rota existente.

**Body (JSON):**

    {
        "id": 1,
        "origin": "GRU",
        "destination": "BRC",
        "price": 16
    }

**Resposta:**

    {
        "data": {
            "id": 1,
            "origin": "GRU",
            "destination": "BRC",
            "price": 16
        }
    }
---

### 🗑️ Deletar Rota

    DELETE /route/delete?id=1

Remove uma rota com base no ID informado.

**Parâmetros de Query:**

| Nome | Tipo | Obrigatório | Descrição              |
|------|------|-------------|--------------------------|
| id   | int  | ✅ Sim      | ID da rota a ser deletada |

**Resposta:**

    {
        "data": {
            "id": 1,
            "origin": "GRU",
            "destination": "BRC",
            "price": 16
        }
    }

---

### 📋 Listar Todas as Rotas

    GET /route/all

Retorna todas as rotas cadastradas.

**Resposta:**

    {
        "data": [
            {
                "id": 1,
                "origin": "GRU",
                "destination": "BRC",
                "price": 10
            },
            {
                "id": 2,
                "origin": "BRC",
                "destination": "SCL",
                "price": 5
            }
        ]
    }
