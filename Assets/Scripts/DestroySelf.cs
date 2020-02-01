using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float TimeTillDestroy = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelfNow", TimeTillDestroy);
    }

    private void DestroySelfNow() {
        Destroy(gameObject);
    }
}
