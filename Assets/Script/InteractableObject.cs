using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Renderer>().material != null)
        {
            mat = GetComponent<Renderer>().material;

            // real time���� emission�� �����Ϸ��� �Ʒ� �� ���� �߰��ؾ��Ѵ�.
            mat.EnableKeyword("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
    }

    private void OnMouseEnter()
    {
        if (mat != null)
        {
            mat.SetColor("_EmissionColor", Color.blue);
            RenderSettings.ambientLight = Color.white;
        }
    }

    private void OnMouseExit()
    {
        if (mat != null)
        {
            mat.SetColor("_EmissionColor", Color.black);
            RenderSettings.ambientLight = Color.black;
        }
    }
}
