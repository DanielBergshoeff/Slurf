using UnityEngine;

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

    public void ShowScore()
    {
        Transform transform1 = FindObjectOfType<Canvas>().transform.root;
        foreach (Transform child in transform1)
        {
            Destroy(child.gameObject);
        }
        Instantiate(scoreBord, transform1);
    }
}