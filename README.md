# PurchaseOrderAPI

API REST desenvolvida em **C# (.NET 10)** para simular o processo de pedidos de compra com fluxo de aprovação hierárquico, conforme especificação do desafio técnico.

---

## Objetivo

Implementar uma API que represente o ciclo completo de um pedido de compra dentro de uma organização, contemplando:

* Criação de pedidos
* Cálculo automático de valores
* Fluxo de aprovação por alçada
* Solicitação de revisão
* Cancelamento de pedidos
* Registro completo do histórico de ações

---

## Tecnologias Utilizadas

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Swagger (Swashbuckle)

---

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter instalado:

* .NET SDK 10 ou superior
* SQL Server (Express ou LocalDB)
* Entity Framework CLI

### Instalação do Entity Framework CLI

```bash
dotnet tool install --global dotnet-ef
```

---

## Estrutura do Projeto

```
PurchaseOrderAPI/
│
├── Controllers/
│   └── PurchaseOrdersController.cs
│   └── UsersController.cs
│
├── Application/
│   ├── DTOs/
│   └── Services/
│
├── Domain/
│   ├── Entities/
│   └── Enums/
│
├── Infrastructure/
│   ├── Data/
│   └── Repositories/
│
├── Migrations/
│
├── postman/
│   └── PurchaseOrderAPI.postman_collection.json
│
├── docs/
│   └── diagrams/
│       ├── class-diagram.png
│       ├── activity-diagram.png
│       └── database-diagram.png
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── README.md
```

---

## Diagramas

### Diagrama de Classes

![Class Diagram](docs/diagrams/class-diagram.png)

---

### Diagrama de Atividades

![Activity Diagram](docs/diagrams/activity-diagram.png)

---

### Diagrama de Banco de Dados

![Database Diagram](docs/diagrams/database-diagram.png)

---

## Regras de Negócio Implementadas

* O pedido deve conter pelo menos um item
* O valor total é calculado automaticamente com base nos itens

### Fluxo de aprovação por alçada:

* Até R$ 100: aprovação pela área de Supply
* De R$ 101 até R$ 1000: aprovação por Supply e Manager
* Acima de R$ 1000: aprovação por Supply, Manager e Director

### Regras adicionais:

* O processo de aprovação é sequencial
* Qualquer aprovador pode solicitar revisão, reiniciando o fluxo
* Todas as ações são registradas no histórico do pedido
* O pedido só é concluído após todas as aprovações exigidas
* O pedido pode ser cancelado em qualquer etapa, exceto após conclusão

---

## Configuração do Banco de Dados

A aplicação utiliza **arquivos de configuração por ambiente**.

Por padrão, o .NET carrega automaticamente:

* `appsettings.json` (configuração base)
* `appsettings.Development.json` (configuração específica para desenvolvimento, sobrescreve a base)

### Importante

O arquivo `appsettings.Development.json` **não é criado automaticamente em todos os projetos**.
Caso ele não exista, é necessário criá-lo manualmente na raiz do projeto.

### Passo a passo

1. Crie o arquivo:

```
appsettings.Development.json
```

2. Adicione a configuração de conexão com o banco:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR\\INSTANCIA;Database=PurchaseDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### Parâmetros

| Campo        | Descrição                                     |
| ------------ | --------------------------------------------- |
| SEU_SERVIDOR | Nome do servidor (ex: localhost, DESKTOP-123) |
| INSTANCIA    | Nome da instância (ex: SQLEXPRESS)            |
| PurchaseDB   | Nome do banco de dados                        |

### Exemplo

```json
"Server=localhost\\SQLEXPRESS;Database=PurchaseDB;Trusted_Connection=True;TrustServerCertificate=True"
```

---

## Execução do Projeto

### 1. Restaurar dependências

```bash
dotnet restore
```

### 2. Criar migration

```bash
dotnet ef migrations add InitialCreate
```

### 3. Aplicar migrations no banco

```bash
dotnet ef database update
```

### 4. Executar a aplicação

```bash
dotnet run
```

---

## Reset do Banco de Dados (Opcional)

```bash
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Observação:** Este processo remove todos os dados e deve ser utilizado apenas em ambiente de desenvolvimento.

---

## Acesso à API

Swagger disponível em:

```
http://localhost:5139/swagger
```

---

## Testes

Importe a collection disponível em:

```
postman/PurchaseOrderAPI.postman_collection.json
```

Fluxo sugerido de testes:

1. Criar usuários
2. Criar pedido
3. Executar aprovações sequenciais (Supply → Manager → Director)
4. Consultar histórico
5. Testar cenários de revisão e cancelamento

---

## Problemas Comuns

### Erro: `dotnet ef` não encontrado

Instale a ferramenta global:

```bash
dotnet tool install --global dotnet-ef
```

---

### Erro: Não é possível abrir o banco de dados "PurchaseDB"

Isso ocorre quando o banco ainda não foi criado.

Solução:

```bash
dotnet ef database update
```

---

### Erro de SSL ao conectar no SQL Server

Caso ocorra erro relacionado a certificado, adicione na connection string:

```json
TrustServerCertificate=True
```

---

## Histórico do Pedido

O sistema registra automaticamente as seguintes ações:

* Criação
* Aprovação
* Solicitação de revisão
* Cancelamento
* Conclusão

Cada registro contém:

* Usuário responsável
* Data e hora
* Tipo de ação executada

---

## Status do Projeto

* CRUD de pedidos implementado
* Fluxo completo de aprovação
* Suporte a revisão e cancelamento
* Histórico rastreável
* Collection de testes disponível

---

## Autor

Desenvolvido como parte de um desafio técnico para uma vaga de desenvolvimento back-end.

---
