using UnityEngine;
using UnityEngine.UI;

public class OverlayEffectManager : MonoBehaviour
{
    [Header("Dependencies")]
    private Material _material;
    [SerializeField] private Image image;

    //private readonly float _strength = ShaderHandler

    private void Awake()
    {
        _material = new Material(image.material);
        image.material = _material;
    }

    private void Update()
    {
        
    }
}
