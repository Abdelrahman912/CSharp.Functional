﻿using CSharp.Functional.Constructs;
using CSharp.Functional.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;
using static CSharp.Functional.Extensions.ExceptionalExtension;
using System.Threading.Tasks;

namespace CSharp.Functional.Extensions
{
    public static class ValidationExtension
    {
        public static Validation<T> Valid<T>(T value) =>
            new Validation<T>(value);

        public static Validation.Invalid Invalid(params Error[] errors) =>
            new Validation.Invalid(errors);

        public static Validation<R> Invalid<R>(params Error[] errors) => 
            new Validation.Invalid(errors);

        public static Validation.Invalid Invalid(IEnumerable<Error> errors) => 
            new Validation.Invalid(errors);

        public static Validation<R> Invalid<R>(IEnumerable<Error> errors) => 
            new Validation.Invalid(errors);

        public static Validation<R> Map<T, R>(this Validation<T> validation, Func<T, R> f) =>
           validation.Match(errors => Invalid(errors),
                            data => Valid(f(data)));

        public static Validation<R> Select<T, R>(this Validation<T> validation, Func<T, R> f) =>
            validation.Map(f);

        public static Validation<Unit> ForEach<T>(this Validation<T> validation, Action<T> action) =>
         validation.Match(errors => Invalid(errors),
                          data => Valid(action.ToFunc().Invoke(data)));

        public static Validation<T> Do<T>(this Validation<T> val,Action<T> action)
        {
            val.ForEach(action);
            return val;
        }

        public static Validation<R> Bind<T, R>(this Validation<T> validation, Func<T, Validation<R>> f) =>
           validation.Match(errors => Invalid(errors),
                            data => f(data));

        public static Validation<RR> SelectMany<T, R, RR>(this Validation<T> validation, Func<T, Validation<R>> bind, Func<T, R, RR> project) =>
           validation.Match(errors => Invalid(errors),
                            t => bind(t).Match<Validation<RR>>(errs => Invalid(errs),r=> project(t,r)));

        public static Validation<R> Apply<T, R>(this Validation<Func<T, R>> valF, Validation<T> valT) =>
            valF.Match(
                valid: (f) => valT.Match(valid: (t) => Valid(f(t)),
                    invalid: errs => Invalid(errs)),
                invalid: errF => valT.Match(valid: (t) => Invalid(errF),
                    invalid: errT => Invalid(errF.Concat(errT))));

        public static Validation<Func<T2, R>> Apply<T1, T2, R>(this Validation<Func<T1, T2, R>> validF, Validation<T1> validT) =>
            Apply(validF.Map(f => f.Curry()), validT);

        public static Exceptional<Validation<R>> Traverse<T, R>(this Validation<T> validT, Func<T, Exceptional<R>> f)
            => validT.Match(
                invalid: errs => Exceptional((Validation<R>)Invalid(errs)),
                valid: t =>f(t).Map(r=> Valid(r)));

        public static Task<Validation<R>> TraverseBind<T, R>(this Validation<T> valid, Func<T, Task<Validation<R>>> func) =>
           valid.Match(
                   invalid: errs => Invalid<R>(errs).Async(),
                   valid: t => func(t));

    }
}
