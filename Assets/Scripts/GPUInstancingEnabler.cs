using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GPUInstancingEnabler : MonoBehaviour
{
    private void Awake()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
