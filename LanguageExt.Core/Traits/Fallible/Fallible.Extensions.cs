using System;
using LanguageExt.Common;

namespace LanguageExt.Traits;

/// <summary>
/// Trait for higher-kinded structures that have a failure state `E`
/// </summary>
/// <typeparam name="F">Higher-kinded structure</typeparam>
/// <typeparam name="E">Failure type</typeparam>
public static class FallibleExtensions
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 
    //  Error catching by predicate
    //
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Func<E, bool> Predicate,
        Func<E, K<F, A>> Fail) 
        where F : Fallible<E, F> =>
        F.Catch(fa, Predicate, Fail);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<F, A>> Fail) 
        where F : Fallible<F> =>
        F.Catch(fa, Predicate, Fail);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="ma">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<M, A> Catch<E, M, A>(
        this K<M, A> ma,
        Func<E, bool> Predicate,
        Func<E, K<IO, A>> Fail) 
        where M : Fallible<E, M>, Monad<M> =>
        M.Catch(ma, Predicate, e => M.LiftIO(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="ma">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<M, A> Catch<M, A>(
        this K<M, A> ma,
        Func<Error, bool> Predicate,
        Func<Error, K<IO, A>> Fail) 
        where M : Fallible<M>, Monad<M> =>
        M.Catch(ma, Predicate, e => M.LiftIO(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Func<E, bool> Predicate,
        Func<E, E> Fail) 
        where F : Fallible<E, F> =>
        F.Catch(fa, Predicate, e => F.Fail<A>(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, Error> Fail) 
        where F : Fallible<F> =>
        F.Catch(fa, Predicate, e => F.Fail<A>(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Func<E, bool> Predicate,
        Func<E, A> Fail) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, Predicate, e => F.Pure(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Predicate">Predicate to test any failure values</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, A> Fail) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, Predicate, e => F.Pure(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the catch structure and run its action if a match.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="@catch">Catch structure created by the `@catch` functions.  Contains the
    /// match predicate and action</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of running the catch structure's action if `fa` is in
    /// a failed state and the</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        CatchM<E, F, A> @catch) 
        where F : Fallible<E, F> =>
        F.Catch(fa, @catch.Match, @catch.Action);
        
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the catch structure and run its action if a match.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="@catch">Catch structure created by the `@catch` functions.  Contains the
    /// match predicate and action</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of running the catch structure's action if `fa` is in
    /// a failed state and the</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        CatchM<Error, F, A> @catch) 
        where F : Fallible<F> =>
        F.Catch(fa, @catch.Match, @catch.Action);

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 
    //  Error catching by equality
    //
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Error Match,
        Func<E, K<F, A>> Fail) 
        where F : Fallible<E, F> =>
        F.Catch(fa, e => Match.Equals(e), Fail);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Error Match,
        Func<Error, K<F, A>> Fail) 
        where F : Fallible<F> =>
        F.Catch(fa, e => e.Is(Match), Fail);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="ma">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<M, A> Catch<E, M, A>(
        this K<M, A> ma,
        Error Match,
        Func<E, K<IO, A>> Fail) 
        where M : Fallible<E, M>, Monad<M> =>
        M.Catch(ma, e => Match.Equals(e), e => M.LiftIO(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="ma">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<M, A> Catch<M, A>(
        this K<M, A> ma,
        Error Match,
        Func<Error, K<IO, A>> Fail) 
        where M : Fallible<M>, Monad<M> =>
        M.Catch(ma, e => e.Is(Match), e => M.LiftIO(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Error Match,
        Func<E, E> Fail) 
        where F : Fallible<E, F> =>
        F.Catch(fa, e => Match.Equals(e), e => F.Fail<A>(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Error Match,
        Func<Error, Error> Fail) 
        where F : Fallible<F> =>
        F.Catch(fa, e => e.Is(Match), e => F.Fail<A>(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Error Match,
        Func<E, A> Fail) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, e => Match.Equals(e), e => F.Pure(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Match">Error to match to</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Error Match,
        Func<Error, A> Fail) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, e => e.Is(Match), e => F.Pure(Fail(e)));
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 
    //  Catch all errors 
    //
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> 
        Catch<E, F, A>(
        this K<F, A> fa,
        Func<E, K<F, A>> Fail) 
        where F : Fallible<E, F> =>
        F.Catch(fa, _ => true, Fail);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Func<Error, K<F, A>> Fail) 
        where F : Fallible<F> =>
        F.Catch(fa, _ => true, Fail);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="ma">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<M, A> Catch<E, M, A>(
        this K<M, A> ma,
        Func<E, K<IO, A>> Fail) 
        where M : Fallible<E, M>, Monad<M> =>
        M.Catch(ma, _ => true, e => M.LiftIO(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="ma">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<M, A> Catch<M, A>(
        this K<M, A> ma,
        Func<Error, K<IO, A>> Fail) 
        where M : Fallible<M>, Monad<M> =>
        M.Catch(ma, _ => true, e => M.LiftIO(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Func<E, E> Fail) 
        where F : Fallible<E, F> =>
        F.Catch(fa, _ => true, e => F.Fail<A>(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Func<Error, Error> Fail) 
        where F : Fallible<F> =>
        F.Catch(fa, _ => true, e => F.Fail<A>(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<E, F, A>(
        this K<F, A> fa,
        Func<E, A> Fail) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, _ => true, e => F.Pure(Fail(e)));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, test the failure value
    /// against the predicate.  If, it returns `true`, run the `Fail` function with
    /// the failure value.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <param name="Fail">Handler when in failed state</param>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <returns>Either `fa` or the result of `Fail` if `fa` is in a failed state and the
    /// predicate returns true for the failure value</returns>
    public static K<F, A> Catch<F, A>(
        this K<F, A> fa,
        Func<Error, A> Fail) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, _ => true, e => F.Pure(Fail(e)));
    
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 
    //  Catch all errors and provide alternative values 
    //
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with `fail`.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<E, F, A>(this K<F, A> fa, Fail<E> fail) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Fail<A>(fail.Value));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with `fail`.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<F, A>(this K<F, A> fa, Fail<Error> fail) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Fail<A>(fail.Value));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with `fail`.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<E, F, A>(this K<F, A> fa, E fail) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Fail<A>(fail));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with `fail`.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<F, A>(this K<F, A> fa, Error fail) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Fail<A>(fail));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with
    /// the success value and cancelling the failure.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<E, F, A>(this K<F, A> fa, Pure<A> value) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Pure(value.Value));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with
    /// the success value and cancelling the failure.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<F, A>(this K<F, A> fa, Pure<A> value) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Pure(value.Value));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with
    /// the success value and cancelling the failure.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<E, F, A>(this K<F, A> fa, A value) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Pure(value));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with
    /// the success value and cancelling the failure.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<F, A>(this K<F, A> fa, A value) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => F.Pure(value));
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with
    /// the `alternative` computation, which may succeed or fail.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<E, F, A>(this K<F, A> fa, K<F, A> alternative) 
        where F : Fallible<E, F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => alternative);
    
    /// <summary>
    /// Run the `Fallible` structure.  If in a failed state, replace the failure value with
    /// the `alternative` computation, which may succeed or fail.
    /// </summary>
    /// <param name="fa">`Fallible` structure</param>
    /// <typeparam name="A">Bound value type</typeparam>
    public static K<F, A> Catch<F, A>(this K<F, A> fa, K<F, A> alternative) 
        where F : Fallible<F>, Applicative<F> =>
        F.Catch(fa, _ => true, _ => alternative);
}