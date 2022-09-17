using CSharp.Functional.Constructs;
using CSharp.Functional.Errors;
using static CSharp.Functional.Extensions.OptionExtension;
using static CSharp.Functional.Extensions.ValidationExtension;

namespace Chapter13.Traversables
{
    public static class Double
    {
        public static Option<double> Parse(string s)
        {
            double result;
            return double.TryParse(s, out result)
               ? Some(result) : None;
        }
        public static Validation<double> ParseV(string s)
        {
            double result;
            return double.TryParse(s, out result)
               ? Valid(result) : new ParseToDoubleError(s);
        }
    }
}
