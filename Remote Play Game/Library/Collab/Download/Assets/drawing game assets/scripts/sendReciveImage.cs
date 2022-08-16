using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Bolt.Utils;
using UdpKit;

//done by howard
namespace Bolt.Samples.Photon.Lobby
{
    [BoltGlobalBehaviour("DrawGame scence")]
    public class sendReciveImage : Bolt.GlobalEventListener
    {
        
        public override void Connected(BoltConnection connection)
        {
            // Configure this connection to send at 1000kb/s
            connection.SetStreamBandwidth(1024 * 1000);
        }
        private const string ImageChannelName = "ImageChannelName";
        private static UdpKit.UdpChannelName ImageTranferChannel;

        public override void BoltStartBegin()
        {
            ImageTranferChannel = BoltNetwork.CreateStreamChannel(ImageChannelName, UdpChannelMode.Reliable, 1);//creates channel
        }

        //open image file and convert to byte 
        private byte[] ImagetoByte(string filename)
        {
            byte[] imageByteData = null;
            string[] filepaths;

            if (Directory.Exists("drawings/" + filename + ".png"))
            {
                BoltLog.Info("loading image");
                filepaths = Directory.GetFiles("drawings/" + filename + ".png");

                WWW www = new WWW("file://" + filepaths[0]);//"download file from disk"

                Texture2D new_texture = new Texture2D(1315, 1991);//create texture2D 
                www.LoadImageIntoTexture(new_texture);//insert image into texture

                imageByteData = ImageConversion.EncodeToPNG(new_texture);//encodes the image
            }
            else
            {
                BoltLog.Info("FILE DOES NOT EXIST");
            }
            return imageByteData;
        }
        private Texture2D ByteToImageTexture(byte[] imageByteData)//insert byte array and it retyrbs a texture2D
        {
            Texture2D drawing = new Texture2D(1315, 1991);

            drawing.LoadImage(imageByteData);
            drawing.Apply();

            return drawing;
        }

        public void sendDrawing(string imagename)
        {
            byte[] imageByteData = ImagetoByte(imagename);
            BoltNetwork.Server.StreamBytes(ImageTranferChannel, imageByteData);

        }
        
        public override void StreamDataReceived(BoltConnection connection, UdpStreamData data)
        {
            BoltLog.Info("Received data from {0} on channel {1}: {2} bytes", connection, data.Channel, data.Data.Length);
        }
        
        
        public override void SceneLoadLocalDone(string scene)//debuging
        {
            if (BoltNetwork.IsClient)
            {
                BoltLog.Info("sending image");
                sendDrawing("test image");

            }
            else if (BoltNetwork.IsServer)
            {
                // reciveImage(BoltNetwork.c);
            }
        }
    }
}