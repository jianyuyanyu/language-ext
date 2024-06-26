﻿namespace LanguageExt.ClassInstances;

/// <summary>
/// Option type equality
/// </summary>
public struct HashableOption<A> : Hashable<Option<A>>
{
    /// <summary>
    /// Get hash code of the value
    /// </summary>
    /// <param name="x">Value to get the hash code of</param>
    /// <returns>The hash code of x</returns>
    public static int GetHashCode(Option<A> x) =>
        x.GetHashCode();
}
