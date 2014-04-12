using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ianhd.core.Validation
{
    public class OperationResult<T> : OperationResult
    {
        public T Item { get; set; }
        
        public OperationResult(T item)
            : base()
        {
            this.Item = item;
        }

        public OperationResult() : base() { }
    }
}
