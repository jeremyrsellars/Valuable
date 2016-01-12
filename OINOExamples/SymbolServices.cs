using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace OINO
{
   public static class SymbolServices
   {
      private static readonly Type SymbolType = typeof(Symbol);

      public static IImmutableList<Symbol> GetSymbols(Type type)
      {
         return AddSymbols(type, ImmutableList<Symbol>.Empty);
      }

      private static IImmutableList<Symbol> AddSymbols(Type type, IImmutableList<Symbol> derivedTypeSymbols)
      {
         if (type == null)
            return derivedTypeSymbols;
         var symbolFields = type.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => SymbolType.IsAssignableFrom(field.FieldType));
         var symbols = symbolFields.Select(field => (Symbol)field.GetValue(null));
         return AddSymbols(type.BaseType, derivedTypeSymbols.AddRange(symbols));
      }
   }
}
