//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;

//namespace ConnectAndSell.DataAccessStandard.Common.DataContract
//{
//    public static class AppMethodExtension
//	{

//        #region UtilityFunctions
//        delegate Func<T1, T2> Recursive<T1, T2>(Recursive<T1, T2> r);
//        delegate Func<T1, T2, T3> Recursive<T1, T2, T3>(Recursive<T1, T2, T3> r);
//        delegate Func<T1, T2, T3, T4> Recursive<T1, T2, T3, T4>(Recursive<T1, T2, T3, T4> r);
//        delegate Func<T1, T2, T3, T4, T5> Recursive<T1, T2, T3, T4, T5>(Recursive<T1, T2, T3, T4, T5> r);
//        delegate Func<T1, T2, T3, T4, T5, T6> Recursive<T1, T2, T3, T4, T5, T6>(Recursive<T1, T2, T3, T4, T5, T6> r);
//        delegate Func<T1, T2, T3, T4, T5, T6, T7> Recursive<T1, T2, T3, T4, T5, T6, T7>(Recursive<T1, T2, T3, T4, T5, T6, T7> r);

//        internal static Func<T1, T2> Y<T1, T2>(Func<Func<T1, T2>, Func<T1, T2>> f)
//        {
//            Func<T1, T2> Rec(Recursive<T1, T2> r) => a => f(r(r))(a);
//            return Rec((Recursive<T1, T2>)Rec);
//        }
//        internal static Func<T1, T2, T3> Y<T1, T2, T3>(Func<Func<T1, T2, T3>, Func<T1, T2, T3>> f)
//        {
//            Func<T1, T2, T3> Rec(Recursive<T1, T2, T3> r) => (a, b) => f(r(r))(a, b);
//            return Rec((Recursive<T1, T2, T3>)Rec);
//        }

//        internal static Func<T1, T2, T3, T4> Y<T1, T2, T3, T4>(Func<Func<T1, T2, T3, T4>, Func<T1, T2, T3, T4>> f)
//        {
//            Func<T1, T2, T3, T4> Rec(Recursive<T1, T2, T3, T4> r) => (a, b, c) => f(r(r))(a, b, c);
//            return Rec((Recursive<T1, T2, T3, T4>)Rec);
//        }

//        internal static Func<T1, T2, T3, T4, T5> Y<T1, T2, T3, T4, T5>(Func<Func<T1, T2, T3, T4, T5>, Func<T1, T2, T3, T4, T5>> f)
//        {
//            Func<T1, T2, T3, T4, T5> Rec(Recursive<T1, T2, T3, T4, T5> r) => (a, b, c, d) => f(r(r))(a, b, c, d);
//            return Rec((Recursive<T1, T2, T3, T4, T5>)Rec);
//        }

//        internal static Func<T1, T2, T3, T4, T5, T6> Y<T1, T2, T3, T4, T5, T6>(Func<Func<T1, T2, T3, T4, T5, T6>, Func<T1, T2, T3, T4, T5, T6>> f)
//        {
//            Func<T1, T2, T3, T4, T5, T6> Rec(Recursive<T1, T2, T3, T4, T5, T6> r) => (a, b, c, d, e) => f(r(r))(a, b, c, d, e);
//            return Rec((Recursive<T1, T2, T3, T4, T5, T6>)Rec);
//        }

//        internal static Func<T1, T2, T3, T4, T5, T6, T7> Y<T1, T2, T3, T4, T5, T6, T7>(Func<Func<T1, T2, T3, T4, T5, T6, T7>, Func<T1, T2, T3, T4, T5, T6, T7>> f)
//        {
//            Func<T1, T2, T3, T4, T5, T6, T7> Rec(Recursive<T1, T2, T3, T4, T5, T6, T7> r) => (a, b, c, d, e, ee) => f(r(r))(a, b, c, d, e, ee);
//            return Rec((Recursive<T1, T2, T3, T4, T5, T6, T7>)Rec);
//        }

