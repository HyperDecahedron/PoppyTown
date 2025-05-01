using UnityEngine;

public class ScaleInOut : MonoBehaviour
{
    [SerializeField] private float scaleAmplitude = 0.2f; // How much it scales up/down
    [SerializeField] private float scaleSpeed = 2f;       // How fast it pulses

    private Vector3 initialScale;

    public bool stop = false; 

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (!stop)
        {
            float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleAmplitude;
            transform.localScale = initialScale + Vector3.one * scaleOffset;
        }
        
    }
}
