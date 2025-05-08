using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections;

public class GameTimeManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    [Header("Time Settings")]
    [Tooltip("Starting hour (0-23)")]
    [Range(0, 23)]
    public int startingHour = 8; // Default: 8 AM

    [Tooltip("Real-life minutes that equal 24 in-game hours")]
    [Min(0.1f)]
    public float realMinutesPerGameDay = 20f; // 20 real minutes = 24 game hours

    [Tooltip("How often (in in-game minutes) to update the display")]
    public int displayUpdateInterval = 15; // Only update every 15 in-game minutes

    // ----------------------------------------------------------------------------
    [Header("Display Settings")]
    public Text timeDisplay; 
    public int creepiness = 1; // default 1
    public Image sunImage;

    public Volume stage1Volume;
    public Volume stage2Volume;
    public Volume stage3Volume;
    public float volumeFadeDuration = 3f;

    private float gameTimeInMinutes; // Total in-game minutes passed
    private float lastDisplayedMinutes;
    private float gameMinutesPerRealSecond;

    void Start()
    {
        // Initialize time
        gameTimeInMinutes = startingHour * 60f;
        gameMinutesPerRealSecond = (24f * 60f) / (realMinutesPerGameDay * 60f);
        lastDisplayedMinutes = Mathf.Floor(gameTimeInMinutes / displayUpdateInterval) * displayUpdateInterval;
        UpdateTimeDisplay();

        // set initial stage volumes
        SetStage();
    }

    void FixedUpdate() // Better for consistent time updates
    {
        // Update game time based on real time passed
        gameTimeInMinutes += Time.fixedDeltaTime * gameMinutesPerRealSecond;

        // Check if we should update the display (only every X in-game minutes)
        float currentRoundedMinutes = Mathf.Floor(gameTimeInMinutes / displayUpdateInterval) * displayUpdateInterval;
        if (currentRoundedMinutes != lastDisplayedMinutes)
        {
            lastDisplayedMinutes = currentRoundedMinutes;
            UpdateTimeDisplay();
        }
    }

    void UpdateTimeDisplay()
    {
        if (timeDisplay != null)
        {
            // Calculate current hour and minute (rounded to nearest display interval)
            int totalMinutes = (int)lastDisplayedMinutes;
            int hours = (totalMinutes / 60) % 24;
            int minutes = totalMinutes % 60;

            // Format as 24-hour time (e.g., "14:15")
            timeDisplay.text = $"{hours:D2}:{minutes:D2}";

            // Update sun sprite
            string fileName = "S" + creepiness.ToString() + hours.ToString(); 
            Sprite newSprite = Resources.Load<Sprite>("Visuals/Sun/" + fileName);

            if (newSprite != null)
                sunImage.sprite = newSprite;

            // Update creepiness at 15 and at 20 in the same day
            if (hours == 15 && minutes == 00 && creepiness < 3)
            {
                creepiness++; // update every night at 3
                SetStage();
            }
            else if (hours == 20 && minutes == 00 && creepiness < 3)
            {
                creepiness++; // update every night at 3
                SetStage();
            }


        }
    }

    public void SetStage()
    {
        if (creepiness == 1)
        {
            stage1Volume.weight = 1f;
            stage2Volume.weight = 0f;
            stage3Volume.weight = 0f;
        }
        else if (creepiness == 2)
        {
            StartCoroutine(BlendVolumes(stage1Volume, stage2Volume, volumeFadeDuration));
        }
        else if (creepiness == 3) {
            StartCoroutine(BlendVolumes(stage2Volume, stage3Volume, volumeFadeDuration));
        }
    }

    IEnumerator BlendVolumes(Volume from, Volume to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            from.weight = Mathf.Lerp(1f, 0f, t);
            to.weight = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            yield return null;
        }
        from.weight = 0f;
        to.weight = 1f;
    }

    // For debugging purposes
    public void SetTimeForDebug(int hours, int minutes)
    {
        gameTimeInMinutes = hours * 60 + minutes;
        lastDisplayedMinutes = Mathf.Floor(gameTimeInMinutes / displayUpdateInterval) * displayUpdateInterval;
        UpdateTimeDisplay();
    }
}