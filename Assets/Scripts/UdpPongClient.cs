using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System;

public class UdpPongClient : MonoBehaviour
{
    UdpClient client;
    Thread receiveThread;
    IPEndPoint serverEP;
    volatile bool running = true;

    public string serverIp = "127.0.0.1";
    public int serverPort = 5001;

    public GameObject leftPaddleObj;
    public GameObject rightPaddleObj;
    public GameObject ballObj;
    public Text scoreText;

    GameObject localPaddle;
    GameObject remotePaddle;

    int myId = -1;
    float sendInterval = 0.03f;
    float sendTimer = 0f;

    Queue<Action> mainThreadActions = new Queue<Action>();
    object queueLock = new object();

    void Start()
    {
        serverEP = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        client = new UdpClient();
        client.Connect(serverEP);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
        SendRaw("HELLO");
    }

    void Update()
    {
        lock (queueLock)
        {
            while (mainThreadActions.Count > 0)
                mainThreadActions.Dequeue().Invoke();
        }

        if (localPaddle != null)
        {
            sendTimer += Time.deltaTime;
            if (sendTimer >= sendInterval)
            {
                sendTimer = 0f;
                Vector3 p = localPaddle.transform.position;
                string msg = "POS:" +
                    p.x.ToString("F2", CultureInfo.InvariantCulture) + ";" +
                    p.y.ToString("F2", CultureInfo.InvariantCulture);
                SendRaw(msg);
            }
        }
    }

    void ReceiveData()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        while (running)
        {
            try
            {
                byte[] data = client.Receive(ref remoteEP);
                string msg = Encoding.UTF8.GetString(data);

                if (msg.StartsWith("ASSIGN:"))
                {
                    int id = int.Parse(msg.Substring(7));
                    lock (queueLock) mainThreadActions.Enqueue(() => OnAssigned(id));
                }
                else if (msg.StartsWith("POS:"))
                {
                    string[] parts = msg.Substring(4).Split(';');
                    if (parts.Length == 3)
                    {
                        int id = int.Parse(parts[0]);
                        if (id != myId)
                        {
                            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            lock (queueLock) mainThreadActions.Enqueue(() =>
                            {
                                if (remotePaddle != null)
                                    remotePaddle.GetComponent<ControleDosJogadores>().posicaoRede = new Vector3(x, y, 0);
                            });
                        }
                    }
                }
                else if (msg.StartsWith("POSBOLA:"))
                {
                    string[] parts = msg.Substring(8).Split(';');
                    float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
                    float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    lock (queueLock) mainThreadActions.Enqueue(() =>
                    {
                        if (ballObj != null) ballObj.GetComponent<Bola>().posicaoRede = new Vector3(x, y, 0);
                    });
                }
                else if (msg.StartsWith("SCORE:"))
                {
                    string[] parts = msg.Substring(6).Split(';');
                    int s1 = int.Parse(parts[0]);
                    int s2 = int.Parse(parts[1]);
                    lock (queueLock) mainThreadActions.Enqueue(() =>
                    {
                        if (scoreText != null) scoreText.text = s1 + " X " + s2;
                    });
                }
            }
            catch { }
        }
    }

    void OnAssigned(int id)
    {
        myId = id;
        if (id == 1)
        {
            localPaddle = leftPaddleObj;
            remotePaddle = rightPaddleObj;
        }
        else
        {
            localPaddle = rightPaddleObj;
            remotePaddle = leftPaddleObj;
        }
        leftPaddleObj.GetComponent<ControleDosJogadores>().isLocalPlayer = (localPaddle == leftPaddleObj);
        rightPaddleObj.GetComponent<ControleDosJogadores>().isLocalPlayer = (localPaddle == rightPaddleObj);
    }

    void SendRaw(string msg)
    {
        byte[] b = Encoding.UTF8.GetBytes(msg);
        client.Send(b, b.Length);
    }

    void OnApplicationQuit()
    {
        running = false;
        try { receiveThread.Abort(); } catch { }
        client.Close();
    }
}
