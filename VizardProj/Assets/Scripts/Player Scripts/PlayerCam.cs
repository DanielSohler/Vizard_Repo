using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public menuState currentState;
    [HideInInspector] public bool stateRotator = true;

    //Holds all slots
    public List<GameObject> slotList;

    //references slot manager
    SlotManager slotManager;

    //Refs pause icon
    public GameObject pauseIcon;
    public GameObject tickIcon;

    //Bool for casting spell in area
    public bool inSpellArea = false;
    public bool castingSpell = false;


    private void Start()
    {
        currentState = menuState.menuDisabled;
        // locks cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseIcon.SetActive(false);
        slotManager = FindObjectOfType<SlotManager>();
    }

    private void Update()
    {
        if (currentState == menuState.menuDisabled)
        {
            // gets mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, 0f, 0f);

            // rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        if (Input.GetKeyDown("space"))
        {
            //flips state to opposite of current state
            if (currentState == menuState.menuEnabled)
            {
                currentState = menuState.menuDisabled;
                tickIcon.SetActive(false);
            }
            else
            {
                currentState = menuState.menuEnabled;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (currentState == menuState.menuEnabled)
            {
                if (inSpellArea)
                {
                    castingSpell = true;
                    StartCoroutine(DisableCasting());
                }

                //SlotManage();
                //
            }
            return;
        }

        //sets menu state
        OnAnimatorMove();
    }

    private void OnAnimatorMove()
    {
        if (currentState == menuState.menuEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SlotManage();
        }
        if (currentState == menuState.menuDisabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            slotManager.ResetSlotChildren();
            SlotManage();
        }
    }

    public void SlotManage()
    {
        if (currentState == menuState.menuEnabled)
        {
            foreach (var obj in slotList)
            {
                obj.SetActive(true);
                pauseIcon.SetActive(true);
            }
        }
        if (currentState == menuState.menuDisabled)
        {
            foreach (var obj in slotList)
            {
                obj.SetActive(false);
                pauseIcon.SetActive(false);
                slotManager.ResetSlotChildren();
            }
        }
    }
    public IEnumerator DisableCasting()
    {
        yield return new WaitForSeconds(0.1f);
        castingSpell = false;
        currentState = menuState.menuDisabled;
    }
}

public enum menuState
{
    menuEnabled,
    menuDisabled
}
