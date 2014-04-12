using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ianhd.core.Validation
{
    public class OperationResult
    {
        public string ErrorMessage { get; set; }
        public bool WasSuccessful { get; set; }
        public bool HasErrorMessage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.ErrorMessage);
            }
        }
    }
}
