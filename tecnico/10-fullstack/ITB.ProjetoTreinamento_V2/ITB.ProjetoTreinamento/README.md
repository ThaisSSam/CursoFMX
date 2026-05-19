# ITB — Projeto de Treinamento (Backend .NET)

Projeto base para o curso de backend, espelhando a **Clean Architecture** e o **CQRS** do sistema ITB de produção (`C:\FMX\ITB\backend\ITB`).

O boilerplate inclui infraestrutura (camadas, bus, EF Core, PostgreSQL, Swagger). O **Módulo 1** orienta a implementação completa de **login com JWT** pelos alunos.

## Estrutura da solution

| Projeto | Camada |
|---------|--------|
| `Treinamento.API` | Apresentação |
| `Treinamento.Domain` | Domínio |
| `Treinamento.Domain.Core` | Primitivos compartilhados |
| `Treinamento.CrossCutting` | Cross-cutting |
| `Treinamento.Infrastructure` | Persistência |
| `Treinamento.ServiceBus` | Barramento in-memory |
| `Treinamento.IoC` | Injeção de dependência |
| `Treinamento.UnitTests` | Testes |

## Pré-requisitos (.NET 8)

O curso usa **.NET 8 (LTS)**. O arquivo [`global.json`](global.json) na raiz fixa o SDK na faixa 8.0.x.

| Componente | Versão |
|------------|--------|
| SDK | .NET **8.0** (`dotnet --version` → `8.0.xxx`) |
| Runtime | `Microsoft.AspNetCore.App 8.x` |
| EF Core / dotnet-ef | 9.0.6 (mesmo stack do ITB em net8.0) |
| PostgreSQL | Docker 16 (via `docker-compose.yml`) |

Guia completo: **[docs/ambiente-dotnet8.md](docs/ambiente-dotnet8.md)**

### Verificar instalação

```powershell
cd C:\FMX\ITB\ITB.ProjetoTreinamento
.\scripts\verificar-ambiente.ps1
```

### Ferramentas locais do repositório

```powershell
dotnet tool restore
```

## Como executar

### 1. Subir o PostgreSQL

```powershell
cd C:\FMX\ITB\ITB.ProjetoTreinamento
docker compose up -d
```

### 2. Restaurar e compilar

```powershell
dotnet restore Treinamento.sln
dotnet build Treinamento.sln
```

### 3. Aplicar migrations no banco

