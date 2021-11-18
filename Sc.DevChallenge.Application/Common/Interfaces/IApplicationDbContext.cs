using Microsoft.EntityFrameworkCore;
using Sc.DevChallenge.Domain.Entities;

namespace Sc.DevChallenge.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PriceEntity> Prices { get; set; }
    }
}