using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
public class OnePlaneCuttingController : MonoBehaviour
{
    public GameObject plane1;
    Material mat;
    public Vector3 normal1;
    public Vector3 position1;

    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateShaderProperties();
    }

    private void UpdateShaderProperties()
    {
        normal1 = plane1.transform.TransformVector(new Vector3(0, 0, -1));
        position1 = plane1.transform.position;
        rend.material.SetVector("_PlaneNormal", normal1);
        rend.material.SetVector("_PlanePosition", position1);
    }
}
