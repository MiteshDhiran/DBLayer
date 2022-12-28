

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Creates the array from comma separated string.
        /// </summary>
        /// <param name="commaSeparatedValue">The comma separated value.</param>
        /// <returns></returns>
        public static List<string> CreateArrayFromCommaSeparatedString(string commaSeparatedValue)
        {
            if(string.IsNullOrEmpty(commaSeparatedValue)) return new List<string>();
            return commaSeparatedValue.Split(',').Select(t => t.Trim()).ToList();
        }

        /// <summary>
        /// The type conversion
        /// </summary>
        public static readonly Dictionary<Type, Tuple<Type, Func<object, object>, Func<string, string>, Func<object, object>>> TypeConversion = new Dictionary<Type, Tuple<Type, Func<object, object>, Func<string, string>, Func<object, object>>>()
        {
            {typeof(DateTimeOffset),new Tuple<Type,Func<object,object>,Func<string,string>, Func<object,object> >(typeof(string),(obj) => ((DateTimeOffset?) obj)?.ToString("O"),(cn) =>
                $"CONVERT(datetimeoffset, {cn}, 127)", (obj) => (DateTimeOffset?) obj ?? DateTimeOffset.Now)},
            {typeof(DateTime),new Tuple<Type,Func<object,object>,Func<string,string>, Func<object, object> >(typeof(string),(obj) => ((DateTime?) obj)?.ToString("s"),(cn) =>
                $"CONVERT(DATETIME, {cn}, 127)", (obj) => (DateTime?) obj ?? DateTime.Now)},
            {typeof(Byte[]),new Tuple<Type,Func<object,object>,Func<string,string> , Func<object, object>>(typeof(string),(obj) => ConvertDecimalToByteArray(Decimal.ToInt64(decimal.Parse((string)obj))),(cn) =>
                $"CAST((CONVERT(bigint, {cn})) as decimal)", (obj) => ConvertDecimalToByteArray(Decimal.ToInt64(decimal.Parse((string)obj))))},
            {typeof(string),new Tuple<Type,Func<object,object>, Func<string,string>, Func<object, object>>(typeof(string),(obj) => obj.ToString(),(cn) => cn, (obj) => obj.ToString())}
        };

        /// <summary>
        /// Concatenates the list of string using separator.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string ConcatenateListOfStringUsingSeparator(this List<string> list, string separator)
        {
            if (list == null || list.Count == 0) return string.Empty;
            return list.Aggregate((StringBuilder)null, (sb, s) => { sb = sb == null ? new StringBuilder(s) : sb.Append(separator + s); return sb; }).ToString();
        }

        /// <summary>
        /// The get SQL to net converted value function
        /// </summary>
        public static readonly Func<IColumnNetInfo, object, object> GetSqlToNetConvertedValueFunc = (pi, dbValue) =>
        {
            if (dbValue == null) return null;
            if (pi == null) throw new ArgumentNullException($"{nameof(pi)} is null");
            var type = pi.PropertyType.IsGenericType ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType;
            if (type == null) throw new InvalidOperationException($"Underlying CLR type of property {pi.Name} could not be determined.");
            var func = TypeConversion.ContainsKey(type) ? TypeConversion[type].Item4 : (Func<object, object>)null;
            return func != null ? func(dbValue) : dbValue;
        };
        
        private static byte[] ConvertDecimalToByteArray(Decimal dec)
        {
            //Load four 32 bit integers from the Decimal.GetBits function
            var bits = decimal.GetBits(dec);
            //Create a temporary list to hold the bytes
            var bytes = new List<byte>();
            //iterate each 32 bit integer
            foreach (Int32 i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            bytes.Reverse();
            return bytes.Skip(8).ToArray();
        }

        /// <summary>
        /// Reads the resource as text from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Could not load embedded resource: {resourceName} from assembly:{assembly.FullName}.Please make sure resource exists and is fully qualified by assembly default namespace.</exception>
        public static string ReadResourceAsTextFromAssembly(Assembly assembly, string resourceName)
        {
            using( var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Could not load embedded resource: {resourceName} from assembly:{assembly.FullName}.Please make sure resource exists and is fully qualified by assembly default namespace.");
                }

                using( var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Converts the list.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="type">The type.</param>
        /// <param name="performConversion">if set to <c>true</c> [perform conversion].</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Cannot create cast method on type {containedType.FullName}</exception>
        public static object ConvertList(List<object> items, Type type, bool performConversion = false)
        {
            var containedType = type.GenericTypeArguments.First();
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast))?.MakeGenericMethod(containedType);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList))?.MakeGenericMethod(containedType);

            IEnumerable<object> itemsToCast;

            itemsToCast = performConversion ? items.Select(item => Convert.ChangeType(item, containedType)) : items;
            
            if (castMethod != null && toListMethod != null)
            {
                var castedItems = castMethod.Invoke(null, new[] { itemsToCast });

                return toListMethod.Invoke(null, new[] {castedItems});
            }
            else
            {
                throw new InvalidOperationException($"Cannot create cast method on type {containedType.FullName}");
            }
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