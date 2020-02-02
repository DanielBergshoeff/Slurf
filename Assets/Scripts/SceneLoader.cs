using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject transitionUI;
    [SerializeField] private float transitionOffset = 2f;
    [SerializeField] private AudioClip transitionInAudio;
    [SerializeField] private AudioClip transitionOutAudio;

    private Task fadeIn;
    private Task fadeOut;
    private static GameObject transition;
    private static CanvasGroup canvas;
    private static AudioSource source;
    private const float iterations = 50f;

    public bool interacted { get; set; }

    private void Innit()
    {
        transition = Instantiate(transitionUI);
        transition.SetActive(false);

        canvas = transition.GetComponentInChildren<CanvasGroup>();
        canvas.alpha = 0;

        DontDestroyOnLoad(transition);
    }

    private void OnTriggerEnter(Collider col)
    {
        Innit();
        Interact();
    }

    public void Interact()
    {
        if (GetComponent<Collider>())
            GetComponent<Collider>().enabled = false;

        QuitGame(gameObject.name);
        Innit();
        StartSceneTransition();
    }

    private void StartSceneTransition()
    {
        transition.SetActive(true);

        fadeIn = new Task(FadeOut());

        PlayAudio(transitionInAudio);

        fadeIn.Finished += LoadScene;
    }

    private void EndSceneTransition(Scene scene, LoadSceneMode mode)
    {
        fadeOut = new Task(FadeIn());

        PlayAudio(transitionInAudio);

        fadeOut.Finished += CleanUp;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (source == null)
            source = transition.AddComponent<AudioSource>();

        source.enabled = true;
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    private void LoadScene(bool manual)
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += EndSceneTransition;
        SceneManager.LoadScene(gameObject.name);
    }


    public void LoadScene(string scene, GameObject transitionObject)
    {
        transitionUI = transitionObject;
        gameObject.name = scene;
        Innit();
        StartSceneTransition();
    }

    private void CleanUp(bool manual)
    {
        try
        {
            Destroy(transition);
            Destroy(gameObject);
        }
        catch { }
    }

    private static void QuitGame(string sceneName)
    {
        if (sceneName != "Exit") { return; }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator FadeOut()
    {
        List<AudioSource> sources = FindObjectsOfType<AudioSource>().ToList();
        sources.Remove(source);

        for (int i = 0; i <= iterations; i += 1)
        {
            float a = canvas.alpha;
            a = i / iterations;
            canvas.alpha = a;
            foreach (AudioSource s in sources)
                s.volume = 1 - a;

            yield return null;
        }

        float length = transitionOutAudio ? transitionInAudio.length : 0;
        yield return new WaitForSeconds(length + transitionOffset);
    }

    IEnumerator FadeIn()
    {
        List<AudioSource> sources = FindObjectsOfType<AudioSource>().ToList();
        sources.Remove(source);

        foreach (AudioSource s in sources)
            s.volume = 0f;

        float length = transitionOutAudio ? transitionInAudio.length : 0;
        yield return new WaitForSeconds(length + transitionOffset);

        for (int i = (int)iterations; i >= 0; i -= 1)
        {
            float a = canvas.alpha;
            a = i / iterations;
            canvas.alpha = a;

            foreach (AudioSource s in sources)
                s.volume = Mathf.Abs(a - 1f);

            yield return null;
        }
    }

    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, "Exit.png", true);
    }
}
