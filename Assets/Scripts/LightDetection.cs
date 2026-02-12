using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightDetection : MonoBehaviour
{

    [SerializeField] Light[] _pointLights;
    [SerializeField] Light[] _spotLights;

    public LayerMask layerMask;

    private GameObject[] camLights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camLights = GameObject.FindGameObjectsWithTag("Security Camera");
        for (int i = 0; i < camLights.Length; i++)
        {
            _pointLights[0].color = UnityEngine.Color.green;
            Debug.Log("greeeeen");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInPointLight();
        PlayerInSpotLight();
    }

    private void DebugLogDetectingLight()
    {
    }
    private void HandlePlayerSpottedBySecurityCamera()
    {
    }

    private bool PlayerInSpotLight()
    {
        if (_spotLights == null || _spotLights.Length == 0) return false;

        foreach (var light in _spotLights)
        {
            if (light.transform.parent.gameObject.activeInHierarchy == false ||
                light.gameObject.activeInHierarchy == false ||
                light.enabled == false) continue;

            Vector3 directionFromLightToPlayer = transform.position - light.transform.position;
            float angle = Vector3.Angle(light.transform.forward, directionFromLightToPlayer);
            if (angle < light.spotAngle / 2)
            {
                Debug.Log("Yass queen lightsss");
                DebugLogDetectingLight();
                HandlePlayerSpottedBySecurityCamera(); //DebugLogDetectingLight(light); (should all have light in them)
                return true;
            }
        }
        return false;
    }
    private bool PlayerInPointLight()
    {
        if (_pointLights == null || _pointLights.Length == 0) return false;

        foreach (var light in _pointLights)
        {
            if (light.transform.parent.gameObject.activeInHierarchy == false ||
                light.gameObject.activeInHierarchy == false ||
                light.enabled == false) continue;

            var distance = Vector3.Distance(transform.position, light.transform.position);
            if (distance < light.range)
            {
                var direaction = light.transform.position - transform.position;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 9f, LayerMask.GetMask("Player"))) // transform.foward was direction in tutorial
                {
                    for (int i = 0; i < _pointLights.Length; i++)
                    {
                        _pointLights[0].color = UnityEngine.Color.red; //it no turn red :(
                    }

                    Debug.Log("point lighttsssss");
                    Debug.Log("Hittttting: " + hit.collider.name);
                    //DebugLogDetectingLight(light);
                    return true;
                }
                else
                {
                    _pointLights[0].color = UnityEngine.Color.green;
                }
            }

        }
        return false;
    }
}
