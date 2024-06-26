﻿using System;
using LanguageExt.Traits;

namespace LanguageExt;

public static partial class WritableExtensions
{
    /// <summary>
    /// `pass` is an action that executes the `action`, which returns a value and a
    /// function; it then returns the value with the output having been applied to
    /// the function.
    /// </summary>
    public static K<M, A> Pass<M, W, A>(this K<M, (A Value, Func<W, W> Function)> action)
        where M : Writable<M, W>
        where W : Monoid<W> =>
        M.Pass(action);

    /// <summary>
    /// `listens` executes the action @m@ and adds the result of applying `f` to the
    /// output to the value of the computation.
    /// </summary>
    public static K<M, (A Value, B Output)> Listens<M, W, A, B>(this Func<W, B> f, K<M, A> ma)
        where M : Writable<M, W>, Monad<M>
        where W : Monoid<W> =>
        M.Bind(M.Listen(ma), aw => M.Pure((aw.Value, f(aw.Output))));

    /// <summary>
    /// `listens` executes the action @m@ and adds the result of applying `f` to the
    /// output to the value of the computation.
    /// </summary>
    public static K<M, (A Value, B Output)> Listens<M, W, A, B>(this K<M, A> ma, Func<W, B> f)
        where M : Writable<M, W>, Monad<M>
        where W : Monoid<W> =>
        M.Bind(M.Listen(ma), aw => M.Pure((aw.Value, f(aw.Output))));

    /// <summary>
    /// `censor` executes the action `ma` and applies the function `f` to its output,
    /// leaving the return value unchanged.
    /// </summary>
    public static K<M, A> Censor<M, W, A>(this K<M, A> ma, Func<W, W> f)
        where M : Writable<M, W>, Monad<M>
        where W : Monoid<W> =>
        M.Pass(M.Bind(ma, a => M.Pure((a, f))));

    /// <summary>
    /// `censor` executes the action `ma` and applies the function `f` to its output,
    /// leaving the return value unchanged.
    /// </summary>
    public static K<M, A> Censor<M, W, A>(this Func<W, W> f, K<M, A> ma)
        where M : Writable<M, W>, Monad<M>
        where W : Monoid<W> =>
        M.Pass(M.Bind(ma, a => M.Pure((a, f))));}
