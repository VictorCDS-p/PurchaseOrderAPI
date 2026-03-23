```markdown
# PurchaseOrderAPI

API em C# (.NET 6+) para gerenciamento do fluxo de Pedidos de Compras.

## Estrutura do Projeto

```

PurchaseOrderAPI/
│
├── Controllers/
│   └── PurchaseOrdersController.cs
│
├── Application/
│   ├── DTOs/
│   │   ├── CreateOrderDto.cs
│   │   └── OrderResponseDto.cs
│   │
│   └── Services/
│       └── PurchaseOrderService.cs
│
├── Domain/
│   ├── Entities/
│   │   ├── PurchaseOrder.cs
│   │   ├── PurchaseOrderItem.cs
│   │   ├── Approval.cs
│   │   ├── User.cs
│   │   └── OrderHistory.cs
│   │
│   └── Enums/
│       ├── OrderStatus.cs
│       ├── ApprovalStatus.cs
│       └── UserRole.cs
│
├── Infrastructure/
│   ├── Data/
│   │   └── AppDbContext.cs
│   │
│   └── Repositories/
│       └── (a implementar)
│
├── Configurations/
│   └── DependencyInjection.cs
│
├── Migrations/
│
├── Program.cs
├── PurchaseOrderAPI.csproj
├── appsettings.json
├── appsettings.Development.json
├── PurchaseOrderAPI.http (opcional para testes)
└── README.md

````

## Funcionalidades

- Criar pedidos de compra com múltiplos itens
- Aprovação sequencial por alçadas (Suprimentos → Gestor → Diretor)
- Solicitar revisão de pedidos
- Concluir pedidos
- Histórico de ações do pedido

## Tecnologias

- .NET 6+
- Entity Framework Core
- SQL Server / Azure SQL

## Configuração

1. Atualize `appsettings.json` com sua connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=VICTOR\\SQLEXPRESS;Database=PurchaseDB;Trusted_Connection=True;TrustServerCertificate=True"
}
````

2. Criar e aplicar migrations:

```bash
dotnet ef database update
```

3. Rodar a API:

```bash
dotnet run
```
