using Microsoft.EntityFrameworkCore;
using OpenCqrs.EventSourcing.Store.EntityFrameworkCore.Identity;

namespace EventShop.Web.Data;

public sealed class ApplicationDbContext(
    DbContextOptions<IdentityDomainDbContext> options,
    TimeProvider timeProvider,
    IHttpContextAccessor httpContextAccessor)
    : IdentityDomainDbContext(options, timeProvider, httpContextAccessor);