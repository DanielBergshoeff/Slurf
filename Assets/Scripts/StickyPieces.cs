using System.Collections.Generic;
using UnityEngine;

public class StickyPieces : MonoBehaviour
{
    private Dictionary<Transform, Vector3> originalRotation = new Dictionary<Transform, Vector3>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<Rigidbody>();
            originalRotation[child] = child.rotation.eulerAngles;
        }
    }
}
