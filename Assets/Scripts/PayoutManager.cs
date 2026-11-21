using UnityEngine;

public class PayoutManager : MonoBehaviour
{
    private decimal payout;

    public void HitJackpot(decimal betAmount, SlotSymbol symbol)
    {
         if(symbol == SlotSymbol.Cherry)
        {
            payout =  betAmount * 1.5m;
        }
        else if(symbol == SlotSymbol.Bell)
        {
            payout = betAmount * 1.75m;
        }
        else if(symbol == SlotSymbol.Bar)
        {
            payout = betAmount * 2m;;
        }
         else if(symbol == SlotSymbol.Seven)
        {
            payout = betAmount * 3m;
        }
    }

    public void LoseCash(decimal betAmount)
    {
        payout = -betAmount;
    }

    public decimal GetPayout()
    {
        return payout;
    }
}
