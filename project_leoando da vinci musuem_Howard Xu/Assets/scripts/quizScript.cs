using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class quizScript : MonoBehaviour
{
    
    public GameObject[] listQuetions;
    public GrabCamera gameCam;
    private static int MAXQUETIONS;
    private int currentQuetion=0;
    [SerializeField] private GameObject background;
    public bool QuetionRoomActive = false;
    [SerializeField]
    private GameObject correctAnswerTag;
    [SerializeField]
    private GameObject wrongAnswerTag;
    [SerializeField]
    private GameObject endGameTag;
    [SerializeField]
    private Text wrongAnswerText;
    [SerializeField]
    private Text EndGameText;
    [SerializeField]
    private Button correctNextQuetionButton;
    [SerializeField]
    private Button wrongNextQuetionButton;
    [SerializeField]
    private Button EndGameButton;
    private bool nextQuetionButtonPressed=false;
    private int score = 0;

    public int startScrore =0;
    public int endScrore = 0;

    [SerializeField] bool quizStartPoint = true;
    [SerializeField] GameObject startSplash;
    [SerializeField] Button startButton;
    void Start()
    {
        gameCam = GetComponent<GrabCamera>();
        MAXQUETIONS = listQuetions.Length;
        background.SetActive(false);
        for (int i=0; i < listQuetions.Length; i++)
        {
            listQuetions[i].SetActive(false);
        }
        correctAnswerTag.SetActive(false);
        wrongAnswerTag.SetActive(false);
        correctNextQuetionButton.onClick.AddListener(buttonclicked);
        wrongNextQuetionButton.onClick.AddListener(buttonclicked);
        startButton.onClick.AddListener(startButtonClicked);
        EndGameButton.onClick.AddListener(endQuizButtonPressed);
    }
    bool startQuizRan = false;
    void FixedUpdate()
    {
        if (quizStartPoint == true)//starts the quiz at the start of the game as a control to see if musum is effective
        {
            if (startQuizRan == false)
            {
                background.SetActive(true);
                startSplash.SetActive(true);
                gameCam.GrabCameraFrom(GameObject.FindGameObjectWithTag("Player"));
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                startQuizRan = true;
            }
            if (startButtonPressed == true)
            {
                startSplash.SetActive(false);
       
                if(currentQuetion < MAXQUETIONS) 
                {
                    listQuetions[currentQuetion].SetActive(true);//activates quetion
                    if (listQuetions[currentQuetion].GetComponent<quetionScript>().isAnswered == true)//when the quetion is answered, check if it is correct or not
                    {                       
                        if (listQuetions[currentQuetion].GetComponent<quetionScript>().isCorrect == true)//correct
                        {
                            correctAnswerTag.SetActive(true);
                            addScore();
                        }
                        else if (listQuetions[currentQuetion].GetComponent<quetionScript>().isCorrect == false)//wrong
                        {
                            wrongAnswerTag.SetActive(true);
                            var correctanswer = listQuetions[currentQuetion].GetComponent<quetionScript>().correctAnswer + 1;
                            wrongAnswerText.text ="retake after doing the museum";//tells the user what is the correct answer
                        }
                        if (nextQuetionButtonPressed == true)//once answer is checked, wait till player presses next quetion
                        {
                            listQuetions[currentQuetion].SetActive(false);//disables current quetion
                            currentQuetion += 1;//moves to next quetion
                            correctAnswerTag.SetActive(false);
                            wrongAnswerTag.SetActive(false);
                            nextQuetionButtonPressed = false;
                            scoreAdded = false;
                        }
                        //wait till player presses next quetion
                    }
                }
                if (currentQuetion == MAXQUETIONS)//reaches end of the quetion, displays the player's score
                {
                    endGameTag.SetActive(true);
                    EndGameText.text = score.ToString() +"/" + MAXQUETIONS.ToString(); 
                   
                }
            }
            //wait till the end quiz button is pressed, then start the musuem
            if (endQuizPressed == true)
            {
                startScrore = score;
                resetQuiz();
                quizStartPoint = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameCam.ReleaseCamera();
                background.SetActive(false);
                endQuizPressed = false;
                
            }
        }
        else//quiz room componet for the end of the musum
        {
            if (QuetionRoomActive == true && currentQuetion < MAXQUETIONS)//once player has entered quiz room
            {
                background.SetActive(true);
                listQuetions[currentQuetion].SetActive(true);//activates quetion
                if (listQuetions[currentQuetion].GetComponent<quetionScript>().isAnswered == true)//when the quetion is answered, check if it is correct or not
                {
                    if (listQuetions[currentQuetion].GetComponent<quetionScript>().isCorrect == true)//correct
                    {
                        correctAnswerTag.SetActive(true);
                        addScore();
                    }
                    else if (listQuetions[currentQuetion].GetComponent<quetionScript>().isCorrect == false)//wrong
                    {
                        wrongAnswerTag.SetActive(true);
                        var correctanswer = listQuetions[currentQuetion].GetComponent<quetionScript>().correctAnswer + 1;
                        wrongAnswerText.text = correctanswer.ToString();//tells the user what is the correct answer
                    }
                    if (nextQuetionButtonPressed == true)//once answer is checked, wait till player presses next quetion
                    {
                        listQuetions[currentQuetion].SetActive(false);//disables current quetion
                        currentQuetion += 1;//moves to next quetion
                        correctAnswerTag.SetActive(false);
                        wrongAnswerTag.SetActive(false);
                        nextQuetionButtonPressed = false;
                        scoreAdded = false;
                    }
                    //wait till player presses next quetion

                }
            }
            else if (currentQuetion == MAXQUETIONS)//reaches end of the quetion, displays the player's score
            {
                endGameTag.SetActive(true);
                EndGameText.text = score.ToString() + "/" + MAXQUETIONS.ToString();
                endScrore = score;
            }
            if (endQuizPressed == true)
            {
                print("start score: " + startScrore);
                print("end score: " + endScrore);
                endQuizPressed = false;

                //writes the scores to text file
                string[] lines =
                {
                    "start score: "+startScrore.ToString(),
                    "end score: "+endScrore.ToString()
                };
                File.WriteAllLines("score.txt", lines);
                //restarts the level
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
    bool scoreAdded = false;
    void addScore()
    {
        if (scoreAdded == false)
        {
            score += 1;
            scoreAdded = true;
        }
    }
    void buttonclicked()
    {
        nextQuetionButtonPressed = true;
    }
    bool startButtonPressed=false;    
    void startButtonClicked()
    {
        startButtonPressed = true;
    }
    bool endQuizPressed = false;
    void endQuizButtonPressed()
    {
        endQuizPressed = true;
    }
    void resetQuiz()//resets the quiz parameters
    {
        currentQuetion = 0;
        endGameTag.SetActive(false);
        score = 0;
        
        foreach(var quetion in listQuetions)
        {
            quetion.GetComponent<quetionScript>().isAnswered = false;
        }
    }
    public void OnTriggerEnter(Collider other)//when player enters the quiz zone
    {
        if (other.gameObject.tag=="Player"&& QuetionRoomActive == false)
        {
            print("grab camarea");
            //when player enters room, camera gets grabbed          
            gameCam.GrabCameraFrom(other.gameObject);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            QuetionRoomActive = true;

        }
    }
}
