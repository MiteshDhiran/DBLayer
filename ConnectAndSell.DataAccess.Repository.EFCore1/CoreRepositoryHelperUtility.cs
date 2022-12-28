using System;
using System.Collections.Generic;
using System.Linq;
using ConnectAndSell.DataAccessStandard.Server.Common;
using Microsoft.EntityFrameworkCore;

namespace ConnectAndSell.DataAccess.Repository.EFCore1
{
    internal static class CoreRepositoryHelperUtility
    {
        public static IQueryable<T> IncludeChildRecords<T>(this IQueryable<T> dbQuery, ORMModelMetaInfo model) where T : class
        {
            var allChildPaths = model.GetAllChildPaths(typeof(T));
            foreach (var item in allChildPaths)
            {
                dbQuery = dbQuery.Include(item);
            }

            return dbQuery;
        }

        public static IQueryable<T> IncludeSelectedChildRecords<T>(this IQueryable<T> dbQuery, ORMModelMetaInfo model, List<string> childTableNames)
            where T : class
        {
            var allChildPaths = model.GetFilteredChildPaths(new Tuple<Type, List<string>>(typeof(T),childTableNames));
            foreach (var item in allChildPaths)
            {
                dbQuery = dbQuery.Include(item);
            }

            return dbQuery;
        }
    }
}
