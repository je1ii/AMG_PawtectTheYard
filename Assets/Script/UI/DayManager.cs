using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DayManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening
    }

    public class TimePhase
    {
        public TimeOfDay timeOfDay;
        public int waveCount;
    }

    public TextMeshProUGUI waveCounterText;

    private List<TimePhase> waveSchedule = new List<TimePhase>()
    {
        // Round 1
        new TimePhase { timeOfDay = TimeOfDay.Morning, waveCount = 3 },
        new TimePhase { timeOfDay = TimeOfDay.Afternoon, waveCount = 2 },
        // Round 2
        new TimePhase { timeOfDay = TimeOfDay.Afternoon, waveCount = 2 },
        new TimePhase { timeOfDay = TimeOfDay.Evening, waveCount = 3 },
    };

    private int currentScheduleIndex = 0;
    private int currentWaveInPhase = 1;
    private int currentWaveInRound = 1;
    private int wavesPerRound = 5;

    private TimeOfDay currentTimeOfDay => waveSchedule[currentScheduleIndex].timeOfDay;
    private int wavesInCurrentPhase => waveSchedule[currentScheduleIndex].waveCount;

    void Start()
    {
        UpdateUI();
    }

    public void CompleteWave()
    {
        currentWaveInPhase++;
        currentWaveInRound++;

        if (currentWaveInPhase > wavesInCurrentPhase)
        {
            currentScheduleIndex++;

            if (currentScheduleIndex >= waveSchedule.Count)
                currentScheduleIndex = 0;

            currentWaveInPhase = 1;
        }

        if (currentWaveInRound > wavesPerRound)
        {
            currentWaveInRound = 1; 
        }

        Debug.Log("[DayManager] CompleteWave called");

        UpdateUI();
    }

    void UpdateUI()
    {
        if (waveCounterText != null)
        {
            waveCounterText.text =
                $"Time of Day: {currentTimeOfDay}\n" +
                $"Wave: {currentWaveInRound} / {wavesPerRound}";
        }
    }
}
