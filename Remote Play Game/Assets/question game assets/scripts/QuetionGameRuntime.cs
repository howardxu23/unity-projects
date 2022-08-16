using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.IO;
using UnityEngine.UI;

namespace Bolt.Samples.Photon.Lobby.Drawgame
{
    public class QuetionGameRuntime : Bolt.EntityEventListener<IQuetionGameBoardState>
    {
        public GameObject quetionDisplay;
        public GameObject[] answersDisplay;
        public GameObject correctAnswerDisplay;
        public GameObject timerDisplay;
        private int MaxQuizRounds;
        private int currentQuizRound=0;
        private float remainingTime=30.0f;
        public const float ROUNDTIMES=30.0f;
        public string[] QuizList;

        public const int PointsGiven = 1;

        private int quetionIndex = 0;
        private int ans1Index = 1;
        private int ans2Index = 2;
        private int ans3Index = 3;
        private int ans4Index = 4;
        private int correctAnsIndex = 5;
        private QuetionGameRoundProgressEvent RoundInProgress;

        private AudioSource quetionGameBGM;
        public override void Attached()
        {
            StreamReader sr = new StreamReader("QuetionGameData/quetionGame.txt");//opens file with quetions
                                                                                  //file is stored as: [Quetion,option1,option2, option3, option 4, correct option(int), etc]
            var raw = sr.ReadLine();
            sr.Close();
            QuizList = raw.Split('`');//turns it into a array 
            
            /*   DEBUGGING
            for (int i = 0;i<QuizList.Length; i++)
            {
                BoltLog.Info(QuizList[i]);
            }*/

            MaxQuizRounds = QuizList.Length / 6;//get no. of rounds 
            quetionGameBGM = gameObject.GetComponent<AudioSource>();
            BoltLog.Info(MaxQuizRounds);

            RoundInProgress = QuetionGameRoundProgressEvent.Create();
            RoundInProgress.QuetionGameRoundInProgress = false;
            RoundInProgress.Send();
            quetionGameBGM.Play();
        }
        public void Update()
        {
            if (BoltNetwork.IsServer)
            {
                if (currentQuizRound < MaxQuizRounds)
                {

                    //sets up round
                    string Quetion = QuizList[quetionIndex];
                    string ans1 = QuizList[ans1Index];
                    string ans2 = QuizList[ans2Index];
                    string ans3 = QuizList[ans3Index];
                    string ans4 = QuizList[ans4Index];
                    int correctAns = int.Parse(QuizList[correctAnsIndex]);

                    state.Quetion = Quetion;
                    state.Answer1 = ans1;
                    state.Answer2 = ans2;
                    state.Answer3 = ans3;
                    state.Answer4 = ans4;
                    state.CorrectAnswer = correctAns;

                    //displays the round's quetion
                    quetionDisplay.GetComponentInChildren<Text>().text = state.Quetion;
                    answersDisplay[0].GetComponentInChildren<Text>().text = state.Answer1;
                    answersDisplay[1].GetComponentInChildren<Text>().text = state.Answer2;
                    answersDisplay[2].GetComponentInChildren<Text>().text = state.Answer3;
                    answersDisplay[3].GetComponentInChildren<Text>().text = state.Answer4;

                    if (state.CorrectAnswer == 1)//sets correct answer display at the end accordingly
                    {
                        correctAnswerDisplay.GetComponentInChildren<Text>().text = ("the correct answer is: " + state.Answer1 + "\n press any key to continue");
                    }
                    else if (state.CorrectAnswer == 2)
                    {
                        correctAnswerDisplay.GetComponentInChildren<Text>().text = ("the correct answer is: " + state.Answer2 + "\n press any key to continue");
                    }
                    else if (state.CorrectAnswer == 3)
                    {
                        correctAnswerDisplay.GetComponentInChildren<Text>().text = ("the correct answer is: " + state.Answer3 + "\n press any key to continue");
                    }
                    else if (state.CorrectAnswer == 4)
                    {
                        correctAnswerDisplay.GetComponentInChildren<Text>().text = ("the correct answer is: " + state.Answer4 + "\n press any key to continue");
                    }

                        //sends round start
                    RoundInProgress.QuetionGameRoundInProgress = true;
                    RoundInProgress.Send();
                    //waits for timer to run out or all players answered
                    bool timeIsUp=Timer();

                    if (timeIsUp)
                    {
                        BoltLog.Warn("round ends");

                        correctAnswerDisplay.SetActive(true);//sets the correct answer disaply to true

                        quetionGameBGM.Stop();

                        var players=GameObject.FindGameObjectsWithTag("QuetionGamePlayer");

                        foreach(GameObject player in players)// Handles the checking of answers
                        {
                            var playerScript = player.GetComponent<PlayerNameTag>();
                            var playerChoice = playerScript.getPlayerChoice();
                            if (playerChoice == state.CorrectAnswer)
                            {
                                playerScript.incrementPlayerScore(PointsGiven);
                            }
                        }
                        RoundInProgress.QuetionGameRoundInProgress = false;//tells the mobile that round is up
                        RoundInProgress.Send();

                        BoltLog.Warn("awaiting keypress");
                        if (Input.anyKey)//waits for user to press a key
                        {
                            remainingTime = 30.0f;
                            BoltLog.Info("advance new round");
                            //advances the quetion set
                            quetionIndex += 6;
                            ans1Index += 6;
                            ans2Index += 6;
                            ans3Index += 6;
                            ans4Index += 6;
                            correctAnsIndex += 6;
                            //increments round
                            currentQuizRound += 1;
                            correctAnswerDisplay.SetActive(false);
                            quetionGameBGM.Play();
                        }
                    }
                }
                else
                {
                    //displays who has the highest points                  
                    var players= GameObject.FindGameObjectsWithTag("QuetionGamePlayer");
                    var Winnner = findWinner(players);

                    correctAnswerDisplay.GetComponentInChildren<Text>().text = ("you have reached end of the quiz!\n Winner is: "+Winnner);
                    correctAnswerDisplay.SetActive(true);
                }              
            }
        }
        bool Timer()
        {
            //remainingTime = 30.0f;
            var floorTime = Mathf.FloorToInt(remainingTime);

            remainingTime -= Time.deltaTime;
            var newFloorTime = Mathf.FloorToInt(remainingTime);

            if (newFloorTime != floorTime)
            {
                floorTime = newFloorTime;

                state.Timer = floorTime;
                if (state.Timer <= 0)
                {
                    timerDisplay.GetComponent<Text>().text = state.Timer.ToString();
                }
            }
            if (remainingTime <= 0.0f)
            {
                return true;
            }
            return false;
        }
        string findWinner(GameObject []playerList)
        {
            string winner = null;
            int maxint=-10;

            for (int i = 0; i < playerList.Length; i++)
            {
                int value = playerList[i].GetComponent<PlayerNameTag>().GetPlayerPoints();
                if (value > maxint)
                {
                    maxint = value;
                    winner = playerList[i].GetComponent<PlayerNameTag>().GetPlayerName();
                }
            }
            return winner;
        }
    }
}
