using System;
using System.Collections.Generic;
using Valuable;

namespace ConsoleExample
{
   static class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine(Person.Fields.NameSymbol);

         var Vladimir1980 =
            Person.Empty
               .WithName(PersonName.Empty
                  .WithFirstName("D")
                  .WithMiddleName("R")
                  .WithLastName("Vladimir"))
               .WithBirth(DateTimeOffset.Parse("1980-05-01 06:20:00 -06"));
         var Vladimir2009 =
            Vladimir1980
               .WithHomeAddress(MailingAddress.Empty
                  .WithStreetAddress("65403 NW 42nd St.")
                  .WithCity("Gladstone")
                  .WithState("MO")
                  .WithZipCode("64118"))
               .WithWorkAddress(MailingAddress.Empty
                  .WithStreetAddress("9909 W. 99th St.")
                  .WithCity("Prairie Village")
                  .WithState("KS")
                  .WithZipCode("66207"));
         var Natalia1981 =
            Person.Empty
               .WithName(PersonName.Empty
                  .WithFirstName("Natalia")
                  .WithMiddleName("R")
                  .WithLastName("Giginschlotte"))
               .WithBirth(DateTimeOffset.Parse("1981-10-01 06:20:00 -06"));

         var LibertyHomeAddress = MailingAddress.Empty
               .WithStreetAddress("421 Westbridge Drive")
               .WithCity("Liberty")
               .WithState("MO")
               .WithZipCode("64068");

         var Natalia2008 = // Natalia Marries Vladimir
            Natalia1981.ChangeLastName(Vladimir1980.Name.LastName);
         var Vladimir2015 =
            Vladimir2009.MoveHomeAddress(LibertyHomeAddress);
         var Natalia2009 =
            Natalia2008.MoveHomeAddress(LibertyHomeAddress);

         Print(Natalia2009);
      }

      private static void Print(object v, string indent = "")
      {
         if (v is IEnumerable<KeyValuePair<Symbol,object>>)
         {
            foreach (var kvp in (IEnumerable<KeyValuePair<Symbol, object>>)v)
               Print(kvp.Value, indent + kvp.Key.Name + "=");
         }
         else if (v is string)
            Console.WriteLine(indent + v);
         else if (v is System.Collections.IEnumerable)
         {
            Print("[", indent);
            foreach (var o in (System.Collections.IEnumerable) v)
               Print(o, indent + "  ");
            Print("]", indent);
         }
         else
            Console.WriteLine(indent + v);
      }

      private static Person MoveHomeAddress(this Person person, MailingAddress homeAddress)
      {
         return person
            .WithHomeAddress(homeAddress);
      }

      private static Person ChangeLastName(this Person person, string newLastName)
      {
         return person.WithIn(Person.Fields.Name, PersonName.Fields.LastName, newLastName);
      }
   }
}
