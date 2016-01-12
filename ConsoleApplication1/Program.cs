using System;
using OINOExamples.OINO;

namespace ConsoleApplication1
{
   class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine(Person.Fields.NameSymbol);

         var Jeremy2009 = 
            Person.Empty
               .WithName(PersonName.Empty
                  .WithFirstName("Jeremy")
                  .WithMiddleName("Russell")
                  .WithLastName("Sellars"))
               .WithHomeAddress(MailingAddress.Empty
                  .WithStreetAddress("202 NE 67th St.")
                  .WithCity("Gladstone")
                  .WithState("MO")
                  .WithZipCode("64118"))
               .WithHomeAddress(MailingAddress.Empty
                  .WithStreetAddress("4500 W. 89th St.")
                  .WithCity("Prairie Village")
                  .WithState("KS")
                  .WithZipCode("66207"));
         var Jeremy2015 =
            Jeremy2009
               .WithHomeAddress(MailingAddress.Empty
                  .WithStreetAddress("301 Eastwood Lane")
                  .WithCity("Liberty")
                  .WithState("MO")
                  .WithZipCode("64068"));
         var Amy2001 =
            Person.Empty
               .WithName(PersonName.Empty
                  .WithFirstName("Amy")
                  .WithMiddleName("Ruth")
                  .WithLastName("Armstrong"));
         var Amy2008 =
            Amy2001.WithIn(Person.Fields.Name, PersonName.Fields.LastName, "Sellars");
         var Amy2009 =
            Amy2008.With(Person.Fields.HomeAddress, Jeremy2009.HomeAddress);
      }
   }
}
