using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public Animator Motor;

    [Header("Layer Masks")]
    public LayerMask ClickMask;
    public LayerMask BuildMask;
    public LayerMask InteractMask;

    [Header("References")]
    public GameObject CurrentPlatform;
    public GameObject PlayerGFX;
    public GameObject PlatformModifierCanvas;
    public Camera PlayerCamera;

    [Header("AI")]
    public NavMeshAgent Agent;
    public int StartHoldTime;
    public AnimationClip AtackAnim;

    RaycastHit hit;
    GameObject _MineHit;
    bool IsModPlatform = false;
    public GameObject AimingPlatform;
    [HideInInspector]
    public bool UsingInventory = false;
    [HideInInspector]
    public bool UsingCraftTable = false;
    public static PlayerMovement instance;
    bool isCRunning = false;
    public Camera INVRend;




    private void Awake()
    {
        instance = this;
        Agent = this.GetComponent<NavMeshAgent>();
        PlatformModifierCanvas.SetActive(false);
    }

    void Update()
    {
        float dist = Agent.remainingDistance;
        if (dist != Mathf.Infinity && Agent.pathStatus == NavMeshPathStatus.PathComplete && Agent.remainingDistance == 0)
        {
            Motor.SetBool("IsMoving", false);
        }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount == 1))
        {
            StartCoroutine("UpdateNumber2");
        }
        if (Input.GetMouseButtonUp(0) && (Input.touchCount == 0))
        {
            StopCoroutine("UpdateNumber2");
            isCRunning = false;
        }

        if ((Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)) && !IsModPlatform && !UsingInventory && !UsingCraftTable)
        {
            if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                return;


            StopCoroutine("UpdateNumber2");
            isCRunning = false;
            //Funciona per android?
            Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

            //Picar Materials
            if (Physics.Raycast(ray, out hit, 100, InteractMask))
            {
                if (hit.collider.tag == "Mine" || hit.collider.tag == "PickUp" || hit.collider.tag == "Craft" || hit.collider.tag == "Ladder" || hit.collider.tag == "Enemy")
                {
                    //Move to interact
                    Motor.SetBool("IsMoving", true);
                    _MineHit = hit.collider.gameObject;
                    CancelInvoke("CheckDis");
                    Agent.SetDestination(hit.point);
                    InvokeRepeating("CheckDis", 0, .2f);
                    return;
                }
            }

            //Moviment
            if (Physics.Raycast(ray, out hit, 100, ClickMask))
            {
                if(hit.collider.tag != "Mine" && hit.collider.tag != "Craft" && hit.collider.tag != "PickUp" && !EventSystem.current.IsPointerOverGameObject(-1))
                {
                    CancelInvoke("CheckDis");
                    Agent.SetDestination(hit.point);
                    Motor.SetBool("Atack", false);
                    Motor.SetBool("IsMoving", true);
                }
            }

        }
    }


    IEnumerator UpdateNumber2()
    {
        //falta bloquejar perk la penya no pugui deixar de apretar i despres tornar a apretar
        if (isCRunning)
            yield break;

        isCRunning = true;
        yield return new WaitForSeconds(StartHoldTime);
        if(!Input.GetMouseButton(0) && Input.touchCount == 0)
        {
            isCRunning = false;
            yield break;
        }
        else
        {
            Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, ClickMask))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    AimingPlatform = hit.collider.gameObject;
                    PlatformModifierCanvas.transform.position = new Vector3(AimingPlatform.transform.position.x, AimingPlatform.transform.position.y + 0.294f, AimingPlatform.transform.position.z);
                    PlatformModifierCanvas.SetActive(true);

                    IsModPlatform = true;
                    isCRunning = false;
                }
            }
        }
    }


    //Acivaing UI
    //int HoldTime;
    /*void UpdateNumber()
    {
        HoldTime++;
        if (HoldTime >= StartHoldTime)
        {
            //Funciona per android?
            Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, ClickMask))
            {
                if(hit.collider.gameObject.tag == "Floor")
                {
                    AimingPlatform = hit.collider.gameObject;
                    PlatformModifierCanvas.transform.position = new Vector3(AimingPlatform.transform.position.x, AimingPlatform.transform.position.y + 0.294f, AimingPlatform.transform.position.z);
                    PlatformModifierCanvas.SetActive(true);
                    PlayerCamera.GetComponent<CameraSmoothFollow>().target = AimingPlatform.transform;
                    IsModPlatform = true;
                }
            }
        }
    }
    */

    public void OutModPlatMode()
    {
        PlatformModifierCanvas.SetActive(false);
        AimingPlatform = null;

        Invoke("Delayer", .2f);
    }
    void Delayer()
    {
        IsModPlatform = false;
    }

    void CheckDis()
    {
        Motor.SetBool("Atack", false);
        if (Agent.remainingDistance != 0 && Agent.remainingDistance - .1f <= 0)
        {
            CancelInvoke("CheckDis");
            //Aquest if es per si esta anant a un interactabe
            if (_MineHit != null)
            {




            }

        }
    }

    //IEnumerator Repeat(GameObject Enemy)
    //{

    //}





}
