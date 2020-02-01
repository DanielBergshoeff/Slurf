using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorials;
    private int currentTutorial = 0;
    private float xAxis, zAxis, xAxisRotation = 0f;

    private void Awake()
    {
        StartCoroutine(ShowTutorial(true));
    }

    IEnumerator ShowTutorial(bool skipDelay = false)
    {
        if (!skipDelay)
        {
            yield return new WaitForSeconds(2);
            Destroy(transform.GetChild(0));
        }

        Instantiate(tutorials[currentTutorial], transform);
        yield return default;
    }

    private void Update()
    {
        print(currentTutorial);
        switch (currentTutorial)
        {
            case 1:
                if (xAxis > 0 && zAxis > 0)
                {
                    StartCoroutine(ShowTutorial());
                    currentTutorial++;
                }
                break;
            case 2:
                if (xAxisRotation > 0)
                {
                    StartCoroutine(ShowTutorial());
                    currentTutorial++;
                }
                break;
            default:
                break;
        }
    }

    public void MoveTrunkXAxis(InputAction.CallbackContext context)
    {
        xAxis = (float)context.ReadValueAsObject();
    }

    public void MoveTrunkZAxis(InputAction.CallbackContext context)
    {
        zAxis = (float)context.ReadValueAsObject();
    }

    public void MoveTrunkEnd(InputAction.CallbackContext context)
    {
        xAxisRotation = (float)context.ReadValueAsObject();
    }
}
