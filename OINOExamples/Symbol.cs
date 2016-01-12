namespace OINO
{
   public class Symbol
   {
      public Symbol(object nameSpace, string name)
      {
         NameSpace = nameSpace;
         Name = name;
      }

      public object NameSpace { get; }
      public string Name { get; }

      public override string ToString()
      {
         return base.ToString() + $"({NameSpace}::{Name})";
      }
   }
}