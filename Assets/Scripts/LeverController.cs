using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverController : MonoBehaviour
{
    private CircleCollider2D col;
    private Camera mainCamera;

    [SerializeField] private GameObject pulledLever;
    bool leverPulled = false;
    void Awake()
    {
        col = GetComponent<CircleCollider2D>();    
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(Mouse.current!= null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (col.OverlapPoint(mousePosition) && leverPulled == false)
            {
                StartCoroutine(OnLeverPulled());
            }
        }
    }

    IEnumerator OnLeverPulled()
    {
        leverPulled = true; 

        GetComponent<SpriteRenderer>().enabled = false;
        pulledLever.GetComponent<SpriteRenderer>().enabled =  true;

        yield return new WaitForSeconds(0.25f);

        RNGManager.rngInstance.GenerateRandomSlotResult();

        GetComponent<SpriteRenderer>().enabled = true;
        pulledLever.GetComponent<SpriteRenderer>().enabled =  false;
        leverPulled = false;
    }
}
