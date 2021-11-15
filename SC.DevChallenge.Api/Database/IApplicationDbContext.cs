using Microsoft.EntityFrameworkCore;
using SC.DevChallenge.Api.Database.Entities;

namespace SC.DevChallenge.Api.Database
{
    public interface IApplicationDbContext
    {
        DbSet<PriceEntity> Prices { get; set; }
    }
}