using CSharp.Functional.Constructs;
using CSharp.Functional.Errors;
using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;
using static CSharp.Functional.Extensions.ValidationExtension;
using System.Linq;
using static CSharp.Functional.Extensions.OptionExtension;
using CSharp.Functional.Extensions;

namespace CSharp.Functional
{
    public static class Functional
    {

        public delegate Validation<T> Validator<T>(T t);

        public static Unit Unit() => default(Unit);


        public static Validator<T> FailFast<T>(IEnumerable<Validator<T>> validators) =>
            (t) => validators.Aggregate(Valid(t), (acc, validator) => acc.Bind(_ => validator(t)));

        public static Validator<T> HarvestErrors<T>(IEnumerable<Validator<T>> validators) =>
            (t) =>
            {
                var errors = validators.Select(validate => validate(t))
                                       .Bind(v => v.Match(
                                           invalid: errs => Some(errs)  ,
                                           valid: _ => (Option<IEnumerable<Error>>)None)).ToList();
                return errors.Count == 0 ? Valid(t) : Invalid(errors.Flatten());
            };


        public static Validation<List<T>>  PopOutValidation<T>(this List<Validation<T>> validations)
        {
            (var valids, var errors) = validations.Aggregate(Tuple.Create(new List<T>(), new List<Error>()), (soFar, current) =>
            {
                current.Match(errs => { soFar.Item2.AddRange(errs); Unit(); }, valid => { soFar.Item1.Add(valid); Unit(); });
                return soFar;
            });

            if (errors.Count > 0)
                return Invalid(errors);
            else
                return Valid(valids);
        }

    }
}
