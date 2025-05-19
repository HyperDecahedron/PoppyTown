using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDarkChanger : MonoBehaviour
{
    public GameObject light_map;
    public GameObject dark_map;
    public GameObject glitch;

    private GameTimeManager timeManager;
    public bool isDark = false; 

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        isDark = playerController.startCreepy;
                
        GameObject time = GameObject.FindGameObjectWithTag("Time");
        if (time != null)
        {
            timeManager = time.GetComponent<GameTimeManager>();

            if (timeManager.creepiness >= 3 || isDark)
            {
                SetDarkMap(); // update when changing the scene
            }
            else
            {
                dark_map.SetActive(false);
                light_map.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Time GameObject with tag 'Time' not found in LightDarkChanger");
        }

       
    }

    public void SetDarkMap()
    {
        light_map.SetActive(false);
        dark_map.SetActive(true);
        isDark = true;
    }

    public void SetDarkMapGlitch()
    {
        StartCoroutine(GlitchRoutine());
        isDark = true;
    }

    private IEnumerator GlitchRoutine()
    {
        glitch.SetActive(true);

        yield return new WaitForSeconds(1f);

        light_map.SetActive(false);
        dark_map.SetActive(true);

        yield return new WaitForSeconds(1f);

        glitch.SetActive(false);
    }
}
