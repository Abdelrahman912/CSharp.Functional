using CSharp.Functional.Constructs;
using CSharp.Functional.Errors;
using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;
using static CSharp.Functional.Extensions.ValidationExtension;
using System.Linq;
using static CSharp.Functional.Extensions.OptionExtension;
using CSharp.Functional.Extensions;
using System.Reflection;
using System.Linq.Expressions;

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
                                           invalid: errs => Some(errs),
                                           valid: _ => (Option<IEnumerable<Error>>)None)).ToList();
                return errors.Count == 0 ? Valid(t) : Invalid(errors.Flatten());
            };

        public static Validation<List<T>> PopOutValidation<T>(this List<Validation<T>> validations)
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


        public static T With<T>(this T source, string propertyName, object newValue)
         where T : class
        {
            T @new = source.ShallowCopy();

            typeof(T).GetBackingField(propertyName)
               .SetValue(@new, newValue);

            return @new;
        }


        public static T With<T, P>(this T source, Expression<Func<T, P>> exp, object newValue)
            where T : class
            => source.With(exp.MemberName(),newValue);

        private static string MemberName<T, P>(this Expression<Func<T, P>> exp) =>
            ((MemberExpression)exp.Body).Member.Name;


        private static T ShallowCopy<T>(this T source) =>
            (T)source.GetType().GetTypeInfo()
                               .GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)
                               .Invoke(source, null);

        private static string BackingFieldName(string propertyName) =>
         string.Format("<{0}>k__BackingField", propertyName);

        private static FieldInfo GetBackingField(this Type t, string propertyName) =>
            t.GetTypeInfo().GetField(BackingFieldName(propertyName), BindingFlags.Instance | BindingFlags.NonPublic);

    }
}