//        public static Func<Func<T1, T2>, Func<T1, T2>> GetLazyFunc<T1, T2>(this Func<T1, T2> f)
//        {
//            return f1 => f;
//        }
//        public static Func<Func<T1, T2, T3>, Func<T1, T2, T3>> GetLazyFunc<T1, T2, T3>(this Func<T1, T2, T3> f)
//        {
//            return f1 => f;
//        }

//        public static Func<Func<T1, T2, T3, T4>, Func<T1, T2, T3, T4>> GetLazyFunc<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f)
//        {
//            return f1 => f;
//        }

//        public static Func<Func<T1, T2, T3, T4, T5>, Func<T1, T2, T3, T4, T5>> GetLazyFunc<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f)
//        {
//            return f1 => f;
//        }

//        public static Func<Func<T1, T2, T3, T4, T5, T6>, Func<T1, T2, T3, T4, T5, T6>> GetLazyFunc<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f)
//        {
//            return f1 => f;
//        }

//        public static Func<Func<T1, T2, T3, T4, T5, T6, T7>, Func<T1, T2, T3, T4, T5, T6, T7>> GetLazyFunc<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f)
//        {
//            return f1 => f;
//        }

//        public static Func<T1, T2> DecorateFuncFirstResourceParameter<T1, T2>(this Func<Func<T1, T2>, Func<T1, T2>> lazyFuncToBeDecorated, Action<Action<T1>> lazyResourceProviderAction)
//        {
//            Func<T1, T2> decoratedEnhanced = Y<T1, T2>(fu => _ =>
//            {
//                var retVal = default(T2);
//                lazyResourceProviderAction(res =>
//                    {
//                        retVal = lazyFuncToBeDecorated(fu)(res);
//                    }
//                );
//                return retVal;
//            });
//            return decoratedEnhanced;
//        }

//        public static Func<T1, T2, T3> DecorateFuncFirstResourceParameter<T1, T2, T3>(this Func<Func<T1, T2, T3>, Func<T1, T2, T3>> lazyFuncToBeDecorated, Action<Action<T1>> lazyResourceProviderAction)
//        {
//            var decoratedEnhanced = Y<T1, T2, T3>(fu => (_, arg2) =>
//              {
//                  var retVal = default(T3);
//                  lazyResourceProviderAction(res =>
//                      {
//                          retVal = lazyFuncToBeDecorated(fu)(res, arg2);
//                      }
//                  );
//                  return retVal;
//              });
//            return decoratedEnhanced;
//        }

//        public static Func<T1, T2, T3, T4> DecorateFuncFirstResourceParameter<T1, T2, T3, T4>(this Func<Func<T1, T2, T3, T4>, Func<T1, T2, T3, T4>> lazyFuncToBeDecorated, Action<Action<T1>> lazyResourceProviderAction)
//        {
//            Func<T1, T2, T3, T4> decoratedEnhanced = Y<T1, T2, T3, T4>(fu => (_, arg2, arg3) =>
//               {
//                   var retVal = default(T4);
//                   lazyResourceProviderAction(res =>
//                       {
//                           retVal = lazyFuncToBeDecorated(fu)(res, arg2, arg3);
//                       }
//                   );
//                   return retVal;
//               });
//            return decoratedEnhanced;
//        }

//        public static Func<T1, T2, T3, T4, T5> DecorateFuncFirstResourceParameter<T1, T2, T3, T4, T5>(this Func<Func<T1, T2, T3, T4, T5>, Func<T1, T2, T3, T4, T5>> lazyFuncToBeDecorated, Action<Action<T1>> lazyResourceProviderAction)
//        {
//            var decoratedEnhanced = Y<T1, T2, T3, T4, T5>(fu => (_, arg2, arg3, arg4) =>
//             {
//                 var retVal = default(T5);
//                 lazyResourceProviderAction(res =>
//                     {
//                         retVal = lazyFuncToBeDecorated(fu)(res, arg2, arg3, arg4);
//                     }
//                 );
//                 return retVal;
//             });
//            return decoratedEnhanced;
//        }

