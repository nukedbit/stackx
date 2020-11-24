using System.Collections.Generic;

namespace StackX.ServiceModel
{
    public interface ICrudListResponse<TId, TTable>
    {
        public List<TId> Ids { get; set; }

        public List<TTable> Results { get; set; }
    }
    
    public interface ICrudListResponse<TTable> : ICrudListResponse<int, TTable>
    { 
    }
}