using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class SelectedColumnBuilder
    {
        private Type RootType;
        private readonly List<PropertyInfo> _propertyInfoList = new List<PropertyInfo>();

        public List<PropertyInfo> PropertyInfoList
        {
            get
            {
                return _propertyInfoList;
            }
        }

        public SelectedColumnBuilder AddSelectedColumn<T>(Expression<Func<T, object>> expression)
        {
            switch (expression?.Body)
            {
                case null:
                    throw new ArgumentNullException(nameof(expression));
                case UnaryExpression unaryExp when unaryExp.Operand is MemberExpression memberExp:
                    _propertyInfoList.Add((PropertyInfo)memberExp.Member);
                    break;
                case MemberExpression memberExp:
                    _propertyInfoList.Add((PropertyInfo)memberExp.Member);
                    break;
                default:
                    throw new ArgumentException($"The expression doesn't indicate a valid property. [ {expression} ]");
            }
            return this;
        }

        private SelectedColumnBuilder(Type rootType)
        {
            this.RootType = rootType;
        }

        public static SelectedColumnBuilder CreateSelectedColumn<T>()
        {
            return new SelectedColumnBuilder(typeof(T));
        }

    }
}
