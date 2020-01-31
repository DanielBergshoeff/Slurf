using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TrunkController : MonoBehaviour
{
    [SerializeField] private Transform trunkPart;
    [SerializeField] private Transform trunkEnd;
    [SerializeField] private float speed;

    private Rigidbody trunkPartRigidBody;
    private Rigidbody trunkEndRigidBody;

    private float xAxis;
    private float zAxis;

    private float endAxis;

    // Start is called before the first frame update
    void Start()
    {
        trunkPartRigidBody = trunkPart.GetComponent<Rigidbody>();
        trunkEndRigidBody = trunkEnd.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        trunkPartRigidBody.velocity = transform.right * xAxis + transform.forward * zAxis;
        trunkEndRigidBody.velocity = transform.up * endAxis;
    }

    public void MoveTrunkXAxis(InputAction.CallbackContext context) {
        xAxis = (float)context.ReadValueAsObject();
    }

    public void MoveTrunkZAxis(InputAction.CallbackContext context) {
        zAxis = (float)context.ReadValueAsObject();
    }

    public void MoveTrunkEnd(InputAction.CallbackContext context) {
        endAxis = (float)context.ReadValueAsObject();
    }
}
