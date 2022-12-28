namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public interface IPrimaryKeyBase
    {
        
    }
    //[DataContract]
    public interface IPrimaryKeyBase<T> : IPrimaryKeyBase
    {
    }

    public interface IPrimaryKeyResolveBase
    {
        object GetPrimaryKeyRecordInfo();
    }
    //[DataContract]
    public interface IPrimaryKeyResolveBase<T,TK>: IPrimaryKeyResolveBase where T: IPrimaryKeyBase<TK>
    {
        T PrimaryKeyRecordInfo { get; set; }
    }

    public interface ICompositeRoot
    {
        
    }
    
    
}
