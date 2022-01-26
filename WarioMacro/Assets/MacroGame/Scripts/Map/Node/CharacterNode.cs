using System;
using System.Collections;
using System.Collections.Generic;
using GameTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterNode : MonoBehaviour
{
    [GameType(typeof(SpecialistType))]
    public int type = 1;

    public Character currentChara;

    [SerializeField] private GameObject CharacterEmplacement;
    [SerializeField] private DialogConstructor AlreadyRecruitDialog;
    [SerializeField] private InteractibleNode NodeEventScript;
    private bool HasBeenRecruit;

    private void Awake()
    {
        CharacterManager.RecruitableCharaFinished += Setup;
        HasBeenRecruit = false;
    }

    

    private void Setup()
    {
        Debug.Log(CharacterManager.instance.recruitableCharacters);
        foreach (Character chara in CharacterManager.instance.recruitableCharacters)
        {
            if (chara.characterType == type)
            {
                currentChara = chara;
                //Debug.Log(currentChara.PuppetPrefab.name);
                Instantiate(currentChara.PuppetPrefab, CharacterEmplacement.transform);
                return;
            }
        }
    }

    public void Recruit()
    {
        HasBeenRecruit = true;
        CharacterManager.instance.Recruit(currentChara);

        
        NodeEventScript.EventInteractible.RemoveAllListeners();
        UnityEvent NewEvent = new UnityEvent();
        NewEvent.AddListener(() => DialogManager.instance.StartDialog(AlreadyRecruitDialog));
        //NodeEventScript.EventInteractible.AddListener(() => DialogManager.instance.StartDialog(AlreadyRecruitDialog));
        NodeEventScript.EventInteractible = NewEvent;
    }

}
