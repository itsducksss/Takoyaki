using System;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SoftBodyPhysics : MonoBehaviour
{
    [Range(0.0f, 2.0f)]
    public float softness = 1f;

    [Range(0.1f, 1.0f)]
    public float damping = 1f;

    public float stiffness = 1f;

    private void Start()
    {
        CreateSoftBodyPhysics();
    }

    void CreateSoftBodyPhysics()
    { 
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer == null)
        {
            Debug.Log("missing skinnned mesh renderer");
        }

        Cloth cloth = gameObject.AddComponent<Cloth>();
        cloth.damping = damping;

        cloth.coefficients = GenereateClothCoefficents(skinnedMeshRenderer.sharedMesh.vertices.Length);

    }

    private ClothSkinningCoefficient[] GenereateClothCoefficents(int vertexCount)
    {
        ClothSkinningCoefficient[] coefficients = new ClothSkinningCoefficient[vertexCount];

        for (int i = 0; i < vertexCount; i++)
        {
            coefficients[i].maxDistance = softness;
            coefficients[i].collisionSphereDistance = 0f;
        }
        return coefficients;    
    }
}
