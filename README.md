# AppetitChef.BackEnd

API REST para sistema de restaurante e bar, desenvolvida em **.NET 9.0** com arquitetura **DDD + Clean Architecture + CQRS**.

---

## Stack Tecnológica

| Camada | Tecnologia |
|--------|-----------|
| Framework | ASP.NET Core 9.0 |
| ORM | Entity Framework Core 9 + Pomelo (MySQL) |
| CQRS | MediatR 12 |
| Validação | FluentValidation 11 |
| Autenticação | JWT Bearer (HS256) |
| Segurança | BCrypt.Net |
| Logs | Serilog (Console + File) |
| Docs | Swagger / OpenAPI 3 |
| Testes | xUnit + Moq + FluentAssertions |

---

## Estrutura do Projeto

```
AppetitChef.BackEnd/
├── src/
│   ├── AppetitChef.Domain/              # Entidades, Enums, Interfaces, Exceções
│   │   ├── Entities/                    # Todas as entidades do domínio
│   │   ├── Enums/                       # Enumeradores de domínio
│   │   ├── Exceptions/                  # DomainException, NotFoundException, BusinessRuleException
│   │   └── Interfaces/                  # IRepository<T>, IUnitOfWork, repositórios específicos
│   │
│   ├── AppetitChef.Application/         # Casos de uso (CQRS)
│   │   ├── Common/
│   │   │   ├── Behaviors/               # Pipeline: Logging, Performance, Validation
│   │   │   ├── Interfaces/              # IJwtService, IPasswordService, ICurrentUserService
│   │   │   └── Models/                  # Result<T>, PagedResult<T>
│   │   └── Features/
│   │       ├── Auth/Commands/           # LoginCommand
│   │       ├── Pedidos/Commands/        # AbrirPedido, AdicionarItem, Fechar, Cancelar
│   │       ├── Pedidos/Queries/         # ListarAbertos, ObterPedido
│   │       ├── Produtos/Commands/       # CriarProduto
│   │       ├── Produtos/Queries/        # ListarProdutos
│   │       ├── Mesas/Queries/           # ListarMesas
│   │       ├── Clientes/Commands/       # CriarCliente
│   │       ├── Reservas/Commands/       # CriarReserva
│   │       └── Estoque/Commands/        # RegistrarMovimento
│   │
│   ├── AppetitChef.Infrastructure/      # EF Core, Repositórios, Segurança
│   │   ├── Persistence/
│   │   │   ├── AppetitChefDbContext.cs
│   │   │   ├── UnitOfWork.cs
│   │   │   ├── Configurations/          # Fluent API do EF Core
│   │   │   └── Repositories/            # Implementações dos repositórios
│   │   ├── Security/                    # JwtService, PasswordService (BCrypt)
│   │   └── Services/                    # CurrentUserService
│   │
│   └── AppetitChef.API/                 # Controllers, Middlewares, Program.cs
│       ├── Controllers/                 # Auth, Pedidos, Produtos, Mesas, Clientes, Reservas, Estoque
│       ├── Middlewares/                 # ExceptionMiddleware (tratamento global de erros)
│       └── Extensions/                  # Swagger, JWT Auth, CORS
│
└── tests/
    ├── AppetitChef.UnitTests/           # Testes unitários (Domain + Application)
    └── AppetitChef.IntegrationTests/    # Testes de integração com WebApplicationFactory
```

---

## Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- MySQL 8.0+
- (Opcional) Docker

---

## Configuração

### 1. Banco de dados

Execute o script SQL gerado anteriormente:
```bash
mysql -u root -p < restaurante_bar_db.sql
```

### 2. appsettings.json

Edite `src/AppetitChef.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Port=3306;Database=restaurante_bar;User=root;Password=SUA_SENHA;"
  },
  "Jwt": {
    "Secret": "AppetitChef_Super_Secret_Key_Minimo_32_Chars_2025!",
    "Issuer": "AppetitChef",
    "Audience": "AppetitChefClients",
    "ExpiracaoMinutos": "480"
  }
}
```

---

## Como Rodar

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations (se necessário)
dotnet ef migrations add InitialCreate -p src/AppetitChef.Infrastructure -s src/AppetitChef.API
dotnet ef database update -p src/AppetitChef.Infrastructure -s src/AppetitChef.API

# Rodar a API
cd src/AppetitChef.API
dotnet run

# API disponível em:
# http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

---

## Rodando os Testes

```bash
# Todos os testes
dotnet test

# Só unitários
dotnet test tests/AppetitChef.UnitTests

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

---

## Endpoints da API

### Auth
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/auth/login` | Login → retorna JWT |

### Pedidos
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v1/pedidos/abertos/{filialId}` | Lista pedidos em aberto |
| GET | `/api/v1/pedidos/{id}` | Obtém pedido com itens |
| POST | `/api/v1/pedidos` | Abre novo pedido |
| POST | `/api/v1/pedidos/{id}/itens` | Adiciona item ao pedido |
| PUT | `/api/v1/pedidos/{id}/fechar` | Fecha pedido (com/sem taxa) |
| PUT | `/api/v1/pedidos/{id}/cancelar` | Cancela pedido |

### Produtos
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v1/produtos` | Lista cardápio |
| POST | `/api/v1/produtos` | Cria produto (Admin/Gerente) |

### Mesas
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v1/mesas/{filialId}` | Lista mesas da filial |

### Clientes
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/clientes` | Cadastra cliente |

### Reservas
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/reservas` | Cria reserva |

### Estoque
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/estoque/movimentos` | Registra movimento |

### Health
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/health` | Health check |

---

## Autenticação

Todos os endpoints (exceto `/auth/login` e `/health`) exigem JWT no header:

```
Authorization: Bearer <seu-token>
```

**Perfis disponíveis:** `Admin`, `Gerente`, `Garcom`, `Cozinheiro`, `Barman`, `Caixa`

---

## Arquitetura CQRS + Pipeline

```
Request → ValidationBehavior → LoggingBehavior → PerformanceBehavior → Handler
                 ↑
         FluentValidation
         (rejeita antes do handler)
```

- **Commands**: Escrita (AbrirPedido, Fechar, Cancelar, etc.)
- **Queries**: Leitura (Listar, Obter)
- **Behaviors**: Transversais automáticos (log, validação, performance)

---

## Padrão Result

Todos os handlers retornam `Result<T>` — sem lançar exceções de negócio para a API:

```csharp
// Sucesso
return Result<int>.Ok(pedido.Id, "Pedido criado.");

// Falha
return Result<int>.Falha("Mesa não disponível.");
```

---

## Tratamento de Erros (Middleware Global)

| Exceção | HTTP Status |
|---------|-------------|
| `ValidationException` | 400 Bad Request |
| `NotFoundException` | 404 Not Found |
| `BusinessRuleException` | 422 Unprocessable Entity |
| `UnauthorizedAccessException` | 401 Unauthorized |
| Qualquer outra | 500 Internal Server Error |
