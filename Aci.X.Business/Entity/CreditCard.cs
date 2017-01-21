using Client=Aci.X.ClientLib;
using SF=Aci.X.IwsLib.Storefront;

namespace Aci.X.Business
{
  public class CreditCard
  {
    public static Client.CreditCard Render(SF.Card sfCard)
    {
      return new Client.CreditCard
      {
        City = sfCard.city,
        Zip = sfCard.postcode,
        CardHolderName = sfCard.cardholderName,
        CardType = sfCard.cardType,
        Address = sfCard.address,
        CreditCardNumber = sfCard.ccNumLastFour,
        State = sfCard.state,
        ExpirationDate= sfCard.expDate
      };
    }
  }
}
