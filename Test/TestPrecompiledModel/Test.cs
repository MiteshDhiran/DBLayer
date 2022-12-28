using System.Data.Common;
using DemoApp.DataContract;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using ConnectAndSell.DataAccessStandard.Server.Common;
using ConnectAndSell.EFCore6;
using CastApp.DataContract;

namespace TestPrecompiledModel
{
    public static class Test
    {
        public static readonly SecureConnectionString SFMAPPSecureConnectionString =
        new SecureConnectionString("Data Source=.;Initial Catalog=CDALDB;Integrated Security=True;TrustServerCertificate=True;",null);

        public static readonly SecureConnectionString CastaseedAPPSecureConnectionString =
            new SecureConnectionString("Data Source=.;Initial Catalog=castaseed;Integrated Security=True;TrustServerCertificate=True;", null);

        private static readonly Func<object> preCompiledModelFunc = () => {
            return DemoAppDbContext.Instance;
        };

        public static readonly Func<string> GetCurrentUserFunc = () =>
        {
            return Thread.CurrentPrincipal?.Identity?.Name ?? "system";
        };

        private static readonly Assembly DemoAppDataContractAssembly = typeof(AutoTable).Assembly;

        public static readonly IApplicationRepositoryBase DemoApp21DALHelper =
            ApplicationRepository<Core6DynamicRepository>.GetApplicationRepositoryWithCompiledModel(
        GetCurrentUserFunc
        ,SFMAPPSecureConnectionString
        ,DemoAppDataContractAssembly
        ,$"TestPrecompiledModel.ORMMetadata.xml"
        ,preCompiledModelFunc);

        public static readonly IApplicationRepositoryBase CastaseedDALHelper =
            ApplicationRepository<Core6DynamicRepository>.GetApplicationRepositoryWithCompiledModel(
                GetCurrentUserFunc
                , CastaseedAPPSecureConnectionString
                , DemoAppDataContractAssembly
                , $"TestPrecompiledModel.ORMMetadata.xml"
                , preCompiledModelFunc);

        public static void DOUnitOfWork_Call_MultipleStoredProcedures()
        {
            //options.UseModel(DemoAppDbContextModel.Instance)
            var existingManualTableData = DemoApp21DALHelper.DoExecuteCommand<ManualTable>(c => c.Name == "MT",true);
            if (existingManualTableData.Any())
            {
                foreach (var manualTable in existingManualTableData)
                {
                    manualTable.DataEntityState = DomainEntityState.Deleted;
                }

                DemoApp21DALHelper.SaveRecords<ManualTable>(existingManualTableData);
            }

            var existingAutotableData = DemoApp21DALHelper.DoExecuteCommand<AutoTable>(c => c.Name == "AA",true);
            if (existingAutotableData.Any())
            {
                foreach (var autoTable in existingAutotableData)
                {
                    autoTable.DataEntityState = DomainEntityState.Deleted;
                }
                DemoApp21DALHelper.SaveRecords<AutoTable>(existingAutotableData);
            }

            var autotable = new AutoTable()
            {
                C = "A",
                Name = "AA",
                ChildTableList = new List<ChildTable>()
                    {new ChildTable() {ChildName = "FC", DataEntityState = DomainEntityState.New}},
                DataEntityState = DomainEntityState.New
            };

            var existingRecords = DemoApp21DALHelper.DoExecuteCommand<ManualTable>(m => m.Id > 0,true);
            var maxID = existingRecords.Any() ? existingRecords.Max(t => t.Id) + 1 : 1;
            var childIDs = existingRecords.SelectMany(c => c.ManualChildTable).Select(c => c.ChildID).ToList();
            var maxChildID = childIDs.Any() ? childIDs.Max() + 1 : 1;
            var manual = new ManualTable()
            {
                DataEntityState = DomainEntityState.New,
                Id = maxID
                ,
                Name = "MT",
                ManualChildTable = new List<ManualChildTable>()
                    {new ManualChildTable() {DataEntityState = DomainEntityState.New, ChildName = "MFS", ChildID = maxChildID, ParentID = maxID}}
            };

            var result = DemoApp21DALHelper.DoUnitOfWork<bool>(() =>
            {
                var isAutoTableSaved = DemoApp21DALHelper.SaveRecords<AutoTable>(new List<AutoTable>() { autotable });
                var isManualTableSaved = DemoApp21DALHelper.SaveRecords<ManualTable>(new List<ManualTable>() { manual });
                return isAutoTableSaved && isManualTableSaved;
            });
            Console.WriteLine($"result:{result}");
            
        }

        public static void Test_SystemGeneratedColumns_being_populated()
        {
            var sysgeneraedrecord = new SXARCMWithSysGeneratedColumns()
            {
                DataEntityState = DomainEntityState.New,
                Amount = 11
            };
            DemoApp21DALHelper.SaveRecords<SXARCMWithSysGeneratedColumns>(new List<SXARCMWithSysGeneratedColumns>() { sysgeneraedrecord });
            var rec = DemoApp21DALHelper.DoExecuteCommand<SXARCMWithSysGeneratedColumns>(c => c.Amount == 11,false);
            var firstRec = rec.First();
            Console.WriteLine(firstRec.CreatedWhenUTC);
            Console.WriteLine(firstRec.CreatedBy);
            Console.WriteLine(firstRec.TouchedBy);
            Console.WriteLine(firstRec.TouchedWhenUTC);
            //check whether createdwhenutc and createdby is populated
            
        
        }

        public static void Test_GetJSONDocument()
        {
            //GetJsonDocument
            var doc = CastaseedDALHelper.GetJsonDocument(@"EXEC test_preview_list_load_sp", new List<DbParameter>());
            Console.WriteLine(doc.RootElement);
        }

        [ResultType(typeof(caslist_merged_setting))]
        [ResultType(typeof(category_counts_pivot))]
        [ResultType(typeof(list_enriched))]
        [Function(Name = "dbo.test_preview_list_load_sp")]
        public static void test_preview_list_load_sp(
            [Parameter(Name = "@commaseperatedListID", DbType = "varchar(100)")] string commaseperatedListID
            ,[Parameter(Name = "@UserID", DbType = "int")] System.Nullable<int> userID
            ,[Parameter(Name = "@isjsonoutput", DbType = "bit")] bool isjsonoutput)
        {
            System.Reflection.MethodInfo mInfo = (System.Reflection.MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod();
            if (mInfo == null)
            {
                throw new ArgumentNullException("");
            }
            // Call DAL Method and pass specific methodInfo and get ResultSet
            var result = CastaseedDALHelper.ExecuteStoredProc(mInfo, commaseperatedListID, userID, isjsonoutput);
            var settings  = result.GetResult<caslist_merged_setting>();
            var counts = result.GetResult<category_counts_pivot>().FirstOrDefault();
            var listEnriched = result.GetResult<list_enriched>();
            Console.WriteLine(listEnriched.Count());
        }

        
    }
}