//        public static Func<T1, T2, T3, T4, T5, T6> DecorateFuncFirstResourceParameter<T1, T2, T3, T4, T5, T6>(this Func<Func<T1, T2, T3, T4, T5, T6>, Func<T1, T2, T3, T4, T5, T6>> lazyFuncToBeDecorated, Action<Action<T1>> lazyResourceProviderAction)
//        {
//            var decoratedEnhanced = Y<T1, T2, T3, T4, T5, T6>(fu => (_, arg2, arg3, arg4, arg5) =>
//             {
//                 var retVal = default(T6);
//                 lazyResourceProviderAction(res =>
//                     {
//                         retVal = lazyFuncToBeDecorated(fu)(res, arg2, arg3, arg4, arg5);
//                     }
//                 );
//                 return retVal;
//             });
//            return decoratedEnhanced;
//        }

//        public static Func<T1, T2, T3, T4, T5, T6, T7> DecorateFuncFirstResourceParameter<T1, T2, T3, T4, T5, T6, T7>(this Func<Func<T1, T2, T3, T4, T5, T6, T7>, Func<T1, T2, T3, T4, T5, T6, T7>> lazyFuncToBeDecorated, Action<Action<T1>> lazyResourceProviderAction)
//        {
//            var decoratedEnhanced = Y<T1, T2, T3, T4, T5, T6, T7>(fu => (_, arg2, arg3, arg4, arg5, arg6) =>
//             {
//                 var retVal = default(T7);
//                 lazyResourceProviderAction(res =>
//                     {
//                         retVal = lazyFuncToBeDecorated(fu)(res, arg2, arg3, arg4, arg5, arg6);
//                     }
//                 );
//                 return retVal;
//             });
//            return decoratedEnhanced;
//        }

//        /// <summary>
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="K"></typeparam>
//        /// <param name="poolEnabledAction"></param>
//        /// <param name="mapFunc"></param>
//        /// <param name="destructorAction"></param>
//        /// <returns></returns>
//        public static Action<Action<K>> GetActionWithConvertedInputType<T, K>(
//            this Action<Action<T>> poolEnabledAction, Func<T, K> mapFunc, Action<K> destructorAction) =>
//            action =>
//            {
//                poolEnabledAction(b =>
//                {
//                    var instanceOfAnotherType = default(K);
//                    try
//                    {
//                        instanceOfAnotherType = mapFunc(b);
//                        action(instanceOfAnotherType);
//                    }
//                    finally
//                    {
//                        destructorAction(instanceOfAnotherType);
//                    }
//                });
//            };

//        /// <summary>
//        ///  Creates a function that consumes the object provided by action <paramref name="action"/> by executing the function withing action body
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="K"></typeparam>
//        /// <param name="poolEnableAction"></param>
//        /// <returns></returns>
//        public static Func<Func<T, K>, K> GetDecoratedFunctionFromResourceAction<T, K>(this Action<Action<T>> poolEnableAction) =>
//            f =>
//            {
//                var retVal = default(K);
//                poolEnableAction(pooledObject =>
//                {
//                    retVal = f(pooledObject);
//                });
//                return retVal;
//            };

//        public static Func<T1, K> GetDecoratedFuncConsumingResourceObject<B, T1, K>(this Func<B, T1, K> func, Action<Action<B>> resourceLazyAction)
//        {
//            return (arg2) => func.GetLazyFunc().DecorateFuncFirstResourceParameter(resourceLazyAction)(default(B), arg2);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="B"></typeparam>
//        /// <typeparam name="T1"></typeparam>
//        /// <typeparam name="T2"></typeparam>
//        /// <typeparam name="K"></typeparam>
//        /// <param name="func"></param>
//        /// <param name="resourceLazyAction"></param>
//        /// <returns></returns>
//        public static Func<T1, T2, K> GetDecoratedFuncConsumingResourceObject<B, T1, T2, K>(this Func<B, T1, T2, K> func, Action<Action<B>> resourceLazyAction)
//        {
//            return (arg2, arg3) => func.GetLazyFunc().DecorateFuncFirstResourceParameter(resourceLazyAction)(default(B), arg2, arg3);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="B"></typeparam>
//        /// <typeparam name="T1"></typeparam>
//        /// <typeparam name="T2"></typeparam>
//        /// <typeparam name="T3"></typeparam>
//        /// <typeparam name="K"></typeparam>
//        /// <param name="func"></param>
//        /// <param name="resourceLazyAction"></param>
//        /// <returns></returns>
//        public static Func<T1, T2, T3, K> GetDecoratedFuncConsumingResourceObject<B, T1, T2, T3, K>(this Func<B, T1, T2, T3, K> func, Action<Action<B>> resourceLazyAction)
//        {
//            return (arg2, arg3, arg4) => func.GetLazyFunc().DecorateFuncFirstResourceParameter(resourceLazyAction)(default(B), arg2, arg3, arg4);
//        }

