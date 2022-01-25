using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class HallOfFame : MonoBehaviour
{
    public Run[] hall;
    public Run currentRun;
    public GameObject hallPanel;
    public RankElementHoF[] ranks = new RankElementHoF[10]; 
    
    private bool isOpen;
    
    [Serializable]
    public class Run
    {
        public float score;
        public float time;
        public Dictionary<Character,bool> team = new Dictionary<Character,bool>();
        public Run(){}

        public Run(float s, float t, Dictionary<Character,bool> team)
        {
            score = s;
            time = t;
            this.team = team;
        }
        
        public override string ToString()
        {
            var toString =  score + "," + time + ",";
            foreach (var character in team)
            {
                Debug.Log(character.Key);
                Debug.Log(character.Value);
                toString += character.Key.ToString()+"."+character.Value+":";
            }
            toString = toString.Substring(0, toString.Length - 1);
            Debug.Log(toString);
            return toString;
        }
    }

    public void StartRun(Character[] team)
    {
        foreach (var c in team)
        {
            currentRun.team.Add(c,true);
        }
    }

    public void SetCharacterToJail(Character c)
    {
        if(currentRun.team.ContainsKey(c))
            currentRun.team[c] = false;
    }
    
    public void SetHallOfFame()
    {
        PlayerPrefs.DeleteAll();
        var j = 0;
        var temp = PlayerPrefs.GetString("hallOfFame");
        if (temp == "") return;
        var hallOfFame = temp.Split(';');
        hall = new Run[hallOfFame.Length];
        foreach (var item in hallOfFame)
        {
            Run run = new Run();
            var temp2 = item.Split(',');
            
            var team = temp2[2].Split(':');
            run.score = float.Parse(temp2[0]);
            run.time = float.Parse(temp2[1]);
            Debug.Log(temp2[2]);
            foreach (var couple in team)
            {
                var temp3 = couple.Split('.');
                run.team.Add(GameController.instance.characterManager.GetCharacter(temp3[0]),bool.Parse(temp3[1]));
            }
            hall[j] = run;
            j++;
        }
    }

    public void UpdateHallOfFame(float score, float time)
    {
        currentRun.score = score;
        currentRun.time = time;
        if (hall.Length == 10 && currentRun.score <= hall[hall.Length - 1].score) return;
        if (hall.Length == 10) hall[9] = currentRun;
        if (hall.Length < 10)
        {
            var temp = hall;
            hall = new Run[temp.Length + 1];
            for (int i = 0; i < temp.Length; i++)
            {
                hall[i] = temp[i];
            }
            hall[hall.Length - 1] = currentRun;
        }

        hall = hall.OrderBy(c => c.score).ToArray();
        Array.Reverse(hall);
        PlayerPrefs.DeleteKey("hallOfFame");
        var save = "";
        foreach (var r in hall)
        {
            save += r.ToString();
            save += ";";
        }
        save = save.Substring(0, save.Length - 1);
        PlayerPrefs.SetString("hallOfFame",save);
        Debug.Log(save);

    }


    public void setRanksUI()
    {
        var i = 0;
        foreach (var rank in hall)
        {
            ranks[i].gameObject.SetActive(true);
            ranks[i].SetRank(rank);
            i++;
        }
    }
    
    private void Update()
    {
        if(InputManager.GetKeyDown(ControllerKey.B, true) && isOpen) CloseHall();
    }
    public void OpenHall()
    {
        hallPanel.SetActive(true);
        isOpen = true;
        InputManager.lockInput = true;
        setRanksUI();
    }

    public void CloseHall()
    {
        hallPanel.SetActive(false);
        InputManager.lockInput = false;
        GameController.OnInteractionEnd();
    }
}
