using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCloner : MonoBehaviour
{
    [SerializeField] private GameObject objectToClone;
    [SerializeField] private int numberOfClones = 10;
    [SerializeField] private float spaceSize = 120f;
    private List<GameObject> objectInstantiate = new List<GameObject>();

    [ContextMenu("Object Creator/Create Objects")]
    void CreateObjects()
    {
        for (int i = 0; i < numberOfClones; i++)
        {
            Vector3 randomPosition = GetRandomPositionInOuterSpace();
            Quaternion randomRotation = GetRandomRotation();
            var obj = Instantiate(objectToClone, randomPosition, randomRotation, transform.root);
            objectInstantiate.Add(obj);
        }
    }

    [ContextMenu("Object Creator/Delete Objects")]
    void DeleteObjects()
    {
        for (int i = 0; i < objectInstantiate.Count; i++)
        {
            DestroyImmediate(objectInstantiate[i]);
        }
    }

    Vector3 GetRandomPositionInOuterSpace()
    {
        float x = Random.Range(-spaceSize, spaceSize);
        float y = Random.Range(-spaceSize, spaceSize);
        float z = Random.Range(-spaceSize, spaceSize);

        return new Vector3(x, y, z);
    }

    Quaternion GetRandomRotation()
    {
        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        return Quaternion.Euler(randomX, randomY, randomZ);
    }
}
