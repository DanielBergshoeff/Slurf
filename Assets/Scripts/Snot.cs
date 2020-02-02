using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snot : MonoBehaviour
{
    private AudioSource myAudioSource;

    private void Awake() {
        myAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        CheckSuckable(other.gameObject);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        myAudioSource.PlayOneShot(AudioManager.Instance.AudioSnotImpact);
        Destroy(GetComponent<Collider>());
        Invoke("DestroySelf", 3.0f);
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

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
