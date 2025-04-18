﻿using System;
using System.Collections.Generic;
using static LanguageExt.Prelude;
using System.Diagnostics.Contracts;
using LanguageExt.ClassInstances;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LanguageExt.Traits;

namespace LanguageExt;

/// <summary>
/// Like `Either` but collects the failed values
/// </summary>
/// <typeparam name="MonoidFail"></typeparam>
/// <typeparam name="F"></typeparam>
/// <typeparam name="A"></typeparam>
[Serializable]
public abstract record Validation<F, A> : 
    IEnumerable<A>,
    IComparable<Validation<F, A>>,
    IComparable<A>,
    IComparable,
    IEquatable<Pure<A>>,
    IComparable<Pure<A>>,
    IEquatable<A>, 
    Fallible<Validation<F, A>, Validation<F>, F, A>
    where F : Monoid<F>
{
    [Pure]
    public static Validation<F, A> Success(A value) =>
        new Validation.Success<F, A>(value);

    [Pure]
    public static Validation<F, A> Fail(F value) =>
        new Validation.Fail<F, A>(value);

    /// <summary>
    /// Is the Validation in a Success state?
    /// </summary>
    [Pure]
    public abstract bool IsSuccess { get; }

    /// <summary>
    /// Is the Validation in a Fail state?
    /// </summary>
    [Pure]
    public abstract bool IsFail { get; }

    /// <summary>
    /// Invokes the Success or Fail function depending on the state of the Validation
    /// </summary>
    /// <typeparam name="B">Return type</typeparam>
    /// <param name="Fail">Function to invoke if in a Fail state</param>
    /// <param name="Succ">Function to invoke if in a Success state</param>
    /// <returns>The return value of the invoked function</returns>
    [Pure]
    public abstract B Match<B>(Func<A, B> Succ, Func<F, B> Fail);

    /// <summary>
    /// Empty span
    /// </summary>
    [Pure]
    public abstract ReadOnlySpan<F> FailSpan();

    /// <summary>
    /// Span of right value
    /// </summary>
    [Pure]
    public abstract ReadOnlySpan<A> SuccessSpan();

    /// <summary>
    /// Compare this structure to another to find its relative ordering
    /// </summary>
    [Pure]
    public abstract int CompareTo<OrdF, OrdA>(Validation<F, A> other)
        where OrdF : Ord<F>
        where OrdA : Ord<A>;

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public abstract bool Equals<EqF, EqA>(Validation<F, A> other)
        where EqF : Eq<F>
        where EqA : Eq<A>;

    /// <summary>
    /// Unsafe access to the right-value 
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    internal abstract A SuccessValue { get; }

    /// <summary>
    /// Unsafe access to the left-value 
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    internal abstract F FailValue { get; }

    /// <summary>
    /// Maps the value in the Validation if it's in a Success state
    /// </summary>
    /// <typeparam name="F">Fail</typeparam>
    /// <typeparam name="A">Success</typeparam>
    /// <typeparam name="B">Mapped Validation type</typeparam>
    /// <param name="f">Map function</param>
    /// <returns>Mapped Validation</returns>
    [Pure]
    public abstract Validation<F, B> Map<B>(Func<A, B> f);

    /// <summary>
    /// Bi-maps the value in the Validation if it's in a Success state
    /// </summary>
    /// <typeparam name="F">Fail</typeparam>
    /// <typeparam name="A">Success</typeparam>
    /// <typeparam name="F1">Fail return</typeparam>
    /// <typeparam name="A1">Success return</typeparam>
    /// <param name="Succ">Success map function</param>
    /// <param name="Fail">Fail map function</param>
    /// <returns>Mapped Validation</returns>
    [Pure]
    public abstract Validation<F1, A1> BiMap<F1, A1>(Func<A, A1> Succ, Func<F, F1> Fail)
        where F1 : Monoid<F1>;

    /// <summary>
    /// Monadic bind
    /// </summary>
    /// <typeparam name="F">Fail</typeparam>
    /// <typeparam name="A">Success</typeparam>
    /// <typeparam name="B">Resulting bound value</typeparam>
    /// <param name="f">Bind function</param>
    /// <returns>Bound Validation</returns>
    [Pure]
    public abstract Validation<F, B> Bind<B>(Func<A, Validation<F, B>> f);

    /// <summary>
    /// Bi-bind.  Allows mapping of both monad states
    /// </summary>
    [Pure]
    public abstract Validation<F1, A1> BiBind<F1, A1>(
        Func<A, Validation<F1, A1>> Succ,
        Func<F, Validation<F1, A1>> Fail)
        where F1 : Monoid<F1>;

    /// <summary>
    /// Bind the failure
    /// </summary>
    [Pure]
    public Validation<F1, A> BindFail<F1>(
        Func<F, Validation<F1, A>> Fail)
        where F1 : Monoid<F1> =>
        BiBind(Validation<F1, A>.Success, Fail);

    /// <summary>
    /// Monoid empty
    /// </summary>
    [Pure]
    public static Validation<F, A> Empty { get; } = 
        new Validation.Fail<F, A>(F.Empty);

    /// <summary>
    /// Explicit conversion operator from `` to `R`
    /// </summary>
    /// <param name="value">Value, must not be null.</param>
    /// <exception cref="InvalidCastException">Value is not in a Right state</exception>
    [Pure]
    public static explicit operator A(Validation<F, A> ma) =>
        ma.SuccessValue;

    /// <summary>
    /// Explicit conversion operator from `Validation` to `L`
    /// </summary>
    /// <param name="value">Value, must not be null.</param>
    /// <exception cref="InvalidCastException">Value is not in a Fail state</exception>
    [Pure]
    public static explicit operator F(Validation<F, A> ma) =>
        ma.FailValue;

    /// <summary>
    /// Implicit conversion operator from `A` to `Validation〈F, A〉`
    /// </summary>
    [Pure]
    public static implicit operator Validation<F, A>(A value) =>
        new Validation.Success<F, A>(value);

    /// <summary>
    /// Implicit conversion operator from `F` to `Validation〈F, A〉`
    /// </summary>
    [Pure]
    public static implicit operator Validation<F, A>(F value) =>
        new Validation.Fail<F, A>(value);

    /// <summary>
    /// Invokes the `Succ` or `Fail` action depending on the state of the value
    /// </summary>
    /// <param name="Succ">Action to invoke if in a Success state</param>
    /// <param name="Fail">Action to invoke if in a Fail state</param>
    /// <returns>Unit</returns>
    public Unit Match(Action<A> Succ, Action<F> Fail) =>
        Match(fun(Succ), fun(Fail));

    /// <summary>
    /// Executes the `Fail` function if the value is in a Fail state.
    /// Returns the Success value if the value is in a Success state.
    /// </summary>
    /// <param name="Fail">Function to generate a value if in the Fail state</param>
    /// <returns>Returns an unwrapped value</returns>
    [Pure]
    public A IfFail(Func<A> Fail) =>
        Match(identity, _ => Fail());

    /// <summary>
    /// Executes the `failMap` function if the value is in a Fail state.
    /// Returns the Success value if in a Success state.
    /// </summary>
    /// <param name="failMap">Function to generate a value if in the Fail state</param>
    /// <returns>Returns an unwrapped value</returns>
    [Pure]
    public A IfFail(Func<F, A> failMap) =>
        Match(identity, failMap);

    /// <summary>
    /// Returns the `successValue` if in a Fail state.
    /// Returns the Success value if in a Success state.
    /// </summary>
    /// <param name="successValue">Value to return if in the Fail state</param>
    /// <returns>Returns an unwrapped value</returns>
    [Pure]
    public A IfFail(A successValue) =>
        Match(identity, _ => successValue);

    /// <summary>
    /// Executes the Fail action if in a Fail state.
    /// </summary>
    /// <param name="Fail">Function to generate a Success value if in the Fail state</param>
    /// <returns>Returns unit</returns>
    public Unit IfFail(Action<F> Fail) =>
        Match(_ => { }, Fail);

    /// <summary>
    /// Invokes the `Success` action if in a Success state, otherwise does nothing
    /// </summary>
    /// <param name="Success">Action to invoke</param>
    /// <returns>Unit</returns>
    public Unit IfRight(Action<A> Success) =>
        Match(Success, _ => { });

    /// <summary>
    /// Match Success and return a context.  You must follow this with `.Fail(...)` to complete the match
    /// </summary>
    /// <param name="success">Action to invoke if in a Success state</param>
    /// <returns>Context that must have `Fail()` called upon it.</returns>
    [Pure]
    public ValidationUnitContext<F, A> Success(Action<A> success) =>
        new (this, success);

    /// <summary>
    /// Match Success and return a context.  You must follow this with `.Fail(...)` to complete the match
    /// </summary>
    /// <param name="success">Action to invoke if in a Success state</param>
    /// <returns>Context that must have `Fail()` called upon it.</returns>
    [Pure]
    public ValidationContext<F, A, B> Success<B>(Func<A, B> success) =>
        new (this, success);

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();

    [Pure]
    public int CompareTo(object? obj) =>
        obj is Validation<F, A> t ? CompareTo(t) : 1;

    [Pure]
    public IEnumerator<A> GetEnumerator()
    {
        foreach (var x in SuccessSpan().ToArray()) 
            yield return x;
    }

    /// <summary>
    /// Project the value into a `Lst〈F〉`
    /// </summary>
    /// <returns>If in a Fail state, a `Lst` of `L` with one item.  A zero length `Lst` of `L` otherwise</returns>
    [Pure]
    public Lst<F> FailToList() =>
        new(FailSpan());

    /// <summary>
    /// Project into an `Arr〈F〉`
    /// </summary>
    /// <returns>If in a Fail state, a `Arr` of `L` with one item.  A zero length `Arr` of `L` otherwise</returns>
    [Pure]
    public Arr<F> FailToArray() =>
        new(FailSpan());

    /// <summary>
    /// Project into a `Lst〈A〉`
    /// </summary>
    /// <returns>If in a Success state, a `Lst` of `R` with one item.  A zero length `Lst` of `R` otherwise</returns>
    public Lst<A> ToList() =>
        new(SuccessSpan());

    /// <summary>
    /// Project into an `Arr〈A〉`
    /// </summary>
    /// <returns>If in a Success state, an `Arr` of `R` with one item.  A zero length `Arr` of `R` otherwise</returns>
    public Arr<A> ToArray() =>
        new(SuccessSpan());

    /// <summary>
    /// Convert to sequence of 0 or 1 success values
    /// </summary>
    [Pure]
    public Seq<A> ToSeq() =>
        new(SuccessSpan());

    /// <summary>
    /// Convert either to sequence of 0 or 1 left values
    /// </summary>
    [Pure]
    public Seq<F> FailToSeq() =>
        new(FailSpan());

    [Pure]
    public Either<F, A> ToEither() =>
        this switch
        {
            Validation.Success<F, A> (var x) => new Either.Right<F, A>(x),
            Validation.Fail<F, A> (var x)    => new Either.Left<F, A>(x),
            _ => throw new NotSupportedException()
        };

    /// <summary>
    /// Convert to an Option
    /// </summary>
    /// <returns>Some(Right) or None</returns>
    [Pure]
    public Option<A> ToOption() =>
        this switch
        {
            Validation.Success<F, A> (var x) => Option<A>.Some(x),
            Validation.Fail<F, A>            => Option<A>.None,
            _                                => throw new NotSupportedException()
        };

    /*
    /// <summary>
    /// Convert to a stream
    /// </summary>
    [Pure]
    public StreamT<M, A> ToStream<M>() 
        where M : Monad<M> =>
        IsSuccess
            ? StreamT<M, A>.Pure(SuccessValue) 
            : StreamT<M, A>.Empty;

    /// <summary>
    /// Convert to a stream
    /// </summary>
    [Pure]
    public StreamT<M, F> FailToStream<M>() 
        where M : Monad<M> =>
        IsFail
            ? StreamT<M, F>.Pure(FailValue) 
            : StreamT<M, F>.Empty;

    */
    /// <summary>
    /// Action operator
    /// </summary>
    [Pure]
    public static Validation<F, A> operator >>(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.Action(rhs);
    
    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈 rhs</returns>
    [Pure]
    public static bool operator <(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈= rhs</returns>
    [Pure]
    public static bool operator <=(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉rhs</returns>
    [Pure]
    public static bool operator >(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉= rhs</returns>
    [Pure]
    public static bool operator >=(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈 rhs</returns>
    [Pure]
    public static bool operator <(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈= rhs</returns>
    [Pure]
    public static bool operator <=(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉rhs</returns>
    [Pure]
    public static bool operator >(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉= rhs</returns>
    [Pure]
    public static bool operator >=(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.CompareTo(rhs) >= 0;


    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈 rhs</returns>
    [Pure]
    public static bool operator <(Fail<F> lhs, Validation<F, A> rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) < 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈= rhs</returns>
    [Pure]
    public static bool operator <=(Fail<F>  lhs, Validation<F, A> rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) <= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉rhs</returns>
    [Pure]
    public static bool operator >(Fail<F> lhs, Validation<F, A>rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) > 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉= rhs</returns>
    [Pure]
    public static bool operator >=(Fail<F> lhs, Validation<F, A>  rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) >= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈 rhs</returns>
    [Pure]
    public static bool operator <(Pure<A> lhs, Validation<F, A>  rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) < 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈= rhs</returns>
    [Pure]
    public static bool operator <=(Pure<A> lhs, Validation<F, A> rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) <= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉rhs</returns>
    [Pure]
    public static bool operator >(Pure<A> lhs, Validation<F, A> rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) > 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉= rhs</returns>
    [Pure]
    public static bool operator >=(Pure<A> lhs, Validation<F, A> rhs) =>
        ((Validation<F, A>)lhs).CompareTo(rhs) >= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈 rhs</returns>
    [Pure]
    public static bool operator <(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs〈= rhs</returns>
    [Pure]
    public static bool operator <=(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉rhs</returns>
    [Pure]
    public static bool operator >(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Comparison operator
    /// </summary>
    /// <param name="lhs">The left hand side of the operation</param>
    /// <param name="rhs">The right hand side of the operation</param>
    /// <returns>True if lhs 〉= rhs</returns>
    [Pure]
    public static bool operator >=(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Equality operator override
    /// </summary>
    [Pure]
    public static bool operator ==(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    /// Equality operator override
    /// </summary>
    [Pure]
    public static bool operator ==(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    /// Equality operator override
    /// </summary>
    [Pure]
    public static bool operator ==(Fail<F>  lhs, Validation<F, A> rhs) =>
        ((Validation<F, A>)lhs).Equals(rhs);

    /// <summary>
    /// Equality operator override
    /// </summary>
    [Pure]
    public static bool operator ==(Pure<A> lhs, Validation<F, A>  rhs) =>
        ((Validation<F, A>)lhs).Equals(rhs);
        
    /// <summary>
    /// Non-equality operator override
    /// </summary>
    [Pure]
    public static bool operator !=(Validation<F, A> lhs, Fail<F> rhs) =>
        !(lhs == rhs);

    /// <summary>
    /// Non-equality operator override
    /// </summary>
    [Pure]
    public static bool operator !=(Validation<F, A> lhs, Pure<A> rhs) =>
        !(lhs == rhs);


    /// <summary>
    /// Non-equality operator override
    /// </summary>
    [Pure]
    public static bool operator !=(Fail<F> lhs, Validation<F, A> rhs) =>
        !(lhs == rhs);

    /// <summary>
    /// Non-equality operator override
    /// </summary>
    [Pure]
    public static bool operator !=(Pure<A> lhs, Validation<F, A> rhs) =>
        !(lhs == rhs);

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator +(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.Combine(rhs).As();

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator +(K<Validation<F>, A> lhs, Validation<F, A> rhs) =>
        lhs.Combine(rhs).As();

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator +(Validation<F, A> lhs, K<Validation<F>, A> rhs) =>
        lhs.Combine(rhs.As()).As();

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    public static Validation<F, A> operator +(Validation<F, A> lhs, A rhs) => 
        lhs.Combine(Success(rhs)).As();

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator +(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.Combine(Success(rhs.Value)).As();

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator +(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.Combine(Fail(rhs.Value)).As();

    /// <summary>
    /// Combine operator: uses the underlying `F.Combine` to collect failures
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator +(Validation<F, A> lhs, F rhs) =>
        lhs.Combine(Fail(rhs)).As();    
    
    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator |(Validation<F, A> lhs, Validation<F, A> rhs) =>
        lhs.Choose(rhs).As();

    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator |(K<Validation<F>, A> lhs, Validation<F, A> rhs) =>
        lhs.Choose(rhs).As();

    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator |(Validation<F, A> lhs, K<Validation<F>, A> rhs) =>
        lhs.Choose(rhs.As()).As();

    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    public static Validation<F, A> operator |(Validation<F, A> lhs, A rhs) => 
        lhs.Choose(Success(rhs)).As();

    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator |(Validation<F, A> lhs, Pure<A> rhs) =>
        lhs.Choose(Success(rhs.Value)).As();

    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator |(Validation<F, A> lhs, Fail<F> rhs) =>
        lhs.Choose(Fail(rhs.Value)).As();

    /// <summary>
    /// Choice operator: returns the first argument to succeed.  If both fail, then the last failure is returned.
    /// </summary>
    [Pure, MethodImpl(Opt.Default)]
    public static Validation<F, A> operator |(Validation<F, A> lhs, F rhs) =>
        lhs.Choose(Fail(rhs)).As();

    /// <summary>
    /// Catch operator: returns the first argument if it to succeeds. Otherwise, the `F` failure is mapped.
    /// </summary>
    public static Validation<F, A> operator |(Validation<F, A> lhs, CatchM<F, Validation<F>, A> rhs) =>
        lhs.Catch(rhs).As();
    
    /// <summary>
    /// If any items are Fail then the errors are collected and returned.  If they
    /// all pass then the Success values are collected into a `Seq`.  
    /// </summary>
    [Pure]
    public static Validation<F, Seq<A>> operator &(Validation<F, A> lhs, Validation<F, A> rhs) =>
        (lhs, rhs) switch
        {
            ({ IsSuccess: true } , { IsSuccess: true }) => 
                Validation<F, Seq<A>>.Success([lhs.SuccessValue, rhs.SuccessValue]),
            
            ({ IsFail: true } , {IsFail: true}) => 
                lhs.FailValue.Combine(rhs.FailValue),
            
            ({ IsFail: true } , _) => 
                lhs.FailValue,
            
            _ => 
                rhs.FailValue
        };
    
    /// <summary>
    /// If any items are Fail then the errors are collected and returned.  If they
    /// all pass then the Success values are collected into a `Seq`.  
    /// </summary>
    [Pure]
    public static Validation<F, Seq<A>> operator &(Validation<F, Seq<A>> lhs, Validation<F, A> rhs) =>
        (lhs, rhs) switch
        {
            ({ IsSuccess: true } , { IsSuccess: true }) => 
                Validation<F, Seq<A>>.Success(lhs.SuccessValue.Add(rhs.SuccessValue)),
            
            ({ IsFail: true } , {IsFail: true}) => 
                lhs.FailValue.Combine(rhs.FailValue),
            
            ({ IsFail: true } , _) => 
                lhs.FailValue,
            
            _ => 
                rhs.FailValue
        };
    
    /// <summary>
    /// If any items are Fail then the errors are collected and returned.  If they
    /// all pass then the Success values are collected into a `Seq`.  
    /// </summary>
    [Pure]
    public static Validation<F, Seq<A>> operator &(Validation<F, A> lhs, Validation<F, Seq<A>> rhs) =>
        (lhs, rhs) switch
        {
            ({ IsSuccess: true } , { IsSuccess: true }) => 
                Validation<F, Seq<A>>.Success(lhs.SuccessValue.Cons(rhs.SuccessValue)),
            
            ({ IsFail: true } , {IsFail: true}) => 
                lhs.FailValue.Combine(rhs.FailValue),
            
            ({ IsFail: true } , _) => 
                lhs.FailValue,
            
            _ => 
                rhs.FailValue
        };

    /// <summary>
    /// Override of the True operator to return True if the Either is Right
    /// </summary>
    [Pure]
    public static bool operator true(Validation<F, A> value) =>
        value.IsSuccess;

    /// <summary>
    /// Override of the False operator to return True if the Either is Left
    /// </summary>
    [Pure]
    public static bool operator false(Validation<F, A> value) =>
        value.IsFail;

    /// <summary>
    /// CompareTo override
    /// </summary>
    [Pure]
    public int CompareTo(Validation<F, A>? other) =>
        other is null
            ? 1
            : CompareTo<OrdDefault<F>, OrdDefault<A>>(other);

    /// <summary>
    /// CompareTo override
    /// </summary>
    [Pure]
    public int CompareTo<OrdR>(Validation<F, A> other)
        where OrdR : Ord<A> =>
        CompareTo<OrdDefault<F>, OrdR>(other);

    /// <summary>
    /// CompareTo override
    /// </summary>
    [Pure]
    public int CompareTo(Fail<F> other) =>
        CompareTo((Validation<F, A>)other);

    /// <summary>
    /// CompareTo override
    /// </summary>
    [Pure]
    public int CompareTo(Pure<A> other) =>
        CompareTo((Validation<F, A>)other);

    /// <summary>
    /// CompareTo override
    /// </summary>
    [Pure]
    public int CompareTo(A? other) =>
        other switch
        {
            null => 1,
            _    => CompareTo(Success(other))
        };

    /// <summary>
    /// CompareTo override
    /// </summary>
    [Pure]
    public int CompareTo(F? other) =>
        other switch
        {
            null => 1,
            _    => CompareTo(Fail(other))
        };

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public bool Equals(A? other) =>
        other is not null && Equals(Success(other));

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public bool Equals(F? other) =>
        other is not null && Equals(Fail(other));

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public virtual bool Equals(Validation<F, A>? other) =>
        other is not null && Equals<EqDefault<F>, EqDefault<A>>(other);

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public virtual bool Equals<EqR>(Validation<F, A> other) where EqR : Eq<A> =>
        Equals<EqDefault<F>, EqR>(other);

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public bool Equals(Fail<F> other) =>
        Equals((Validation<F, A>)other);

    /// <summary>
    /// Equality override
    /// </summary>
    [Pure]
    public bool Equals(Pure<A> other) =>
        Equals((Validation<F, A>)other);

    /// <summary>
    /// Match the Success and Fail values but as untyped objects.
    /// </summary>
    [Pure]
    public B MatchUntyped<B>(Func<object?, B> Succ, Func<object?, B> Fail) =>
        Match(x => Succ(x), x => Fail(x));

    /// <summary>
    /// Iterate the value
    /// action is invoked if in the Success state
    /// </summary>
    public Unit Iter(Action<A> Succ) =>
        Match(Succ, _ => { });

    /// <summary>
    /// Invokes a predicate on the success value if it's in the Success state
    /// </summary>
    /// <returns>
    /// True if in a `Left` state.  
    /// `True` if the in a `Right` state and the predicate returns `True`.  
    /// `False` otherwise.</returns>
    [Pure]
    public bool ForAll(Func<A, bool> Succ) =>
        Match(Succ, _ => true);

    /// <summary>
    /// Invokes a predicate on the values 
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="self">Either to forall</param>
    /// <param name="Succ">Predicate</param>
    /// <param name="Fail">Predicate</param>
    /// <returns>True if either Predicate returns true</returns>
    [Pure]
    public bool BiForAll(Func<A, bool> Succ, Func<F, bool> Fail) =>
        Match(Succ, Fail);

    /// <summary>
    /// Validation types are like lists of 0 or 1 items and therefore follow the 
    /// same rules when folding.
    /// </summary>
    /// <typeparam name="S">Aggregate state type</typeparam>
    /// <param name="state">Initial state</param>
    /// <param name="Succ">Folder function, applied if structure is in a Success state</param>
    /// <returns>The aggregate state</returns>
    [Pure]
    public S Fold<S>(S state, Func<S, A, S> Succ) =>
        Match(curry(Succ)(state), _ => state);

    /// <summary>
    /// Either types are like lists of 0 or 1 items, and therefore follow the 
    /// Validation types are like lists of 0 or 1 items and therefore follow the 
    /// same rules when folding.
    /// </summary>
    /// <typeparam name="S">Aggregate state type</typeparam>
    /// <param name="state">Initial state</param>
    /// <param name="Succ">Folder function, applied if in a Success state</param>
    /// <param name="Fail">Folder function, applied if in a Fail state</param>
    /// <returns>The aggregate state</returns>
    [Pure]
    public S BiFold<S>(S state, Func<S, A, S> Succ, Func<S, F, S> Fail) =>
        Match(curry(Succ)(state), curry(Fail)(state));

    /// <summary>
    /// Invokes a predicate on the value if it's in the Success state
    /// </summary>
    /// <param name="pred">Predicate</param>
    /// <returns>True if in a Success state and the predicate returns `True`.  `False` otherwise.</returns>
    [Pure]
    public bool Exists(Func<A, bool> pred) =>
        Match(pred, _ => false);

    /// <summary>
    /// Impure iteration of the bound values in the structure
    /// </summary>
    /// <returns>
    /// Returns the original unmodified structure
    /// </returns>
    public Validation<F, A> Do(Action<A> f) =>
        Map(r => { f(r); return r; });
    
    /// <summary>
    /// Map each element of a structure to an action, evaluate these actions from
    /// left to right, and collect the results.
    /// </summary>
    /// <param name="f"></param>
    /// <param name="ta">Traversable structure</param>
    /// <typeparam name="F">Applicative functor trait</typeparam>
    /// <typeparam name="B">Bound value (output)</typeparam>
    [Pure]
    public K<AF, Validation<F, B>> Traverse<AF, B>(Func<A, K<AF, B>> f) 
        where AF : Applicative<AF> =>
        AF.Map(x => x.As(), Traversable.traverse(f, this));

    /// <summary>
    /// Maps the value in the Either if it's in a Left state
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <typeparam name="F1">Mapped Either type</typeparam>
    /// <param name="f">Map function</param>
    /// <returns>Mapped Either</returns>
    [Pure]
    public Validation<F1, A> MapFail<F1>(Func<F, F1> f)
        where F1 : Monoid<F1> =>
        Match(Validation<F1, A>.Success, e => Validation<F1, A>.Fail(f(e)));

    /// <summary>
    /// Monadic bind
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="f"></param>
    /// <returns>Bound Either</returns>
    [Pure]
    public Validation<F, B> Bind<B>(Func<A, K<Validation<F>, B>> f) =>
        Bind(x => (Validation<F, B>)f(x));

    /// <summary>
    /// Filter the Validation
    /// </summary>
    /// <remarks>
    /// If the predicate returns `false` then the `Validation` goes into a failed state
    /// using `Monoid.Empty` of `F` as its failure value.
    /// </remarks>
    [Pure]
    public Validation<F, A> Filter(Func<A, bool> pred) =>
        Bind(x => pred(x) ? Success(x) : Fail(F.Empty));

    /// <summary>
    /// Filter the Validation
    /// </summary>
    /// <remarks>
    /// If the predicate returns `false` then the `Validation` goes into a failed state
    /// using `Monoid.Empty` of `F` as its failure value.
    /// </remarks>
    [Pure]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Validation<F, A> Where(Func<A, bool> pred) =>
        Filter(pred);

    /// <summary>
    /// Maps the bound value 
    /// </summary>
    [Pure]
    public Validation<F, B> Select<B>(Func<A, B> f) =>
        Map(f);

    /// <summary>
    /// Monadic bind function
    /// </summary>
    [Pure]
    public Validation<F, B> SelectMany<S, B>(Func<A, Validation<F, S>> bind, Func<A, S, B> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
        
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 
    // `Pure` and `Fail` support
    //

    /// <summary>
    /// Monadic bind
    /// </summary>
    /// <param name="f">Bind function</param>
    [Pure]
    public Validation<F, B> Bind<B>(Func<A, Pure<B>> f) =>
        Bind(x => (Validation<F, B>)f(x));

    /// <summary>
    /// Monadic bind
    /// </summary>
    /// <param name="f">Bind function</param>
    [Pure]
    public Validation<F, A> Bind(Func<A, Fail<F>> f) =>
        Bind(x => (Validation<F, A>)f(x));

    /// <summary>
    /// Monadic bind
    /// </summary>
    /// <param name="f">Bind function</param>
    [Pure]
    public Validation<F, Unit> Bind(Func<A, Guard<F, Unit>> f)=>
        Bind(a => f(a).ToValidation());

    /// <summary>
    /// Monadic bind and project
    /// </summary>
    /// <param name="bind">Bind function</param>
    /// <param name="project">Project function</param>
    [Pure]
    public Validation<F, C> SelectMany<B, C>(Func<A, Pure<B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
    
    /// <summary>
    /// Monadic bind and project
    /// </summary>
    /// <param name="bind">Bind function</param>
    /// <param name="project">Project function</param>
    [Pure]
    public Validation<F, C> SelectMany<B, C>(Func<A, Fail<F>> bind, Func<A, B, C> _) =>
        Bind(x => Validation<F, C>.Fail(bind(x).Value));
    
    /// <summary>
    /// Monadic bind and project
    /// </summary>
    /// <param name="bind">Bind function</param>
    /// <param name="project">Project function</param>
    [Pure]
    public Validation<F, C> SelectMany<C>(
        Func<A, Guard<F, Unit>> f,
        Func<A, Unit, C> project) =>
        SelectMany(a => f(a).ToValidation(), project);
    
    [Pure]
    public static implicit operator Validation<F, A>(Pure<A> mr) =>
        Success(mr.Value);

    [Pure]
    public static implicit operator Validation<F, A>(Fail<F> mr) =>
        Fail(mr.Value);

    public override int GetHashCode()
    {
        return HashCode.Combine(IsSuccess, IsFail, SuccessValue, FailValue);
    }
}

/// <summary>
/// Context for the fluent Either matching
/// </summary>
public struct ValidationContext<F, A, B>
    where F : Monoid<F>
{
    readonly Validation<F, A> validation;
    readonly Func<A, B> success;

    internal ValidationContext(Validation<F, A> validation, Func<A, B> success)
    {
        this.validation = validation;
        this.success = success;
    }

    /// <summary>
    /// Fail match
    /// </summary>
    /// <param name="Fail"></param>
    /// <returns>Result of the match</returns>
    [Pure]
    public B Fail(Func<F, B> fail) =>
        validation.Match(success, fail);
}

/// <summary>
/// Context for the fluent Validation matching
/// </summary>
public struct ValidationUnitContext<F, A>
    where F : Monoid<F>
{
    readonly Validation<F, A> validation;
    readonly Action<A> success;

    internal ValidationUnitContext(Validation<F, A> validation, Action<A> success)
    {
        this.validation = validation;
        this.success = success;
    }

    public Unit Left(Action<F> fail) =>
        validation.Match(success, fail);
}
