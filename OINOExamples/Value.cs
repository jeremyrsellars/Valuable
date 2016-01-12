using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OINO
{
   //public class Value
   //{
   //   private static ImmutableDictionary<Type, IImmutableList<Symbol>> SymbolCache =
   //      ImmutableDictionary<Type, IImmutableList<Symbol>>.Empty;

   //   public IEnumerable<Symbol> Symbols =>
   //      ImmutableInterlocked.GetOrAdd(ref SymbolCache, GetType(), SymbolServices.GetSymbols);
   //}

   public class Value<T> //: Value
      where T : Value<T>, new()
   {
      public static readonly T Empty = new T();

#warning what if it's nil?
      //, TValue @def = default(TValue)

      public TValue Get<TValue>(Field<T, TValue> symbol)
      {
         return symbol.Get((T)this);
      }

      public TValue GetIn<T1, TValue>(Field<T, T1> f1,
                                      Field<T1, TValue> f2)
         where T1 : Value<T1>, new()
      {
         return Get(f1).Get(f2);
      }

      public TValue GetIn<T1, T2, TValue>(
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, TValue> f3,
         TValue value)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
      {
         return Get(f1).Get(f2).Get(f3);
      }
      public TValue GetIn<T1, T2, T3, TValue>(
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, T3> f3,
         Field<T3, TValue> f4,
         TValue value)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
         where T3 : Value<T3>, new()
      {
         return Get(f1).Get(f2).Get(f3).Get(f4);
      }


      public T With<TValue>(Field<T,TValue> symbol, TValue value)
      {
         return symbol.With((T)this, value);
      }

      public T WithIn<T1, TValue>(Field<T, T1> f1,
                                  Field<T1, TValue> f2,
                                  TValue value)
         where T1 : Value<T1>, new()
      {
         return Update(f1, o => o.With(f2, value));
      }

      public T WithIn<T1, T2, TValue>(
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, TValue> f3,
         TValue value)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
      {
         return Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.With(f3,
                  value)));
      }
      public T WithIn<T1, T2, T3, TValue>(
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, T3> f3,
         Field<T3, TValue> f4,
         TValue value)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
         where T3 : Value<T3>, new()
      {
         return Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.Update(f3,
                  o3 => o3.With(f4,
                     value))));
      }


      public T Update<TValue>(Field<T, TValue> symbol, Func<TValue,TValue> transform)
      {
         var oldValue = symbol.Get((T) this);
         var newValue = transform(oldValue);
         if (ReferenceEquals(oldValue, newValue))
            return (T)this;
         return symbol.With((T)this, newValue);
      }
      public T UpdateIn<T1,TValue>(Field<T, T1> f1,
                                   Field<T1, TValue> f2,
                                   Func<TValue, TValue> transform)
         where T1 : Value<T1>, new()
      {
         return Update(f1, o => o.Update(f2, transform));
      }
      public T UpdateIn<T1, T2, TValue>(
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, TValue> f3,
         Func<TValue, TValue> transform)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
      {
         return Update(f1, 
            o1 => o1.Update(f2,
               o2 => o2.Update(f3,
                  transform)));
      }
      public T UpdateIn<T1, T2, T3, TValue>(
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, T3> f3,
         Field<T3, TValue> f4,
         Func<TValue, TValue> transform)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
         where T3 : Value<T3>, new()
      {
         return Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.Update(f3,
                  o3 => o3.Update(f4,
                     transform))));
      }

   }

   public class Field<TObject, TField>
   {
      public Field(Symbol symbol, Func<TObject, TField, TObject> with, Func<TObject, TField> @get)
      {
         Symbol = symbol;
         With = with;
         Get = get;
      }

      public Symbol Symbol { get; }
      public Func<TObject, TField, TObject> With { get; }
      public Func<TObject, TField> Get { get; }
   }
}