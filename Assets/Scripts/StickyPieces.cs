using System.Collections.Generic;
using UnityEngine;

public class StickyPieces : MonoBehaviour
{
    private Dictionary<Transform, Vector3> originalRotation = new Dictionary<Transform, Vector3>();
    public List<Transform> pieces = new List<Transform>();
    private const string Name = "Combined";

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.AddComponent<StickyPiece>().root = this;
            originalRotation[child] = child.rotation.eulerAngles;
        }
    }

    private void Update()
    {
        if (pieces.Count < 2) { return; }

        Combine(pieces);

        pieces.Clear();
    }

    private void Combine(List<Transform> toCombine)
    {
        GameObject root = new GameObject(Name, typeof(Rigidbody), typeof(StickyPiece));
        root.transform.SetParent(transform.root, true);

        CreateCombined(root, toCombine);
        // Destroy(toCombine[0].gameObject);
    }

    private void CreateCombined(GameObject root, List<Transform> toCombine)
    {
        root.transform.position = CenterMass(toCombine);

        foreach (Transform child in toCombine)
        {
            child.SetParent(root.transform, true);
            Destroy(child.GetComponent<Rigidbody>());
        }
    }

    private Vector3 CenterMass(List<Transform> toCombine)
    {
        Vector3 center = Vector3.zero;

        for (int i = 0; i < toCombine.Count; i++)
        {
            Transform child = toCombine[i];
            center += child.position;
        }

        center /= toCombine.Count;
        return center;
    }
}
