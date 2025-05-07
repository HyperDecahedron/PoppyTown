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
        "I need <b>a watering can</b> and <b>Poppy seeds</b>. They should all be located around the crops!", // dante 1
        "Oh, well maybe next time then...", // dante 2
    };

    private string[] dante_farming2 = new string[]
    {
        "Have you brought me all the items?", // dante
        "Yes, here they are.", // delilah 1
        "No, not yet.", // delilah 2
        "Oh dear, you're still missing something! Everything should be around the crops...", // dante 1 - missing items
        "Thank you dear!", // dante 1 - all items
        "Oh, well maybe next time then...", // dante 2
    };

    private string[] dante_fountain = new string[]
    {
     /*0*/   "Hey there! I heard you were fixing up the fountain for us!", 
     /*1*/   "Yes, now it's all clean again!", 
     /*2*/   "Actually, I'm here about something else.", 
     /*3*/   "How kind of you to help out like that! Better let Polina know!", 
     /*4*/   "Oh? What about?", 
     /*5*/   "I'm looking for someone who is supposed to be here.",
     /*6*/   "My sister Mary, I have reason to believe she's here in town.", 
     /*7*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!",
     /*8*/   "Who is it? Maybe I can be of assistance?",
     /*9*/   "Her name is Mary.",
     /*10*/   "Her name is May.",
    };

    // POLINA
    private string[] polina_farming = new string[]
    {
     /*0*/   "How is your search for the farming supplies going?", // polina
     /*1*/   "I've found all the items!", // delilah 1
     /*2*/   "Actually, I wanted to ask about my sister...", // delilah 2
     /*3*/   "Wonderful to hear! Head back to the Poppy Fields and let Dante know!", // polina 1
     /*4*/   "Oh? What about your sister?", // polina 2
     /*5*/   "My sister sent me a letter from here, I'm trying to find her.", // delilah 3
     /*6*/   "Mary, I have reason to believe she's here in town.", // delilah 4
     /*7*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!",
     /*8*/   "Oh? Who is your sister? Maybe I know her and can be of assistance?",
     /*9*/   "Her name is Mary.",
     /*10*/   "Her name is May.",
    };

    private string[] polina_fountain = new string[]
   {
     /*0*/   "Hey Delilah! Could you maybe assist me with something? If it's not too much of an issue that is!", // 
     /*1*/   "Sure, what do you need?", // 
     /*2*/   "Sorry, not right now", //
     /*3*/   "Oh, thank you! Our fountain needs some cleaning up, more than I was able to do yesterday. Please grab a <b>sponge</b> and <b>clean away any grime on the fountains to the north</b>!", // 
     /*4*/   "Oh, well that's okay. I guess I can try to manage on my own...",  
   };

    private string[] polina_fountain2 = new string[]
   {
     /*0*/   "Delilah! Is our fountain back to being sparkling clean?",  
     /*1*/   "Yes it sure is!",
     /*2*/   "No, not yet.", 
     /*3*/   "Oh, are you sure? It seems you may have missed a spot or two...",  
     /*4*/   "Oh how lovely! Thank you so much for helping me out with this! Now our town centerpiece is beautiful again!", 
     /*5*/   "Oh... well maybe you just need to use the sponge then!", 
   };

    private string[] default_state = new string[]
  {
     /*0*/   "Hey there!?",  
     /*1*/   "I'm looking for someone who is supposed to be here.",
     /*2*/   "Do you know Mary?.", 
     /*3*/   "Who is it? Maybe I can be of assistance?",  
     /*4*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!", 
     /*5*/   "Her name is Mary.",
     /*6*/   "A young girl.", 
     /*7*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!",
     /*8*/   "It rings a bell, but I can't remember.",
  };

    private string[] template = new string[]
   {
     /*0*/   "",  
     /*1*/   "",
     /*2*/   "", 
     /*3*/   "",  
     /*4*/   "", 
     /*5*/   "", 
     /*6*/   "", 
     /*7*/   "",
     /*8*/   "",
     /*9*/   "",
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
                        SetDialog(dante_farming[3], null, null, false);

                        // STATE 1: player accepted farming task
                        playerController.state = 1;
                    }
                    else if (selected == 2)
                    {
                        SetDialog(dante_farming[4]);
                    }
                }));
            }
            else if (task == "farming2")
            {
                SetDialog(dante_farming2[0], dante_farming2[1], dante_farming2[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        if(playerController.has_watering_can && playerController.has_seeds)
                        {
                            // all items
                            SetDialog(dante_farming2[4]);

                            // STATE 2: player finished farming task
                            playerController.state = 2;
                        }
                        else
                        {
                            // missing items
                            SetDialog(dante_farming2[3]);
                        }
                    }
                    else if (selected == 2)
                    {
                        SetDialog(dante_farming2[5]);
                    }
                }));
            }

            else if (task == "fountain")
            {

                SetDialog(dante_fountain[0], dante_fountain[1], dante_fountain[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(dante_fountain[3]);
                    }
                    else if (selected == 2)
                    {
                        SetDialog(dante_fountain[4], dante_fountain[5], dante_fountain[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            if (selected == 1)
                            {
                                SetDialog(dante_fountain[8], dante_fountain[9], dante_fountain[10]);

                                StartCoroutine(WaitForOptionSelection((selected) =>
                                {
                                    SetDialog(dante_fountain[7]);
                                }));
                            }
                            else if (selected == 2)
                            {
                                SetDialog(dante_fountain[7]);
                            }
                        }));
                    }
                }));
            }
        }

        else if(character == "polina")
        {
            if(task == "farming")
            {
                SetDialog(polina_farming[0], polina_farming[1], polina_farming[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(polina_farming[3]);
                    }
                    else if (selected == 2)
                    {
                        SetDialog(polina_farming[4], polina_farming[5], polina_farming[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            if (selected == 1)
                            {
                                SetDialog(polina_farming[8], polina_farming[9], polina_farming[10]);

                                StartCoroutine(WaitForOptionSelection((selected) =>
                                {
                                    SetDialog(polina_farming[7]);
                                }));
                            }
                            else if (selected == 2)
                            {
                                SetDialog(polina_farming[7]);
                            }
                        }));
                    }
                }));
            }

            else if(task == "fountain")
            {
                SetDialog(polina_fountain[0], polina_fountain[1], polina_fountain[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(polina_fountain[3], null, null, false); // bold
                        // accepted task
                        playerController.state = 3;
                    }
                    else if (selected == 2)
                    {
                        SetDialog(polina_fountain[4]);
                    }
                }));
            }

            else if(task == "fountain2")
            {
                SetDialog(polina_fountain2[0], polina_fountain2[1], polina_fountain2[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        if (playerController.has_cleaned1 && playerController.has_cleaned2 && playerController.has_cleaned3)
                        {
                            // all items
                            SetDialog(polina_fountain2[4]);

                            // STATE 2: player finished farming task
                            playerController.state = 4;
                        }
                        else
                        {
                            // missing items
                            SetDialog(polina_fountain2[3]);
                        }
                    }
                    else if (selected == 2)
                    {
                        SetDialog(polina_fountain2[5]);
                    }
                }));
            }
        }

        else if(character == "default_state")
        {
            SetDialog(default_state[0], default_state[1], default_state[2]);

            StartCoroutine(WaitForOptionSelection((selected) =>
            {
                if (selected == 2)
                {
                    SetDialog(default_state[4]);
                }
                else if (selected == 1)
                {
                    SetDialog(default_state[3], default_state[5], default_state[6]);

                    StartCoroutine(WaitForOptionSelection((selected) =>
                    {
                        if (selected == 1)
                        {
                            SetDialog(default_state[7]);
                        }
                        else if (selected == 2)
                        {
                            SetDialog(default_state[8]);
                        }
                    }));
                }
            }));
        }
    }


    public void SetDialog(string main, string op1 = null, string op2 = null, bool type = true)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        fullText = main;
        typingCoroutine = StartCoroutine(TypeText(fullText, type));

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


    private IEnumerator TypeText(string textToType, bool type)
    {
        if (type)
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
        else
        {
            mainText.text = textToType;
        }
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
