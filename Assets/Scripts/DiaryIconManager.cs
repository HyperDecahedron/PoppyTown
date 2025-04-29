using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiaryIconManager : MonoBehaviour
{
    [SerializeField] private Transform transformOut;
    [SerializeField] private Transform transformIn;
    [SerializeField] private float secondsAnimation = 1f;
    [SerializeField] private AudioClip soundOpen;
    [SerializeField] private AudioClip soundClose;
    [SerializeField] private GameObject diaryObject;

    private bool isDiaryOpen = false;
    private bool isAnimating = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        diaryObject.transform.position = transformOut.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAnimating)
        {
            if (isDiaryOpen)
            {
                StartCoroutine(MoveDiary(transformIn.position, transformOut.position, soundClose));
            }
            else
            {
                StartCoroutine(MoveDiary(transformOut.position, transformIn.position, soundOpen));
            }

            isDiaryOpen = !isDiaryOpen;
        }
    }

    private System.Collections.IEnumerator MoveDiary(Vector3 from, Vector3 to, AudioClip clip)
    {
        isAnimating = true;
        float elapsed = 0f;
        audioSource.PlayOneShot(clip);

        while (elapsed < secondsAnimation)
        {
            diaryObject.transform.position = Vector3.Lerp(from, to, elapsed / secondsAnimation);
            elapsed += Time.deltaTime;
            yield return null;
        }

        diaryObject.transform.position = to;
        isAnimating = false;
    }
}
