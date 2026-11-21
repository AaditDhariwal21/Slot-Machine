using System;
using TMPro;
using UnityEngine;

public class HUDContoller : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currentCashText;
    [SerializeField] private TextMeshProUGUI lastBetResultText;
    [SerializeField] private TMP_InputField betInput;

    public decimal GetBetInputValue()
    {
        if(decimal.TryParse(betInput.text, out decimal betValue))
        {
            return betValue;
        }

        return 0m;
    }

    public void LockBetInputField()
    {
        betInput.interactable = false;
    }

    public void UnlockBetInputField()
    {
        betInput.interactable = true;
    }

    public void SetCurrentCash(decimal currentCash)
    {
        currentCashText.text = "$" + currentCash;
    }

    public void SetBetResult(String betResult, Color color)
    {
        lastBetResultText.text = betResult;
        lastBetResultText.color = color;
    }
}
