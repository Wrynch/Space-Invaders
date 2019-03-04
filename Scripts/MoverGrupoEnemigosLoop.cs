using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverGrupoEnemigosLoop : MonoBehaviour
{
    private Transform miTransform;
    public Vector3 _velocidad;
    public float velocidad = 0;

    public GameObject Win;

    private GameObject cliente;

    private string p1;
    private string p2;
    // Use this for initialization
    void Start()
    {
        cliente = GameObject.Find("Cliente");

        Time.timeScale = 1;
        miTransform = this.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        miTransform.Translate(_velocidad * velocidad * Time.deltaTime);

        int puntosJugador1 = int.Parse(GameObject.Find("PuntosJ1").GetComponent<UnityEngine.UI.Text>().text);
        int puntosJugador2 = int.Parse(GameObject.Find("PuntosJ2").GetComponent<UnityEngine.UI.Text>().text);

        if (transform.childCount == 0)
        {
            if(puntosJugador1 > puntosJugador2)
            {
                Win.SetActive(true);
                GameObject.Find("textoWin").GetComponent<UnityEngine.UI.Text>().text = p1 + " HA GANADO!";
            }
            if (puntosJugador2 > puntosJugador1)
            {
                Win.SetActive(true);
                GameObject.Find("textoWin").GetComponent<UnityEngine.UI.Text>().text = p2 + " HA GANADO!";
            }

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals("Pared"))
        {
            _velocidad.x *= -1;
        }
    }

    public void setp1(string nombre)
    {
        this.p1 = nombre;
    }
    public void setp2(string nombre)
    {
        this.p2 = nombre;
    }

}
