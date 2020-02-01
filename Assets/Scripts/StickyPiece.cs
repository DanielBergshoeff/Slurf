﻿using UnityEngine;
using System.Collections.Generic;

class StickyPiece : MonoBehaviour, IStickable
{
    public List<Transform> ConnectedPieces;
    public bool fixedJoint = false;

    private bool sticky = false;
    private bool dragging = false;
    private float distance;
    private Vector3 startDist;

    /*private void OnMouseDown()
    {
        dragging = true;
        MakeSticky();
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(distance);
        startDist = transform.position - rayPoint;
    }*/

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
        if (collision.transform.GetComponent<IStickable>() == null) { return; }

        if (ConnectedPieces.Contains(collision.transform))
            return;

        if (!collision.GetContact(0).thisCollider.GetComponent<StickyPiece>().sticky)
            return;

        StickyPiece sp = collision.gameObject.GetComponent<StickyPiece>();
        if (sp.fixedJoint)
            return;

        ConnectedPieces.Add(collision.transform);
        sp.ConnectedPieces.Add(transform);
        sp.transform.parent = transform;
        Destroy(sp.GetComponent<Rigidbody>());
        sp.fixedJoint = true;
    }

    public void MakeSticky()
    {
        sticky = true;
    }
}