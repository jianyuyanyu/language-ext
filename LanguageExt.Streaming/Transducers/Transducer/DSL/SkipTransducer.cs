namespace LanguageExt;

record SkipTransducer<A>(int Amount) : Transducer<A, A> 
{
    public override ReducerAsync<A, S> Reduce<S>(ReducerAsync<A, S> reducer)
    {
        var amount = Amount;
        return (s, x) =>
               {
                   if (amount > 0)
                   {
                       amount--;
                       return Reduced.ContinueAsync(s);
                   }
                   else
                   {
                       return reducer(s, x);
                   }
               };
    }
}
