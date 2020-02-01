using UnityEngine;

class StickyPiece : MonoBehaviour, IStickable
{
    public StickyPieces root;

    private bool sticky = false;
    private bool dragging = false;
    private float distance;
    private Vector3 startDist;

    private void OnMouseDown()
    {
        dragging = true;
        MakeSticky();
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
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

        root.pieces.Add(collision.transform);
    }

    public void MakeSticky()
    {
        sticky = true;
    }
}