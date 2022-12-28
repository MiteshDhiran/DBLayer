


public static class CastAppDALHelperSPWrapper
{

		[ResultType(typeof(CastanetDataContract.caslist_merged_setting))]
		[ResultType(typeof(CastanetDataContract.category_counts_pivot))]
		[ResultType(typeof(CastanetDataContract.list_enriched))]
		[Function(Name = "test_preview_list_load_sp")]
		public static MDRX.DataAccessCore.Server.Common.ResultSet test_preview_list_load_sp ([Parameter(Name="@commaseperatedListID", DbType="varchar(100)")]  string commaseperatedListID,[Parameter(Name="@UserID", DbType="int")]  System.Nullable<int> userID,[Parameter(Name="@isjsonoutput", DbType="bit")]  bool isjsonoutput)
		{
			System.Reflection.MethodInfo mInfo = (System.Reflection.MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod();
			// Call DAL Method and pass specific methodInfo and get ResultSet
			ResultSet retVal = CastAppDALHelper.ExecuteStoredProcedure(mInfo, commaseperatedListID,userID,isjsonoutput);
			// return IMultipleResults
			return retVal;
		}



}