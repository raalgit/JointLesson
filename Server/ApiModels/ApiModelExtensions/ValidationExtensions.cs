using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.ApiModelExtensions
{
    public static class ValidationExtensions
    {
        public static bool ValidateOfNull(this IRequest request)
        {
            if (request == null) return false;

            // TODO: fields validation

            return true;
        }
    }
}
