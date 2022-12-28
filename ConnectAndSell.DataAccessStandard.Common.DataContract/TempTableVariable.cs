using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class TempTableVariable
    {
        public readonly string TempTableVariableName;

        public TempTableVariable(string tempTableVariableName)
        {
            if(string.IsNullOrEmpty(tempTableVariableName) == true) throw new ArgumentNullException($"{nameof(tempTableVariableName)}");
            if((tempTableVariableName.StartsWith("#") || tempTableVariableName.StartsWith("[#") ) == false) throw new ArgumentException($"Name of temp table should start with # sign. The value of argument {nameof(tempTableVariableName)} is {tempTableVariableName} which does not starts with # ");
            TempTableVariableName = tempTableVariableName;
        }

        public override string ToString()
        {
            return TempTableVariableName;
        }
    }
}