//        #endregion

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="exp"></param>
//        /// <returns></returns>
//        public static bool IsDefaultExpression(this LambdaExpression exp)
//		{
//			return exp == null || String.Compare(exp.Body.ToString(), "True", true) == 0;
			
//		}

//        /// <summary>
//        /// Memonize function calls
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="TK"></typeparam>
//        /// <param name="funToMemonize"></param>
//        /// <returns></returns>
//        public static Func<T, TK> Memonize<T, TK>(this Func<Func<T, TK>, T, TK> funToMemonize)
//        {
//            var m = new ConcurrentDictionary<T, TK>();
//            var lockDic = new ConcurrentDictionary<T, object>(); //added locking mechanism so that only one function call is made in case of concurrently function is called for same input parameter
//            Func<T, TK> retVal = null;
//            retVal = x =>
//            {
//                if (m.ContainsKey(x))
//                {
//                    return m[x];
//                }
//                else
//                {
//                    object lockObj = new object();
//                    lockDic.TryAdd(x, lockObj);
//                    object objFromDic;
//                    lockObj = lockDic.TryGetValue(x, out objFromDic) ? objFromDic : lockObj; //if lockDic.TryRemove(x, out obj); is called in another thread then value will not be in dic - so taking extra caution
//                    TK result;
//                    lock (lockObj)
//                    {
//                        if (m.ContainsKey(x) == false) // double check if the object is already fetched
//                        {
//                            result = funToMemonize(retVal, x);
//                            m.TryAdd(x, result);
//                        }
//                        else
//                        {
//                            m.TryGetValue(x, out result);
//                        }
//                    }
//                    object obj;
//                    lockDic.TryRemove(x, out obj);
//                    return result;
//                }
//            };
//            return retVal;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="seperator"></param>
//        /// <returns></returns>
//        public static string ConcatinateListOfStringUsingSeperator(this List<string> list, string seperator)
//        {
//            if (list == null || list.Count == 0) return string.Empty;
//            return list.Aggregate((StringBuilder)null, (sb, s) => { sb = sb == null ? new StringBuilder(s) : sb.Append(seperator + s); return sb; }).ToString();
//        }

//        /// <summary>
//        /// Creates lambda expression of property 
//        /// </summary>
//        /// <param name="objectType"></param>
//        /// <param name="pinfo"></param>
//        /// <returns></returns>
//        public static LambdaExpression GetPropertyLambda(this Type objectType, PropertyInfo pinfo)
//        {
//            ParameterExpression param = Expression.Parameter(objectType, "x");
//            Expression property = Expression.Property(param, pinfo.Name);
//            LambdaExpression lambdaExpression = Expression.Lambda(property, true, new[] { param });
//            return lambdaExpression;
//        }

		

		

		

		

		

//		/// <summary>
//		/// Sort List collection on column
//		/// </summary>
//		/// <typeparam name="T">Generic Parameter</typeparam>
//		/// <param name="list">List collection</param>
//		/// <param name="sortColumnName">Sort column name</param>
//		/// <returns></returns>
//		public static IEnumerable<T> Sort<T>(this IEnumerable<T> list, string sortColumnName) where T : class
//		{
//			IEnumerable<T> retVal = new List<T>();

