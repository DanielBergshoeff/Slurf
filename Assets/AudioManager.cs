using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager mInstance;

    public static AudioManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                mInstance = go.AddComponent<AudioManager>();
            }
            return mInstance;
        }
    }

    private void Awake()
    {
        mInstance = this;
    }

    public AudioClip GetSingleAudioClipTouch()
    {
        return AudioStickyPieceTouch[Random.Range(0, AudioStickyPieceTouch.Count)];
    }

    public List<AudioClip> AudioStickyPieceTouch;
    public AudioClip AudioSnotImpact;
    public AudioClip AudioSnotTouch;
}
