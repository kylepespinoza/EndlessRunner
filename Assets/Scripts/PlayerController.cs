using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EndlessRunnerController
{

    Rigidbody rigid;
    public float speed, xSpeed;

    Vector3 startPos;
    Quaternion startRot;

    public Vector3 centerOfMass;

    bool canJump = true;

    public bool unconscious;

    public Material defaultMaterial, knockedOutMaterial;

    public MeshRenderer meshRenderer;

    RigidbodyConstraints defaultRigidbodyConstraints;

    public enum MovementType
    {
        Free,
        Rails
    }

    Vector3 railsPositions;

    public enum RailPosition
    {
        Left,
        Center,
        Right
    }

    public RailPosition currentRailPosition = RailPosition.Center;
    public MovementType movementType;

    void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        railsPositions = new Vector3(transform.position.x - 3f, transform.position.x, transform.position.x + 3f); // Positions for the rails which the player moves along (L , C , R)
        currentRailPosition = RailPosition.Center;
        defaultRigidbodyConstraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Start()
    {
        Recover();
    }

    void Update()
    {
        //if (speed < 300)
        //{
        //    rigid.AddRelativeForce((Vector3.forward * 1000f) * Time.deltaTime, ForceMode.Force);
        //}
        speed = Vector3.SqrMagnitude(rigid.velocity);
        if (!unconscious)
        {
            if (movementType == MovementType.Free)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rigid.AddRelativeForce((Vector3.left * 1000f) * Time.deltaTime, ForceMode.Force);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rigid.AddRelativeForce((Vector3.right * 1000f) * Time.deltaTime, ForceMode.Force);
                }
            }
            else if (movementType == MovementType.Rails)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CheckRailPosition_AttemptMove(RailPosition.Left);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CheckRailPosition_AttemptMove(RailPosition.Right);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
            {
                StartCoroutine(ResetJump());
                rigid.AddRelativeForce((Vector3.up * 500) * Time.deltaTime, ForceMode.Impulse);
            }
        }
        if (transform.position.y < -50f)
        {
            //just reset for now - no death logic
            ResetPlayer();
        }

    }

    public override void OnNotification(string p_event_path, Object p_target, params Object[] p_data)
    {
        if (p_event_path == EndlessRunnerNotification.PlayerDied)
        {
            StartCoroutine(Knockout_Recover_andReset());
        }
    }

    IEnumerator ResetJump()
    {
        canJump = false;
        yield return new WaitForSeconds(2f);
        canJump = true;
    }

    void CheckRailPosition_AttemptMove(RailPosition direction)
    {
        if (direction == RailPosition.Left)
        {
            if (currentRailPosition == RailPosition.Left) return;
            else if (currentRailPosition == RailPosition.Center)
            {
                transform.position = new Vector3(railsPositions[0], transform.position.y, transform.position.z);
                currentRailPosition = RailPosition.Left;
            }
            else if (currentRailPosition == RailPosition.Right)
            {
                transform.position = new Vector3(railsPositions[1], transform.position.y, transform.position.z);
                currentRailPosition = RailPosition.Center;
            }

        }
        else if (direction == RailPosition.Right)
        {
            if (currentRailPosition == RailPosition.Right) return;
            else if (currentRailPosition == RailPosition.Center)
            {
                transform.position = new Vector3(railsPositions[2], transform.position.y, transform.position.z);
                currentRailPosition = RailPosition.Right;
            }
            else if (currentRailPosition == RailPosition.Left)
            {
                transform.position = new Vector3(railsPositions[1], transform.position.y, transform.position.z);
                currentRailPosition = RailPosition.Center;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Collectible") && other.gameObject.layer != LayerMask.NameToLayer("Default"))
        {
            App.Notify(EndlessRunnerNotification.ScoreIncreased, this, other.gameObject);
        }
    }

    public IEnumerator Knockout_Recover_andReset()
    {
        Knockout();
        yield return new WaitForSeconds(2f);
        Recover();
        ResetPlayer();
    }

    public void Knockout()
    {
        Debug.Log("Knocked out!");
        rigid.constraints = RigidbodyConstraints.None;
        rigid.mass = 0.5f;
        meshRenderer.material = knockedOutMaterial;
        unconscious = true;
        rigid.ResetCenterOfMass();
    }

    public void Recover()
    {
        Debug.Log("Recovering...");
        rigid.constraints = defaultRigidbodyConstraints;
        meshRenderer.material = defaultMaterial;
        rigid.centerOfMass = centerOfMass;
        unconscious = false;
    }

    public void ResetPlayer()
    {
        Debug.Log("Resetting the player");
        rigid.Sleep();
        transform.position = startPos;
        transform.rotation = startRot;
        currentRailPosition = RailPosition.Center;
    }
}
