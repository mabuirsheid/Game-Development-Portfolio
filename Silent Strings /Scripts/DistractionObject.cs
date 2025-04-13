using UnityEngine;

public class DistractionObject : MonoBehaviour
{
    [SerializeField] private float distractionRadius = 5f;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        NoiseSystem.Instance.MakeNoise(transform.position, distractionRadius);
        Destroy(gameObject, lifetime);
    }
}