//			if (list == null) { return null; }
//			var queriable = list.AsQueryable();
//			IQueryable query =            // The original unordered query
//			from p in queriable
//			select p;

//			string propToOrderBy = sortColumnName;

//			ParameterExpression paramExpression = Expression.Parameter(typeof(T), "p");
//			MemberExpression member = Expression.PropertyOrField(paramExpression, propToOrderBy);
//			LambdaExpression lambda = Expression.Lambda(member, paramExpression);

//			Type[] exprArgTypes = { query.ElementType, lambda.Body.Type };

//			MethodCallExpression methodCall =
//				Expression.Call(typeof(Queryable), "OrderBy", exprArgTypes, query.Expression, lambda);


//			IQueryable orderedQuery = query.Provider.CreateQuery(methodCall);



//			foreach (T item in orderedQuery)
//			{
//				((List<T>)retVal).Add(item);
//			}

//			return retVal;
//		}

		
//		/// <summary>
//		/// Filtre by condition
//		/// </summary>
//		/// <typeparam name="T">IList Type</typeparam>
//		/// <typeparam name="U">Data type</typeparam>
//		/// <param name="list">The list.</param>
//		/// <param name="columnName">Name of the column.</param>
//		/// <param name="value">The value.</param>
//		/// <returns></returns>
//		public static IList<T> FilterByCondition<T, U>(this IList<T> list, string columnName, U value) where T : class
//		{
//			IList<T> retVal = new List<T>();

//			var queriableData = list.AsQueryable();
			
//            ParameterExpression pe = Expression.Parameter(typeof(T), "p");
			
//            MemberExpression left = Expression.PropertyOrField(pe, columnName);
			
//            Expression right = Expression.Constant(value, typeof(U));
			
//            Expression eqalityExpression = Expression.Equal(left, right);
			
//            Expression predicateBody = eqalityExpression;
			
//            MethodCallExpression whereCallExpression = Expression.Call(typeof(Queryable),
//														"Where",
//														 new Type[] { queriableData.ElementType },
//														 queriableData.Expression
//														 , Expression.Lambda<Func<T, bool>>(predicateBody, new ParameterExpression[] { pe })
//														 );
			
//            IQueryable filterQueriable = queriableData.Provider.CreateQuery(whereCallExpression);
			
//            foreach (T item in filterQueriable)
//			{
//				retVal.Add(item);
//			}
			
//            return retVal;
//		}

		
		
//		/// <summary>
//		/// Get the last day of the month for any
//		/// full date
//		/// </summary>
//		/// <param name="dtDate">The dt date.</param>
//		/// <param name="Period">The period.</param>
//		/// <returns></returns>
//		public static DateTimeOffset GetLastDayOfMonth(this DateTimeOffset dtDate, int Period)
//		{
//			// Set return value to the last day of the month
//			// for any date passed in to the method
//			// create a datetime variable set to the passed in date

//			DateTimeOffset LastDayOfMonth = dtDate;

//			// overshoot the date by a month
//			LastDayOfMonth = LastDayOfMonth.AddMonths(1);

//			// remove all of the days in the next month
//			// to get bumped down to the last day of the 
//			// previous month

//			LastDayOfMonth = LastDayOfMonth.AddDays(-(LastDayOfMonth.Day));

//			if (Period != 0 && Period != 1)
//			{
//				LastDayOfMonth = LastDayOfMonth.AddMonths(Period - 1);
//			}
//			// return the last day of the month
//			return LastDayOfMonth;

//		}

//		/// <summary>
//		/// Get the Fisrt day of the month for any
//		/// full date
//		/// </summary>
//		/// <param name="dtDate">The dt date.</param>
//		/// <returns></returns>
//		public static DateTime FirstDayOfMonth(this DateTime dtDate)
//		{
//			// Set return value to the last day of the month
//			// for any date passed in to the method
//			// create a datetime variable set to the passed in date

//			DateTime FirstDayOfMonth = new DateTime(dtDate.Year, dtDate.Month, 1);

//			// return the First Day Of Month of the month
//			return FirstDayOfMonth;

//		}

