
using System;
using UnityEngine;
using DG.Tweening;

public class ReelController : MonoBehaviour
{
    [Header("Reel Setup")]
    [SerializeField] private Transform reelContent;
    [SerializeField] private float symbolSpacing = 1f;
    [SerializeField] private float recycleBoundary = -4f;

    [Header("Spin Settings")]
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float accelerationDuration = 0.5f;
    [SerializeField] private float spinningDuration = 2f;
    [SerializeField] private float decelerationRate = 4f;

    private ReelSymbol[] symbols;

    private float currentSpeed;
    private float targetDeceleration;

    private bool isSpinning;
    private bool isDecelerating;

    private Action spinCompleteCallback;

    private const float CenterY = 0f;

    private void Awake()
    {
        symbols = reelContent.GetComponentsInChildren<ReelSymbol>();
    }

    private void Update()
    {
        if (!isSpinning)
            return;

        if (isDecelerating)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                targetDeceleration * Time.deltaTime
            );
        }

        float movement = currentSpeed * Time.deltaTime;

        foreach (ReelSymbol symbol in symbols)
        {
            Vector3 position = symbol.transform.localPosition;
            position.y -= movement;

            while (position.y < recycleBoundary)
            {
                position.y += symbols.Length * symbolSpacing;
            }

            symbol.transform.localPosition = position;
        }

        if (isDecelerating && currentSpeed <= 0f)
        {
            FinishSpin();
        }
    }

    public void SpinTo(SlotSymbol result, Action onComplete)
    {
        if (isSpinning)
            return;

        isSpinning = true;
        isDecelerating = false;
        currentSpeed = 0f;
        spinCompleteCallback = onComplete;

        Sequence sequence = DOTween.Sequence();

        // Accelerate to maximum speed.
        sequence.Append(
            DOTween.To(
                () => currentSpeed,
                x => currentSpeed = x,
                maxSpeed,
                accelerationDuration
            ).SetEase(Ease.InCubic)
        );

        // Maintain maximum speed.
        sequence.AppendInterval(spinningDuration);

        // Begin deceleration.
        sequence.OnComplete(() =>
        {
            BeginDeceleration(result);
        });
    }

    private void BeginDeceleration(SlotSymbol result)
    {
        ReelSymbol target = FindNextTarget(result);

        if (target == null)
        {
            Debug.LogError("Target symbol not found: " + result);
            FinishSpin();
            return;
        }

        float totalHeight = symbols.Length * symbolSpacing;

        // Distance the target must travel downwards to reach Y = 0.
        float targetY = target.transform.localPosition.y;
        float distance = Mathf.Repeat(targetY, totalHeight);

        if (distance < 0.01f)
            distance = totalHeight;

        // Add two complete rotations for a more natural slowdown.
        distance += totalHeight * 2f;

        // Calculate the deceleration needed to stop at that distance.
        targetDeceleration =
            (currentSpeed * currentSpeed) / (2f * distance);

        isDecelerating = true;
    }

    private ReelSymbol FindNextTarget(SlotSymbol result)
    {
        ReelSymbol target = null;
        float smallestDistance = float.MaxValue;

        float totalHeight = symbols.Length * symbolSpacing;

        foreach (ReelSymbol symbol in symbols)
        {
            if (symbol.symbolType != result)
                continue;

            float y = symbol.transform.localPosition.y;

            float distance = Mathf.Repeat(y, totalHeight);

            if (distance < 0.01f)
                distance = totalHeight;

            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                target = symbol;
            }
        }

        return target;
    }

    private void FinishSpin()
    {
        currentSpeed = 0f;
        isSpinning = false;
        isDecelerating = false;

        spinCompleteCallback?.Invoke();
        spinCompleteCallback = null;
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
