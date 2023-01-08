using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _horizontal;
    float _vertical;
    float _lastHorizontal;  //last recorded inputs before we hit 0 for idle states
    float _lastVertical;
    float _headHorizontal;
    float _headVertical;
    float _horizontalHeadError;
    float _verticalHeadError;

    public float attackCoolDown;    //temp

    public float speed;
    public float baseCamSpeed;
    public float targetDistance;    //distance the target can be before it goes back to player prespective
    public float playerDistance;    //distance that the camera will go away from the player before trying to pull it back to the player 
    float _camSpeed;
    public bool hovering;   //is the mouse over an object that can be tracked by camera
    public float lookTime;
    public float mouseDistance; //the distance we can look with the mouse before it hates us and it ang >:(
    public bool mouseLookDelay; //choppy stoppy
    public float mouseDelayTime;

    //bool cooldowns
    bool isAttackCooldown;
    bool isLookCooldown;

    //bool states
    bool attack;
    bool walk;
    bool sneak;
    bool sprint;

    public Item currentWeapon;

    Vector3 cameraDestination;
    Vector3 lastMousePos;
    public Transform cameraDestinationTransform;
    public GameObject playerArm;
    GameObject _playerSwing;
    public GameObject playerWeapon;
    BoxCollider2D _bcWeapon;
    SpriteRenderer _srWeapon;
    Animator anim;
    Animator headAnim;
    [HideInInspector]public ContainerArray inventory;
    public int inventorySize;

    private void Awake()
    {
        cameraDestination = transform.position;
        cameraDestinationTransform = transform;
        _bcWeapon = playerWeapon.GetComponent<BoxCollider2D>();
        _srWeapon = playerWeapon.GetComponent<SpriteRenderer>();
        _playerSwing = playerWeapon.transform.parent.gameObject;
        anim = GetComponent<Animator>();
        headAnim = GetComponentsInChildren<Animator>()[1];
        inventory = new ContainerArray(inventorySize, GameObject.Find("Player Grid"));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            inventory.Test();
        }

        _camSpeed = Vector2.Distance(cameraDestination, Camera.main.transform.position) > 5 ? baseCamSpeed : 
            baseCamSpeed * Mathf.Abs(Vector2.Distance(cameraDestination, Camera.main.transform.position) / 5);

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            _lastHorizontal = _horizontal;
            _lastVertical = _vertical;
        }
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        transform.Translate(speed * Time.deltaTime * new Vector2(_horizontal, _vertical).normalized);

        if(GameObject.FindGameObjectsWithTag("Equip Slot")[0].GetComponent<Slot>().heldItem != null)
        {
            currentWeapon = GameObject.FindGameObjectsWithTag("Equip Slot")[0].GetComponent<Slot>().heldItem;
            _srWeapon.sprite = currentWeapon.inventoryIcon;
        }
        else
        {
            _srWeapon.sprite = null;
        }

        Vector3 newScale = transform.localScale;
        newScale.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x > 0 ? 1 : -1;
        playerArm.transform.localScale = newScale;

        //check if the target position is still within player range
        if (cameraDestinationTransform != transform)
        {
            if (Vector2.Distance(transform.position, cameraDestination) > targetDistance)
            {
                //switching to player tracking
                cameraDestination = transform.position;
                cameraDestinationTransform = transform;
            }
            else
            {
                //updating for other target tracking
                cameraDestination = cameraDestinationTransform.position;
            }
        }
        else
        {
            //updating for player tracking
            cameraDestination = transform.position;
        }
        

        if(Input.GetMouseButtonDown(2) && cameraDestinationTransform != transform)
        {
            cameraDestinationTransform = transform;
            cameraDestination = transform.position;
        }

        if (walk || sneak || sprint)
        {
            if(!Input.GetMouseButton(2))
            {
                StopAllCoroutines();
                isLookCooldown = false;
                _headHorizontal = _horizontal;
                _headVertical = _vertical;
            }
        }
        if (lastMousePos != Input.mousePosition)
        {
            if (!walk && !sneak && !sprint || walk && Input.GetMouseButton(2) || sneak && Input.GetMouseButton(2) || sprint && Input.GetMouseButton(2))
            {
                StopAllCoroutines();
                isLookCooldown = true;
                StartCoroutine(LookTimer());
            }
        }
        else if (!isLookCooldown && !walk && !sneak && !sprint)
        {
            _headHorizontal = _lastHorizontal;
            _headVertical = _lastVertical;

        }

        if (isLookCooldown)
        {
            _headHorizontal = GameObject.Find("MouseTracker").transform.localPosition.x;
            _headVertical = GameObject.Find("MouseTracker").transform.localPosition.y;

            if (HeadCheck(_lastHorizontal, _lastVertical, _headHorizontal, _headVertical) == "Out of Range")
            if (_horizontal != 0 && _vertical != 0)
            {
                if (HeadCheck(_horizontal, _vertical, _headHorizontal, _headVertical) == "Out of Range")
                {
                    _headHorizontal = _horizontalHeadError;
                    _headVertical = _verticalHeadError;
                }
            }
            else
            {
                _headHorizontal = _horizontalHeadError;
                _headVertical = _verticalHeadError;
                if(!walk && !sprint && !sneak)
                {
                    _horizontal += _horizontal + _headHorizontal > 1 || _horizontal + _headHorizontal < -1 ? 0 : _headHorizontal;
                    _vertical += _vertical + _headVertical > 1 || _vertical + _headVertical < -1 ? 0 : _headVertical;
                    _lastHorizontal += _lastHorizontal + _headHorizontal > 1 || _lastHorizontal + _headHorizontal < -1 ? 0 : _headHorizontal;
                    _lastVertical += _lastVertical + _headVertical > 1 || _lastVertical + _headVertical < -1 ? 0 : _headVertical;
                }
            }
        }

        if (cameraDestinationTransform != transform)
        {
            _headHorizontal = cameraDestinationTransform.localPosition.x - transform.position.x;
            _headVertical = cameraDestinationTransform.localPosition.y - transform.position.y;
            if (HeadCheck(_horizontal, _vertical, _headHorizontal, _headVertical) == "Out of Range")
            {
                _headHorizontal = _horizontalHeadError;
                _headVertical = _verticalHeadError;
                //check first if we should
                //_lastHorizontal += _lastHorizontal + _headHorizontal > 1 || _lastHorizontal + _headHorizontal < -1 ? 0 : _headHorizontal;
                //_lastVertical += _lastVertical + _headVertical > 1 || _lastVertical + _headVertical < -1 ? 0 : _headVertical;
            }
            //check for combat/enemy target
            //move body with head
        }

        lastMousePos = Input.mousePosition;

        //controls

        if(Input.GetMouseButtonDown(0) && !isAttackCooldown && currentWeapon != null)
        {
            Invoke("AttackCooldown", attackCoolDown);
            isAttackCooldown = true;
            attack = true;  //also animation
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            sneak = true;
            speed = 2;
        }
        else
        {
            sneak = false;
            if(!sprint)
                speed = 4;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
            speed = 8;
        }
        else
        {
            sprint = false;
            if(!sneak)
                speed = 4;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            inventory.ToggleInventory();

        //----------animation---------------

        if (_horizontal == 0 && _vertical == 0)
        {
            walk = false;
        }
        else
        {
            walk = true;
        }

        //move player arm to mouse
        if(!attack)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            playerArm.transform.right = -mousePos;
        }

        //animation states
        anim.SetFloat("horizontal", _horizontal);
        anim.SetFloat("vertical", _vertical);
        anim.SetBool("attack", attack);
        anim.SetFloat("lastHorizontal", _lastHorizontal);
        anim.SetFloat("lastVertical", _lastVertical);
        anim.SetBool("walk", walk);
        anim.SetBool("sneak", sneak);
        anim.SetBool("sprint", sprint);
        if(!attack)
        {
            headAnim.SetFloat("headHorizontal", _headHorizontal);
            headAnim.SetFloat("headVertical", _headVertical);
        }
    }

    void AttackCooldown()
    {
        isAttackCooldown = false;
        attack = false;
    }
    IEnumerator LookTimer()
    {
        yield return new WaitForSeconds(lookTime);
        isLookCooldown = false;
    }
    void MouseLookDelay()
    {
        mouseLookDelay = false;
    }
    public string HeadCheck(float bodyHorizontal, float bodyVertical, float headHorizontal, float headVertical)   //realitically return Empty is the same as In Range
    {
        _horizontalHeadError = 0;
        _verticalHeadError = 0;

        if(bodyHorizontal == 1)
        {
            if(bodyVertical == 1)
            {
                //y>= -x                    //return on these floats is irrelivant if we return in range
                _horizontalHeadError = headVertical > 0 ? -1 : 1;
                _verticalHeadError = headVertical > 0 ? 1 : -1; 
                if (headVertical >= -headHorizontal)
                    return "In Range";
                else
                    return "Out of Range";
                
            }
            else if(bodyVertical == 0)
            {
                //x>= 0
                _horizontalHeadError = 0;
                _verticalHeadError = headVertical > 0 ? 1 : -1;
                if (headHorizontal >= 0)
                    return "In Range";
                else
                    return "Out of Range";
            }
            else if(bodyVertical == -1)
            {
                //y<= x
                _horizontalHeadError = headVertical > 0 ? 1 : -1; 
                _verticalHeadError = headVertical > 0 ? 1 : -1;
                if (headVertical <= headHorizontal)
                    return "In Range";
                else
                    return "Out of Range";
            }
        }
        else if(bodyHorizontal == 0)
        {
            if (bodyVertical == 1)
            {
                //y >= 0
                _horizontalHeadError = headHorizontal > 0 ? 1 : -1; 
                _verticalHeadError = 0;
                if (headVertical >= 0)
                    return "In Range";
                else
                    return "Out of Range";
            }
            else if (bodyVertical == -1)
            {
                //y <= 0
                _horizontalHeadError = headHorizontal > 0 ? 1 : -1;
                _verticalHeadError = 0;
                if (headVertical <= 0)
                    return "In Range";
                else
                    return "Out of Range";
            }
        }
        else if(bodyHorizontal == -1)
        {
            if (bodyVertical == 1)
            {
                //y>= x
                _horizontalHeadError = headVertical > 0 ? 1 : -1;
                _verticalHeadError = headVertical > 0 ? 1 : -1;
                if (headVertical >= headHorizontal)
                    return "In Range";
                else
                    return "Out of Range";
            }
            else if (bodyVertical == 0)
            {
                //x<= 0
                _horizontalHeadError = 0;
                _verticalHeadError = headVertical > 0 ? 1 : -1;
                if (headHorizontal <= 0)
                    return "In Range";
                else
                    return "Out of Range";
            }
            else if (bodyVertical == -1)
            {
                //y<= -x
                _horizontalHeadError = headVertical > 0 ? -1 : 1;
                _verticalHeadError = headVertical > 0 ? 1 : -1;
                if (headVertical <= -headHorizontal)
                    return "In Range";
                else
                    return "Out of Range";
            }
        }
        return "Empty";
    }

    private void LateUpdate() 
    {
        if (isLookCooldown && cameraDestinationTransform == transform && Input.GetMouseButton(2))
        {
            if (!mouseLookDelay && Mathf.Round(Vector2.Distance(transform.position, Camera.main.transform.position)) <= mouseDistance)
            {
                Vector2 addedMousePos = GameObject.Find("MouseTracker").transform.position;
                addedMousePos = new Vector2((addedMousePos.x + transform.position.x) / 2, (addedMousePos.y + transform.position.y) / 2);
                cameraDestination = (Vector3)addedMousePos;
            }
            else if(!mouseLookDelay)
            {
                mouseLookDelay = true;
                Invoke("MouseLookDelay", mouseDelayTime);
            }
        }

        //moving camera & controls
        if (Camera.main.transform.position != cameraDestination)
        {
            Vector2 direction = (cameraDestination - Camera.main.transform.position).normalized;

            Camera.main.transform.position += _camSpeed * Time.deltaTime * (Vector3)direction;
        }
    }
}
