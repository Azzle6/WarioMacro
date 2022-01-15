using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming 
// ReSharper disable once CheckNamespace
public class MusicManager : MonoBehaviour, ITickable
{
    public static MusicManager instance;
    
    [Header("Visualize music Timer")] [Range(0, 1f)] public float musicTime;
    [FormerlySerializedAs("MusicsSO")] public MusicManagerSO musicSO;
    public AudioSource AudioS; 
    public AudioClip currentAudioClip;
    
    private AudioClip nextAudioClip;

    public void OnTick()
    {
        // TODO
        FindMusic((int) Ticker.gameBPM * 2, Soundgroup.PhaseState.MACROGAME, Soundgroup.CurrentPhase.RECRUIT);

        if (currentAudioClip.Equals(nextAudioClip)) return;
        
        Debug.Log("nextAudioClip: " + nextAudioClip);
        float nextTimer = ConvertMusicTimers();

        AudioS.clip = nextAudioClip;
        currentAudioClip = nextAudioClip;
        AudioS.time = nextTimer;
        AudioS.Play();
    }
    
    private void FindMusic(int bpm, Soundgroup.PhaseState gameState, Soundgroup.CurrentPhase gamePhase) 
    {
        if (bpm % 20 != 0) return;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (Soundgroup group in musicSO.MusicList)
        {
            if (group.musicPhase != gamePhase || group.musicState != gameState) continue;
            
            foreach (SoundRef musicRef in group.sounds.Where(sounds => sounds.BPM == bpm))
            {
                nextAudioClip = musicRef.Clip;
                //Debug.Log("nextAudioClip: " + nextAudioClip);
                return;
            }
        }
        
        //Debug.Log("No music corresponding to : " + bpm + " " + gameState + " " + gamePhase);
    } 

    private float ConvertMusicTimers() 
    { 
        double percentage = (double) AudioS.time / currentAudioClip.length;
        return (float) (nextAudioClip.length * percentage);
    } 
     
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            musicSO = Resources.Load<MusicManagerSO>("MusicManagerSO");
            AudioS = transform.GetChild(0).GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable() 
    {
        //GameManager.Register();

        FindMusic(100, Soundgroup.PhaseState.MACROGAME, Soundgroup.CurrentPhase.RECRUIT);
        currentAudioClip = nextAudioClip;
        AudioS.clip = currentAudioClip;
        AudioS.Play();
        AudioS.loop = true;
    }

    private void Update() // L'update est juste pour afficher le slider
    {
        if(AudioS.clip != null)
            musicTime = AudioS.time / AudioS.clip.length; 
    }
} 
 


