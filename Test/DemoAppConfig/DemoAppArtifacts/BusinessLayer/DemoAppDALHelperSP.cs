


public static class DemoAppDALHelperSPWrapper
{

		[Function(Name = "GetAutoTableSelPr")]
		public static MDRX.DataAccessCore.Server.Common.ResultSet GetAutoTableSelPr ([Parameter(Name="@id", DbType="int")]  System.Nullable<int> id)
		{
			System.Reflection.MethodInfo mInfo = (System.Reflection.MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod();
			// Call DAL Method and pass specific methodInfo and get ResultSet
			ResultSet retVal = DemoAppDALHelper.ExecuteStoredProcedure<AutoTableFromSP>(mInfo, id);
			// return IMultipleResults
			return retVal;
		}

		[ResultType(typeof(SFMStandardDataContract.AutoTableFromSP))]
		[ResultType(typeof(SFMStandardDataContract.ChildTableSP))]
		[Function(Name = "GetAutoTableWithChildTableSelPr")]
		public static MDRX.DataAccessCore.Server.Common.ResultSet GetAutoTableWithChildTableSelPr ([Parameter(Name="@id", DbType="int")]  System.Nullable<int> id)
		{
			System.Reflection.MethodInfo mInfo = (System.Reflection.MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod();
			// Call DAL Method and pass specific methodInfo and get ResultSet
			ResultSet retVal = DemoAppDALHelper.ExecuteStoredProcedure(mInfo, id);
			// return IMultipleResults
			return retVal;
		}



}