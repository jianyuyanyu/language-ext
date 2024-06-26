﻿using System.Diagnostics.Contracts;
using LanguageExt.Traits;

namespace LanguageExt.ClassInstances;

public struct HashableCompositions<A> : Hashable<Compositions<A>>
    where A : Monoid<A>
{
    [Pure]
    public static int GetHashCode(Compositions<A> x) =>
        x.GetHashCode();
}
