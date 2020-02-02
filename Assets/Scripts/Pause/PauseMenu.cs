using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

[ExecuteInEditMode]
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private List<PauseObject> toPause = new List<PauseObject>();

    [SerializeField]
    private GameObject pauseUI;
    private bool pauzed = false;
    private List<MonoBehaviour> objectToPause = new List<MonoBehaviour>();
    private GameObject cachedPauseUI;
    private AudioSource[] allAudioSources;

    [ContextMenu("FindPauseables")]
    private void FindPausables()
    {
        MonoBehaviour[] pauseables = FindObjectsOfType<MonoBehaviour>();
        List<string> exists = new List<string>();
        for (int i = 0; i < pauseables.Length; i++)
        {
            MonoBehaviour pauseable = pauseables[i];

            if (pauseable == this) { continue; }

            string behaviorName = pauseable.GetType().Name;
            if (exists.FirstOrDefault(stringToCheck => stringToCheck.Contains(behaviorName)) == null)
            {
                toPause.Add(new PauseObject(behaviorName));
                exists.Add(behaviorName);
            }
        }
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            MonoBehaviour[] pauseables = FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < pauseables.Length; i++)
            {
                MonoBehaviour pauseable = pauseables[i];

                if (pauseable == this) { continue; }
                if (pauseable.enabled == false) { continue; }

                objectToPause.Add(pauseable);
            }

            // pauseUI = (GameObject)Resources.Load("PauseMenu");

            cachedPauseUI = Instantiate(pauseUI, transform);
            cachedPauseUI.GetComponentInChildren<Button>().onClick.AddListener(Pause);
            cachedPauseUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Pause(); }
    }

    public void Pause()
    {
        pauzed = TogglePause();
        ToggleMonoBehaviours();
    }

    private bool TogglePause()
    {
        Cursor.visible = !Cursor.visible;

        if (pauzed)
        {
            Time.timeScale = 1f;
            cachedPauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            cachedPauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            return (true);
        }
    }

    private void ToggleMonoBehaviours()
    {
        var copy = new List<MonoBehaviour>(objectToPause);
        foreach (MonoBehaviour pausable in copy)
        {
            if (pausable == default) { objectToPause.Remove(pausable); continue; }

            if (!MayPause(pausable)) { continue; }

            try
            {
                pausable.enabled = !pauzed;
            }
            catch
            {
                pausable.enabled = true;
                Debug.LogWarning("Something went wrong but it's no problem.");
            }
        }
        allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            if (source.isPlaying)
                source.Pause();
            else
                source.UnPause();
        }

    }

    private bool MayPause(MonoBehaviour behavior)
    {
        foreach (PauseObject item in toPause)
        {
            if (behavior.GetType().Name == item.behavior && item.pause)
            {
                return true;
            }
        }
        return false;
    }
}