//        /// <summary>
//        /// Lasts the day of month.
//        /// </summary>
//        /// <param name="dtDate">The dt date.</param>
//        /// <returns></returns>
//        public static DateTime LastDayOfMonth(this DateTime dtDate)
//        {
//            DateTime FirstDayOfMonth = new DateTime(dtDate.Year, dtDate.Month, 1);

//            // return the First Day Of Month of the month
//            return FirstDayOfMonth.AddMonths(1).AddDays(-1); ;
//        }

	   


	   

	   
//		/// <summary>
//		/// Compare decimal and long value
//		/// </summary>
//		/// <param name="decimalValue">The decimal value.</param>
//		/// <param name="value">The value.</param>
//		/// <returns>
//		/// 	<c>true</c> if [is ID equal] [the specified decimal value]; otherwise, <c>false</c>.
//		/// </returns>
//		public static bool IsIDEqual(this long decimalValue, long value)
//		{
//			bool returnValue = false;
//			long temp = 0;
//			if (long.TryParse(decimalValue.ToString(), out temp))
//			{
//				if (temp == value)
//				{
//					returnValue = true;
//				}
//			}
//			return returnValue;
//		}
//		/// <summary>
//		/// Compare decimal and long value
//		/// </summary>
//		/// <param name="decimalValue">The decimal value.</param>
//		/// <param name="value">The value.</param>
//		/// <returns>
//		/// 	<c>true</c> if [is ID equal] [the specified decimal value]; otherwise, <c>false</c>.
//		/// </returns>
//		public static bool IsIDEqual(this Nullable<Decimal> decimalValue, Nullable<long> value)
//		{
//			bool returnValue = false;
//			long temp = 0;
//			if (long.TryParse(decimalValue.Value.ToString(), out temp))
//			{
//				if (temp == value.Value)
//				{
//					returnValue = true;
//				}
//			}
//			return returnValue;
//		}



	 


//		/// <summary>
//		/// Contains the specified list.
//		/// </summary>
//		/// <typeparam name="T"></typeparam>
//		/// <param name="list">The list.</param>
//		/// <param name="objectClass">The object class.</param>
//		/// <returns></returns>
//		public static bool Contain<T>(this IEnumerable<T> list, T objectClass)
//		{
//			bool retVal = false;
//			retVal = list.Contains<T>(objectClass);
//			return retVal;
//		}

//		/// <summary>
//		/// TO check list contain given object with list or not
//		/// </summary>
//		/// <typeparam name="T">Generic object</typeparam>
//		/// <param name="list">object list</param>
//		/// <param name="item">item</param>
//		/// <returns>True/false</returns>
//		public static bool DoesNotContain<T>(this IEnumerable<T> list, T item)
//		{
//			bool retVal = false;
//			retVal = list.Contains<T>(item);
//			return !retVal;
//		}


		

//		/// <summary>
//		/// given source date is with in taget date
//		/// </summary>
//		/// <param name="sourceDateTime">Source date</param>
//		/// <param name="targetDateTime">Target date</param>
//		/// <param name="hour">specified hour</param>
//		/// <returns></returns>
//		public static bool WithinNumberOfHours(this DateTime sourceDateTime, DateTime targetDateTime, double hour)
//		{
//			bool retVal = false;
//			targetDateTime.AddHours(hour);
//			if (targetDateTime > sourceDateTime)
//			{
//				retVal = true;
//			}
//			return retVal;
//		}

//		/// <summary>
//		/// source date is with in taget date
//		/// </summary>
//		/// <param name="sourceDateTime">The source date time.</param>
//		/// <param name="targetDateTime">The target date time.</param>
//		/// <param name="days">The days.</param>
//		/// <returns></returns>
//		public static bool WithinNumberOfDays(this DateTime sourceDateTime, DateTime targetDateTime, double days)
//		{
//			bool retVal = false;
//			targetDateTime.AddDays(days);
//			if (targetDateTime > sourceDateTime)
//			{
//				retVal = true;
//			}
//			return retVal;
//		}


		
//		/// <summary>
//		/// Get  object by uniue key
//		/// </summary>
//		/// <typeparam name="TElement">Object</typeparam>
//		/// <typeparam name="TKey">Key</typeparam>
//		/// <param name="source">Source</param>
//		/// <param name="keySelector">The key selector.</param>
//		/// <returns></returns>
//		public static IEnumerable<TElement> UniqueBy<TElement, TKey>(this IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
//		{
//			var results = new LinkedList<TElement>();
//			var nodeMap = new Dictionary<TKey, LinkedListNode<TElement>>();

