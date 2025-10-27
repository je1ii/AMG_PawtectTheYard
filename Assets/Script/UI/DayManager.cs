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
    public Image dayIcon;
    public Sprite[] dayIcons;

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
    
    private CanvasGroup waveCanvasGroup;

    private AudioSource waveSFX;
    private AudioSource waveSongSFX;

    private TimeOfDay currentTimeOfDay => waveSchedule[currentScheduleIndex].timeOfDay;
    private int wavesInCurrentPhase => waveSchedule[currentScheduleIndex].waveCount;

    void Start()
    {
        UpdateUI();
        UpdatePostProcessing();
        
        waveSFX = GameObject.Find("SFX Success Wave").GetComponent<AudioSource>();
        waveSongSFX = GameObject.Find("SFX Success Wave Song").GetComponent<AudioSource>();
        
        if (waveUI != null)
        {
            waveCanvasGroup = waveUI.GetComponent<CanvasGroup>();
            if (waveCanvasGroup == null)
                waveCanvasGroup = waveUI.AddComponent<CanvasGroup>();

            waveCanvasGroup.alpha = 0f;
            waveUI.SetActive(false);
        }
    }

    public void CompleteWave()
    {
        StartCoroutine(HandleNextWaveImage());
    }

    private IEnumerator HandleNextWaveImage()
    {
        if (waveUI != null)
        {
            waveUI.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(waveCanvasGroup, 0f, 1f, 0.2F));
        }
        
        waveSFX.Play();
        waveSongSFX.Play();
        yield return new WaitForSeconds(nextWaveDelay);

        if (waveUI != null)
            yield return StartCoroutine(FadeCanvasGroup(waveCanvasGroup, 1f, 0f, 1F));
            
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
    
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;
        cg.alpha = start;
        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        cg.alpha = end;
    }

    void UpdateUI()
    {
        if (waveCounterText != null)
        {
            waveCounterText.text =
                $"{currentTimeOfDay}\n" +
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
                dayIcon.sprite = dayIcons[0];
                break;
            case TimeOfDay.Afternoon:
                postProcessVolume.profile = afternoonProfile;
                dayIcon.sprite = dayIcons[1];
                break;
            case TimeOfDay.Evening:
                postProcessVolume.profile = eveningProfile;
                dayIcon.sprite = dayIcons[2];
                break;
        }
    }
}
