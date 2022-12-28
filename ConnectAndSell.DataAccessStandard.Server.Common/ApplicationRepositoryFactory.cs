//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Text;
//using MDRX.DataAccess.DataContract.Common;

//namespace MDRX.DataAccessCore.Server.Common
//{
//    public static class ApplicationRepositoryFactory
//    {
        
//        public static ApplicationRepositoryBase GetApplicationRepository<TK>(Func<string> getCurrentUserFunc,
//            SecureConnectionString securedConnectionString,
//            Assembly dataContractAssembly, string embeddedResourceFullName)
//            where TK : RepositoryProviderBase, IRepositoryProvider
//        {
            
//            var resourceText = Utility.ReadResourceAsTextFromAssembly(dataContractAssembly, embeddedResourceFullName);
//            var context = ORMContext.GetORMContext(new Tuple<string, Assembly>(resourceText, dataContractAssembly));
//            return ApplicationRepository<TK>.GetApplicationRepository(
//                new Tuple<Func<string>, SecureConnectionString, ORMContext>(getCurrentUserFunc, securedConnectionString,
//                    context));
//        }
//    }
//}
