using UnityEngine.InputSystem;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorials;
    private int currentTutorial = 0;
    private float xAxis, zAxis, xAxisRotation = 0f;
    private bool busy;
    private bool snotShooted;

    [SerializeField] private InputAction triggerAction;
    [SerializeField] private InputAction suckingAction;
    private bool sucking;
    private bool triggerPressed = false;
    private float endAxis;

    private void Awake()
    {
        suckingAction.started += SuckingTrue;
        triggerAction.started += TriggerPressedTrue;

        ShowTutorial();
    }

    private void ShowTutorial()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);

        Instantiate(tutorials[currentTutorial], transform);
        currentTutorial++;
        busy = false;
    }

    private void Update()
    {
        if (busy) { return; }
        print(currentTutorial);
        switch (currentTutorial)
        {
            case 1:
                if (xAxis > 0 && zAxis > 0)
                {
                    busy = true;
                    Invoke("ShowTutorial", 2f);
                }
                break;
            case 2:
                if (endAxis > 0)
                {
                    busy = true;
                    Invoke("ShowTutorial", 2f);
                }
                break;
            case 3:
                if (sucking) // needs a suck
                {
                    busy = true;
                    Invoke("ShowTutorial", 2f);
                }
                break;
            case 4:
                if (xAxisRotation > 0) //move the snoot only
                {
                    busy = true;
                    Invoke("ShowTutorial", 2f);
                }
                break;
            case 5:
                if (snotShooted) //shoot snot
                {
                    busy = true;
                    Invoke("ShowTutorial", 2f);
                }
                break;
            default:
                break;
        }
    }

    public void MoveTrunkXAxis(InputAction.CallbackContext context)
    { xAxis = (float)context.ReadValueAsObject(); }

    public void MoveTrunkZAxis(InputAction.CallbackContext context)
    { zAxis = (float)context.ReadValueAsObject(); }

    public void MoveTrunkEnd(InputAction.CallbackContext context)
    {
        if (!triggerPressed)
            endAxis = (float)context.ReadValueAsObject();
        else
            xAxisRotation = (float)context.ReadValueAsObject();
    }


    public void ShootSnot(InputAction.CallbackContext context)
    { snotShooted = true; }

    private void SuckingTrue(InputAction.CallbackContext context)
    { sucking = true; }

    private void OnEnable()
    {
        suckingAction.Enable();
        triggerAction.Enable();

    }

    private void OnDisable()
    {
        suckingAction.Disable();
        triggerAction.Disable();
    }

    private void TriggerPressedTrue(InputAction.CallbackContext context)
    { triggerPressed = true; }
}
