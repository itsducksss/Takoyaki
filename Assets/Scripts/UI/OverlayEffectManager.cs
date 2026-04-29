using UnityEngine;
using UnityEngine.UI;

public class OverlayEffectManager : MonoBehaviour
{
    public PlayerStateMachine sm;
    private const string _materialParam = "_Strength";
    [Header("Dependencies")]
    private Material _material;
    [SerializeField] private Image image;
    private bool hasPlayed;

    public static OverlayEffectManager Instance;

    private void Awake()
    {
        sm = FindFirstObjectByType<PlayerStateMachine>();
        _material = new Material(image.material);
        image.material = _material;

        Instance = this;

        ResetEffect();
    }

    [SerializeField] private float _timer;
    private float _smooth;
    private float _velocity;
    [SerializeField] private bool _decrease;

    public void TriggerEffect(float duration = 1f)
    {
        _timer += duration;
        _decrease = false;

        print("Triggered");
    }

    public void EnableEffect()
    {
        _decrease = false;
        print("Enabled");
    }

    public void DisableEffect()
    {
        _decrease = true;
        print("Disabled");
    }

    private void Update()
    {
        //DetectInput();

        // Update Effect if the timer is dropping or if increasing is true
        if (_smooth > 0 || !_decrease)
        {
            _timer = _decrease ? _timer - Time.deltaTime : _timer + Time.deltaTime;
            _timer = Mathf.Clamp01(_timer);

            _smooth = Mathf.SmoothDamp(_smooth, _timer, ref _velocity, Time.deltaTime);
            UpdateEffect();

            sm.OnDetected();
        }
    }

    private void UpdateEffect()
    {
        _material.SetFloat(_materialParam, _smooth);
    }

    public void ResetEffect()
    {
        _material.SetFloat(_materialParam, 0f);
    }

    // For testing purposes
    //private void DetectInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1)) TriggerEffect();
    //    if (Input.GetKeyDown(KeyCode.Alpha2)) EnableEffect();
    //    if (Input.GetKeyDown(KeyCode.Alpha3)) DisableEffect();
    //}
}
