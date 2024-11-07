using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository(
    ILogger<C8SRepository> logger,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IMapper mapper)
{
}