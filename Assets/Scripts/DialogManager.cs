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

    [SerializeField] private Moral_Meter moralMeter;

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
    // DANTE -------------------------------------------------------------------------------------
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
        "Thank you, dear! I think Polina needs help too. Follow the path to the west, you'll find her in town", // dante 1 - all items
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

    private string[] dante_harvest = new string[]
    {
         /*0*/   "Hey there Delilah! How is your harvesting task going?",  
         /*1*/   "I've harvested all he needed!",
         /*2*/   "Actually, I'm here about something else.", 
         /*3*/   "Well then head on back, let's not keep him waiting!",  
         /*4*/   "Oh? What about?", 
         /*5*/   "I'm looking for someone who is supposed to be here.", 
         /*6*/   "My sister Mary, I have reason to believe she's here in town.", 
         /*7*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!",
         /*8*/   "Who is it? Maybe I can be of assistance?",
         /*9*/   "Her name is Mary.",
         /*10*/  "Her name is May",
    };

    // POLINA ----------------------------------------------------------------------------------------
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
     /*4*/   "Oh how lovely! Thank you so much! Now our town centerpiece is beautiful again! I believe Mayor Belzy might have another task for you! He’s inside the big house to the north, next to the market.", 
     /*5*/   "Oh... well maybe you just need to use the sponge then!", 
     /*6*/   "I believe Mayor Belzy might have another task for you! He’s inside the big house to the north, next to the market.",
   };

    private string[] default_state = new string[]
  {
     /*0*/   "Hey there!",  
     /*1*/   "I'm looking for someone who is supposed to be here.",
     /*2*/   "Do you know Mary?.", 
     /*3*/   "Who is it? Maybe I can be of assistance?",  
     /*4*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!", 
     /*5*/   "Her name is Mary.",
     /*6*/   "A young girl.", 
     /*7*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!",
     /*8*/   "It rings a bell, but I can't remember.",
  };

    private string[] polina_harvest = new string[]
    {
     /*0*/   "I heard you were bringing Mayor Belzy some Poppies! How is the search going?", // polina
     /*1*/   "I've harvested all he needed!", // delilah 1
     /*2*/   "Actually, I wanted to ask about my sister...", // delilah 2
     /*3*/   "Wonderful! Well don't keep Mayor Belzy waiting over me, head on over there!", // polina 1
     /*4*/   "Oh? What about your sister?", // polina 2
     /*5*/   "My sister sent me a letter from here, I'm trying to find her.", // delilah 3
     /*6*/   "Mary, I have reason to believe she's here in town.", // delilah 4
     /*7*/   "I don't know of any Mary here in Poppy Town, but I'm sure she's fine wherever she is!",
     /*8*/   "Oh? Who is your sister? Maybe I know her and can be of assistance?",
     /*9*/   "Her name is Mary.",
     /*10*/   "Her name is May.",
    };

    // BELZY -----------------------------------------------------------------------------------------
    private string[] default_belzy = new string[]
   {
      /*0*/   "Ah, Miss Delilah, what an absolute pleasure it is to make your acquaintance.",  
     /*1*/   "Who are you?",
     /*2*/   "I came to ask about someone I'm looking for.", 
     /*3*/   "I am none other than Mayor Belzy himself, at your service, with utmost pleasure.",  
     /*4*/   "Oh? Well do tell, who is this mystery person?", 
     /*5*/   "Mary.", 
     /*6*/   "My sister, Mary.", 
     /*7*/   "Ah, Mary! Yes, I remember her! Cheerful girl, she really liked the Poppies!",
     /*8*/   "Sorry, I'm busy now. Hahaha!",
     /*9*/   "Where is she now?",
     /*10*/  "What happened to her?",
     /*11*/  "I'm sure she's around somewhere! Can't have disappeared into thin air, can she now? Oh forgive me, I jest.",
     /*12*/  "Happened to her..? Why would anything have happened to her? Are you quite alright Delilah? I'm sure that Mary is fine wherever she is!",
  };

    private string[] belzy_farming = new string[]
  {
     /*0*/   "Ah, Delilah. I heard you were helping Dante out with the Poppies! How lovely!",  
     /*1*/   "Yes, she asked me to help her clean it.",
     /*2*/   "Yes, but I came to ask about someone I'm looking for.", 
     /*3*/   "Delightful! How nice to see you helping out so quickly! I'm sure your contributions to our town will be of great value!",  
     /*4*/   "Oh? Well do tell, who is this mystery person?", 
     /*5*/   "Mary.", 
     /*6*/   "My sister, Mary.", 
     /*7*/   "Ah, Mary! Yes, I remember her! Cheerful girl, she really liked the Poppies! She would be delighted to see you helping Polina with the town fountain!",
     /*8*/   "Sorry, I'm busy now. Hahaha!",
     /*9*/   "Where is she now?",
     /*10*/  "What happened to her?",
     /*11*/  "I'm sure she's around somewhere! Can't have disappeared into thin air, can she now? Oh forgive me, I jest. Speaking of the town fountain, better run off and make sure it's sparkling!",
     /*12*/  "Happened to her..? Why would anything have happened to her? Are you quite alright Delilah? I'm sure that Mary is fine wherever she is! Speaking of the town fountain, better run off and make sure it's sparkling!",
  };

    private string[] belzy_fountain = new string[]
  {
     /*0*/   "Ah, Delilah. I heard you were helping Polina out with some town maintenence! The fountain, if I remember correctly?",  
     /*1*/   "Yes, he asked me to find a few items to help with planting more crops.",
     /*2*/   "Yes, but I came to ask you something else. I am looking for someone.", 
     /*3*/   "Wonderful! Well, best not to keep him waiting, off you run!",  
     /*4*/   "Oh? Who are you looking for?", 
     /*5*/   "Mary.", 
     /*6*/   "My sister, Mary.", 
     /*7*/   "Ah, Mary! Yes, I remember her! Cheerful girl, she really liked the Poppies! She would be delighted to see you helping Dante with the Poppy Fields!",
     /*8*/   "Sorry, I'm busy now. Hahaha!",
     /*9*/   "Where is she now?",
     /*10*/  "What happened to her?",
     /*11*/  "I'm sure she's around somewhere! Can't have disappeared into thin air, can she now? Oh forgive me, I jest. Speaking of the Poppy Fields, better run off and get those supplies to Dante!",
     /*12*/  "Happened to her..? Why would anything have happened to her? Are you quite alright Delilah? I'm sure that Mary is fine wherever she is! Speaking of the Poppy Fields, better run off and get those supplies to Dante!",
  };

    private string[] belzy_harvest = new string[]
    {
     /*0*/   "Ah, Delilah! I was just wondering what you were up to! Say, would you be able to lend me a hand?",  
     /*1*/   "Sure, what do you need?.",
     /*2*/   "Sorry, not right now", 
     /*3*/   "Wonderful! Well you see, I happen to be in need of some of our lovely Poppy flowers! <b>Harvest five fully grown Poppies</b> and bring them to me!",  
     /*4*/   "How unfortunate... Well, perhaps you can assist me sometime later then.", 
     };

    private string[] belzy_harvest2 = new string[]
    {
     /*0*/   "Ah! Have you come to bring me my Poppies?",  
     /*1*/   "Yes, here they are.",
     /*2*/   "No, not yet.", 
     /*3*/   "Oh my, but this is not what I asked for! It seems you still have some poppies to collect. Run along now!",  
     /*4*/   "Wonderful! Thank you for your assistance! These will be very useful for what I have in store! Now then, I suggest you return home and take some rest.",
     /*5*/   "Oh, how disappointing. Well, what are you waiting for? Bring me my Poppies!",
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
                        Debug.Log("player state = " + playerController.state);

                        moralMeter.UpdateReputation(10);
                    }
                    else if (selected == 2)
                    {
                        SetDialog(dante_farming[4]);
                        moralMeter.UpdateReputation(-5);
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
                            Debug.Log("player state = " + playerController.state);

                            moralMeter.UpdateReputation(10);
                        }
                        else
                        {
                            // missing items
                            SetDialog(dante_farming2[3]);
                            moralMeter.UpdateReputation(-5);
                        }
                    }
                    else if (selected == 2)
                    {
                        SetDialog(dante_farming2[5]);
                        moralMeter.UpdateReputation(-5);
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
                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);
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

            else if (task == "harvest")
            {
                SetDialog(dante_harvest[0], dante_harvest[1], dante_harvest[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(dante_harvest[3]);

                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);

                        SetDialog(dante_harvest[4], dante_harvest[5], dante_harvest[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            if (selected == 1)
                            {
                                SetDialog(dante_harvest[8], dante_harvest[9], dante_harvest[10]);

                                StartCoroutine(WaitForOptionSelection((selected) =>
                                {
                                    SetDialog(dante_harvest[7]);
                                }));
                            }
                            else if (selected == 2)
                            {
                                SetDialog(dante_harvest[7]);
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

                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);

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
                        Debug.Log("player state = " + playerController.state);

                        moralMeter.UpdateReputation(10);
                    }
                    else if (selected == 2)
                    {
                        SetDialog(polina_fountain[4]);

                        moralMeter.UpdateReputation(-5);
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
                            Debug.Log("player state = " + playerController.state);

                            moralMeter.UpdateReputation(10);
                        }
                        else
                        {
                            // missing items
                            SetDialog(polina_fountain2[3]);

                            moralMeter.UpdateReputation(-5);
                        }
                    }
                    else if (selected == 2)
                    {
                        SetDialog(polina_fountain2[5]);

                        moralMeter.UpdateReputation(-5);
                    }
                }));
            }

            else if (task == "harvest")
            {
                SetDialog(polina_harvest[0], polina_harvest[1], polina_harvest[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(polina_harvest[3]);

                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);

                        SetDialog(polina_harvest[4], polina_harvest[5], polina_harvest[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            if (selected == 1)
                            {
                                SetDialog(polina_harvest[8], polina_harvest[9], polina_harvest[10]);

                                StartCoroutine(WaitForOptionSelection((selected) =>
                                {
                                    SetDialog(polina_harvest[7]);
                                }));
                            }
                            else if (selected == 2)
                            {
                                SetDialog(polina_harvest[7]);
                            }
                        }));
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

        else if(character == "belzy")
        {
            if(task == "default_belzy")
            {
                SetDialog(default_belzy[0], default_belzy[1], default_belzy[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(default_belzy[3]);

                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);

                        SetDialog(default_belzy[4], default_belzy[5], default_belzy[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            // the selected option does not matter

                            SetDialog(default_belzy[7], default_belzy[9], default_belzy[10]);

                            StartCoroutine(WaitForOptionSelection((selected) =>
                            {
                                if (selected == 1)
                                {
                                    SetDialog(default_belzy[11]);
                                }
                                else if (selected == 2)
                                {
                                    SetDialog(default_belzy[12]);
                                }
                            }));
                        }));
                    }
                }));
            }
            
            else if(task == "farming")
            {
                SetDialog(belzy_farming[0], belzy_farming[1], belzy_farming[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(belzy_farming[3]);

                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);

                        SetDialog(belzy_farming[4], belzy_farming[5], belzy_farming[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            // the selected option does not matter

                            SetDialog(belzy_farming[7], belzy_farming[9], belzy_farming[10]);

                            StartCoroutine(WaitForOptionSelection((selected) =>
                            {
                                if (selected == 1)
                                {
                                    SetDialog(belzy_farming[11]);
                                }
                                else if (selected == 2)
                                {
                                    SetDialog(belzy_farming[12]);
                                }
                            }));
                        }));
                    }
                }));
            }

            else if (task == "fountain")
            {
                SetDialog(belzy_fountain[0], belzy_fountain[1], belzy_fountain[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(belzy_fountain[3]);

                        moralMeter.UpdateReputation(5);
                    }
                    else if (selected == 2)
                    {
                        moralMeter.UpdateReputation(-5);

                        SetDialog(belzy_fountain[4], belzy_fountain[5], belzy_fountain[6]);

                        StartCoroutine(WaitForOptionSelection((selected) =>
                        {
                            // the selected option does not matter

                            SetDialog(belzy_fountain[7], belzy_fountain[9], belzy_fountain[10]);

                            StartCoroutine(WaitForOptionSelection((selected) =>
                            {
                                if (selected == 1)
                                {
                                    SetDialog(belzy_fountain[11]);
                                }
                                else if (selected == 2)
                                {
                                    SetDialog(belzy_fountain[12]);
                                }
                            }));
                        }));
                    }
                }));
            }

            else if (task == "harvest")
            {
                SetDialog(belzy_harvest[0], belzy_harvest[1], belzy_harvest[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        SetDialog(belzy_harvest[3], null, null, false);

                        playerController.state = 5;
                        Debug.Log("player state = " + playerController.state);

                        moralMeter.UpdateReputation(10);
                    }
                    else if (selected == 2)
                    {
                        SetDialog(belzy_harvest[4]);

                        moralMeter.UpdateReputation(-5);
                    }
                }));
            }

            else if (task == "harvest2")
            {
                SetDialog(belzy_harvest2[0], belzy_harvest2[1], belzy_harvest2[2]);

                StartCoroutine(WaitForOptionSelection((selected) =>
                {
                    if (selected == 1)
                    {
                        if (playerController.poppies >= 5)
                        {
                            // all items
                            SetDialog(belzy_harvest2[4]);
                            playerController.state = 6;
                            Debug.Log("player state = " + playerController.state);

                            moralMeter.UpdateReputation(10);
                        }
                        else
                        {
                            // missing items
                            SetDialog(belzy_harvest2[3]);

                            moralMeter.UpdateReputation(-5);
                        }
                    }
                    else if (selected == 2)
                    {
                        SetDialog(belzy_harvest2[5]);

                        moralMeter.UpdateReputation(-5);
                    }
                }));
            }
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