//			foreach (TElement element in source)
//			{
//				TKey key = keySelector(element);
//				LinkedListNode<TElement> currentNode; if (nodeMap.TryGetValue(key, out currentNode))
//				{
//					// Seen it before. Remove if non-null            
//					if (currentNode != null)
//					{
//						results.Remove(currentNode);
//						nodeMap[key] = null;
//					}            // Otherwise no action needed        
//				}
//				else
//				{
//					LinkedListNode<TElement> node = results.AddLast(element);
//					nodeMap[key] = node;
//				}
//			}

//			foreach (TElement element in results)
//			{
//				yield return element;

//			}
//		}

		
//        /*
//        public static T CloneWithoutRegisteredEntity<T>(this T source)
//        {
//            if (typeof(T).IsSerializable)
//            {
//                IFormatter formatter = new BinaryFormatter();
//                return (T)source.SerializationWithPooledMemoryStream(formatter);
//            }
//            else
//            {
//                throw new SerializationException(string.Format("Unable to Serialize object: Mark {0} as [Serializable].", typeof(T).ToString()), null);
//            }
//        }


//       /// <summary>
//       /// It will use pooled memory stream.
//       /// </summary>
//       /// <typeparam name="T"></typeparam>
//       /// <param name="fmt"></param>
//       /// <param name="src"></param>
//       /// <returns></returns>
//        public static object SerializationWithPooledMemoryStream<T>(this T src,IFormatter fmt)
//        {
//            return PooledEnabledFuncHelper.DoUsingDefaultPooledMemoryStream((stream,lsource,lformatter) =>
//            {
//                try
//                {
//                    if (lsource != null)
//                    {
//                        lformatter.Serialize(stream, lsource);
//                        stream.Seek(0, SeekOrigin.Begin);
//                        return lformatter.Deserialize(stream);
//                    }
//                    else
//                        return null;
//                }
//                catch (NotSupportedException ex)
//                {
//                    Console.WriteLine(ex);
//                    //LogHelper.Log(LogSeverityLevel.Error, LogCategory.Exception, ex);
//                    return lsource.SerializeWithoutPooledMemoryStream(lformatter);
//                }
                
//            },src,fmt);
//        }
//        */
//        /// <summary>
//        /// It will serialize with default memory stream instead of pooled one.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="source"></param>
//        /// <param name="formatter"></param>
//        /// <returns></returns>
//        private static object SerializeWithoutPooledMemoryStream<T>(this T source,IFormatter formatter)
//        {
//            using (MemoryStream stream = new MemoryStream())
//            {
//                formatter.Serialize(stream, source);
//                stream.Seek(0, SeekOrigin.Begin);
//                return formatter.Deserialize(stream);
//            }
//        }


//        public static List<T> CloneListWithoutRegisteredEntity<T>(this IEnumerable<T> records) where T : class, ICloneable
//        {
//            List<T> retLists = new List<T>();
            
//            if (records != null)
//            {
//                retLists.AddRange(records.Select(r => (T)r.Clone()));
//            }
//            return retLists;
//        }


		
		
//				/// <summary>
//		/// Checks if given collection is null or empty.
//		/// </summary>
//		/// <typeparam name="T"></typeparam>
//		/// <param name="list">The list.</param>
//		/// <returns></returns>
//		public static bool IsNullOrEmpby<T>(this ICollection<T> list) where T : class
//		{
//			return null == list || 0 == list.Count;
//		}

