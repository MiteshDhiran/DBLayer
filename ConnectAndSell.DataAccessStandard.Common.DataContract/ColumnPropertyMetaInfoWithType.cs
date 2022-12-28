

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// IColumnNetInfo Interface
    /// </summary>
    /// <seealso cref="System.Collections.IEqualityComparer" />
    public interface IColumnNetInfo  : IEqualityComparer
    {
        string Name { get;  }
        Type PropertyType { get; }
        object GetValue(object obj);
        void SetValue(object obj, object value);
        string SerializableName { get; }
    }

    /// <summary>
    ///  ColumnNetInfo class
    /// </summary>
    /// <seealso cref="IColumnNetInfo" />
    public class ColumnNetInfo : IColumnNetInfo
    {
        /// <summary>
        /// Gets the column net information name property type equality comparer.
        /// </summary>
        /// <value>
        /// The column net information name property type equality comparer.
        /// </value>
        public static NamePropertyTypeEqualityComparer ColumnNetInfoNamePropertyTypeEqualityComparer => new NamePropertyTypeEqualityComparer();

        /// <summary>
        /// NamePropertyTypeEqualityComparer
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IEqualityComparer{ConnectAndSell.DataAccessStandard.Common.DataContract.IColumnNetInfo}" />
        public sealed class NamePropertyTypeEqualityComparer : IEqualityComparer<IColumnNetInfo>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type T to compare.</param>
            /// <param name="y">The second object of type T to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(IColumnNetInfo x, IColumnNetInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Name == y.Name && x.PropertyType.Equals(y.PropertyType);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public int GetHashCode(IColumnNetInfo obj)
            {
                unchecked
                {
                    return (obj.Name.GetHashCode() * 397) ^ obj.PropertyType.GetHashCode();
                }
            }
        }

        private readonly PropertyInfo _propertyInfo;
        private readonly FieldInfo _fieldInfo;
        private readonly string _name;
        private readonly Type _propertyType;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => _name;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public Type PropertyType => _propertyType;

        /// <summary>
        /// 
        /// </summary>
        public string SerializableName { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public object GetValue(object obj)
        {
            if (_propertyInfo != null)
            {
              return  _propertyInfo.GetValue(obj);
            }
            else
            {
                return _fieldInfo.GetValue(obj);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object obj, object value)
        {
            if (_propertyInfo != null)
            {
                _propertyInfo.SetValue(obj,value);
            }
            else
            {
                _fieldInfo.SetValue(obj,value);
            }
        }
        
        private ColumnNetInfo(PropertyInfo propInfo, FieldInfo fieldInfo)
        {
            if (propInfo == null && fieldInfo == null)
            {
                throw new ArgumentNullException($"Both the input arguments are null. Arguments: {nameof(fieldInfo)} {nameof(propInfo)}");
            }
            _propertyInfo = propInfo;
            _fieldInfo = fieldInfo;
            _name = _propertyInfo?.Name ?? _fieldInfo?.Name;
            if (_name.StartsWith("m_BatchID",StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("In class ColumnNetInfo Name property is starting with m_BatchID");
            }
            _propertyType = _propertyInfo?.PropertyType ?? _fieldInfo?.FieldType;
            var declaringType = _propertyInfo?.DeclaringType ?? _fieldInfo?.DeclaringType;
            var declaringTypeDecoratedWithDataContract = declaringType.GetCustomAttributesData()
                .Any(c => string.Equals(c.AttributeType.Name, "DataContract",
                    StringComparison.CurrentCultureIgnoreCase));
            bool isMemberMarkedAsSerializable = true;
            if (declaringTypeDecoratedWithDataContract)
            {
                isMemberMarkedAsSerializable =_propertyInfo != null 
                        ?_propertyInfo.GetCustomAttributesData().Any(c => string.Equals(c.AttributeType.Name, "DataMember",
                            StringComparison.CurrentCultureIgnoreCase)) 
                        : _fieldInfo.GetCustomAttributesData().Any(c => string.Equals(c.AttributeType.Name, "DataMember",
                            StringComparison.CurrentCultureIgnoreCase))
                    ;
            }

            if (isMemberMarkedAsSerializable)
            {
                SerializableName = _name;
            }
            else if(_name.EndsWith("_CL",StringComparison.InvariantCultureIgnoreCase))
            {
                SerializableName = _name.Replace("_CL", "List");
                //TODO:Double check via reflection whether such member exists
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnNetInfo"/> class.
        /// </summary>
        /// <param name="propInfo">The property information.</param>
        /// <exception cref="ArgumentNullException">propInfo</exception>
        public ColumnNetInfo(PropertyInfo propInfo) : this(propInfo, null)
        {
            if (propInfo == null)
            {
                throw new ArgumentNullException(nameof(propInfo));
            }
        }


        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnNetInfo"/> class.
        /// </summary>
        /// <param name="fieldInfo">The field information.</param>
        /// <exception cref="ArgumentNullException">fieldInfo</exception>
        
        public ColumnNetInfo(FieldInfo fieldInfo) : this(null, fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }
        }*/

        /*
        /// <summary>
        /// Gets the type of the column net information from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyOrFieldNameList">The property or field name list.</param>
        /// <returns></returns>
        public static List<IColumnNetInfo> GetColumnNetInfoFromType(Type type, List<string> propertyOrFieldNameList)
        {
            if (propertyOrFieldNameList == null || propertyOrFieldNameList.Any() == false)
            {
                return new List<IColumnNetInfo>();
            }

            var fieldList = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => propertyOrFieldNameList.Contains(f.Name, StringComparer.InvariantCultureIgnoreCase))
                .Select(c => new ColumnNetInfo(c)).Cast<IColumnNetInfo>().ToList();

            var propertiesList =
                    type.GetProperties()
                        .Where(p => fieldList.Exists(pc =>
                            pc.Name.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase)) == false)
                        .Where(p => propertyOrFieldNameList.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
                        .Select(c => new ColumnNetInfo(c)).Cast<IColumnNetInfo>().ToList()
                ;
                
            return propertiesList.Union(fieldList).ToList();
        }
        */

         bool IEqualityComparer.Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            if (x is IColumnNetInfo && y is IColumnNetInfo)
            {
                var xType = ((IColumnNetInfo)x);
                var yType = ((IColumnNetInfo)y);
               return xType.Name == yType.Name && xType.PropertyType.Equals(yType.PropertyType);
            }
            return false;
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            unchecked
            {
                var iColObj = (IColumnNetInfo)obj;
                return (iColObj.Name.GetHashCode() * 397) ^ iColObj.PropertyType.GetHashCode();
            }
        }
    }

    /// <summary>
    /// ColumnPropertyMetaInfoWithType class
    /// </summary>
    /// <seealso cref="IColumnNetInfo" />
    public class ColumnPropertyMetaInfoWithType : IColumnNetInfo
    {
        /// <summary>
        /// Gets the column property meta information.
        /// </summary>
        /// <value>
        /// The column property meta information.
        /// </value>
        public ColumnPropertyMetaInfo ColumnPropertyMetaInfo { get;}
        private readonly IColumnNetInfo _columnNetInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnPropertyMetaInfoWithType"/> class.
        /// </summary>
        /// <param name="columnPropertyMetaInfo">The column property meta information.</param>
        /// <param name="columnNetInfo">The column net information.</param>
        /// <exception cref="ArgumentNullException">columnNetInfo</exception>
        public ColumnPropertyMetaInfoWithType(ColumnPropertyMetaInfo columnPropertyMetaInfo, IColumnNetInfo columnNetInfo)
        {
            ColumnPropertyMetaInfo = columnPropertyMetaInfo;
            _columnNetInfo = columnNetInfo ?? throw new ArgumentNullException(nameof(columnNetInfo));
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public object GetValue(object obj) => _columnNetInfo.GetValue(obj);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object obj, object value) => _columnNetInfo.SetValue(obj, value);

        /// <summary>
        /// 
        /// </summary>
        public string SerializableName => _columnNetInfo.SerializableName;
        /// <summary>
        /// Gets the column property information.
        /// </summary>
        /// <value>
        /// The column property information.
        /// </value>
        public IColumnNetInfo ColumnPropertyInfo => _columnNetInfo;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => _columnNetInfo.Name;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public Type PropertyType => _columnNetInfo.PropertyType;

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            if (x is IColumnNetInfo && y is IColumnNetInfo)
            {
                var xType = ((IColumnNetInfo)x);
                var yType = ((IColumnNetInfo)y);
                return xType.Name == yType.Name && xType.PropertyType.Equals(yType.PropertyType);
            }
            return false;
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            unchecked
            {
                var iColObj = (IColumnNetInfo)obj;
                return (iColObj.Name.GetHashCode() * 397) ^ iColObj.PropertyType.GetHashCode();
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