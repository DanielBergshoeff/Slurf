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

    private void OnCollisionEnter(Collision collision) {
        CheckSuckable(collision.gameObject);
        Destroy(gameObject);
    }

    private void CheckSuckable(GameObject go) {
        if (!go.CompareTag("Suckable"))
            return;

    }
}
