# ManadaIA - Sistema de Gestão de Rebanho Bovino

API REST desenvolvida com .NET 8, Clean Architecture e Supabase para gerenciamento completo do seu rebanho.

## 🏗️ Arquitetura

Este projeto segue os princípios de **Clean Architecture** (Arquitetura Limpa), organizado em 4 camadas:

```
src/
├── ManadaIA.Domain/              # Camada de Domínio (sem dependências externas)
│   ├── Entities/                 # Entidades: Animal, Propriedade, Lote, Pesagem, Vacina
│   ├── Interfaces/               # Contratos de repositório
│   └── Exceptions/               # Exceções de domínio
│
├── ManadaIA.Application/         # Casos de Uso, DTOs, Validações
│   ├── Features/Animais/
│   │   ├── Commands/             # CreateAnimalCommand, etc.
│   │   └── Queries/              # GetAnimalByIdQuery, etc.
│   ├── DTOs/                     # Data Transfer Objects
│   ├── Validators/               # FluentValidation
│   └── Behaviors/                # Pipeline Behaviors (Validation)
│
├── ManadaIA.Infrastructure/      # Supabase, Repositórios, Serviços Externos
│   ├── Supabase/                 # Client Factory, Settings
│   ├── Models/                   # Models do Postgrest
│   └── Repositories/             # Implementações dos repositórios
│
└── ManadaIA.API/                 # ASP.NET Web API
    ├── Controllers/              # AnimaisController, etc.
    ├── Middleware/               # ExceptionMiddleware
    └── Program.cs                # Configuração da aplicação
```

## 🚀 Tecnologias

- **.NET 8** - Framework principal
- **Supabase** - Backend-as-a-Service (PostgreSQL + Auth + Storage)
- **MediatR** - CQRS e Mediator Pattern
- **FluentValidation** - Validação de comandos/queries
- **Serilog** - Logging estruturado
- **JWT Bearer** - Autenticação
- **Swagger/OpenAPI** - Documentação da API

## 📦 Pacotes NuGet

### ManadaIA.Application
- `MediatR` - Padrão Mediator e CQRS
- `FluentValidation` - Validação de entrada
- `FluentValidation.DependencyInjectionExtensions` - DI

### ManadaIA.Infrastructure
- `supabase-csharp` - Cliente oficial do Supabase
- `Postgrest` - ORM para PostgreSQL

### ManadaIA.API
- `Serilog.AspNetCore` - Logging
- `Microsoft.AspNetCore.Authentication.JwtBearer` - Autenticação JWT

## 🗄️ Banco de Dados

O esquema do banco está em `database/schema.sql`. Execute no SQL Editor do Supabase.

### Tabelas Principais
- `propriedades` - Fazendas/propriedades rurais
- `lotes` - Agrupamento de animais
- `animais` - Rebanho bovino
- `pesagens` - Histórico de peso dos animais
- `vacinas` - Registro de vacinação

### Row Level Security (RLS)
Todas as tabelas possuem RLS habilitado:
- Usuários autenticados veem apenas dados de suas propriedades
- `service_role` (backend) tem acesso total

## ⚙️ Configuração

### 1. Supabase

Crie um projeto em [supabase.com](https://supabase.com) e execute o script `database/schema.sql`.

### 2. appsettings.json

Atualize as credenciais do Supabase em `src/ManadaIA.API/appsettings.json`:

```json
{
  "Supabase": {
    "Url": "https://seu-projeto.supabase.co",
    "AnonKey": "sua-anon-key",
    "ServiceRoleKey": "sua-service-role-key",
    "JwtSecret": "seu-jwt-secret"
  }
}
```

### 3. Executar

```bash
cd src/ManadaIA.API
dotnet run
```

A API estará disponível em `https://localhost:7xxx` com Swagger na raiz.

## 🔐 Autenticação

Todos os endpoints (exceto autenticação) requerem JWT Bearer token do Supabase.

```http
Authorization: Bearer <token-do-supabase>
```

## 📋 Endpoints Principais

### Animais

- `GET /api/animais/propriedade/{id}` - Lista animais de uma propriedade
- `GET /api/animais/{id}` - Obtém um animal por ID
- `POST /api/animais` - Cadastra novo animal
- `DELETE /api/animais/{id}` - Remove animal

## 🏛️ Princípios Aplicados

- **Clean Architecture** - Separação de responsabilidades em camadas
- **CQRS** - Commands e Queries separados
- **DDD** - Entidades ricas com lógica de negócio
- **SOLID** - Princípios de design orientado a objetos
- **Repository Pattern** - Abstração de acesso a dados
- **Dependency Injection** - Inversão de controle

## 📝 Próximos Passos

- [ ] Implementar comandos de Update e Delete
- [ ] Adicionar controllers para Propriedades, Lotes, Pesagens e Vacinas
- [ ] Implementar paginação
- [ ] Adicionar filtros avançados
- [ ] Testes unitários e de integração
- [ ] Docker e CI/CD

## 📄 Licença

Este projeto é de uso interno.
