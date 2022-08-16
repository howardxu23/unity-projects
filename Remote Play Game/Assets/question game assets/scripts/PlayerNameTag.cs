using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace Bolt.Samples.Photon.Lobby.Drawgame
{
    public class PlayerNameTag : Bolt.EntityEventListener<IQuetionGamePlayerState>
    {
        private string playername;
        public Text PlayerName;
        
        public override void Attached()
        {
            if (entity.IsOwner)
            {
                StreamReader sr = new StreamReader("PlayerData/player_name.txt");//opens file with player name
                playername = sr.ReadLine();//reads the playername
                //print(playername);

                state.Name = playername;//sets the state to the player's name
                sr.Close();
            }
            state.AddCallback("Name", nameChanged);
        }
        public void Update()
        {
            gameObject.transform.parent = GameObject.Find("MainCanvas").transform;
        }
        void nameChanged()
        {
            PlayerName.GetComponent<Text>().text = state.Name;
        }
        public int getPlayerChoice()
        {
            return state.AnswerChoice;
        }
        public void incrementPlayerScore(int score)
        {
            state.Points += score;
        }
        public int GetPlayerPoints()
        {
            return state.Points;
        }
        public string GetPlayerName()
        {
            return state.Name;
        }

    }
}