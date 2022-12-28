using ConnectAndSell.DataAccessStandard.Server.Common;
using Microsoft.EntityFrameworkCore;

namespace ConnectAndSell.DataAccess.Repository.EFCore1
{
    public class GenericDbContext : DbContext
    {
        public ORMContext ORMContext { get; }

        public GenericDbContext(ORMContext ormContext, DbContextOptionsBuilder<GenericDbContext> builder) : base(options: builder.Options)
        {
            ORMContext = ormContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Core1ModelBuilder.CreateEFModel(ORMContext, modelBuilder);
        }
    }
}
