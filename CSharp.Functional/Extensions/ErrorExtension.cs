using CSharp.Functional.Constructs;
using CSharp.Functional.Errors;
using System.Collections.Generic;
using System.Linq;
using static CSharp.Functional.Extensions.ValidationExtension;
using static CSharp.Functional.Functional;
using Unit = System.ValueTuple;

namespace CSharp.Functional.Extensions
{
    public static class ErrorExtension
    {
        public static Validation<Unit> AsUnitValidation(this IEnumerable<Error> errs)
        {
            if (errs.Count() > 0)
                return Invalid(errs);
            else
                return Unit();
        }

    }
}
