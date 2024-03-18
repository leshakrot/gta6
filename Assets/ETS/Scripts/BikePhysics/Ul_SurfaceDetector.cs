using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ul_SurfaceDetector : MonoBehaviour
{
    [System.Serializable]
    public class SurfacePreset
    {
        public string SurfaceName = "Normal";
        public string SurfaceTagName = "Untagged";
        [Range(0.0000000001f, 1f)] public float Dynamicfriction = 0.1f;
        [Range(0.0000000001f, 1f)] public float SideGrip = 0.1f;
        [Range(0.00000001f, 10000.0f)] public float MaxSideSlipForce = 0.00001f;
        [Range(0.00000001f, 10000.0f)] public float MaxFowardSlipForce = 0.0001f;
        [Range(0.00000001f, 10000.0f)] public float MaxSideRotationalForce = 0.00001f;

    }
    public List<SurfacePreset> SurfacePresets = new List<SurfacePreset>(1);
    [Header("Drift Settings")]
    [Range(0.0000000001f, 1f)] public float Driftfriction = 0.89898898f;
    [Range(0.001f, 1f)] public float SideGrip = 1f;
    public float MaxSideSlipForce = 1000f;
    public float MaxFowardSlipForce = 2000f;
    public float MaxSideRotationalForce = 1000f;
    public float RotationDifferenceLimit = 0.2511f;
    public float KillDriftDelay = 0.98f;

}
