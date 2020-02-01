using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        CheckSuckable(other.gameObject);
        Destroy(gameObject);
    }

    private void CheckSuckable(GameObject go) {
        if (!go.CompareTag("Suckable"))
            return;

        StickyPiece sp = go.GetComponent<StickyPiece>();
        if (sp == null)
            return;

        sp.MakeSticky();
        sp.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
    }
}
