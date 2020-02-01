using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class StickyPiece : MonoBehaviour, IStickable
{
    private const string Name = "Combined";
    private bool sticky = false;
    private bool dragging = false;
    private float distance;
    private Vector3 startDist;
    private List<Transform> toCombine;

    private void OnMouseDown()
    {
        MakeSticky();
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(distance);
        startDist = transform.position - rayPoint;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void Update()
    {
        if (!dragging) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(distance);
        transform.position = rayPoint + startDist;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!sticky) { return; }

        if (collision.transform.GetComponent<IStickable>() == null) { return; }

        Debug.Log(collision.transform.name);

        toCombine = collision.transform.Cast<Transform>().ToList();
        toCombine.Add(transform);

        Combine(toCombine);
    }

    private static void Combine(List<Transform> toCombine)
    {
        GameObject root = new GameObject(Name, typeof(Rigidbody), typeof(StickyPiece));

        CreateCombined(root, toCombine);
        // Destroy(toCombine[0].gameObject);
    }

    private static void CreateCombined(GameObject root, List<Transform> toCombine)
    {
        root.transform.position = CenterMass(toCombine);

        foreach (Transform child in toCombine)
        {
            child.SetParent(root.transform, true);
            Destroy(child.GetComponent<Rigidbody>());
            Destroy(child.GetComponent<StickyPiece>());
        }
    }

    private static Vector3 CenterMass(List<Transform> toCombine)
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

    public void MakeSticky()
    {
        sticky = true;
    }
}