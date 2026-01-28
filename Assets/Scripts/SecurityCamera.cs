using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Cecil.Cil;

public class SecurityCamera : MonoBehaviour
{
    //Code from https://www.youtube.com/watch?v=pjqs4H8cHp4
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
    [SerializeField] float DetectionRange = 20f;
    [SerializeField] SphereCollider DetectionTrigger;
    [SerializeField] Light DetectionLight;
    [SerializeField] Color Colour_NothingDetected = Color.green;
    [SerializeField] Color Colour_FullyDetected = Color.red;
    [SerializeField] float DetectionBuildRate = 0.5f;
    [SerializeField] float DetectionDecayRate = 0.5f;
    [SerializeField] List<string> DetectableTags;
    [SerializeField] LayerMask DetectionLayerMask = ~0;
    float CosDetectionHalfAngle;

    public RenderTexture OutputTexture { get; private set; }
    public string DisplayName => _DisplayName;
    public GameObject CurrentlyDetectedTarget { get; private set; }

    float CurrentAngle = 0f;
    bool SweepClockwise = true;
    //List<SecurityConsole> CurrentlywatchingConsoles = new List<SecurityConsole>(); make script called SecurityConsole
    class PotentialTarget
    {
        public GameObject LinkedGO;
        public bool InFOV;
        public float DetectionLevel;
    }

    Dictionary<GameObject, PotentialTarget> AllTargets = new Dictionary<GameObject, PotentialTarget>();

    #endregion
    void Start()
    {
        //setup for the light and collider
        DetectionLight.color = Colour_NothingDetected;
        DetectionLight.range = DetectionRange;
        DetectionLight.spotAngle = DetectionHalfAngle * 2f;
        DetectionTrigger.radius = DetectionRange;

        //cache the detection data
        CosDetectionHalfAngle = Mathf.Cos(Mathf.Deg2Rad * DetectionHalfAngle);
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
        //refresh each target
        foreach (var target in AllTargets)
        {
            var targetInfo = target.Value;

            bool isVisible = false;

            //is the target in the field of view
            Vector3 vecToTarget = targetInfo.LinkedGO.transform.position - LinkedCamera.transform.position;
            if (Vector3.Dot(LinkedCamera.transform.forward, vecToTarget.normalized) >= CosDetectionHalfAngle)
            {
                //check if we can see the target
                RaycastHit hitInfo;
                if (Physics.Raycast(LinkedCamera.transform.position, LinkedCamera.transform.forward, out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                { 
                    if(hitInfo.collider.gameObject == targetInfo.LinkedGO)
                        isVisible = true;
                }
            } 

            //update the detection level
            targetInfo.InFOV = isVisible;
            if (isVisible)
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionBuildRate * Time.deltaTime);
            else
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionDecayRate * Time.deltaTime);

            //found a more detected target?
            if (targetInfo.DetectionLevel > targetInfo.DetectionLevel)
            { 
                highestDetectionLevel = targetInfo.DetectionLevel;
                CurrentlyDetectedTarget = targetInfo.LinkedGO;
            }
        }
        // updates the light colour
        if (CurrentlyDetectedTarget != null)
            DetectionLight.color = Color.Lerp(Colour_NothingDetected, Colour_FullyDetected, highestDetectionLevel);
        else
            DetectionLight.color = Colour_NothingDetected;
    }

    private void OnTriggerEnter(Collider other)
    {
        //skip if the tag is not supported
        if(!DetectableTags.Contains(other.tag))
            return;

        //add to target list
        AllTargets[other.gameObject] = new PotentialTarget() { LinkedGO = other.gameObject };
    }
    private void OnTriggerExit(Collider other)
    {
        //skip if the tag is not supported
        if (!DetectableTags.Contains(other.tag))
            return;

        //remove to target list
        AllTargets.Remove(other.gameObject);
    }
}
