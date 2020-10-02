using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Dialog dialog;

    public bool startOnAwake = false;

    GameObject gameManager;

    private void Start()
    {
        gameManager = GameManager.instance.gameObject;
        NextMessage();
    }


    public float lettersAppearSpeedProSec = 2;
    int lastSchownLetterIndex = 0, currentMessageLenght = 0;
    int currentMessageIndex = -1;
    string nextMessage;

    float t;

    private void Awake()
    {
        if (!startOnAwake)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if(gameManager.activeSelf)
        {
            t += Time.deltaTime;
            int appearedLettersCount = (int) (t * lettersAppearSpeedProSec);
            if(appearedLettersCount > lastSchownLetterIndex && currentMessageLenght > appearedLettersCount)
            {
                lastSchownLetterIndex = appearedLettersCount;
                UpdateMessage();
            }

            if (Input.GetKeyDown(KeyCode.Return))
                NextMessage();
        }
    }

    public void NextMessage()
    {
        if (lastSchownLetterIndex < currentMessageLenght - 1)
        {
            lastSchownLetterIndex = currentMessageLenght - 1;
        }
        else
        {
            if(currentMessageIndex == dialog.messages.Length - 1)
            {
                gameObject.SetActive(false);
                return;
            }

            lastSchownLetterIndex = -1;
            nextMessage = dialog.messages[++currentMessageIndex];
            currentMessageLenght = nextMessage.Length;
            t = 0;
        }
        UpdateMessage();
    }

    void UpdateMessage()
    {
        text.text = nextMessage.Substring(0, lastSchownLetterIndex + 1);
    }

}
