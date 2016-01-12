using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OINO;

namespace OINOExamples.Poco
{
   public class Person
   {
      public PersonName Name { get; private set; }
      public Person WithName(PersonName name)
      {
         var @new = (Person) MemberwiseClone();
         @new.Name = name;
         return @new;
      }
      public Person UpdateName(Func<PersonName, PersonName> transform)
      {
         var @new = (Person)MemberwiseClone();
         @new.Name = transform(Name);
         return @new;
      }

      public MailingAddress HomeAddress { get; private set; }
      public Person WithHomeAddress(MailingAddress HomeAddress)
      {
         var @new = (Person)MemberwiseClone();
         @new.HomeAddress = HomeAddress;
         return @new;
      }
      public Person UpdateHomeAddress(Func<MailingAddress,MailingAddress> transform)
      {
         var @new = (Person)MemberwiseClone();
         @new.HomeAddress = transform(HomeAddress);
         return @new;
      }

      public MailingAddress WorkAddress { get; private set; }
      public Person WithWorkAddress(MailingAddress WorkAddress)
      {
         var @new = (Person)MemberwiseClone();
         @new.WorkAddress = WorkAddress;
         return @new;
      }
      public Person UpdateWorkAddress(Func<MailingAddress, MailingAddress> transform)
      {
         var @new = (Person)MemberwiseClone();
         @new.WorkAddress = transform(WorkAddress);
         return @new;
      }

      public string PrimaryEmail { get; private set; }
      public Person WithPrimaryEmail(string PrimaryEmail)
      {
         var @new = (Person)MemberwiseClone();
         @new.PrimaryEmail = PrimaryEmail;
         return @new;
      }
      public Person UpdatePrimaryEmail(Func<string, string> transform)
      {
         var @new = (Person)MemberwiseClone();
         @new.PrimaryEmail = transform(PrimaryEmail);
         return @new;
      }
   }

   public class PersonName
   {
      public string FirstName { get; private set; }
      public PersonName WithFirstName(string FirstName)
      {
         var @new = (PersonName)MemberwiseClone();
         @new.FirstName = FirstName;
         return @new;
      }
      public PersonName UpdateFirstName(Func<string,string> transform)
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

   public class MailingAddress
   {
      public string StreetAddress { get; private set; }
      public MailingAddress WithStreetAddress(string StreetAddress)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.StreetAddress = StreetAddress;
         return @new;
      }
      public MailingAddress UpdateStreetAddress(Func<string, string> transform)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.StreetAddress = transform(StreetAddress);
         return @new;
      }

      public string StreetAddress2 { get; private set; }
      public MailingAddress WithStreetAddress2(string StreetAddress2)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.StreetAddress2 = StreetAddress2;
         return @new;
      }
      public MailingAddress UpdateStreetAddress2(Func<string, string> transform)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.StreetAddress2 = transform(StreetAddress2);
         return @new;
      }

      public string City { get; private set; }
      public MailingAddress WithCity(string City)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.City = City;
         return @new;
      }
      public MailingAddress UpdateCity(Func<string, string> transform)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.City = transform(City);
         return @new;
      }

      public string State { get; private set; }
      public MailingAddress WithState(string State)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.State = State;
         return @new;
      }
      public MailingAddress UpdateState(Func<string, string> transform)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.State = transform(State);
         return @new;
      }

      public string ZipCode { get; private set; }
      public MailingAddress WithZipCode(string ZipCode)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.ZipCode = ZipCode;
         return @new;
      }
      public MailingAddress UpdateZipCode(Func<string, string> transform)
      {
         var @new = (MailingAddress)MemberwiseClone();
         @new.ZipCode = transform(ZipCode);
         return @new;
      }
   }

}
