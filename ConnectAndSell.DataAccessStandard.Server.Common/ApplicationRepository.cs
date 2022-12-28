

using System;
using System.Reflection;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    /// ApplicationRepository Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ApplicationRepositoryBase" />
    public sealed class ApplicationRepository<T> : ApplicationRepositoryBase where T : RepositoryProviderBase, IRepositoryProvider
    {
        
        private ApplicationRepository(Func<string> getCurrentUserFunc, 
        SecureConnectionString securedConnectionString,
            ORMContext context,
            Func<object> compileModelFunc
            ):base(getCurrentUserFunc,securedConnectionString,context,typeof(T),compileModelFunc)
        {
            using (var repo = GetRepository(GetNewTraceInfo()))
            {
                ((RepositoryProviderBase)repo.RepositoryProvider).PreCompile();
            }
        }

        private static readonly Func<Tuple<Func<string>, SecureConnectionString, ORMContext,Func<object>>, IApplicationRepositoryBase>
        GetApplicationRepositoryInternal =
            DBUtility.Memonize<Tuple<Func<string>, SecureConnectionString, ORMContext,Func<object>>, IApplicationRepositoryBase>(
                (_, tuple) =>
                {
                    var retVal = new ApplicationRepository<T>(tuple.Item1, tuple.Item2, tuple.Item3,tuple.Item4);
                    return retVal;
                });

        /// <summary>
        /// Gets the application repository.
        /// </summary>
        /// <param name="getCurrentUserFunc">The get current user function.</param>
        /// <param name="securedConnectionString">The secured connection string.</param>
        /// <param name="dataContractAssembly">The data contract assembly.</param>
        /// <param name="embeddedResourceFullName">Full name of the embedded resource.</param>
        /// <returns></returns>
        public static IApplicationRepositoryBase GetApplicationRepository(
        Func<string> getCurrentUserFunc,
            SecureConnectionString securedConnectionString,
            Assembly dataContractAssembly, string embeddedResourceFullName)
        {
            var resourceText = Utility.ReadResourceAsTextFromAssembly(dataContractAssembly, embeddedResourceFullName);
            var context = ORMContext.GetORMContext(new Tuple<string, Assembly>(resourceText, dataContractAssembly));
            return GetApplicationRepositoryInternal(
                new Tuple<Func<string>, SecureConnectionString, ORMContext, Func<object>>(getCurrentUserFunc, securedConnectionString,
                    context, null));
            
        }

        public static IApplicationRepositoryBase GetApplicationRepositoryWithCompiledModel(
        Func<string> getCurrentUserFunc,
            SecureConnectionString securedConnectionString,
            Assembly dataContractAssembly
            ,string embeddedResourceFullName
            ,Func<object> compileModelFunc
            )
        {
            var resourceText = Utility.ReadResourceAsTextFromAssembly(dataContractAssembly,embeddedResourceFullName);
            var context = ORMContext.GetORMContext(new Tuple<string,Assembly>(resourceText,dataContractAssembly));
            return GetApplicationRepositoryInternal(
                new Tuple<Func<string>,SecureConnectionString,ORMContext,Func<object>>(getCurrentUserFunc,securedConnectionString,
                    context,compileModelFunc));

        }

        public static IApplicationRepositoryBase GetApplicationRepository(
            Func<string> getCurrentUserFunc,
            SecureConnectionString securedConnectionString,
            Assembly dataContractAssembly,
            string embeddedResourceFullName,
            Func<object> getCompiledModelFunc )
        {
            var resourceText = Utility.ReadResourceAsTextFromAssembly(dataContractAssembly,embeddedResourceFullName);
            var context = ORMContext.GetORMContext(new Tuple<string,Assembly>(resourceText,dataContractAssembly));
            return GetApplicationRepositoryInternal(
                new Tuple<Func<string>,SecureConnectionString,ORMContext,Func<object>>(getCurrentUserFunc,securedConnectionString,
                    context,getCompiledModelFunc));

        }

        /// <summary>
        /// Gets the application repository.
        /// </summary>
        /// <param name="getCurrentUserFunc">The get current user function.</param>
        /// <param name="securedConnectionString">The secured connection string.</param>
        /// <param name="ormContextFunc">The orm context function.</param>
        /// <returns></returns>
        public static IApplicationRepositoryBase GetApplicationRepository(
        Func<string> getCurrentUserFunc,
            SecureConnectionString securedConnectionString,
            Func<ORMContext> ormContextFunc )
        {
            var context = ormContextFunc();
            return GetApplicationRepositoryInternal(
                new Tuple<Func<string>, SecureConnectionString, ORMContext,Func<object>>(getCurrentUserFunc, securedConnectionString,
                    context,null));
            
        }
    }
    
    
}
/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */