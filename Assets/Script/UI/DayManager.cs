using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DayManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening
    }

    [System.Serializable]
    public class TimePhase
    {
        public TimeOfDay timeOfDay;
        public int waveCount;
    }

    [Header("UI References")]
    public TextMeshProUGUI waveCounterText;
    public Image waveCompleteImage; // Image shown after each wave

    [Header("Fade Settings")]
    public float fadeDuration = 1f;          // Time for fade in/out
    public float displayDuration = 1.5f;     // Time to stay fully visible

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

    private Coroutine fadeCoroutine;

    void Start()
    {
        if (waveCompleteImage != null)
        {
            SetImageAlpha(0f);
            waveCompleteImage.gameObject.SetActive(true);
        }

        UpdateUI();
    }

    void Update()
    {
        // For testing purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CompleteWave();
        }
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

        UpdateUI();

        // Start fade animation
        if (waveCompleteImage != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeImageSequence());
        }
    }

    IEnumerator FadeImageSequence()
    {
        // Fade in
        yield return FadeImage(0f, 1f, fadeDuration);

        // Wait while fully visible
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return FadeImage(1f, 0f, fadeDuration);
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = waveCompleteImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

            color.a = newAlpha;
            waveCompleteImage.color = color;

            yield return null;
        }

        // Ensure final alpha value is exact
        color.a = endAlpha;
        waveCompleteImage.color = color;
    }

    void SetImageAlpha(float alpha)
    {
        if (waveCompleteImage != null)
        {
            Color color = waveCompleteImage.color;
            color.a = alpha;
            waveCompleteImage.color = color;
        }
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

