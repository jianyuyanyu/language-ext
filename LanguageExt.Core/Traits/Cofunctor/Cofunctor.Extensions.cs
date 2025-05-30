using System;

namespace LanguageExt.Traits;

public static class CofunctorExtensions
{
    /// <summary>
    /// The class of contravariant functors.
    /// Whereas one can think of a `Functor` as containing or producing values, a contravariant functor is a functor that
    /// can be thought of as consuming values.
    /// 
    /// Contravariant functors are referred to colloquially as Cofunctor, even though the dual of a `Functor` is just
    /// a `Functor`. 
    /// </summary>
    public static K<F, A> Comap<F, A, B>(this K<F, B> fb, Func<A, B> f) 
        where F : Cofunctor<F> =>
        F.Comap(f, fb);
    
    /// <summary>
    /// The class of contravariant functors.
    /// Whereas one can think of a `Functor` as containing or producing values, a contravariant functor is a functor that
    /// can be thought of as consuming values.
    /// 
    /// Contravariant functors are referred to colloquially as Cofunctor, even though the dual of a `Functor` is just
    /// a `Functor`. 
    /// </summary>
    public static K<F, A> Comap<F, A, B>(this Func<A, B> f, K<F, B> fb) 
        where F : Cofunctor<F> =>
        F.Comap(f, fb);    
}
