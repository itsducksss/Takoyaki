using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SecurityCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectRadius = 3f;
    [SerializeField] private LayerMask detectionLayerMask;

    [Header("Components")]
    [SerializeField] private Transform detectPosition;
    private bool _hasDetected;

    private void FixedUpdate()
    {
        DetectTarget();

        if (_hasDetected)
        {

        }
    }

    private void DetectTarget()
    {
        Physics.SphereCast(detectPosition.position, detectRadius, Vector3.forward, out RaycastHit hit, Mathf.Infinity, detectionLayerMask);
        if (hit.collider)
        {
            if (hit.collider) _hasDetected = true;
        }
        else
        {
            _hasDetected = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPosition.position, detectRadius);
    }
}

#region Old Code
/*[Header("General Settings")]
    [SerializeField] string _DisplayName;
    [SerializeField] GameObject LinkedCamera;

    [SerializeField] bool SyncToMainCameraConfig = true;

    [SerializeField] AudioListener CameraAudio;
    [SerializeField] Transform PivotPoint;
    [SerializeField] float DefaultPitch = 20f;
    [SerializeField] float AngleSwept = 60f;
    [SerializeField] float SweepSpeed = 6f;
    [SerializeField] int OutputTextureSize = 256;
    [SerializeField] float MaxRotationSpeed = 15f;

    [Header("Detection")]
    [SerializeField] float DetectionHalfAngle = 30f;
    [SerializeField] float DetectionRange = 20f;
    [SerializeField] float TargetVOffset = 1f;
    [SerializeField] SphereCollider DetectionTrigger;
    [SerializeField] Light DetectionLight;
    [SerializeField] Color Colour_NothingDetected = Color.green;
    [SerializeField] Color Colour_FullyDetected = Color.red;
    [SerializeField] float DetectionBuildRate = 0.5f;
    [SerializeField] float DetectionDecayRate = 0.5f;
    [SerializeField][Range(0f, 1f)] float SuspicionThreshold = 0.5f;
    [SerializeField] List<string> DetectableTags;
    [SerializeField] LayerMask DetectionLayerMask = ~0;
    [SerializeField] private Transform detectPosition;
    [SerializeField] private float detectRadius = 3f;

    [SerializeField] UnityEvent<GameObject> OnDetected = new UnityEvent<GameObject>();
    [SerializeField] UnityEvent OnAllClear = new UnityEvent();

    public RenderTexture OutputTexture { get; private set; }
    public string DisplayName => _DisplayName;
    public GameObject CurrentlyDetectedTarget { get; private set; }
    public bool HasDetectedTarget { get; private set; } = false;

    float CurrentAngle = 0f;
    float CosDetectionHalfAngle;
    bool SweepClockwise = true;

    class PotentialTarget
    {
        public GameObject LinkedGO;
        public bool InFOV;
        public float DetectionLevel;
        public bool OnDetectedEventSent;
    }

    Dictionary<GameObject, PotentialTarget> AllTargets = new Dictionary<GameObject, PotentialTarget>();

    // Start is called before the first frame update
    void Start()
    {
        // turn the camera off by default
        //LinkedCamera.enabled = false;
        //CameraAudio.enabled = false;

        // setup the collider and light
        DetectionLight.color = Colour_NothingDetected;
        DetectionLight.range = DetectionRange;
        DetectionLight.spotAngle = DetectionHalfAngle * 2f;
        DetectionTrigger.radius = DetectionRange;

        // cache the detection data
        CosDetectionHalfAngle = Mathf.Cos(Mathf.Deg2Rad * DetectionHalfAngle);

        //if (SyncToMainCameraConfig)
        //{
        //    LinkedCamera.clearFlags = Camera.main.clearFlags;
        //    LinkedCamera.backgroundColor = Camera.main.backgroundColor;
        //}

        // setup the render texture
        OutputTexture = new RenderTexture(OutputTextureSize, OutputTextureSize, 32);
        //LinkedCamera.targetTexture = OutputTexture;
    }

    // Update is called once per frame
    void Update()
    {
        RefreshTargetInfo();
    }

    void RefreshTargetInfo()
    {
        float highestDetectionLevel = 0f;
        CurrentlyDetectedTarget = null;

        // refresh each target
        foreach (var target in AllTargets)
        {
            var targetInfo = target.Value;

            bool isVisible = false;

            // is the target in the field of view
            Vector3 vecToTarget = (targetInfo.LinkedGO.transform.position + TargetVOffset * Vector3.up -
                                   LinkedCamera.transform.position).normalized;
            if (Vector3.Dot(LinkedCamera.transform.forward, vecToTarget) >= CosDetectionHalfAngle)
            {
                // check if we can see the target
                //RaycastHit hitInfo;
                //if (Physics.Raycast(LinkedCamera.transform.position, vecToTarget,
                //                    out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                //{
                //    if (hitInfo.collider.gameObject == targetInfo.LinkedGO)
                //        isVisible = true;
                //}

                Physics.SphereCast(detectPosition.position, detectRadius, Vector3.forward, out RaycastHit hit, Mathf.Infinity, DetectionLayerMask);
                if (hit.collider != null)
                {
                    if (hit.collider == targetInfo.LinkedGO) isVisible = true;
                }
            }

            // update the detection level
            targetInfo.InFOV = isVisible;
            if (isVisible)
            {
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionBuildRate * Time.deltaTime);

                // notify that a target was seen
                if (targetInfo.DetectionLevel >= 1f && !targetInfo.OnDetectedEventSent)
                {
                    HasDetectedTarget = true;
                    targetInfo.OnDetectedEventSent = true;
                    OnDetected.Invoke(targetInfo.LinkedGO);
                }
            }
            else
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel - DetectionDecayRate * Time.deltaTime);

            // found a new more detected target?
            if (targetInfo.DetectionLevel > highestDetectionLevel)
            {
                highestDetectionLevel = targetInfo.DetectionLevel;
                CurrentlyDetectedTarget = targetInfo.LinkedGO;
            }
        }

        // update the light colour
        if (CurrentlyDetectedTarget != null)
            DetectionLight.color = Color.Lerp(Colour_NothingDetected, Colour_FullyDetected, highestDetectionLevel);
        else
        {
            DetectionLight.color = Colour_NothingDetected;

            if (HasDetectedTarget)
            {
                HasDetectedTarget = false;
                OnAllClear.Invoke();
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // skip if the tag isn't supported
    //    if (!DetectableTags.Contains(other.tag))
    //        return;

    //    // add to our target list
    //    AllTargets[other.gameObject] = new PotentialTarget() { LinkedGO = other.gameObject };
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    // skip if the tag isn't supported
    //    if (!DetectableTags.Contains(other.tag))
    //        return;

    //    // remove from the target list
    //    AllTargets.Remove(other.gameObject);
    //}*/
#endregion