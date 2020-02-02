using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{
    private ShowTutorial tutorialShower;
    private float xAxis, zAxis, xAxisRotation = 0f;
    private bool busy;
    private bool snotShooted;
    private int currentTutorial = 1;
    private GameObject checkMark;

    [SerializeField] private InputAction triggerAction;
    [SerializeField] private InputAction suckingAction;
    private bool sucking;
    private bool triggerPressed = false;
    private float endAxis;
    private TrunkController controller;

    private void Awake()
    {
        controller = FindObjectOfType<TrunkController>();
        tutorialShower = FindObjectOfType<ShowTutorial>();
        tutorialShower.reset.AddListener(StepDone);
        suckingAction.started += SuckingTrue;
        triggerAction.started += TriggerPressedTrue;
    }

    private void Start()
    {
        string name1 = "Check" + gameObject.name.Last();
        checkMark = GameObject.Find(name1);
        checkMark.SetActive(false);
    }

    private void StepDone()
    {
        string name1 = "Check" + gameObject.name.Last();
        checkMark = GameObject.Find(name1);
        checkMark.SetActive(false);
        busy = false;
    }

    private void Update()
    {
        if (busy) { return; }
        switch (currentTutorial)
        {
            case 1:
                if (xAxis > 0 && zAxis > 0)
                {
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 2:
                if (endAxis > 0)
                {
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 3:
                if (sucking && controller.suckingItem != null)
                {
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 4:
                if (xAxisRotation > 0)
                {
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 5:
                if (snotShooted)
                {
                    Invoke("DidTutorial", 2f);
                }
                break;
            default:
                break;
        }
    }

    private void DidTutorial()
    {
        busy = true;
        checkMark.SetActive(true);
        tutorialShower.playersDone++;
        currentTutorial++;
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
    {
        if (currentTutorial > 4)
            snotShooted = true;
    }

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
