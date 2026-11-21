using UnityEngine;

public class RNGManager : MonoBehaviour
{
    private SlotSymbol[] symbolPool = new SlotSymbol[100];
    private SlotSymbol[] slotResult = new SlotSymbol[3];

    private const int a = 21;
    private const int c = 1;
    private const int m = 100;

    private int seed = 7;

    void Awake()
    {
        InitializeSymbolPool();
    }

    //Initializing the array to have 100 slot symbols
    private void InitializeSymbolPool()
    {
        int index = 0;

        // 40 Cherries
        for (int i = 0; i < 40; i++)
            symbolPool[index++] = SlotSymbol.Cherry;

        // 30 Bells
        for (int i = 0; i < 30; i++)
            symbolPool[index++] = SlotSymbol.Bell;

        // 20 Bars
        for (int i = 0; i < 20; i++)
            symbolPool[index++] = SlotSymbol.Bar;

        // 10 Sevens
        for (int i = 0; i < 10; i++)
            symbolPool[index++] = SlotSymbol.Seven;

        ShuffleSymbolPool();
    }

    //Shuffling the symbol array so that the array is now randomly arranged with 100 slot symbols
    private void ShuffleSymbolPool()
    {
        for (int i = symbolPool.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (symbolPool[i], symbolPool[j]) = (symbolPool[j], symbolPool[i]);
        }
    }

    //Generating 3 symbols and storing them in a result array
    public SlotSymbol[] GenerateRandomSlotResult()
    {
        for(int i = 0; i < 3; i++)
        {
            int index = LCG();
            slotResult[i] = symbolPool[index]; 
        }

        Debug.Log(slotResult[0] + " , " + slotResult[1] + " , " + slotResult[2]);
        return slotResult;
    }
    //Actual Mathematical Algorithm : using LCG with parameters (21,1,100) and seed 7 so that they satisfy Hull-Dobell Theorem and we get a full period.
    private int LCG()
    {
        seed = (a * seed + c) % m;
        return seed;
    }
}

// Could have directly used the exisisting .NET's CSPRNG (System.Security.Cryptography.RandomNumberGenerator) but actually building my own PRNG
// taught me more than directly using an exisiting API/function call would.
