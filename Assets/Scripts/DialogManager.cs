using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogCanvas;
    [SerializeField] private Text mainText;

    [SerializeField] private GameObject imageE;

    [SerializeField] private GameObject option1;
    [SerializeField] private GameObject option2;

    private Text option1Text;
    private Text option2Text;

    private GameObject selector1;
    private GameObject selector2;

    private int selectedOption = 1;
    private bool selectionMode = false;

    private PlayerController playerController;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullText;
    private float typingSpeed = 0.03f;

    // Characters Dialog
    // DANTE
    private string[] dante_farming = new string[]
    {
        "Say, Delilah! Perhaps you could assist me? I've just finished tilling this soil, could you bring me some planting supplies?", // dante
        "Sure, what do you need?", // delilah 1
        "Sorry, not right now", // delilah 2
        "I need a <b>watering can, Poppy seeds and a bucket of nutrition</b>. They should all be located around the crops!", // dante 1
        "Oh, well maybe next time then...", // dante 2
    };


    void Start()
    {
        option1Text = option1.transform.GetChild(0).GetComponent<Text>();
        selector1 = option1.transform.GetChild(1).gameObject;

        option2Text = option2.transform.GetChild(0).GetComponent<Text>();
        selector2 = option2.transform.GetChild(1).gameObject;

        selector1.SetActive(false);
        selector2.SetActive(false);
        option1.SetActive(false);
        option2.SetActive(false);
        imageE.SetActive(false);

        // Get player controller
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.Log("Character: PlayerController not found");
        }
    }

    public void SetTaskDialog(string character, string task)
    {
        if (character == "dante")
        {
            if (task == "farming")
            {
                SetDialog(dante_farming[0], dante_farming[1], dante_farming[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        typingSpeed = 0.01f;
                        SetDialog(dante_farming[3]);

                        // the player is doing the farming task
                        playerController.farming_task = true;
                    }
                    else if (selected == 2)
                    {
                        SetDialog(dante_farming[4]);
                    }
                }));
            }
        }

        typingSpeed = 0.03f;

    }


    public void SetDialog(string main, string op1 = null, string op2 = null)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        fullText = main;
        typingCoroutine = StartCoroutine(TypeText(fullText));

        if (!string.IsNullOrEmpty(op1) && !string.IsNullOrEmpty(op2))
        {
            option1Text.text = op1;
            option2Text.text = op2;

            option1.SetActive(true);
            option2.SetActive(true);

            selector1.SetActive(true); // always show selected option 1 first
            selector2.SetActive(false);

            imageE.SetActive(false);
            selectionMode = true;
            selectedOption = 1;
        }
        else
        {
            option1.SetActive(false);
            option2.SetActive(false);
            selector1.SetActive(false);
            selector2.SetActive(false);

            imageE.SetActive(true);
            selectionMode = false;
        }

        playerController.canMove = false;
        DialogCanvas.SetActive(true);
    }


    private IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        mainText.text = "";
        foreach (char c in textToType)
        {
            mainText.text += c;
            yield return new WaitForSeconds(typingSpeed); 
        }
        isTyping = false;
    }

    void Update()
    {
        if (isTyping && Input.GetKeyDown(KeyCode.E))
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                mainText.text = fullText;
                isTyping = false;
            }
            return;
        }

        if (selectionMode)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedOption = 2;
                selector1.SetActive(false);
                selector2.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedOption = 1;
                selector1.SetActive(true);
                selector2.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.E) && !isTyping)
            {
                Debug.Log("Selected option: " + selectedOption);

                option1.SetActive(false);
                option2.SetActive(false);
                selector1.SetActive(false);
                selector2.SetActive(false);

                selectionMode = false;

                imageE.SetActive(true);

                //SetDialog("I'm just a farmer."); // triggers typing again
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && !isTyping)
            {
                imageE.SetActive(false);
                DialogCanvas.SetActive(false);
                playerController.canMove = true;
            }
        }
    }

    private IEnumerator WaitForOptionSelection(System.Action<int> onSelected)
    {
        while (selectionMode || isTyping)
        {
            yield return null;
        }
        onSelected(selectedOption);
    }

}
