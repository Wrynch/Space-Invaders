using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player
{
    public string playerName;
    public int posJugador;
    public GameObject avatar;
    public int connectId;
}

public class Cliente : MonoBehaviour {

    private const int MAX_CONNECTION = 2;
    private int port = 5701;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unReliableChannel;

    private float connectionTime;
    private int connectionId;
    private bool isConnected;
    private bool isStarted = false;

    //para evitar el lag
    private bool movido = false;

    private byte error;

    //el nombre del usuario
    public string playerName;
    private int ourClientId;

    public Transform jugador1, jugador2;


    public List<Player> jugadores = new List<Player>();
    public GameObject playerPrefab;
    public GameObject enemigo;
    public GameObject grupoEnemigos;
    public Vector3 _velocidadEnemigo;
    public float velocidad;
    public GameObject canvas1;
    public GameObject canvas2;


    public GameObject grupoEnem;
    public void Connect()
    {

        string pName = GameObject.Find("NameInput").GetComponent<InputField>().text;

        if (pName == "")
        {
            Debug.Log("Debes escribir un nombre");
            return;
        }

        playerName = pName;

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unReliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;

    }
    // Use this for initialization
    void Start ()
    {

    }
    // Update is called once per frame
    void Update ()
    {
        if (!isConnected)
        {
            return;
        }
        
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            /*case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                break;*/

            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                //Debug.Log("receiving: " + msg);
                string[] splitData = msg.Split('|');
                //Debug.Log("dato del case" + splitData[0] + "segundo valor" + splitData[1]);
                switch (splitData[0])
                {
                    case "ASKNAME":
                        OnAskName(splitData);
                        break;

                    case "CNN":
                        //Debug.Log("dato del cnn " + splitData[1] + " segundo valor " + splitData[2]);
                        SpawnPlayer(splitData[1], int.Parse(splitData[2]));
                        break;

                    case "DC":
                        break;

                    case "EMPEZAR":
                        //Aparecer Enemigos
                        grupoEnemigos.SetActive(true);
                        break;

                    case "MOVER":
                        jugadores.Find(x => x.playerName == splitData[1]).avatar.GetComponent<MoverNave>().CambiarDireccion(int.Parse(splitData[2]));
                        movido = false;
                        break;

                    case "DISPARAR":
                         jugadores.Find(x => x.playerName == splitData[1]).avatar.GetComponent<MoverNave>().disparoNave();
                        break;
                    default:
                        Debug.Log("Mensaje Invalido" + msg);
                        break;

                }
                break;

                /* case NetworkEventType.DisconnectEvent:
                     break;*/
        }
        //control del movimiento
        if (Input.GetKey(KeyCode.A) && movido == false)
        {
            //Enviar el nombre al servidor
            Send("IZQUIERDA|" + playerName, reliableChannel);
            movido = true;
        }
        else if (Input.GetKey(KeyCode.D) && movido == false)
        {
            //Enviar el nombre al servidor
            Send("DERECHA|" + playerName, reliableChannel);
            movido = true;
        }
        else if (Input.GetKey(KeyCode.F) && isStarted == false)
        {
            //Enviar el nombre al servidor
            Send("EMPEZAR|", reliableChannel);
            isStarted = true;

        }

        else if (Input.GetKeyUp(KeyCode.A))
        {
            //Parar movimiento continuo
            Send("PARARNAVE|" + playerName, reliableChannel);
            movido = false;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            //Parar movimiento continuo
            Send("PARARNAVE|" + playerName, reliableChannel);
            movido = false;
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //Parar movimiento continuo
            Send("DISPARAR|" + playerName, reliableChannel);
        }
    }
    private void OnAskName(string[] data)
    {
        //Id del player
        ourClientId = int.Parse(data[1]);

        //Enviar el nombre al servidor
        Send("NAMEIS|" + playerName, reliableChannel);

        //enviar datos al resto de jugadores
        for (int i = 2; i < data.Length - 1; i++)
        {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]));
        }

    }
 

    private void Send(string message, int channelId)
    {
        //Debug.Log("Sending: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);

    }

    public void Colision(string message)
    {
        //Debug.Log("Sending colision: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, reliableChannel, msg, message.Length * sizeof(char), out error);

    }

    private void SpawnPlayer(string playerName, int cnnId)
    {


        if (cnnId == ourClientId)
        {
            canvas1.SetActive(false);
            canvas2.SetActive(true);
        }

        Player p = new Player();
        if (cnnId % 2 != 0)
        {
            p.avatar = Instantiate(playerPrefab, new Vector3(-4.0f, -3.4f, 90), Quaternion.identity);//con esto creo un jugador
            Debug.Log(jugador1.position);
        }
        else
        {
            p.avatar = Instantiate(playerPrefab, jugador2.position, Quaternion.identity);//con esto creo un jugador
        }


        p.playerName = playerName;
        p.connectId = cnnId;
        p.avatar.GetComponentInChildren<TextMesh>().text = playerName;
        jugadores.Add(p);


        if(jugadores.Count == 2)
        {
            for( int i = 0; i < jugadores.Count; i++)
            {
            if( i == 0)
                {
                    jugadores[i].avatar.GetComponent<MoverNave>().asignarPool(GameObject.Find("poolObjectManager"));
                }
                else
                {
                    jugadores[i].avatar.GetComponent<MoverNave>().asignarPool(GameObject.Find("poolObjectManager2"));
                    grupoEnem.GetComponent<MoverGrupoEnemigosLoop>().setp1(jugadores[0].playerName);
                    grupoEnem.GetComponent<MoverGrupoEnemigosLoop>().setp2(jugadores[1].playerName);
                } 
            }
        }
    }

    public int getClienteId()
    {

        return connectionId;
    }

    public void cerrarCliente()
    {
        Application.Quit();
    }

    public void muteAudio()
    {
        AudioListener.volume = 1 - AudioListener.volume;
    }
}
