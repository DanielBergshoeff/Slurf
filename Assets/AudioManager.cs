using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public AudioClip GetSingleAudioClipTouch() {
        return AudioStickyPieceTouch[Random.Range(0, AudioStickyPieceTouch.Count)];
    }
     
    public List<AudioClip> AudioStickyPieceTouch;
    public AudioClip AudioSnotImpact;
    public AudioClip AudioSnotTouch;
}
