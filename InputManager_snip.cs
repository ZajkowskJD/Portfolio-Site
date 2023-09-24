void Update()
    {

        if(acceptingInput) inputChar = Input.inputString;
        if(!string.IsNullOrEmpty(inputChar))
        {
            if(inputChar == " ")
            {
                GetComponent<AudioSource>().PlayOneShot(clicks[Random.Range(0, clicks.Length - 1)]);
                userInput = userInput + inputChar;
                GameObject.Find("User Input").GetComponent<TextMeshProUGUI>().text = userInput;

                Debug.Log("pressed space");
                return;
            }
            else if(inputChar=="\n" || inputChar=="\r")
            {
                GetComponent<AudioSource>().PlayOneShot(clicks[Random.Range(0, clicks.Length - 1)]);
                if (!gameStarted)
                {
                    gameStarted = true;
                    GameObject.Find("Opening text").GetComponent<TextMeshProUGUI>().enabled = false;
                    GameObject.Find("Text log").GetComponent<TextMeshProUGUI>().enabled = true;
                    GameObject.Find("User Input").GetComponent<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    UpdateTextLog(userInput);
                    ReadUserInput();
                }
                userInput = ">";
                GameObject.Find("User Input").GetComponent<TextMeshProUGUI>().text = userInput;

                Debug.Log("pressed enter");
                return;
            }
            else if (inputChar == "\b")
            {
                GetComponent<AudioSource>().PlayOneShot(bClicks[Random.Range(0, bClicks.Length - 1)]);
                userInput = userInput.Substring(0, userInput.Length - 1);
                GameObject.Find("User Input").GetComponent<TextMeshProUGUI>().text = userInput;

                Debug.Log("pressed delete");
                return;
            }
            GetComponent<AudioSource>().PlayOneShot(clicks[Random.Range(0, clicks.Length - 1)]);
            userInput = userInput + inputChar;
            GameObject.Find("User Input").GetComponent<TextMeshProUGUI>().text = userInput;
            Debug.Log("pressed " + inputChar);
        }
    }