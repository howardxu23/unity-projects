using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Bolt.Utils;
using UdpKit;
using System.Linq;

//done by howard
namespace Bolt.Samples.Photon.Lobby
{
    //[BoltGlobalBehaviour("DrawGame scence")]
    class drawingTextureMeta : IProtocolToken
    {
        private NetworkId _entityID;
        private int _size;
        private int _width;
        private int _height;
        private int _format;

        private byte[] _textureData;
        private Texture2D _internalTexture = null;

        public NetworkId TargetEntity { get { return _entityID; } }
        public Texture2D TargetTexture
        {
            get
            {
                if (_internalTexture == null && _textureData != null)
                {
                    _internalTexture = new Texture2D(_width, _height, (TextureFormat)_format, false);
                    _internalTexture.LoadRawTextureData(_textureData);
                }

                return _internalTexture;
            }
        }

        public drawingTextureMeta() { }

        public drawingTextureMeta(NetworkId entityID, Texture2D drawingTexData)
        {
            _entityID = entityID;
            _width = drawingTexData.width;
            _height = drawingTexData.height;
            _format = (int)drawingTexData.format;

            _textureData = drawingTexData.GetRawTextureData();
            _size = _textureData.Length;
        }
        public void Read(UdpPacket packet)
        {
            _entityID = packet.ReadNetworkId();
            _width = packet.ReadInt();
            _height = packet.ReadInt();
            _format = packet.ReadInt();
        }
        public void Write(UdpPacket packet)
        {
            packet.WriteNetworkId(_entityID);
            packet.WriteInt(_width);
            packet.WriteInt(_height);
            packet.WriteInt(_format);
        }
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(_size);
                    writer.Write(_textureData);

                    var metaSerialized = this.ToByteArray();
                    var metaSize = metaSerialized.Length;

                    writer.Write(metaSize);
                    writer.Write(metaSerialized);
                }

                return m.ToArray();
            }
        }
        public static drawingTextureMeta Deserialize(byte[] data)
        {
            drawingTextureMeta result = null;
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    var size = reader.ReadInt32();
                    var textureData = reader.ReadBytes(size);

                    var metaSize = reader.ReadInt32();
                    var metaSerialized = reader.ReadBytes(metaSize);

                    result = metaSerialized.ToToken() as drawingTextureMeta;

                    if (result != null)
                    {
                        result._size = size;
                        result._textureData = textureData;
                    }
                }
            }

            return result;
        }
    }
    [BoltGlobalBehaviour("DrawGame scence")]
    public class sendReciveImageMangager : Bolt.GlobalEventListener
    {
        private const string TextureTransmitChannelName = "TextureTransmitChannel";
        private UdpKit.UdpChannelName _textureTransmitChannel;
        public override void BoltStartBegin()
        {
            // Define Reliable Channel
            _textureTransmitChannel = BoltNetwork.CreateStreamChannel(TextureTransmitChannelName, UdpChannelMode.Reliable, 1);

            BoltNetwork.RegisterTokenClass<drawingTextureMeta>();
            Debug.Log("channel created");
        }
        public override void Connected(BoltConnection connection)
        {
            connection.SetStreamBandwidth(2000 * 100);
        }
        public void SendTexture(NetworkId entityId, Texture2D texture, BoltConnection origin)
        {
            if (BoltNetwork.Connections.Any() == false) { return; }

            var textureInfo = new drawingTextureMeta(entityId, texture);

            SendTexture(textureInfo.Serialize(), origin);
        }

        private void SendTexture(byte[] playerTexture, BoltConnection origin)
        {
            foreach (var remoteConn in BoltNetwork.Connections)
            {
                // skip original sender
                if (origin != null && remoteConn.Equals(origin))
                {
                    continue;
                }

                remoteConn.StreamBytes(_textureTransmitChannel, playerTexture);
            }
        }
        public override void StreamDataReceived(BoltConnection connection, UdpStreamData data)
        {
            var playerDrawing = drawingTextureMeta.Deserialize(data.Data);

            // Retransmit
            if (playerDrawing != null)
            {
                BrokerSystem.PublishTexture(playerDrawing.TargetEntity, playerDrawing.TargetTexture, connection);
            }
        }
    }
    
}