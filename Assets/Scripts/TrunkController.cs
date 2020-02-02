using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TrunkController : MonoBehaviour
{
    [Header("Trunk pieces")]
    [SerializeField] private Transform trunkPart;
    [SerializeField] private Transform trunkEnd;
    [SerializeField] private Transform suckPosition;

    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float snotCoolDownTotal = 3f;
    [SerializeField] private float snotForce = 250f;
    [SerializeField] private float suckingForce = 2.0f;

    private PlayerInput playerInput;

    [SerializeField] private GameObject snotPrefab;
    [SerializeField] private GameObject snotMuzzlePrefab;
    [SerializeField] private int trunkLayer;

    [Header("Audio")]
    [SerializeField] private AudioClip audioSnotShoot;
    [SerializeField] private AudioClip audioSucking;

    private Rigidbody trunkPartRigidBody;
    private Rigidbody trunkEndRigidBody;

    private float xAxis = 0f;
    private float zAxis = 0f;

    private float endAxis = 0f;

    private float xAxisRotation = 0f;
    private float zAxisRotation = 0f;

    private bool triggerPressed = false;
    private bool sucking = false;

    public Transform suckingItem = null;
    private SuckPosition suckPositionScript;

    private AudioSource snotAudio;
    private AudioSource suckAudio;

    private float snotCoolDown = 0f;

    private void Awake()
    {
        trunkPartRigidBody = trunkPart.GetComponent<Rigidbody>();
        trunkEndRigidBody = trunkEnd.GetComponent<Rigidbody>();

        suckPositionScript = suckPosition.GetComponent<SuckPosition>();
        if (suckPositionScript.suckEvent == null)
            suckPositionScript.suckEvent = new MyCollisionEvent();

        suckPositionScript.suckEvent.AddListener(SuckPositionTouched);

        snotAudio = gameObject.AddComponent<AudioSource>();
        suckAudio = gameObject.AddComponent<AudioSource>();

        if(MultiplayerSpawn.Instance != null) {
            if(MultiplayerSpawn.Instance.amtOfPlayers == 1) {
                transform.position = MultiplayerSpawn.Instance.Player1Position.position;
            }
            else {
                transform.position = MultiplayerSpawn.Instance.Player2Position.position;
            }
        }

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Rotate"].started += TriggerPressedTrue;
        playerInput.actions["Rotate"].canceled += TriggerPressedFalse;

        playerInput.actions["Suck"].started += SuckingTrue;
        playerInput.actions["Suck"].canceled += SuckingFalse;
    }

    private void SuckPositionTouched(Collider other)
    {
        if (!sucking || suckingItem != null || other.GetComponent<StickyPiece>() == null)
            return;

        suckingItem = other.transform.GetComponentInParent<Rigidbody>().transform;
        suckingItem.GetComponent<Rigidbody>().useGravity = false;
        suckingItem.GetComponent<Rigidbody>().isKinematic = true;
        suckingItem.parent = suckPosition;
        suckingItem.gameObject.layer = trunkLayer;
    }

    private void OnEnable()
    {
        playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();
    }

    private void TriggerPressedTrue(InputAction.CallbackContext context)
    {
        triggerPressed = true;
    }

    private void TriggerPressedFalse(InputAction.CallbackContext context)
    {
        triggerPressed = false;
    }

    private void SuckingTrue(InputAction.CallbackContext context)
    {
        sucking = true;
        suckAudio.clip = audioSucking;
        suckAudio.loop = true;
        suckAudio.Play();
        suckAudio.volume = 0.1f;
    }

    private void SuckingFalse(InputAction.CallbackContext context)
    {
        sucking = false;
        suckAudio.Stop();
        suckAudio.volume = 1.0f;

        if (suckingItem == null || suckingItem.parent != suckPosition)
        {
            suckingItem = null;
            return;
        }

        suckingItem.GetComponent<Rigidbody>().useGravity = true;
        suckingItem.GetComponent<Rigidbody>().isKinematic = false;
        suckingItem.parent = null;
        suckingItem.gameObject.layer = default;
        suckingItem = null;
    }

    private void Update()
    {
        UpdateTrunkPosition();
        Suck();
        UpdateSnotCooldown();
    }

    private void UpdateSnotCooldown()
    {
        if (snotCoolDown <= 0f)
            return;

        snotCoolDown -= Time.deltaTime;
    }

    private void UpdateTrunkPosition() {
        trunkPartRigidBody.velocity = transform.right * xAxis * speed + transform.forward * zAxis * speed;
        trunkEndRigidBody.velocity += transform.up * endAxis * speed;

        if (triggerPressed) {
            //trunkEnd.RotateAround(-Camera.main.transform.right * xAxisRotation * rotateSpeed + Camera.main.transform.forward * zAxisRotation * rotateSpeed);
            trunkEnd.RotateAround(trunkEnd.position, -Vector3.forward, xAxisRotation * rotateSpeed);
            trunkEnd.RotateAround(trunkEnd.position, Vector3.up, zAxisRotation * rotateSpeed);
        }
    }

    private void Suck()
    {
        if (!sucking)
            return;

        if (suckingItem != null)
            return;

        RaycastHit hit;
        Debug.DrawRay(trunkEnd.transform.position, trunkEnd.transform.up * 100f);
        if (Physics.Raycast(trunkEnd.transform.position, trunkEnd.transform.up, out hit, 100f, ~(1 << trunkLayer)))
        {
            if (!hit.collider.CompareTag("Suckable"))
                return;

            Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();
            Vector3 heading = (suckPosition.position - rb.transform.position).normalized;
            rb.MovePosition(rb.transform.position + heading * Time.deltaTime * suckingForce);
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
        if (!triggerPressed)
            endAxis = (float)context.ReadValueAsObject();
        else
            xAxisRotation = (float)context.ReadValueAsObject();
    }

    public void RotateTrunkEnd(InputAction.CallbackContext context)
    {
        if (triggerPressed)
            zAxisRotation = (float)context.ReadValueAsObject();
    }

    public void ShootSnot(InputAction.CallbackContext context)
    {
        if (snotCoolDown > 0f)
            return;

        snotCoolDown = snotCoolDownTotal;
        snotAudio.PlayOneShot(audioSnotShoot);

        GameObject snot = Instantiate(snotPrefab);
        snot.GetComponent<Rigidbody>().AddForce(suckPosition.up * snotForce);
        snot.transform.position = suckPosition.position;

        GameObject snotMuzzle = Instantiate(snotMuzzlePrefab);
        snotMuzzle.transform.position = suckPosition.position;
        snotMuzzle.transform.rotation = Quaternion.LookRotation(suckPosition.up);
    }
}
