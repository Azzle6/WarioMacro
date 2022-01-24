using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HallOfFame : MonoBehaviour
{
    public Run[] hall;
    public CharacterManager characterManager;
    public Run runTest;
    
    [Serializable]
    public class Run
    {
        public float score;
        public float time;
        public Character[] team = new Character[4];

        public override string ToString()
        {
            var toString =  score + "," + time + ",";
            foreach (var character in team)
            {
                toString += character+":";
            }
            toString = toString.Substring(0, toString.Length - 1);
            Debug.Log(toString);
            return toString;
        }
    }

    void Awake()
    {
        SetHallOfFame();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateHallOfFame(runTest);
        }
    }
    public void SetHallOfFame()
    {
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
            for (int i = 0;i<4;i++)
            {
                run.team[i] = characterManager.GetCharacter(team[i]);
            }

            hall[j] = run;
            j++;

        }
    }

    public void UpdateHallOfFame(Run run)
    {
        if (hall.Length == 10 && run.score <= hall[hall.Length - 1].score) return;
        if (hall.Length == 10) hall[9] = run;
        if (hall.Length < 10)
        {
            var temp = hall;
            hall = new Run[temp.Length + 1];
            for (int i = 0; i < temp.Length; i++)
            {
                hall[i] = temp[i];
            }
            hall[hall.Length - 1] = run;
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
}
