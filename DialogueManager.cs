using Ink.Runtime;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Collections.Generic;


public class DialogueManager : MonoBehaviour, ISaveable
{
    [Tooltip("Json story")]
    public TextAsset inkJson;
    private Story story;
    [Tooltip("Text box to display dialogue")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image portrait;
    [SerializeField] private Image dialogBG;

    private string dialogue;
    [Tooltip("Current position in the script")]
    private string currentKnotName;

    private float testTime = 0;
    [Tooltip("Time to read each line")]
    [SerializeField] private float timeToWait = 0.5f;//time before moving onto the next line

    [SerializeField] private bool isDialoguePlaying = false;

    [SerializeField] private float defaultTextCrawlSpeed = 0.01f;


    private CinemachineLookAt cinemachineManager;
    private Transform[] cameraFocalPoints;
    private UnityEvent[] eventsToInvoke;

    [SerializeField] private bool debugMode = false;


    private List<string> playedDialogue = new List<string>();

    // Start is called before the first frame update
    // Will load in the story
    //Need to set up save state
    //Save file will contain save state?

    private void Awake()
    {
        cinemachineManager = GameObject.FindGameObjectWithTag("CinemachineManager").GetComponent<CinemachineLookAt>();
        cameraFocalPoints = Array.Empty<Transform>();
        eventsToInvoke = Array.Empty<UnityEvent>();
        if(DataPersistenceManagerV2.IsValid()) { DataPersistenceManagerV2.Instance.RegisterSaveable(this); }
    }

    private void OnDestroy()
    {
        if (DataPersistenceManagerV2.IsValid()) { DataPersistenceManagerV2.Instance.UnregisterSaveable(this); }
    }

    void Start()
    {
        story = new Story(inkJson.text);
        story.Continue();
    }

    //This will display the dialogue and empty it if the dialogue has ended
    public void NextPieceOfDialogue()
    {
        isDialoguePlaying = true;
        if (story.canContinue) dialogue = story.Continue();
        else
        {
            text.text = "";
            portrait.sprite = null;
            portrait.enabled = false;
            Array.Clear(cameraFocalPoints, 0, cameraFocalPoints.Length);
            Array.Clear(eventsToInvoke, 0, eventsToInvoke.Length);
            isDialoguePlaying = false;
            return;
        }

        //Checks the tag on current line if the story can stop
        //If it can the text box will empty, mark that dialogue is over, and exit the function
        if (story.currentTags.Count > 0)
        {
            for (int i = 0; i < story.currentTags.Count; i++)
            {
                UseTags(i);
            }

        }
        else
        {
            portrait.sprite = null;
            portrait.enabled = false;
        }

        StartCoroutine("TypeLine");
    }

    private IEnumerator TypeLine() //types the next line of dialog character by character
    {
        int substringStartPos = -1;
        int index = 0;
        int length = 1;
        foreach (char c in dialogue.ToCharArray())
        {
            //if(text skip button pressed) {
            //text.text = dialogue;
            //break out of foreach loop
            //}
            //else {

            switch (c)
            {
                case '<':
                    substringStartPos = index;
                    while (dialogue.ToCharArray()[substringStartPos + length] != '>')
                    {
                        length++;
                    }
                    length++;
                    text.text += dialogue.Substring(substringStartPos, length);
                    index++;
                    break;
                case ':':
                case '.':
                case ',':
                case ';':
                case '\'':
                case '\"':
                case '(':
                case '!':
                case '?':
                case ')':
                case '+':
                case '-':
                case '*':
                case '/':
                case '=':
                    index++;
                    if (length == 1)
                    {
                        text.text += "<size=95%>" + c + "</size>";
                        yield return new WaitForSeconds(defaultTextCrawlSpeed);
                    }
                    else
                    {
                        length--;
                        yield return null;
                    }
                    break;

                default:
                    index++;
                    if (length == 1)
                    {
                        text.text += c;
                        yield return new WaitForSeconds(defaultTextCrawlSpeed);
                    }
                    else
                    {
                        length--;
                        yield return null;
                    }
                    break;
            }



            //}
        }
        //Start Audio
        yield return new WaitForSeconds(timeToWait);
        //End Audio
        text.text = "";
        NextPieceOfDialogue();
    }

    //This will go to the knot specified by the the knotName String in the story and play that dialogue from that section
    //Can use this for event calling
    public void GoToNextSection(string knotName)
    {
        bool dPlayed = DataPersistenceManagerV2.Instance.GetSavedValueBool(knotName,DataPersistenceManagerV2.SaveGroups.Dialogue,false);
        if (dPlayed)
        {
            playedDialogue.Add(knotName);
        }
        //Debug.Log("Going to " + knotName);
        if (knotName == null || (knotName == currentKnotName && isDialoguePlaying) || playedDialogue.Contains(knotName) || dPlayed || debugMode)
        {
            //Debug.Log("No knot selected. Will not do anything");
            //Debug.Log(playedDialogue.Contains(knotName));
            //Debug.Log(dPlayed);
            //Debug.Log(isDialoguePlaying);
            return;
        }
        playedDialogue.Add(knotName);
        DataPersistenceManagerV2.Instance.AddSavedValueBoolAndSave(knotName, true, DataPersistenceManagerV2.SaveGroups.Dialogue);
        //if (isDialoguePlaying)
        //{
            //Debug.Log("New dialog source found, interrupting original to play newest.");
            StopAllCoroutines();
            text.text = "";
        //}
        currentKnotName = knotName;
        dialogBG.enabled = true;
        story.ChoosePathString(knotName);
        NextPieceOfDialogue();
    }

    public void UpdateCameraFocalPoints(GameObject pullFrom)
    {
        Transform[] focalpoints = pullFrom.GetComponent<FocalTransformList>().GetFocalPoints();
        cameraFocalPoints = focalpoints;
    }

    public void UpdateInvokables(GameObject pullFrom)
    {
        eventsToInvoke = pullFrom.GetComponent<DialogInvokables>().GetInvokables();
    }

    public List<SaveDataContainer> GetSaveData()
    {
        List<SaveDataContainer> saveData = new List<SaveDataContainer>();
        foreach (string s in playedDialogue)
        {
            saveData.Add(new SaveDataContainer(s, true.ToString(), DataPersistenceManagerV2.SaveGroups.Dialogue));
        }
        return saveData;
    }

    private int UseTags(int i)
    {
        switch (story.currentTags[i])
        {
            case "stop here":
                text.text = "";
                isDialoguePlaying = false;
                dialogBG.enabled = false;
                Array.Clear(cameraFocalPoints, 0, cameraFocalPoints.Length);
                Array.Clear(eventsToInvoke, 0, eventsToInvoke.Length);
                return i;
            case "portrait":
                portrait.enabled = true;
                string portrait_str = story.currentTags[i + 1];
                if (portrait_str == "blank")
                {
                    portrait.sprite = null;
                    i++;
                }
                else if (portrait_str == "clear")
                {
                    portrait.enabled = false;
                    i++;
                }
                else
                {
                    if (portrait_str == "fr_grp_n")
                    {
                        
                    }
                    else
                    {
                        
                    }
                    string sprite_str = story.currentTags[i + 2];
                    portrait.sprite = LoadSprite(portrait_str, sprite_str);
                    i += 2;
                }
                return i;
            case "lookat":
                if (cinemachineManager.gameObject != null)
                {
                    int focalPointIndex = int.Parse(story.currentTags[i + 1]);
                    cinemachineManager.LookAtNewTarget(cameraFocalPoints[focalPointIndex]);
                }
                else Debug.Log("CinemachineManager not found, make sure it's tagged properly");
                i++;
                return i;
            case "endlookat":
                Array.Clear(cameraFocalPoints, 0, cameraFocalPoints.Length);
                cinemachineManager.StopLookAtNewTarget();
                return i;
            case "maxanchors":
                float xMax = 0;
                float yMax = 0;
                try
                {
                    xMax = float.Parse(story.currentTags[i + 1]);
                    yMax = float.Parse(story.currentTags[i + 2]);

                    portrait.GetComponent<RectTransform>().anchorMax = new Vector2(xMax, yMax);
                    portrait.GetComponent<RectTransform>().offsetMax = Vector2.zero;

                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }
                //Debug.Log(xMax + ", " + yMax);
                i += 2;
                return i;
            case "flash":
                dialogBG.gameObject.GetComponent<Animator>().SetTrigger("flash");
                return i;
            case "shake":
                dialogBG.gameObject.GetComponent<Animator>().SetTrigger("shake");
                return i;
            case "invoke":
                int invokeIndex = -1;
                invokeIndex = int.Parse(story.currentTags[i + 1]);
                if (invokeIndex != -1 && invokeIndex < eventsToInvoke.Length) eventsToInvoke[invokeIndex].Invoke();
                else Debug.Log(invokeIndex);
                i++;
                return i;
            case "paralyze":
                GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsCharacterController>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                return i;
            case "unparalyze":
                GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsCharacterController>().enabled = true;
                return i;
            case "waitset":
                timeToWait = float.Parse(story.currentTags[i + 1]);
                i++;
                return i;
            case "scrollset":
                defaultTextCrawlSpeed = float.Parse(story.currentTags[i + 1]);
                i++;
                return i;
            case "box":
                string box_str = story.currentTags[i + 1];
                string box_sprite = story.currentTags[i + 2];
                dialogBG.sprite = LoadSprite(box_str, box_sprite);
                i += 2;
                return i;
            default: return i;
        }
    }

    private Sprite LoadSprite(string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>("Sprites/" + imageName);
        foreach (var s in all)
        {
            if (s.name == spriteName) return s;
        }
        return null;
    }

}
