namespace LanguageExt;

record TakeTransducer<A>(int Amount) : Transducer<A, A> 
{
    public override ReducerAsync<A, S> Reduce<S>(ReducerAsync<A, S> reducer)
    {
        var taken = 0;
        return (s, x) =>
               {
                   if (taken < Amount)
                   {
                       taken++;
                       return reducer(s, x);
                   }
                   else
                   {
                       return Reduced.DoneAsync(s);
                   }
               };
    }

    public override ReducerM<M, A, S> ReduceM<M, S>(ReducerM<M, A, S> reducer) 
    {
        var taken = 0;
        return (s, x) =>
               {
                   if (taken < Amount)
                   {
                       taken++;
                       return reducer(s, x);
                   }
                   else
                   {
                       return M.Pure(s);
                   }
               };
    }
}
