```markdown
## Estrutura do Projeto PurchaseOrderAPI

```
```
PurchaseOrderAPI/
│
├── Controllers/
│   └── PurchaseOrdersController.cs       # Endpoints da API
│
├── Application/
│   ├── DTOs/
│   │   ├── CreateOrderDto.cs             # DTO para criar pedido (vazio / futuro)
│   │   └── OrderResponseDto.cs           # DTO de resposta (vazio / futuro)
│   │
│   └── Services/
│       └── PurchaseOrderService.cs       # Lógica de criação, aprovação e fluxo
│
├── Domain/
│   ├── Entities/
│   │   ├── PurchaseOrder.cs              # Entidade Pedido
│   │   ├── PurchaseOrderItem.cs          # Entidade Item do Pedido
│   │   ├── Approval.cs                    # Entidade Aprovação
│   │   ├── User.cs                        # Entidade Usuário
│   │   └── OrderHistory.cs               # Histórico de ações
│   │
│   └── Enums/
│       ├── OrderStatus.cs                 # Status do Pedido
│       ├── ApprovalStatus.cs              # Status da Aprovação
│       └── UserRole.cs                    # Papel do Usuário
│
├── Infrastructure/
│   ├── Data/
│   │   └── AppDbContext.cs               # DbContext EF Core
│   │
│   └── Repositories/                      # Futuras implementações de repositório
│
├── Configurations/
│   └── DependencyInjection.cs             # Configurações de DI (vazio / futuro)
│
├── Migrations/                            # Migrations do EF Core
│
├── Program.cs                             # Inicialização da aplicação
├── PurchaseOrderAPI.csproj                # Projeto .NET
├── appsettings.json                        # Configurações gerais
├── appsettings.Development.json            # Configurações dev
├── PurchaseOrderAPI.http (opcional)       # Testes de requisição (Postman/Insomnia)
└── README.md                               # Documentação do projeto

```

