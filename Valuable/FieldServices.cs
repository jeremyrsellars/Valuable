using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Valuable
{
   public static class FieldServices
   {
      private static readonly Type FieldType = typeof(Field);

      public static IImmutableList<Field> GetFields(Type type)
      {
         return AddFields(ImmutableList<Field>.Empty, type);
      }

      private static IImmutableList<Field> AddFields(IImmutableList<Field> derivedTypeFields, Type type)
      {
         if (type == null)
            return derivedTypeFields;
         var fields = type.GetNestedTypes()
            .Aggregate(derivedTypeFields, AddFields);
         var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => FieldType.IsAssignableFrom(field.FieldType));
         var fieldsForThisType = fieldInfos.Select(field => (Field)field.GetValue(null));
         return AddFields(fields.AddRange(fieldsForThisType), type.BaseType);
      }
   }
}