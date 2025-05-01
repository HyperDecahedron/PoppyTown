using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene loading

public class IntroManager : MonoBehaviour
{
    public GameObject postprocessing;
    public ScaleInOut panelButton;
    private bool hasStarted = false;

    void Start()
    {
        postprocessing.SetActive(false);
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.E))
        {
            hasStarted = true;
            StartCoroutine(PlayIntroSequence());
        }
    }

    private IEnumerator PlayIntroSequence()
    {
        panelButton.stop = true;
        postprocessing.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        postprocessing.SetActive(false);
        yield return new WaitForSeconds(1f);
        postprocessing.SetActive(true);
        yield return new WaitForSeconds(3f);                

        SceneManager.LoadScene("TrainStationScene");
    }
}
