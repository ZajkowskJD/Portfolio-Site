using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class checkpointManager : MonoBehaviour
{

    [SerializeField] PolygonCollider2D[] checkpoints;
    [SerializeField] TextMeshProUGUI lapCounter;

    [SerializeField] TextMeshProUGUI[] leaderboard;
    float[] leaderboardTimes = { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue};
    string[] leaderboardRecipients = { " ", " ", " ", " " };
    int[] leaderboardCheckpointTracker = { -1, -1, -1, -1 };
    int[] leaderboardLapTracker = { -1, -1, -1, -1 };

    [SerializeField] int lapNum = 0;
    private int activeTrigger = 0;

    // Start is called before the first frame update
    void Awake()
    {
        checkpoints = gameObject.GetComponentsInChildren<PolygonCollider2D>();

    }

    public void NextCheckpoint(int trigger_id, GameObject other)
    {
        if(trigger_id == activeTrigger && other.tag == "Player")
        {
            if (activeTrigger == checkpoints.Length - 1)
            {
                activeTrigger = 0;
            }
            else activeTrigger++;

            if(activeTrigger == 1)
            {
                lapNum++;
                GameObject.Find("lapSource").GetComponent<lapAudioController>().PlayLap();
                if (lapNum > 3)
                {
                    lapNum = 0;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                lapCounter.text = "Lap " + lapNum;
                if (!GameObject.Find("Timer").GetComponent<timer>().raceStarted) GameObject.Find("Timer").GetComponent<timer>().raceStarted = true;
            }
        }
        PersonalTime time = other.GetComponent<PersonalTime>();
        time.UpdatePersonalTime(GameObject.Find("Timer").GetComponent<timer>().returnCurrentTime(), trigger_id);
    }

    public void UpdateLeaderboard(float time, string recipient, int lastCheckpoint, int currentLap)
    {
        int DupeCheckPos = -1;
        for(int i = 0; i < leaderboardRecipients.Length; i++)
        {
            if (leaderboardRecipients[i] == recipient) DupeCheckPos = i;
        }



        int currentPos = -1;
        if(currentLap > leaderboardLapTracker[leaderboardLapTracker.Length - 1] || lastCheckpoint > leaderboardCheckpointTracker[leaderboardCheckpointTracker.Length - 1])
        {
            if(DupeCheckPos != -1)
            {
                for(int i = DupeCheckPos; i < leaderboard.Length - 1; i++)
                {
                    leaderboardTimes[i] = leaderboardTimes[i + 1];
                    leaderboardRecipients[i] = leaderboardRecipients[i + 1];
                    leaderboardCheckpointTracker[i] = leaderboardCheckpointTracker[i + 1];
                    leaderboardLapTracker[i] = leaderboardLapTracker[i + 1];
                }
            }

            leaderboardTimes[leaderboardTimes.Length - 1] = time;
            leaderboardRecipients[leaderboardRecipients.Length - 1] = recipient;
            leaderboardCheckpointTracker[leaderboardCheckpointTracker.Length - 1] = lastCheckpoint;
            leaderboardLapTracker[leaderboardLapTracker.Length - 1] = currentLap;

            currentPos = leaderboard.Length - 1;
        }

        if(currentPos != -1)
        {

            //Debug.Log(currentPos);
            while (currentPos != 0 && leaderboardLapTracker[currentPos] > leaderboardLapTracker[currentPos - 1])
            {
                //Debug.Log("before: " + currentLap);
                swapUp(currentPos, time, recipient, lastCheckpoint, currentLap);
                currentPos--;
                //Debug.Log("swapped by lap, " + currentLap);
                //Debug.Log(currentPos);
            }
            while (currentPos != 0 && leaderboardCheckpointTracker[currentPos] > leaderboardCheckpointTracker[currentPos - 1])
            {
                swapUp(currentPos, time, recipient, lastCheckpoint, currentLap);
                currentPos--;
                //Debug.Log("swapped by checkpoint");
                //Debug.Log(currentPos);
            }
            while (currentPos != 0 && leaderboardTimes[currentPos] < leaderboardTimes[currentPos - 1])
            {
                swapUp(currentPos, time, recipient, lastCheckpoint, currentLap);
                currentPos--;
                //Debug.Log("swapped by time");
                //Debug.Log(currentPos);
            }

            for(int i = 0; i < leaderboard.Length; i++)
            {
                if (leaderboardRecipients[i] != " ") leaderboard[i].text = leaderboardRecipients[i] +" " + (int)leaderboardTimes[i] / 60 + ":" + (int)leaderboardTimes[i] % 60 + ":" + (int)((leaderboardTimes[i] % 60 - (int)leaderboardTimes[i] % 60) * 1000);
                else leaderboard[i].text = " ";
            }
        }
        
        
        void swapUp(int currentPos, float time, string recipient, int lastCheckpoint, int currentLap)
        {
            float tmp_float = leaderboardTimes[currentPos - 1];
            string tmp_string = leaderboardRecipients[currentPos - 1];
            int tmp_int1 = leaderboardCheckpointTracker[currentPos - 1];
            int tmp_int2 = leaderboardLapTracker[currentPos - 1];

            leaderboardTimes[currentPos - 1] = leaderboardTimes[currentPos];
            leaderboardRecipients[currentPos - 1] = leaderboardRecipients[currentPos];
            leaderboardCheckpointTracker[currentPos - 1] = leaderboardCheckpointTracker[currentPos];
            leaderboardLapTracker[currentPos - 1] = leaderboardLapTracker[currentPos];

            leaderboardTimes[currentPos] = tmp_float;
            leaderboardRecipients[currentPos] = tmp_string;
            leaderboardCheckpointTracker[currentPos] = tmp_int1;
            leaderboardLapTracker[currentPos] = tmp_int2;
        }

    }
    
}
