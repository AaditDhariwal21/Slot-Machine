using System.Collections;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    private SpriteRenderer leverSprite;
    [SerializeField] private SpriteRenderer pulledLeverSprite;

    void Awake()
    {
        leverSprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator OnLeverPulled()
    {
        leverSprite.enabled = false;
        pulledLeverSprite.enabled =  true;

        yield return new WaitForSeconds(0.25f);

        leverSprite.enabled = true;
        pulledLeverSprite.enabled =  false;
    }
}
