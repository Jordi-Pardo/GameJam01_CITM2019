//Code by MaykG (Miquel Suau)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player_Motor : MonoBehaviour
{

    [Header("Movement Values")]
    public float MovementSpeed;
    public Rigidbody rb;
    public bool CanMove = true;
    public float SafeDistance;


    [Header("References")]
    public Camera PlayerCamera;
    public LayerMask MouseHits;
    public Transform RelativeMovement;

    [Header("Bools")]
    public bool[] Inputs = {false, false, false, false};
    [HideInInspector]
    public List<KeyCode> Base = new List<KeyCode> { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A };
    public List<KeyCode> AuxBas = new List<KeyCode>();

    [Header("Vectors")]
    public Vector3 RotLimit;

    [Header("Animations")]
    public Animator anim;
    public bool IsMoving = false;
    public static Player_Motor instance;

    private void Awake()
    {
        AuxBas = Base;
        instance = this;
    }

    private void Update()
    {

        Vector3 rot = PlayerCamera.transform.rotation.eulerAngles;
        rot.x = 0;

        RelativeMovement.eulerAngles = rot;

        if (CanMove)
        {
            if (Input.GetKey(AuxBas[0]))
            {
                Inputs[0] = true;
                IsMoving = true;
            }
            else
            {
                Inputs[0] = false;
            }

            if (Input.GetKey(AuxBas[1]))
            {
                Inputs[1] = true;
                IsMoving = true;
            }
            else
            {
                Inputs[1] = false;
            }

            if (Input.GetKey(AuxBas[2]))
            {
                Inputs[2] = true;
                IsMoving = true;
            }
            else
            {
                Inputs[2] = false;
            }

            if (Input.GetKey(AuxBas[3]))
            {
                Inputs[3] = true;
                IsMoving = true;
            }
            else
            {
                Inputs[3] = false;
            }

            MouseController();

            if (!IsMoving)
            {
                anim.SetBool("isRunning", false);
                anim.SetFloat("VelocityX", 0.5f);
                anim.SetFloat("VelocityY", 0);
            }

        }


    }

    public void FixedUpdate()
    {

        Vector3 FW = Vector3.zero;
        Vector3 BW = Vector3.zero;
        Vector3 RW = Vector3.zero;
        Vector3 LW = Vector3.zero;

        if(rb.velocity.magnitude <= new Vector3(2, 2, 2).magnitude)
        {
            IsMoving = false;
            anim.SetFloat("VelocityX", 0.5f);
            anim.SetFloat("VelocityY", 0);
        }

        Vector3 LocalVel = transform.InverseTransformDirection(rb.velocity);

        if (LocalVel.z > 3f)
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("VelocityY", 1);
            anim.SetFloat("VelocityX", 0.5f);
        }
        else if (LocalVel.z < -3f)
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("VelocityY", 0);
            anim.SetFloat("VelocityX", 0.5f);
        }

        if(LocalVel.x > 3f)
        {
            //Dreta
            anim.SetBool("isRunning", true);
            anim.SetFloat("VelocityX", 1);
            anim.SetFloat("VelocityY", 0);

        }
        else if (LocalVel.x < -3f)
        {

            anim.SetBool("isRunning", true);
            anim.SetFloat("VelocityX", 0);
            anim.SetFloat("VelocityY", 0);

        }

        if (Inputs[0])
        {
            FW = (RelativeMovement.forward) * MovementSpeed * Time.deltaTime;
        }

        if (Inputs[1])
        {
            BW = (RelativeMovement.forward * -1) * MovementSpeed * Time.deltaTime;
        }


        if (Inputs[2])
        {
            RW = RelativeMovement.right * MovementSpeed * Time.deltaTime;
        }

        if (Inputs[3])
        {
            LW = (RelativeMovement.right * -1) * MovementSpeed * Time.deltaTime;
        }

        rb.velocity = FW + BW + RW + LW;

        if((Inputs[0] == true && Inputs[2] == true) || (Inputs[0] == true && Inputs[3] == true) || (Inputs[1] == true && Inputs[3] == true) || (Inputs[1] == true && Inputs[2] == true))
        {
            rb.velocity /= 1.5f;
        }

    }



    public void MouseController()
    {
        RaycastHit _hit;

        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, MouseHits))
        {
            if (Vector3.Distance(this.gameObject.transform.position, _hit.point) > SafeDistance && _hit.collider.gameObject.tag != "Player")
            {
                this.transform.LookAt(_hit.point);
            }
        }

    }




}