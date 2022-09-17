using System;
using System.Threading.Tasks;
using Unit = System.ValueTuple;
using static CSharp.Functional.Functional;
using System.Runtime.InteropServices.ComTypes;
using CSharp.Functional.Constructs;

namespace CSharp.Functional.Extensions
{
    public static class TaskExtension
    {
        public static async Task<R> Map<T, R>(this Task<T> task, Func<T, R> f) =>
            f(await task);

        public static async Task<R> Bind<T,R>(this Task<T> task , Func<T,Task<R>> f)=>
            await f(await task);

        public static Task<T> Async<T>(this T t) =>
            Task.FromResult(t);

        public static async Task<RR> SelectMany<T, R, RR>
       (this Task<T> task, Func<T, Task<R>> bind, Func<T, R, RR> project)
        {
            T t = await task;
            R r = await bind(t);
            return project(t, r);
        }

        public static async Task<RR> SelectMany<R, RR>
           (this Task task, Func<Unit, Task<R>> bind, Func<Unit, R, RR> project)
        {
            await task;
            R r = await bind(Unit());
            return project(Unit(), r);
        }

        public static async Task<R> Select<T, R>(this Task<T> task, Func<T, R> f)
           => f(await task);


        public static Task<T> OrElse<T>(this Task<T> task, Func<Task<T>> fallback) =>
            task.ContinueWith(t => t.Status == TaskStatus.Faulted ? fallback() : t).Unwrap();

        public static Task<T> Recover<T>(this Task<T> task, Func<Exception, T> fallback) =>
            task.ContinueWith(t => t.Status == TaskStatus.Faulted ? fallback(t.Exception) : t.Result);

        public static Task<T> Retry<T>(this Func<Task<T>> start, int retries, int delayMillis) =>
            retries == 0
            ? start()
            : start().OrElse(() =>
                from _ in Task.Delay(delayMillis)
                from t in start.Retry((retries - 1), delayMillis*2)
                select t
                );


        public static Task<R> Map<T, R>(this Task<T> task, Func<Exception, R> faulted, Func<T, R> completed) =>
            task.ContinueWith(t =>
                t.Status == TaskStatus.Faulted ? faulted(t.Exception) : completed(t.Result));

        public static async Task<R> Apply<T,R>(this Task<Func<T,R>> f , Task<T> arg)=>
            (await f)(await arg);

        public static  Task<Func<T2, R>> Apply<T1, T2, R>(this Task<Func<T1, T2, R>> f, Task<T1> arg) =>
            Apply(f.Map(t => t.Curry()), arg);

        public static Task<Validation<R>> Select<T, R>(this Task<Validation<T>> self, Func<T, R> map) =>
            self.Map(v => v.Map(map));

        public static Task<Validation<R>> SelectMany<T, R>(this Task<Validation<T>> task, Func<T, Task<Validation<R>>> bind) =>
             task.Bind(v => v.TraverseBind(bind));


        public static Task<Validation<RR>> SelectMany<T, R, RR>(this Task<Validation<T>> task, Func<T, Task<Validation<R>>> bind, Func<T, R, RR> project)
            => task.Map(v => v.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r))))).Unwrap();


        public static Task<Option<R>> Select<T, R>(this Task<Option<T>> task, Func<T, R> map) =>
            task.Map(opt => opt.Map(map));

        public static Task<Option<R>> SelectMany<T, R>(this Task<Option<T>> task, Func<T, Task<Option<R>>> bind) =>
            task.Bind(opt => opt.TraverseBind(bind));

        public static Task<Option<RR>> SelectMany<T, R, RR>(this Task<Option<T>> task, Func<T, Task<Option<R>>> bind, Func<T, R, RR> project) =>
            task.Map(opt => opt.TraverseBind(t => bind(t).Map(or => or.Map(r => project(t, r))))).Unwrap();


    }
}
