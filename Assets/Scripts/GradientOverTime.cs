using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GradientOverTime : MonoBehaviour
{
    [field: SerializeField]
    public Gradient Gradient { get; private set; }

    [field: SerializeField] 
    public float CycleDuration { get; private set; } = 2f;
    
    private Renderer _renderer;
    private float _timer;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        var rawValue = _timer % (CycleDuration * 2);
        var inversed = rawValue > CycleDuration;
        var value =  (_timer % CycleDuration) / CycleDuration;
        value = inversed ? 1 - value : value;
        
        var color = Gradient.Evaluate(value);
        _renderer.material.color = color;
        
        _timer += Time.deltaTime;
    }
}
