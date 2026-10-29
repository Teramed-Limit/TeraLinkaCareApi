> **此 repo 已封存（2026-10-29）。** 程式碼已連同完整歷史併入 monorepo [Teramed-Limit/TeraLinkaWoundCare](https://github.com/Teramed-Limit/TeraLinkaWoundCare) 的 `TeraLinkaCareApi/` 目錄，請改至該處開發。

# Getting Started with EF

## 安裝工具

#### dotnet ef 可以安裝為全域或本機工具。 大部分開發人員偏好使用下列命令安裝 dotnet ef 為全域工具：

`dotnet tool install --global dotnet-ef`

#### 使用下列命令更新工具：

`dotnet tool update --global dotnet-ef`

#### 確認安裝

`dotnet ef`

## 建立 Table

### dotnet ef dbcontext scaffold

#### 為 資料庫產生 和 實體類型的程式碼 DbContext 。 為了讓此命令產生實體類型，資料庫資料表必須有主鍵。

`dotnet ef dbcontext scaffold "Server=localhost;Database=CRSCoreDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context CRSCoreDBContext --context-dir src/Infrastructure/Persistence --output-dir src/Core/Domain/CRSCoreDB --use-database-names --force`

`dotnet ef dbcontext scaffold "Server=localhost;Database=CRSPatientDataDB1;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context CRSPatientDataDbContext --context-dir src/Infrastructure/Persistence --output-dir src/Core/Domain/CRSPatientDataDB --use-database-names --force`

`dotnet ef dbcontext scaffold "Server=DESKTOP-DSL3QH8\MSSQLSERVER01;Database=CRS;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context CRSDbContext --context-dir src/Infrastructure/Persistence --output-dir src/Core/Domain/Entities --use-database-names --force`

## 參考

See the [Link](https://docs.microsoft.com/zh-tw/ef/core/cli/dotnet) for more information.
See the [Entity framework tutorial](https://www.entityframeworktutorial.net/code-first/setup-entity-framework-code-first-environment.aspx) for more information.
