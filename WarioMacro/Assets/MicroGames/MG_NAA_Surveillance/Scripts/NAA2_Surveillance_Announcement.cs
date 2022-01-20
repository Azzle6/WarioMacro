using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAA2_Surveillance_Announcement : MonoBehaviour, ITickable
{

    [SerializeField]
    GameObject announcementGO;
    [SerializeField]
    Animator announcementGOanimator;
    [SerializeField]
    AnimationClip announcementAnim;

    AnimationEvent victory = new AnimationEvent();

    [SerializeField]
    NAA2_Surveillance_MicroGameController mcg;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Register();
        GameController.Init(this);
    }

    // Update is called once per frame
  
    public void OnTick()
    {

    }

    public void RelaunchAnnouncement()
    {
        announcementGO.SetActive(true);
        announcementGOanimator.Play(announcementAnim.name, 0);
        
    }

    void StartDuPauvre()
    {
        announcementGO.SetActive(false);
    }
}
