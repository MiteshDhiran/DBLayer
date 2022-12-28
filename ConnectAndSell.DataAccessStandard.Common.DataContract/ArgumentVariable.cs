using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ArgumentVariable
    {
        public readonly string ArgumentVariableName;

        public ArgumentVariable(string argumentVariableName)
        {
            if (string.IsNullOrEmpty(argumentVariableName))
            {
                throw new ArgumentNullException(nameof(argumentVariableName));
            }

            if (argumentVariableName.StartsWith("@") == false)
            {
                throw new ArgumentException($"SQL Argument name should start with @sign");
            }
            
            ArgumentVariableName = argumentVariableName ?? throw new ArgumentNullException(nameof(argumentVariableName));
        }

        public override string ToString()
        {
            return ArgumentVariableName;
        }
    }
}