using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HallOfFame : MonoBehaviour
{
    public Run[] hall = new Run[10];
    public CharacterManagerTest characterManagerTest;
    public Run runTest;
    
    [Serializable]
    public class Run
    {
        public float score;
        public float time;
        public Character[] team = new Character[4];
    }

    void Start()
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
        var i = 0;
        var temp = PlayerPrefs.GetString("hallOfFame");
        if (temp == "") return;
        var hallOfFame = temp.Split(';');
        hall = new Run[Mathf.Clamp(hallOfFame.Length,0,10)];
        foreach (var item in hallOfFame)
        {
            Run run = new Run();
            var temp2 = item.Split(',');
            var team = temp2[2].Split(':');
            run.score = float.Parse(temp2[0]);
            run.time = float.Parse(temp2[1]);
            foreach (var character in team)
            {
                run.team[i] = characterManagerTest.GetCharacter(character);
                i++;
            }
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
            save += r.score+","+r.time + ",";
            foreach (var character in r.team)
            {
                save += character.ToString()+":";
            }
            save = save.Substring(0, save.Length - 1);
            save += ";";
        }
        save = save.Substring(0, save.Length - 1);
        PlayerPrefs.SetString("hallOfFame",save);
        
    }
}
