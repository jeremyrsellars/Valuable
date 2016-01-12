using System;
using Valuable;

namespace ConsoleExample
{
   public class MailingAddress : Value<MailingAddress>
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
