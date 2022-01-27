using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class NAB1_Guard : MonoBehaviour
{
    
    private RaycastHit hit;
    public bool seen = false;
    public bool seen2 = false;
    public bool flashlight = true;
    [SerializeField] private GameObject pl;
    private int angle;

    void Update()
    {
        if (GameController.difficulty > 1)
        {
            GuardRays();
        }
        else
        {
            DualRays();
        }
    }

    private void GuardRays()
    {
        if (seen) return;
        seen = Physics.Raycast(new Ray(new Vector3(transform.position.x, 0.5f,transform.position.z) + transform.forward*0.5f,  transform.forward),out hit, 4);
        if (seen && hit.collider.name == "Ch45" && flashlight)
        {
            hit.collider.SendMessageUpwards("Spotted", SendMessageOptions.DontRequireReceiver);
            Camera.main.GetComponent<Animator>().SetBool("Seen", true);
            GetComponent<Animator>().SetTrigger("Spot");
            MoveIdle(GetComponent<Animator>());
        }
        else
        {
            seen = false;
        }
    }

    private void DualRays()
    {
        if (seen || seen2) return;
        seen = Physics.BoxCast(new Vector3(transform.position.x, 0.5f,transform.position.z), new Vector3(2,0,2), transform.forward, out hit);
        if (seen && hit.collider.name == "Ch45" && flashlight)
        {
            hit.collider.SendMessageUpwards("Spotted", SendMessageOptions.DontRequireReceiver);
            Camera.main.GetComponent<Animator>().SetBool("Seen", true);
            GetComponent<Animator>().SetTrigger("Spot");
            MoveIdle(GetComponent<Animator>());
        }
        else
        {
            seen = false;
        }
    }


    public void InitAngle(int a)
    {
        if (angle == 0)
        {
            angle = a;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180 + angle, transform.rotation.z);
        }
    }

    public void SwitchSide(Animator animator)
    {

        angle *= -1;

        if (angle < 0)
        {
            MoveLeft(animator);
        }
        else
        {
            MoveRight(animator);
        }
    }

    public void SwitchSideAsTwo(Animator animator)
    {
        if (angle > 3 || angle < -3)
        {
            angle /= -11;
            MoveIdle(animator);
        }
        else
        {
            angle *= -11;
            if (angle > 0)
            {
                MoveRight(animator);
            }
            else
            {
                MoveLeft(animator);
            }
        }
    }

    public void SwitchLight()
    {
        flashlight = !flashlight;
        pl.SetActive(flashlight);
    }

    private void MoveRight(Animator animator)
    {
        animator.SetBool(("Right"), true);
        animator.SetBool("Idle", false);
        animator.SetBool("Left", false);

        transform.rotation = Quaternion.Euler(transform.rotation.x, angle + 180, transform.rotation.z);
    }

    private void MoveLeft(Animator animator)
    {
        animator.SetBool("Left", true);
        animator.SetBool("Idle", false);
        animator.SetBool(("Right"), false);
        
        transform.rotation = Quaternion.Euler(transform.rotation.x, angle + 180, transform.rotation.z);
    }

    public void MoveIdle(Animator animator)
    {
        animator.SetBool(("Idle"), true);
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        
        transform.rotation = Quaternion.Euler(transform.rotation.x, angle + 180, transform.rotation.z);
    }
}
