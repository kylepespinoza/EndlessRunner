using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rigid;
    public float speed, xSpeed;

    Vector3 startPos;
    Quaternion startRot;

    bool canJump = true;

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
        rigid = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        railsPositions = new Vector3(transform.position.x - 3f, transform.position.x, transform.position.x + 3f); // Positions for the rails which the player moves along (L , C , R)
        currentRailPosition = RailPosition.Center;
    }

    void Update()
    {
        //if (speed < 300)
        //{
        //    rigid.AddRelativeForce((Vector3.forward * 1000f) * Time.deltaTime, ForceMode.Force);
        //}
        speed = Vector3.SqrMagnitude(rigid.velocity);
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
            rigid.AddRelativeForce((Vector3.up * 1000) * Time.deltaTime, ForceMode.Impulse);
        }
        if (transform.position.y < -50f)
        {
            ResetPlayer();
        }
        //if (Vector3.SqrMagnitude(Vector3.Scale(rigid.velocity, Vector3.right)) > 0f)
        //{

        //}
        //xSpeed = Vector3.SqrMagnitude(Vector3.Scale(rigid.velocity, Vector3.right));
        if (rigid.velocity.x > 0f)
        {

        }
        if (transform.InverseTransformDirection(rigid.velocity).x > 0 && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity += Vector3.left * Time.deltaTime * 10f;
        }
        if (transform.InverseTransformDirection(rigid.velocity).x < 0 && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity += Vector3.right * Time.deltaTime * 10f;
        }

    }

    void ResetPlayer()
    {
        rigid.Sleep();
        transform.position = startPos;
        transform.rotation = startRot;
        currentRailPosition = RailPosition.Center;
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
}
