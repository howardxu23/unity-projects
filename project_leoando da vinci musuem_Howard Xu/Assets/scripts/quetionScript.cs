using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class quetionScript : MonoBehaviour
{
    public int quetionChoice=0;
    public bool isAnswered=false;
    public bool isCorrect = false;
    public int correctAnswer;
    [SerializeField]
    [Tooltip("for the answer choices")]
    private Button[] mutipleChoiceButtons;

    public void Awake()
    {
        //adds listeners to the buttons as to tell which is pressed.
        mutipleChoiceButtons[0].onClick.AddListener(answer1pressed);
        mutipleChoiceButtons[1].onClick.AddListener(answer2pressed);
        mutipleChoiceButtons[2].onClick.AddListener(answer3pressed);
        mutipleChoiceButtons[3].onClick.AddListener(answer4pressed);
    }
    public void resetQuetion()
    {
        isAnswered = false;
        isCorrect = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isAnswered == true)//wait till player presses the choice button
        {
            if (quetionChoice == correctAnswer)
            {
                isCorrect = true;
            }
        }
    }
    void answer1pressed()
    {
        quetionChoice = 0;
        isAnswered = true;
    }
    void answer2pressed()
    {
        quetionChoice = 1;
        isAnswered = true;
    }
    void answer3pressed()
    {
        quetionChoice = 2;
        isAnswered = true;
    }
    void answer4pressed()
    {
        quetionChoice = 3;
        isAnswered = true;
    }
}
