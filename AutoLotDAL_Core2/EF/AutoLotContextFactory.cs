using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AutolotDAL_Core2.EF
{
    public class AutoLotContextFactory : IDesignTimeDbContextFactory<AutoLotContext>
    {
        public AutoLotContext CreateDbContext(String[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AutoLotContext>();

            var connectionstring = @"server=(LocalDb)\MSSQLLocalDB;database=AutoLotCore2;integrated security=True; MultipleActiveResultSets=True;App=EntityFramework;";
            optionsBuilder.UseSqlServer(
                    connectionstring,
                    options => options.EnableRetryOnFailure())
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            return new AutoLotContext(optionsBuilder.Options);
        }
    }
}