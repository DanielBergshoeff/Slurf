using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuckPosition : MonoBehaviour
{
    public MyCollisionEvent suckEvent;

    private void OnTriggerEnter(Collider other) {
        suckEvent.Invoke(other);
    }
}

public class MyCollisionEvent : UnityEvent<Collider> { }
