using UnityEngine;
using UnityEngine.Events;

public class ShowTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorials;
    private int currentTutorial = 0;

    public UnityEvent reset;

    private int _playersDone;
    public int playersDone
    {
        get { return _playersDone; }
        set
        {
            _playersDone += value;
            if (_playersDone < 2) return;

            Show();
            _playersDone = 0;
        }
    }

    private void Awake()
    {
        Show();
    }

    private void Show()
    {
        print("Show next");
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);

        Instantiate(tutorials[currentTutorial], transform);
        reset.Invoke();
        currentTutorial++;
    }
}
