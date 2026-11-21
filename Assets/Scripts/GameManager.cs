using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager gmInstance;

    [SerializeField] private HUDContoller hudController;
    private decimal startingCash = 5000;
    private decimal currentCash;

    [SerializeField] private LeverController leverController;
    private Collider2D leverCol;
    private Camera mainCamera;
    private decimal betAmount;

    [SerializeField] RNGManager rngManager;
    [SerializeField] PayoutManager payoutManager;
    private bool slotMachineSpinning = false;

    void Awake()
    {
        gmInstance = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        currentCash = startingCash;
        hudController.SetCurrentCash(startingCash);

        leverCol = leverController.GetComponent<Collider2D>();
    }

    void Update()
    {
        if(Mouse.current!= null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (leverCol.OverlapPoint(mousePosition) && slotMachineSpinning == false)
            {
                betAmount = hudController.GetBetInputValue();

                if (CheckBet())
                {
                    StartCoroutine(leverController.OnLeverPulled());
                    BetPlaced();
                }
                else
                {
                    Debug.Log("You cannot bet more than what you have!");
                }
            }
        }
    }

    private bool CheckBet()
    {
        return betAmount <= currentCash && betAmount >= 10;
    }
    private void BetPlaced()
    {
        hudController.LockBetInputField();
        SpinSlotMachine();
    }
    private void SpinSlotMachine()
    {
        slotMachineSpinning = true;
        SlotSymbol[] spinResult = rngManager.GenerateRandomSlotResult();

        if(spinResult[0] == spinResult[1] && spinResult[1] == spinResult[2])
        {
            payoutManager.HitJackpot(betAmount, spinResult[0]);
        }
        else
        {
            payoutManager.LoseCash(betAmount);
        }

        decimal payout = payoutManager.GetPayout();


        if(payout > 0)
        {
            hudController.SetBetResult("+ $" + payout, Color.green);
        }
        else
        {
            hudController.SetBetResult("- $" + betAmount, Color.red);
        }

        currentCash += payout;
        hudController.SetCurrentCash(currentCash);
        hudController.UnlockBetInputField();
        slotMachineSpinning = false;
    }
}
