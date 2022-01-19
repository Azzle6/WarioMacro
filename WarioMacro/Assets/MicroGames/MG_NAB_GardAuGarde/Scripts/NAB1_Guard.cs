using UnityEngine;

public class NAB1_Guard : MonoBehaviour
{
    private Color color = Color.green;
    private RaycastHit hit;
    public bool seen = false;

    private int angle;
    

    void Update()
    {
        GuardRays();
    }

    private void GuardRays()
    {
        if (seen) return;
        Debug.DrawRay(new Vector3(transform.position.x, 0.5f,transform.position.z) + transform.forward*0.5f,  transform.forward * 4, color);
        seen = Physics.Raycast(new Ray(new Vector3(transform.position.x, 0.5f,transform.position.z) + transform.forward*0.5f,  transform.forward),out hit, 4);
        if (seen && hit.collider.name == "Ch45")
        {
            hit.collider.SendMessageUpwards("Spotted", SendMessageOptions.DontRequireReceiver);
            Camera.main.GetComponent<Animator>().SetBool("Seen", true);
            color = Color.red;
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

    public void SwitchSideRandom(Animator animator)
    {
        if (Random.value >= 0.5)
        {
            SwitchSide(animator);
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
