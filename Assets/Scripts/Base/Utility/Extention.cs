using System.Collections.Generic;
using UnityEngine;

public static class Extention
{
    public static GameObject Random(this List<GameObject> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count - 1)];
    }
}
