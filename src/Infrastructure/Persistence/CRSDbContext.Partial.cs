using Microsoft.EntityFrameworkCore;
using TeraLinkaCareApi.Core.Domain.Entities;

namespace TeraLinkaCareApi.Infrastructure.Persistence;

public partial class CRSDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Isolation、OP 在部分院區資料庫為字串型別、部分為 int，且系統未使用，不對應以避免 InvalidCastException
        modelBuilder.Entity<A_PtEncounter>(entity =>
        {
            entity.Ignore(e => e.Isolation);
            entity.Ignore(e => e.OP);
        });
    }
}
