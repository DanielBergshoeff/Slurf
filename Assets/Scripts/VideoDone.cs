using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoDone : MonoBehaviour
{
    private void Start()
    {
        GetComponent<VideoPlayer>().loopPointReached += loadTutorial;
    }

    private void loadTutorial(VideoPlayer source)
    {
        gameObject.GetComponent<SceneLoader>().Interact();
    }
}
