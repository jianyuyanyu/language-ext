using System;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace LanguageExt;

public static partial class Prelude
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    //  Error specific
    //

    /// <summary>
    /// Catch all `Error` errors of subtype `E` and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> catchOf<E, M, A>(Func<E, K<M, A>> Fail)
        where E : Error
        where M : Fallible<Error, M>, Monad<M> =>
        matchError<Error, M, A>(e => e is E || e.IsType<E>(),
                                es => es is E e 
                                          ? Fail(e) 
                                          : es.Filter<E>().ForAllM(e => Fail((E)e))); 

    /// <summary>
    /// Catch all `Error` errors of subtype `E` and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> catchOfFold<E, M, A>(Func<E, K<M, A>> Fail)
        where E : Error
        where M : Fallible<Error, M>, MonoidK<M> =>
        matchError<Error, M, A>(e => e is E || e.IsType<E>(),
                                es => es is E e 
                                          ? Fail(e) 
                                          : es.Filter<E>().FoldM(e => Fail((E)e))); 

    /// <summary>
    /// Catch an error if the error matches the argument provided 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(Error error, Func<Error, K<M, A>> Fail) 
        where M : Fallible<Error, M> =>
        matchError(error.Is, Fail);
    
    /// <summary>
    /// Catch an error if the error matches the argument provided 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(Error error, K<M, A> Fail)
        where M : Fallible<Error, M> =>
        matchError(error.Is, (Error _) => Fail);
    
    /// <summary>
    /// Catch an error if the error matches the argument provided 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(int errorCode, Func<Error, K<M, A>> Fail)
        where M : Fallible<Error, M> =>
        matchError(e => e.Code == errorCode || e.HasCode(errorCode), Fail);
    
    /// <summary>
    /// Catch an error if the error matches the argument provided 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(int errorCode, K<M, A> Fail)
        where M : Fallible<Error, M> =>
        matchError(e => e.Code == errorCode || e.HasCode(errorCode), (Error _) => Fail);
    
    /// <summary>
    /// Catch an error if the error matches the argument provided 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(Func<Error, bool> predicate, Func<Error, K<M, A>> Fail)
        where M : Fallible<Error, M> =>
        matchError(predicate, Fail);
    
    /// <summary>
    /// Catch an error if the error matches the argument provided 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(Func<Error, bool> predicate, K<M, A> Fail) 
        where M : Fallible<Error, M> =>
        matchError(predicate, _ => Fail);
    
    /// <summary>
    /// Catch all errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(Func<Error, K<M, A>> Fail) 
        where M : Fallible<Error, M> =>
        matchError(static _ => true, Fail);
    
    /// <summary>
    /// Catch all errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> @catch<M, A>(K<M, A> Fail) 
        where M : Fallible<Error, M> =>
        matchError(static _ => true, (Error _) => Fail);
    
    
    /// <summary>
    /// Catch all `Expected` errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> expected<M, A>(Func<Expected, K<M, A>> Fail) 
        where M : Fallible<Error, M>, Monad<M> =>
        catchOf(Fail);
    
    /// <summary>
    /// Catch all `Expected` errors of subtype `E` and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> expectedOf<E, M, A>(Func<E, K<M, A>> Fail)
        where E : Expected 
        where M : Fallible<Error, M>, Monad<M> =>
        catchOf(Fail);
    
    /// <summary>
    /// Catch all `Exceptional` errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> exceptional<M, A>(Func<Exceptional, K<M, A>> Fail) 
        where M : Fallible<Error, M>, Monad<M> =>
        catchOf(Fail);
    
    /// <summary>
    /// Catch all errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> exceptionalOf<E, M, A>(Func<E, K<M, A>> Fail) 
        where E : Exceptional 
        where M : Fallible<Error, M>, Monad<M> =>
        catchOf(Fail);
    
    /// <summary>
    /// Catch all `Expected` errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> expectedFold<M, A>(Func<Expected, K<M, A>> Fail) 
        where M : Fallible<Error, M>, MonoidK<M> =>
        catchOfFold(Fail);
    
    /// <summary>
    /// Catch all `Expected` errors of subtype `E` and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> expectedOfFold<E, M, A>(Func<E, K<M, A>> Fail)
        where E : Expected 
        where M : Fallible<Error, M>, MonoidK<M> =>
        catchOfFold(Fail);
    
    /// <summary>
    /// Catch all `Exceptional` errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> exceptionalFold<M, A>(Func<Exceptional, K<M, A>> Fail) 
        where M : Fallible<Error, M>, MonoidK<M> =>
        catchOfFold(Fail);
    
    /// <summary>
    /// Catch all errors and return Fail 
    /// </summary>
    public static CatchM<Error, M, A> exceptionalOfFold<E, M, A>(Func<E, K<M, A>> Fail) 
        where E : Exceptional 
        where M : Fallible<Error, M>, MonoidK<M> =>
        catchOfFold(Fail);    
}
