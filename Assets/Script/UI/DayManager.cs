using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    [Header("UI References")]
    public TextMeshProUGUI waveCounterText;
    public GameObject waveUI;
    public float nextWaveDelay = 3f;

    [Header("Post Processing")]
    public Volume postProcessVolume;
    public VolumeProfile morningProfile;
    public VolumeProfile afternoonProfile;
    public VolumeProfile eveningProfile;

    private List<TimePhase> waveSchedule = new List<TimePhase>()
    {
        // Round 1
        new TimePhase { timeOfDay = TimeOfDay.Morning, waveCount = 3 },
        new TimePhase { timeOfDay = TimeOfDay.Afternoon, waveCount = 2 },
        // Round 2
        new TimePhase { timeOfDay = TimeOfDay.Evening, waveCount = 5 },
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
        UpdatePostProcessing();

        if (waveUI != null)
            waveUI.SetActive(false);
    }

    public void CompleteWave()
    {
        StartCoroutine(HandleNextWaveImage());
    }

    private IEnumerator HandleNextWaveImage()
    {
        if (waveUI != null)
            waveUI.SetActive(true);

        yield return new WaitForSeconds(nextWaveDelay);

        if (waveUI != null)
            waveUI.SetActive(false);

        currentWaveInPhase++;
        currentWaveInRound++;

        if (currentWaveInPhase > wavesInCurrentPhase)
        {
            currentScheduleIndex++;

            if (currentScheduleIndex >= waveSchedule.Count)
                currentScheduleIndex = 0;

            currentWaveInPhase = 1;

            UpdatePostProcessing();
        }

        if (currentWaveInRound > wavesPerRound)
        {
            currentWaveInRound = 1;
        }

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

    void UpdatePostProcessing()
    {
        if (postProcessVolume == null) return;

        switch (currentTimeOfDay)
        {
            case TimeOfDay.Morning:
                postProcessVolume.profile = morningProfile;
                break;
            case TimeOfDay.Afternoon:
                postProcessVolume.profile = afternoonProfile;
                break;
            case TimeOfDay.Evening:
                postProcessVolume.profile = eveningProfile;
                break;
        }
    }
}
