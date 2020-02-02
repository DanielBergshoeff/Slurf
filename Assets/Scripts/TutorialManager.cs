using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class TutorialManager : MonoBehaviour
{
    private ShowTutorial tutorialShower;
    private float xAxis, zAxis, xAxisRotation = 0f;
    private bool busy;
    private bool snotShooted;
    private int currentTutorial = 1;
    private GameObject _checkMark;
    public GameObject checkMark
    {
        get { return _checkMark; }
        set
        {
            _checkMark = value;
            _checkMark.SetActive(false);
        }
    }

    private bool sucking;
    private bool triggerPressed = false;
    private float endAxis;
    private TrunkController controller;
    private string name1;

    private PlayerInput playerInput;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
        { Destroy(this); }

        if (FindObjectsOfType<TutorialManager>().Length == 1)
            gameObject.name += "1";
        else
            gameObject.name += "2";

        controller = FindObjectOfType<TrunkController>();
        tutorialShower = FindObjectOfType<ShowTutorial>();
        tutorialShower.reset.AddListener(ResetCheckmark);
        name1 = "Check" + gameObject.name.Last();

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Rotate"].started += TriggerPressedTrue;
        playerInput.actions["Suck"].started += SuckingTrue;

        var pieces = FindObjectsOfType<StickyPiece>();
        foreach (var piece in pieces)
        {
            piece.tutorialDone = false;
        }
    }

    private void ResetCheckmark()
    {
        GetCheckMark();
        busy = false;
    }

    private void GetCheckMark()
    {
        checkMark = GameObject.Find(name1);
    }

    private void Update()
    {
        if (busy) { return; }
        switch (currentTutorial)
        {
            case 1:
                if (xAxis > 0 && zAxis > 0)
                {
                    busy = true;
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 2:
                if (endAxis > 0)
                {
                    busy = true;
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 3:
                if (sucking && controller.suckingItem != null)
                {
                    busy = true;
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 4:
                if (xAxisRotation > 0)
                {
                    busy = true;
                    Invoke("DidTutorial", 2f);
                }
                break;
            case 5:
                if (snotShooted)
                {
                    busy = true;
                    Invoke("DidTutorial", 2f);
                }
                break;
            default:
                print("done it");
                Invoke("TutorialDone", 2f);
                break;
        }
    }

    private void TutorialDone()
    {
        var pieces = FindObjectsOfType<StickyPiece>();
        foreach (var piece in pieces)
        {
            piece.tutorialDone = true;
        }
    }

    private void DidTutorial()
    {
        if (checkMark == null)
            GetCheckMark();
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
        playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();
    }

    private void TriggerPressedTrue(InputAction.CallbackContext context)
    { triggerPressed = true; }
}
