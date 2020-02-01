using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TrunkController : MonoBehaviour
{
    [SerializeField] private Transform trunkPart;
    [SerializeField] private Transform trunkEnd;
    [SerializeField] private Transform suckPosition;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float snotCoolDownTotal = 3f;

    [SerializeField] private InputAction triggerAction;
    [SerializeField] private InputAction suckingAction;

    [SerializeField] private GameObject snotPrefab;

    private Rigidbody trunkPartRigidBody;
    private Rigidbody trunkEndRigidBody;

    private float xAxis = 0f;
    private float zAxis = 0f;

    private float endAxis = 0f;

    private float xAxisRotation = 0f;
    private float zAxisRotation = 0f;

    private bool triggerPressed = false;
    private bool sucking = false;

    private Transform suckingItem = null;

    private float snotCoolDown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        trunkPartRigidBody = trunkPart.GetComponent<Rigidbody>();
        trunkEndRigidBody = trunkEnd.GetComponent<Rigidbody>();

        triggerAction.started += TriggerPressedTrue;
        triggerAction.canceled += TriggerPressedFalse;

        suckingAction.started += SuckingTrue;
        suckingAction.canceled += SuckingFalse;
    }

    private void OnEnable() {
        triggerAction.Enable();
        suckingAction.Enable();
    }

    private void OnDisable() {
        triggerAction.Disable();
        suckingAction.Disable();
    }

    private void TriggerPressedTrue(InputAction.CallbackContext context) {
        triggerPressed = true;
    }

    private void TriggerPressedFalse(InputAction.CallbackContext context) {
        triggerPressed = false;
    }

    private void SuckingTrue(InputAction.CallbackContext context) {
        sucking = true;
    }

    private void SuckingFalse (InputAction.CallbackContext context) {
        sucking = false;

        if (suckingItem == null)
            return;

        suckingItem.GetComponentInParent<Rigidbody>().useGravity = true;
        suckingItem.GetComponentInParent<Rigidbody>().isKinematic = false;
        suckingItem.parent = null;
        suckingItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTrunkPosition();
        Suck();
        UpdateSnotCooldown();
    }

    private void UpdateSnotCooldown() {
        if (snotCoolDown <= 0f)
            return;

        snotCoolDown -= Time.deltaTime;
    }

    private void UpdateTrunkPosition() {
        trunkPartRigidBody.velocity = transform.right * xAxis * speed + transform.forward * zAxis * speed;
        trunkEndRigidBody.velocity += transform.up * endAxis * speed;

        if (triggerPressed)
            trunkEnd.Rotate(-trunkEnd.transform.right * xAxisRotation * 10f + trunkEnd.transform.forward * zAxisRotation * 10f);
    }

    private void Suck() {
        if (!sucking)
            return;

        if (suckingItem != null)
            return;

        RaycastHit hit;
        Debug.DrawRay(trunkEnd.transform.position, trunkEnd.transform.up * 100f);
        if (Physics.Raycast(trunkEnd.transform.position, trunkEnd.transform.up, out hit, 100f)) {
            if (!hit.collider.CompareTag("Suckable"))
                return;
            
            Vector3 heading = (suckPosition.position - hit.collider.transform.position).normalized;
            hit.collider.transform.position = hit.collider.transform.position + heading * Time.deltaTime * 1f;

            if(Vector3.Distance(hit.collider.transform.position, suckPosition.position) < hit.collider.transform.localScale.x / 2f + 0.05f) {
                suckingItem = hit.collider.transform;
                suckingItem.GetComponentInParent<Rigidbody>().useGravity = false;
                suckingItem.GetComponentInParent<Rigidbody>().isKinematic = true;
                suckingItem.parent = suckPosition;
            }
        }
    }

    public void MoveTrunkXAxis(InputAction.CallbackContext context) {
        xAxis = (float)context.ReadValueAsObject();
    }

    public void MoveTrunkZAxis(InputAction.CallbackContext context) {
        zAxis = (float)context.ReadValueAsObject();
    }

    public void MoveTrunkEnd(InputAction.CallbackContext context) {
        if(!triggerPressed)
            endAxis = (float)context.ReadValueAsObject();
        else
            xAxisRotation = (float)context.ReadValueAsObject();
    }

    public void RotateTrunkEnd(InputAction.CallbackContext context) {
        if (triggerPressed)
            zAxisRotation = (float)context.ReadValueAsObject();
    }   

    public void ShootSnot(InputAction.CallbackContext context) {
        if (snotCoolDown > 0f)
            return;

        snotCoolDown = snotCoolDownTotal;

        GameObject snot = Instantiate(snotPrefab);
        snot.GetComponent<Rigidbody>().AddForce(suckPosition.up * 500f);
        snot.transform.position = suckPosition.position;
    }
}
