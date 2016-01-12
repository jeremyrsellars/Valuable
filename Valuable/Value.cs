using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Valuable
{
   public class Value<T> : Value, IEnumerable<KeyValuePair<Symbol,object>>
      where T : Value<T>, new()
   {
      public static readonly T Empty = new T();

      public IEnumerator<KeyValuePair<Symbol, object>> GetEnumerator()
      {
         return ValueExtensions.GetEnumerator(this);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }

   public class Value
   {
      private static ImmutableDictionary<Type, IImmutableList<Symbol>> SymbolCache =
         ImmutableDictionary<Type, IImmutableList<Symbol>>.Empty;

      public IEnumerable<Symbol> Symbols =>
         ImmutableInterlocked.GetOrAdd(ref SymbolCache, GetType(), SymbolServices.GetSymbols);

      private static ImmutableDictionary<Type, IImmutableList<Field>> FieldCache =
         ImmutableDictionary<Type, IImmutableList<Field>>.Empty;

      public IEnumerable<Field> Fields =>
         ImmutableInterlocked.GetOrAdd(ref FieldCache, GetType(), FieldServices.GetFields);
   }

   public static class ValueExtensions
   {
#warning what if it's nil?
      //, TValue @def = default(TValue)

      public static TValue Get<T,TValue>(
         this T obj, 
         Field<T, TValue> symbol, 
         TValue @default = default(TValue))
         where T : Value<T>, new()
      {
         if (ReferenceEquals(obj, null))
            return @default;
         return symbol.Get(obj);
      }

      public static TValue GetIn<T,T1, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, TValue> f2,
         TValue @default = default(TValue))
         where T1 : Value<T1>, new()
         where T : Value<T>, new()
      {
         if (ReferenceEquals(obj, null))
            return @default;
         var obj1 = obj.Get(f1);
         if (ReferenceEquals(obj1, null))
            return @default;
         return obj1.Get(f2, @default);
      }

      public static TValue GetIn<T, T1, T2, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, TValue> f3,
         TValue value,
         TValue @default = default(TValue))
         where T : Value<T>, new()
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
      {
         if (ReferenceEquals(obj, null))
            return @default;
         var obj1 = obj.Get(f1);
         if (ReferenceEquals(obj1, null))
            return @default;
         var obj2 = obj1.Get(f2);
         if (ReferenceEquals(obj2, null))
            return @default;
         return obj2.Get(f3, @default);
      }

      public static TValue GetIn<T, T1, T2, T3, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, T3> f3,
         Field<T3, TValue> f4,
         TValue value,
         TValue @default = default(TValue))
         where T : Value<T>, new()
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
         where T3 : Value<T3>, new()
      {
         if (ReferenceEquals(obj, null))
            return @default;
         var obj1 = obj.Get(f1);
         if (ReferenceEquals(obj1, null))
            return @default;
         var obj2 = obj1.Get(f2);
         if (ReferenceEquals(obj2, null))
            return @default;
         var obj3 = obj2.Get(f3);
         if (ReferenceEquals(obj3, null))
            return @default;
         return obj3.Get(f4);
      }


      public static T With<T,TValue>(this T obj, Field<T, TValue> symbol, TValue value)
         where T : new()
      {
         var o = ReferenceEquals(obj, null) ? new T() : obj;
         return symbol.With(o, value);
      }

      public static T WithIn<T, T1, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, TValue> f2,
         TValue value)
         where T : Value<T>, new()
         where T1 : Value<T1>, new()
      {
         return obj.Update(f1, o => o.With(f2, value));
      }

      public static T WithIn<T, T1, T2, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, TValue> f3,
         TValue value)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
      {
         return obj.Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.With(f3,
                  value)));
      }
      public static T WithIn<T, T1, T2, T3, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, T3> f3,
         Field<T3, TValue> f4,
         TValue value)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
         where T3 : Value<T3>, new()
      {
         return obj.Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.Update(f3,
                  o3 => o3.With(f4,
                     value))));
      }


      public static T Update<T, TValue>(
         this T obj, 
         Field<T, TValue> symbol, 
         Func<TValue, TValue> transform)
      {
         var oldValue = symbol.Get(obj);
         var newValue = transform(oldValue);
         if (ReferenceEquals(oldValue, newValue))
            return obj;
         return symbol.With(obj, newValue);
      }
      public static T UpdateIn<T, T1, TValue>(
         this T obj, 
         Field<T, T1> f1,
         Field<T1, TValue> f2,
         Func<TValue, TValue> transform)
         where T1 : Value<T1>, new()
      {
         return obj.Update(f1, o => o.Update(f2, transform));
      }
      public static T UpdateIn<T, T1, T2, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, TValue> f3,
         Func<TValue, TValue> transform)
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
      {
         return obj.Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.Update(f3,
                  transform)));
      }
      public static T UpdateIn<T, T1, T2, T3, TValue>(
         this T obj,
         Field<T, T1> f1,
         Field<T1, T2> f2,
         Field<T2, T3> f3,
         Field<T3, TValue> f4,
         Func<TValue, TValue> transform)
         where T : Value<T>, new()
         where T1 : Value<T1>, new()
         where T2 : Value<T2>, new()
         where T3 : Value<T3>, new()
      {
         return obj.Update(f1,
            o1 => o1.Update(f2,
               o2 => o2.Update(f3,
                  o3 => o3.Update(f4,
                     transform))));
      }

      public static IEnumerable<KeyValuePair<Symbol, object>> ToEnumerable<T>(this Value<T> obj)
         where T : Value<T>, new()
      {
         return obj.Fields
            .Select(f => new KeyValuePair<Symbol, object>(f.Symbol, f.GetValue(obj)));
      }

      public static IEnumerator<KeyValuePair<Symbol,object>> GetEnumerator<T>(this Value<T> obj)
         where T : Value<T>, new()
      {
         return obj.ToEnumerable().GetEnumerator();
      }
   }
}