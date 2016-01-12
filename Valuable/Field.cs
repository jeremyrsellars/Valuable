using System;

namespace Valuable
{
   public abstract class Field
   {
      public Field(Symbol symbol)
      {
         Symbol = symbol;
      }

      public Symbol Symbol { get; }
      public abstract object GetValue(object obj);
   }
   public class Field<TObject, TField> : Field
   {
      public Field(Symbol symbol, Func<TObject, TField, TObject> with, Func<TObject, TField> @get)
         : base (symbol)
      {
         With = with;
         Get = get;
      }

      public Func<TObject, TField, TObject> With { get; }
      public Func<TObject, TField> Get { get; }

      public override object GetValue(object obj)
      {
         return Get((TObject) obj);
      }
   }
}