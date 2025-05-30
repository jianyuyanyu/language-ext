using LanguageExt.Traits;

namespace LanguageExt;

record TakeTransducerM<M, A>(int Amount) : TransducerM<M, A, A> 
    where M : Applicative<M>
{
    public override ReducerM<M, A, S> Reduce<S>(ReducerM<M, A, S> reducer) 
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
