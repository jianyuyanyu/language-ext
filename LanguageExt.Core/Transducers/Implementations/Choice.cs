﻿#nullable enable

using System;

namespace LanguageExt;

record ChoiceTransducer<E, X, B>(Seq<Transducer<E, Sum<X, B>>> Transducers) : Transducer<E, Sum<X, B>>
{
    public override Reducer<E, S> Transform<S>(Reducer<Sum<X, B>, S> reduce) =>
        Transducers.Head.Value?.Transform(new Reduce<S>(reduce, Transducers)) ?? throw new InvalidOperationException("Need at least one choice!");

    record Reduce<S>(Reducer<Sum<X, B>, S> Reducer, Seq<Transducer<E, Sum<X, B>>> Transducers) : Reducer<Sum<X, B>, S>
    {
        public override TResult<S> Run(TState state, S stateValue, Sum<X, B> value) =>
            value switch
            {
                SumRight<X, B> r => Reducer.Run(state, stateValue, value),
                _ => Transducers.IsEmpty
                    ? TResult.Complete(stateValue)
                    : TResult.Recursive(state, stateValue, value, new Reduce<S>(Reducer, Transducers.Tail))
            };
    }
        
    public override string ToString() =>  
        "choice";
}
