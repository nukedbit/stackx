namespace StackX.ServiceModel
{
    public interface ICrudResponse<TId, TTable>
    {
        public TId Id { get; set; }
        public TTable Result { get; set; }
    }
    
    public interface ICrudResponse<TTable> : ICrudResponse<int, TTable>
    { 
        
    }
}