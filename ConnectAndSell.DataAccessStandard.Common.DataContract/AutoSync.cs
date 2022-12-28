namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public enum AutoSync
    {
        /// <summary>Automatically selects the value.</summary>
        Default,
        /// <summary>Always returns the value.</summary>
        Always,
        /// <summary>Never returns the value.</summary>
        Never,
        /// <summary>Returns the value only after an insert operation.</summary>
        OnInsert,
        /// <summary>Returns the value only after an update operation.</summary>
        OnUpdate,
    }
}
