using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ESAnimationLayerInfo
{
    static string LastLayerName = "Null";
    static int LastLayerIndex = 0;
    static string LastAnimationName;
    public static void StoreLayerName(string Name)
    {
        LastLayerName = Name;
    }
    //
    public static void StoreLayerIndex(int LayerIndex)
    {
        LastLayerIndex = LayerIndex;
    }
    //
    public static string GetLastLayerName()
    {
        string lastname = LastLayerName;

        return lastname;
    }
    //
    public static void SetLastAnimationName(string Name)
    {
        LastAnimationName = Name;
    }
    //
    public static string GetLastAnimationName()
    {
        string lastname = LastAnimationName;

        return lastname;
    }
    //
    public static int GetLastLayerInt()
    {
        int lastindex = LastLayerIndex;

        return lastindex;
    }
}
