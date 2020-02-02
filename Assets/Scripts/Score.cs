using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    static Score mInstance;

    public static Score Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                mInstance = go.AddComponent<Score>();
            }
            return mInstance;
        }
    }

    private GameObject scoreBord;

    private void Awake()
    {
        scoreBord = (GameObject)Resources.Load("Scoreboard_Inbetween");
    }

    public void ShowScore(string s)
    {
        Transform transform1 = FindObjectOfType<Canvas>().transform.root;
        foreach (Transform child in transform1)
        {
            Destroy(child.gameObject);
        }
        GameObject gameObject1 = Instantiate(scoreBord, transform1);
        gameObject1.GetComponentInChildren<Button>().name = s;
        FindObjectOfType<SceneLoader>().Interact();

    }
}