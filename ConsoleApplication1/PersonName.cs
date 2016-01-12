using System;
using Valuable;

namespace ConsoleExample
{
   public class PersonName : Value<PersonName>
   {
      public static class Fields
      {
         public static Symbol FirstNameSymbol = new Symbol(typeof(PersonName).FullName, nameof(FirstName));
         public static readonly Field<PersonName, string> FirstName =
            new Field<PersonName, string>(FirstNameSymbol, (name, @new) => name.WithFirstName(@new), name => name.FirstName);
         public static Symbol MiddleNameSymbol = new Symbol(typeof(PersonName).FullName, nameof(MiddleName));
         public static readonly Field<PersonName, string> MiddleName =
            new Field<PersonName, string>(MiddleNameSymbol, (name, @new) => name.WithMiddleName(@new), name => name.MiddleName);
         public static Symbol LastNameSymbol = new Symbol(typeof(PersonName).FullName, nameof(LastName));
         public static readonly Field<PersonName, string> LastName =
            new Field<PersonName, string>(LastNameSymbol, (name, @new) => name.WithLastName(@new), name => name.LastName);
      }

      public string FirstName { get; private set; }
      public PersonName WithFirstName(string FirstName)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.FirstName = FirstName;
         return @new;
      }
      public PersonName UpdateFirstName(Func<string, string> transform)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.FirstName = transform(FirstName);
         return @new;
      }

      public string MiddleName { get; private set; }
      public PersonName WithMiddleName(string MiddleName)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.MiddleName = MiddleName;
         return @new;
      }
      public PersonName UpdateMiddleName(Func<string, string> transform)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.MiddleName = transform(FirstName);
         return @new;
      }

      public string LastName { get; private set; }
      public PersonName WithLastName(string LastName)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.LastName = LastName;
         return @new;
      }
      public PersonName UpdateLastName(Func<string, string> transform)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.LastName = transform(LastName);
         return @new;
      }
   }
}