Com o PostgreSQL no ar, aplique o schema (veja [Migrations e banco de dados](#migrations-e-banco-de-dados)):

```powershell
dotnet tool restore
dotnet ef database update `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext
```

### 4. Rodar a API

```powershell
dotnet run --project src\Treinamento.API\Treinamento.API.csproj
```

- Swagger: http://localhost:5000/swagger  
- Health: `GET http://localhost:5000/api/v1/health/ping`

### 5. Testes

```powershell
dotnet test Treinamento.sln
```

## Modelo base — `Usuario` (Módulo 1)

O boilerplate já traz o esqueleto de persistência para o agregado de login:

| Artefato | Caminho |
|----------|---------|
| Entidade (só `Id`) | `src/Treinamento.Domain/Aggregates/Usuarios/Usuario.cs` |
| Mapping EF | `src/Treinamento.Infrastructure/Mappings/UsuarioConfiguration.cs` |
| Migration EF | `src/Treinamento.Infrastructure/Migrations/*_Sprint01_02_CriarTabelaUsuario*` |
| Script SQL | `sql/migrations/Sprint01/Sprint01_02_CriarTabelaUsuario_UP.sql` |

Tabela: `treinamento.tb_usuarios` (`id` identity, PK `pk_tb_usuarios_id`). Os alunos estendem a entidade e o mapping com e-mail, senha, situação, etc.

## Migrations e banco de dados

O projeto usa **duas formas** de evoluir o schema, alinhadas ao ITB de produção:

| Abordagem | Quando usar | Onde ficam os artefatos |
|-----------|-------------|-------------------------|
| **EF Core migrations** | Desenvolvimento local (alunos e instrutores no dia a dia) | `src/Treinamento.Infrastructure/Migrations/` |
| **Scripts SQL versionados** | Deploy / ambiente do cliente / DBA | `sql/migrations/SprintXX/` |

O contexto usado pelos comandos `dotnet ef` é **`TreinamentoContext`** (alias do `TreinamentoWriteContext`). A connection string vem de `src/Treinamento.API/appsettings.Development.json` (`WriteConnection`).

### Pré-requisitos

1. PostgreSQL rodando (`docker compose up -d`).
2. Ferramenta `dotnet-ef` instalada no repositório:

```powershell
cd C:\FMX\ITB\ITB.ProjetoTreinamento
dotnet tool restore
```

3. Solution compilada (`dotnet build Treinamento.sln`).

Na **primeira** subida do container, `scripts/init-db.sql` cria o schema `treinamento`. As migrations EF (ou os scripts `Sprint01_*`) criam as tabelas em cima disso.

### Aplicar migrations pendentes (uso mais comum)

Execute na raiz do repositório, com o banco acessível em `localhost:5432`:

```powershell
dotnet ef database update `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext
```

Isso aplica todas as migrations ainda não registradas na tabela `__EFMigrationsHistory` (por exemplo `Sprint01_02_CriarTabelaUsuario` → tabela `treinamento.tb_usuarios`).

**Conferir no PostgreSQL:**

```powershell
docker exec -it treinamento-postgres psql -U treinamento -d treinamento -c "\dt treinamento.*"
docker exec -it treinamento-postgres psql -U treinamento -d treinamento -c "SELECT * FROM \"__EFMigrationsHistory\" ORDER BY \"MigrationId\";"
```

### Criar uma nova migration (após alterar entidade/mapping)

1. Altere a entidade em `Treinamento.Domain` e o mapping em `Treinamento.Infrastructure/Mappings/`.
2. Gere a migration:

```powershell
dotnet ef migrations add Sprint01_03_DescricaoCurta `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext `
  --output-dir Migrations
```

3. Revise os arquivos gerados em `Migrations/`.
4. Aplique com `dotnet ef database update` (comando acima).

**Desfazer a última migration** (ainda não aplicada no banco compartilhado):

```powershell
dotnet ef migrations remove `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext
```

**Listar migrations:**

```powershell
dotnet ef migrations list `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext
```

### Scripts SQL manuais (sem EF)

Para ambientes em que só scripts SQL são executados (ex.: pipeline do cliente):

1. Rode na ordem os arquivos `*_UP.sql` da sprint em `sql/migrations/Sprint01/` (após `Sprint01_01_CriarSchemaTreinamento_UP.sql`, se o schema ainda não existir).
2. Para rollback, use os pares `*_DOWN.sql` na ordem inversa.

Exemplo Sprint 01 — usuário base:

- `sql/migrations/Sprint01/Sprint01_02_CriarTabelaUsuario_UP.sql`

Mantenha o script SQL **equivalente** à migration EF quando ambos existirem no mesmo sprint (padrão ITB).

### Gerar script SQL a partir das migrations EF (opcional)

Útil para revisar o DDL ou entregar ao DBA:

```powershell
dotnet ef migrations script `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext `
  --output sql\migrations\gerado\ultimo.sql
```

Script de uma migration específica até a mais recente (use o id completo de `dotnet ef migrations list`):

```powershell
dotnet ef migrations script 20260519032838_Sprint01_02_CriarTabelaUsuario `
  --project src\Treinamento.Infrastructure\Treinamento.Infrastructure.csproj `
  --startup-project src\Treinamento.API\Treinamento.API.csproj `
  --context TreinamentoContext
```

Sem parâmetros de migration, gera o script completo do banco vazio até o snapshot atual.

### Connection string

Padrão local (já em `appsettings.Development.json`):

```text
Host=localhost;Port=5432;Database=treinamento;Username=treinamento;Password=treinamento
```

Para sobrescrever sem editar o arquivo, use variável de ambiente (veja `.env.local.example`):

```powershell
$env:ConnectionStrings__WriteConnection = "Host=localhost;Port=5432;Database=treinamento;Username=treinamento;Password=treinamento"
```

### Problemas comuns

| Sintoma | O que verificar |
|---------|-----------------|
| `Could not connect` / timeout | `docker compose ps` — container `treinamento-postgres` healthy? Porta 5432 livre? |
| `dotnet ef` não encontrado | `dotnet tool restore` na raiz do repo |
| Erro ao criar migration (`ErroValidacaoDominio`) | Tipos de validação de domínio não são entidades — já ignorados em `TreinamentoModelBuilder` |
| Banco “sujo” / quer recomeçar | `docker compose down -v` (apaga volume) e suba de novo; depois `database update` |
| Migration já aplicada manualmente via SQL | Registre em `__EFMigrationsHistory` ou alinhe com `dotnet ef migrations script` antes de rodar `update` |
