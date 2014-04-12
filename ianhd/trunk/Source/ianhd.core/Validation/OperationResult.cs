using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ianhd.core.Validation
{
    public class OperationResult
    {
        public string Message { get; set; }
        public bool WasSuccessful { get; set; }
        public bool HasMessage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Message);
            }
        }
    }
}
