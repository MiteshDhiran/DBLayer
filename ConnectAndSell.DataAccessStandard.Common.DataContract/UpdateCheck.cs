namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public enum UpdateCheck
    {
        /// <summary>Always check. This is the default unless <see cref="P:System.Data.Linq.Mapping.ColumnAttribute.IsVersion" /> is <see langword="true" /> for a member.</summary>
        Always,
        /// <summary>Never check.</summary>
        Never,
        /// <summary>Check only when the object has been changed.</summary>
        WhenChanged,
    }
}
