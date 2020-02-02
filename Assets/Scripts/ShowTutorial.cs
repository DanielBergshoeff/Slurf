using UnityEngine;
using UnityEngine.Events;

public class ShowTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorials;
    private int currentTutorial = 0;

    public UnityEvent reset;

    private bool innit = false;

    private int _playersDone;
    public int playersDone
    {
        get { return _playersDone; }
        set
        {
            _playersDone += value;
            if (_playersDone <= 2) return;

            Show();
            _playersDone = 0;
        }
    }

    private void Update()
    {
        if (FindObjectOfType<TutorialManager>() != null && !innit)
        { Show(); innit = true; }
    }

    private void Show()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);

        Instantiate(tutorials[currentTutorial], transform);
        currentTutorial++;
        reset.Invoke();
    }
}
