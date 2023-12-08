using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    public static void Destroy(this Transform transform) => Object.Destroy(transform.gameObject);
}
