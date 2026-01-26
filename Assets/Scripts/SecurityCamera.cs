using NUnit.Framework;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    #region Variables
    [Header("General Settings")]
    [SerializeField] string _DisplayName;
    [SerializeField] Camera LinkedCamera;

    [SerializeField] bool SyncToMainCameraConfig = true;

    [SerializeField] AudioListener CameraAudio;
    [SerializeField] Transform PivotPoint;
    [SerializeField] float DefaultPitch = 20f;
    [SerializeField] float AngleSwept = 60f;
    [SerializeField] float SweepSpeed = 6f;
    [SerializeField] int OutputTextureSize = 256;

    [Header("Detection")]
    [SerializeField] float DetectionHalfAngle = 30f;
    [SerializeField] float DetectionRage = 20f;
    [SerializeField] SphereCollider DetectionTrigger;
    [SerializeField] Light DetectionLight;
    [SerializeField] Color Colour_NothingDetected = Color.green;
    [SerializeField] Color Colour_FullyDetected = Color.red;
    [SerializeField] float DetectionBuildRate = 0.5f;
    [SerializeField] float DetectionDecayRate = 0.5f;

    public RenderTexture OutputTexture { get; private set; }
    public string DisplayName => _DisplayName;

    float CurrentAngle = 0f;
    bool SweepClockwise = true;
    //List<SecurityConsole> CurrentlywatchingConsoles = new List<SecurityConsole>(); make script called SecurityConsole

    #endregion
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
