﻿using System;
using LanguageExt.Traits;
using System.Diagnostics.Contracts;

namespace LanguageExt.ClassInstances;

/// <summary>
/// Short integer number
/// </summary>
public struct TShort : Num<short>, Bool<short>
{
    /// <summary>
    /// Equality test
    /// </summary>
    /// <param name="x">The left hand side of the equality operation</param>
    /// <param name="y">The right hand side of the equality operation</param>
    /// <returns>True if x and y are equal</returns>
    [Pure]
    public static bool Equals(short x, short y) =>
        x == y;

    /// <summary>
    /// Compare two values
    /// </summary>
    /// <param name="x">Left hand side of the compare operation</param>
    /// <param name="y">Right hand side of the compare operation</param>
    /// <returns>
    /// if x greater than y : 1
    /// 
    /// if x less than y    : -1
    /// 
    /// if x equals y       : 0
    /// </returns>
    [Pure]
    public static int Compare(short x, short y) => 
        x.CompareTo(y);

    /// <summary>
    /// Find the sum of two numbers
    /// </summary>
    /// <param name="x">left hand side of the addition operation</param>
    /// <param name="y">right hand side of the addition operation</param>
    /// <returns>The sum of x and y</returns>
    [Pure]
    public static short Add(short x, short y) =>
        (short)(x + y);

    /// <summary>
    /// Find the difference between two values
    /// </summary>
    /// <param name="x">left hand side of the subtraction operation</param>
    /// <param name="y">right hand side of the subtraction operation</param>
    /// <returns>The difference between x and y</returns>
    [Pure]
    public static short Subtract(short x, short y) =>
        (short)(x - y);

    /// <summary>
    /// Fund the product of two numbers
    /// </summary>
    /// <param name="x">left hand side of the product operation</param>
    /// <param name="y">right hand side of the product operation</param>
    /// <returns>The product of x and y</returns>
    [Pure]
    public static short Multiply(short x, short y) =>
        (short)(x * y);

    /// <summary>
    /// Divide x by y
    /// </summary>
    /// <param name="x">left hand side of the division operation</param>
    /// <param name="y">right hand side of the division operation</param>
    /// <returns>x / y</returns>
    [Pure]
    public static short Divide(short x, short y) =>
        (short)(x / y);

    /// <summary>
    /// Find the absolute value of a number
    /// </summary>
    /// <param name="x">The value to find the absolute value of</param>
    /// <returns>The non-negative absolute value of x</returns>
    [Pure]
    public static short Abs(short x) =>
        Math.Abs(x);

    /// <summary>
    /// Find the sign of x
    /// </summary>
    /// <param name="x">The value to find the sign of</param>
    /// <returns>-1, 0, or +1</returns>
    [Pure]
    public static short Signum(short x) =>
        (short)Math.Sign(x);

    /// <summary>
    /// Generate a numeric value from an integer
    /// </summary>
    /// <param name="x">The integer to use</param>
    /// <returns>The equivalent of x in the Num〈A〉</returns>
    [Pure]
    public static short FromInteger(int x) =>
        (short)x;

    /// <summary>
    /// Generate a numeric value from a float
    /// </summary>
    /// <param name="x">The float to use</param>
    /// <returns>The equivalent of x in the Num〈A〉</returns>
    [Pure]
    public static short FromDecimal(decimal x) =>
        (short)x;

    /// <summary>
    /// Generate a numeric value from a double
    /// </summary>
    /// <param name="x">The double to use</param>
    /// <returns>The equivalent of x in the Num〈A〉</returns>
    [Pure]
    public static short FromFloat(float x) =>
        (short)x;

    /// <summary>
    /// Generate a numeric value from a decimal
    /// </summary>
    /// <param name="x">The decimal to use</param>
    /// <returns>The equivalent of x in the Num〈A〉</returns>
    [Pure]
    public static short FromDouble(double x) =>
        (short)x;

    /// <summary>
    /// Negate the value
    /// </summary>
    /// <param name="x">Value to negate</param>
    /// <returns>The negated source value</returns>
    [Pure]
    public static short Negate(short x) => (short)-x;

    /// <summary>
    /// Get the hash-code of the provided value
    /// </summary>
    /// <returns>Hash code of x</returns>
    [Pure]
    public static int GetHashCode(short x) =>
        x.GetHashCode();

    /// <summary>
    /// Returns True
    /// </summary>
    /// <returns>True</returns>
    [Pure]
    public static short True() =>
        -1;

    /// <summary>
    /// Returns False
    /// </summary>
    /// <returns>False</returns>
    [Pure]
    public static short False() =>
        0;

    /// <summary>
    /// Returns the result of the bitwise AND operation between `a` and `b`
    /// </summary>
    /// <returns>The result of the bitwise AND operation between `a` and `b`</returns>
    [Pure]
    public static short And(short a, short b) =>
        (short)(a & b);

    /// <summary>
    /// Returns the result of the bitwise OR operation between `a` and `b`
    /// </summary>
    /// <returns>The result of the bitwise OR operation between `a` and `b`</returns>
    [Pure]
    public static short Or(short a, short b) =>
        (short)(a | b);

    /// <summary>
    /// Returns the result of the bitwise NOT operation on `a`
    /// </summary>
    /// <returns>The result of the bitwise NOT operation on `a`</returns>
    [Pure]
    public static short Not(short a) =>
        (short)(~a);

    /// <summary>
    /// Returns the result of the bitwise exclusive-OR operation between `a` and `b`
    /// </summary>
    /// <returns>The result of the bitwise exclusive-OR operation between `a` and `b`</returns>
    [Pure]
    public static short XOr(short a, short b) =>
        (short)(a ^ b);

    /// <summary>
    /// Logical implication
    /// </summary>
    /// <returns>If `a` is true that implies `b`, else `true`</returns>
    [Pure]
    public static short Implies(short a, short b) =>
        And(a, True()) == False()
            ? True()
            : b;

    /// <summary>
    /// Bitwise bi-conditional. 
    /// </summary>
    /// <returns>`Not(XOr(a, b))`</returns>
    [Pure]
    public static short BiCondition(short a, short b) =>
        Not(XOr(a, b));
}
