using System.Linq;
using GameTypes;
using UnityEngine;
using UnityEngine.Events;

public class CharacterNode : MonoBehaviour
{
    [GameType(typeof(SpecialistType))]
    public int type = 1;

    public Character currentChara;

    [SerializeField] private GameObject CharacterEmplacement;
    [SerializeField] private DialogConstructor AlreadyRecruitDialog;
    [SerializeField] private InteractibleNode NodeEventScript;

    private void Start()
    {
        CharacterManager.RecruitableCharaFinished += Setup;
    }

    private void Setup()
    {
        foreach (Character chara in CharacterManager.instance.recruitableCharacters.Where(chara => chara.characterType == type))
        {
            currentChara = chara;
            Instantiate(currentChara.PuppetPrefab, CharacterEmplacement.transform);

            var dialogs = GetComponents<DialogConstructor>();
            foreach (DialogConstructor dialogC in dialogs)
            {
                dialogC.chara = currentChara.fullSizeSprite;
                dialogC.name = currentChara.characterName;
            }
        }
        //StartCoroutine(SetupWithTiming());
    }

    /*private IEnumerator SetupWithTiming()
    {
        yield return new WaitForSecondsRealtime(5f);
        foreach (Character chara in CharacterManager.instance.recruitableCharacters)
        {
            if (chara.characterType == type)
            {
                currentChara = chara;
                Instantiate(currentChara.PuppetPrefab, CharacterEmplacement.transform);
                yield return null;
            }
        }
    }*/

    public void Recruit()
    {
        CharacterManager.instance.Recruit(currentChara);
        AudioManager.MacroPlaySound(currentChara.mastery == Character.Level.Novice ? "NoviceSelection" : "CharacterSelection");

        NodeEventScript.EventInteractible.RemoveAllListeners();
        UnityEvent NewEvent = new UnityEvent();
        NewEvent.AddListener(() => DialogManager.instance.StartDialog(AlreadyRecruitDialog));
        //NodeEventScript.EventInteractible.AddListener(() => DialogManager.instance.StartDialog(AlreadyRecruitDialog));
        NodeEventScript.EventInteractible = NewEvent;
    }

    public void PlayCharacterSound()
    {
        switch (currentChara.gender)
        {
            case Character.Gender.Dog :
                AudioManager.MacroPlaySound("DogDialog");
                break;
            case Character.Gender.Male :
                AudioManager.MacroPlayRandomSound("CharacterDialogsBoys");
                break;
            case Character.Gender.Female :
                AudioManager.MacroPlayRandomSound("CharacterDialogsGirls");
                break;
        }
    }
}
