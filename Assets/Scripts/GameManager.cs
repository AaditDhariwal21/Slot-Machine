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

    [SerializeField] private ReelController[] reels;
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
                    if(betAmount > currentCash)
                    {
                        hudController.SetBetResult("Invalid Bet", Color.yellow);
                    }
                    else if(betAmount < 10)
                    {
                        hudController.SetBetResult("Min Bet : $10", Color.yellow);
                    }
                    
                    
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
        
        currentCash -= betAmount;
        hudController.SetCurrentCash(currentCash);
        hudController.SetBetResult("- $" + betAmount, Color.red);

        SlotSymbol[] spinResult = rngManager.GenerateRandomSlotResult();

        int completedReels = 0;

        for (int i = 0; i < reels.Length; i++)
        {
            int reelIndex = i;

            reels[reelIndex].SpinTo(
                spinResult[reelIndex],
                () =>
                {
                    completedReels++;

                    // Resolve payout only after all reels stop.
                    if (completedReels == reels.Length)
                    {
                        ResolveSpin(spinResult);
                    }
                }
            );
        }
    }

    private void ResolveSpin(SlotSymbol[] spinResult)
    {
        if(spinResult[0] == spinResult[1] && spinResult[1] == spinResult[2])
        {
            payoutManager.HitJackpot(betAmount, spinResult[0]);

            decimal payout = payoutManager.GetPayout();


            if(payout > 0)
            {
                hudController.SetBetResult("+ $" + payout, Color.green);
            }

            currentCash += payout;
            hudController.SetCurrentCash(currentCash);
        }
        
        hudController.UnlockBetInputField();
        slotMachineSpinning = false;
    }
}
