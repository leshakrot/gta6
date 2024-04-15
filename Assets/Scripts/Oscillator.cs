using UnityEngine;
using UnityEngine.AI;

public class Oscillator : MonoBehaviour
{
    [Header("Oscillator")]
    [SerializeField] private bool isOscillating = true;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float period = 2f;
    [Range(0, 1)][SerializeField] private float movementFactor;
    [SerializeField] private float cycleOffset;
    [SerializeField] private bool isRandomCycleOffset;

    [Header("IsRotatableAtEdge")]
    [SerializeField] private bool isRotatableAtEdge;

    private Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;

        if (isRandomCycleOffset) cycleOffset = Random.Range(0f, 1f);
    }

    private void Update()
    {
        if (!isOscillating) return;
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period + cycleOffset;

        const float tau = Mathf.PI * 2f;

        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
