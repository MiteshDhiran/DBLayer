using ConnectAndSell.DataAccessStandard.Server.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectAndSell.EFCore6
{
    internal class GenericDbContext : DbContext
    {
        public ORMContext ORMContext { get; }

        public Func<object> PreCompiledModelFunc { get; }

        public GenericDbContext(ORMContext ormContext
        ,DbContextOptionsBuilder<GenericDbContext> builder
        ,Func<object> precompiledModelFunc
        ) : base(options: builder.Options)
        {
            ORMContext = ormContext;
            PreCompiledModelFunc = precompiledModelFunc;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (PreCompiledModelFunc != null)
            {
                var preCompiledModel = PreCompiledModelFunc();
                if (preCompiledModel == null) throw new ApplicationException($"Compiled Model return by CompiledModelFunc is null");
                if(preCompiledModel != null)
                {
                    if((preCompiledModel is IModel) == false)
                    {
                        throw new ApplicationException($"Compiled Model return by CompiledModelFunc is not of type IModel");
                    }
                    optionsBuilder.UseModel((IModel)preCompiledModel);
                }
            }
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Core6ModelBuilder.CreateEFModel(ORMContext,modelBuilder);
        }
    }
}
