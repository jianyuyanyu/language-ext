﻿using System;
using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace LanguageExt
{
    public static class OptionTExtensions
    {
        public static Option<Arr<B>> Traverse<A, B>(this Arr<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }

            return Option<Arr<B>>.Some(new Arr<B>(res));
        }

        public static Option<Either<L, B>> Traverse<L, A, B>(this Either<L, Option<A>> ma, Func<A, B> f)
        {
            if (ma.IsLeft || ma.RightValue.IsNone)
            {
                return None;
            }
            else
            {
                return Option<Either<L, B>>.Some(f(ma.RightValue.Value));
            }
        }
        
        public static Option<EitherUnsafe<L, B>> Traverse<L, A, B>(this EitherUnsafe<L, Option<A>> ma, Func<A, B> f)
        {
            if (ma.IsLeft || ma.RightValue.IsNone)
            {
                return None;
            }
            else
            {
                return Option<EitherUnsafe<L, B>>.Some(f(ma.RightValue.Value));
            }
        }

        public static Option<HashSet<B>> Traverse<L, A, B>(this HashSet<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }
            return Option<HashSet<B>>.Some(new HashSet<B>(res));            
        }

        public static Option<Identity<B>> Traverse<L, A, B>(this Identity<Option<A>> ma, Func<A, B> f) =>
            ma.Value.IsSome
                ? Option<Identity<B>>.Some(new Identity<B>(f(ma.Value.Value)))
                : Option<Identity<B>>.None;
        
        public static Option<Lst<B>> Traverse<L, A, B>(this Lst<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }
            return Option<Lst<B>>.Some(new Lst<B>(res));                
        }

        public static Option<Option<B>> Traverse<A, B>(this Option<Option<A>> ma, Func<A, B> f)
        {
            if (ma.IsNone || ma.Value.IsNone)
            {
                return None;
            }
            else
            {
                return Some(Some(f(ma.Value.Value)));
            }
        }
        
        public static Option<OptionUnsafe<B>> Traverse<A, B>(this OptionUnsafe<Option<A>> ma, Func<A, B> f)
        {
            if (ma.IsNone || ma.Value.IsNone)
            {
                return None;
            }
            else
            {
                return Some(SomeUnsafe(f(ma.Value.Value)));
            }
        }
        
        public static Option<Que<B>> Traverse<L, A, B>(this Que<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }
            return Option<Que<B>>.Some(new Que<B>(res));                
        }
        
        public static Option<Seq<B>> Traverse<L, A, B>(this Seq<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }
            return Option<Seq<B>>.Some(new Seq<B>(res));                
        }
        
        public static Option<Set<B>> Traverse<L, A, B>(this Set<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }
            return Option<Set<B>>.Some(new Set<B>(res));                
        }
        
        public static Option<Stck<B>> Traverse<L, A, B>(this Stck<Option<A>> ma, Func<A, B> f)
        {
            var res = new B[ma.Count];
            var ix = 0;
            foreach (var xs in ma)
            {
                if (xs.IsNone) return None;
                res[ix] = f(xs.Value);
                ix++;
            }
            return Option<Stck<B>>.Some(new Stck<B>(res));                
        }
        
        public static Option<Try<B>> Traverse<L, A, B>(this Try<Option<A>> ma, Func<A, B> f)
        {
            var tres = ma.Try();
            
            if (tres.IsBottom || tres.IsFaulted || tres.Value.IsNone)
            {
                return None;
            }
            else
            {
                return Some(Try(f(tres.Value.Value)));
            }
        }
        
        public static Option<TryOption<B>> Traverse<L, A, B>(this TryOption<Option<A>> ma, Func<A, B> f)
        {
            var tres = ma.Try();
            
            if (tres.IsBottom || tres.IsFaulted || tres.Value.IsNone|| tres.Value.Value.IsNone)
            {
                return None;
            }
            else
            {
                return Some(TryOption(f(tres.Value.Value.Value)));
            }
        }
        
        public static Option<Validation<L, B>> Traverse<L, A, B>(this Validation<L, Option<A>> ma, Func<A, B> f)
        {
            if (ma.IsFail || ma.SuccessValue.IsNone)
            {
                return None;
            }
            else
            {
                return Some(Validation<L, B>.Success(f(ma.SuccessValue.Value)));
            }
        }

        public static Option<Validation<MonoidL, L, B>> Traverse<MonoidL, L, A, B>(
            this Validation<MonoidL, L, Option<A>> ma, Func<A, B> f)
            where MonoidL : struct, Monoid<L>, Eq<L>
        {
            if (ma.IsFail || ma.SuccessValue.IsNone)
            {
                return None;
            }
            else
            {
                return Some(Validation<MonoidL, L, B>.Success(f(ma.SuccessValue.Value)));
            }
        }
    }
}
