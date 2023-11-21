using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
    }

    public static void SetRenderLayerInChildren(Transform transform, int layerNumber)
    {
        Transform[] childrenTransforms = transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform trans in childrenTransforms)
            trans.gameObject.layer = layerNumber;
    }
}
