using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageTrail : MonoBehaviour
{
    public float meshRefreshRate = 0.1f;
    public Material effectMaterial;

    private MeshRenderer meshRenderer; 

    private void Update() {
        CreateTrail();
    }

    private void CreateTrail()
    {
        if(meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        
        Mesh mesh = new Mesh();
        Physics.BakeMesh(mesh.GetInstanceID(), false);

        Graphics.DrawMesh(mesh, transform.position, transform.rotation, effectMaterial, 1, Camera.main, 0, null, castShadows: true, receiveShadows: true, useLightProbes: true);
    }
}