//        /// <summary>
//        /// Add a new item to a collection only
//        /// if it is not null or not in the collection already.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="list">The list.</param>
//        /// <param name="itemNew">Item to add.</param>
//        /// <returns></returns>
//        public static void AddUniqueNotNull<T>(this ICollection<T> list, T itemNew) where T : class
//        {
//            if (null != itemNew && !list.Contains(itemNew))
//                list.Add(itemNew);

//            return;
//        }

        
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="inputList"></param>
//        /// <param name="batchSize"></param>
//        /// <returns></returns>
//        public static IEnumerable<List<T>> GetBatch<T>(this List<T> inputList, int batchSize)
//        {
//            if (inputList == null || inputList.Any() == false) yield return new List<T>(new List<T>());
//            if (batchSize == 0) throw new ArgumentOutOfRangeException("BatchSize cannot be zero");
//            if (inputList != null)
//            {
//                int totalRecords = inputList.Count();
//                int remainder = totalRecords % batchSize;
//                int roughPageNumbers = totalRecords / batchSize;
//                int totalPages = remainder > 0 ? roughPageNumbers + 1 : roughPageNumbers;
//                for (int i = 0; i < totalPages; i++)
//                {
//                    yield return inputList.Skip(i * batchSize).Take(batchSize).ToList();
//                } 
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="TI1"></typeparam>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="TO"></typeparam>
//        /// <param name="inputList"></param>
//        /// <param name="batchSize"></param>
//        /// <param name="inputToFunc"></param>
//        /// <param name="funcToExecuteInBatch"></param>
//        /// <returns></returns>
//        public static List<TO> ExecuteInBatch<TI1,T,TO>(this List<T> inputList, int batchSize,TI1 inputToFunc , Func<TI1,List<T>,List<TO>> funcToExecuteInBatch)
//        {
//            return inputList.GetBatch(batchSize).Select(b => funcToExecuteInBatch(inputToFunc, b)).SelectMany(c => c).ToList();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="TI1"></typeparam>
//        /// <typeparam name="TI2"></typeparam>
//        /// <typeparam name="TO"></typeparam>
//        /// <param name="batchSize"></param>
//        /// <param name="funcToExecuteInBatch"></param>
//        /// <returns></returns>
//        public static Func<TI1,List<TI2>,List<TO>> DecorateFuncToExecuteInBatch<TI1,TI2,TO>(int batchSize, Func<TI1, List<TI2>, List<TO>> funcToExecuteInBatch)
//        {
//            return (tio, list) => ExecuteInBatch<TI1,TI2,TO>(list, batchSize, tio, funcToExecuteInBatch);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="TI"></typeparam>
//        /// <typeparam name="TO"></typeparam>
//        /// <param name="batchList"></param>
//        /// <param name="func"></param>
//        /// <returns></returns>
//        private static List<TO> PerformActionInBatch<TI, TO>(this IEnumerable<List<TI>> batchList, Func<List<TI>, List<TO>> func)
//        {
//            return batchList.SelectMany(func).ToList();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="TI"></typeparam>
//        /// <typeparam name="TO"></typeparam>
//        /// <param name="inputList"></param>
//        /// <param name="batchSize"></param>
//        /// <param name="func"></param>
//        /// <returns></returns>
//        public static List<TO> PerformActionInBatch<TI, TO>(this List<TI> inputList, int batchSize, Func<List<TI>, List<TO>> func)
//        {
//            if (inputList == null) return new List<TO>();
//            if (batchSize == 0) throw new ArgumentOutOfRangeException("BatchSize cannot be zero");
//            return inputList.GetBatch(batchSize).PerformActionInBatch(func);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="TI"></typeparam>
//        /// <param name="inputList"></param>
//        /// <param name="batchSize"></param>
//        /// <param name="action"></param>
//        public static void PerformActionInBatch<TI>(this List<TI> inputList, int batchSize, Action<List<TI>> action)
//        {
//            if (inputList == null) return ;
//            if (batchSize == 0) throw new ArgumentOutOfRangeException("BatchSize cannot be zero");
//            inputList.GetBatch(batchSize).ToList().ForEach(action);
//        }
//	}
//}
