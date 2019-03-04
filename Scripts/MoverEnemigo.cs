using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverEnemigo : MonoBehaviour {
    private Transform miTransform;
    public Vector3 _velocidad;
    public float velocidad = 0;
    private GameObject cliente;


    private Animator miAnimator;

    private bool isStarted = false;

    public float delay = 1f;


    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        miTransform = this.transform;

        cliente = GameObject.Find("Cliente");
        miAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    //Colisiones con la bala ( destruir el enemigo )
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("disparo"))
        {
            miAnimator.SetTrigger("tocado");
            //Destroy(gameObject);
            //this.gameObject.SetActive(false);
            Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
        
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals("Pared"))
        {
            _velocidad.x *= -1;
        }

        //cuando choco
        string NombreJugColi = cliente.GetComponent<Cliente>().playerName + "";
        if (other.transform.tag.Equals("Barra") && NombreJugColi == jugador1)
        {
            cliente.GetComponent<Cliente>().Colision("PELOTACHOQUEPALAS|");
        }
        else if (other.transform.tag.Equals("Pared"))
        {
            //cliente.GetComponent<Cliente>().Colision("PELOTACHOQUEFONDO|");
            Debug.Log("AGAWGAWG");
        }
        else if (other.transform.tag.Equals("Suelo") && NombreJugColi == jugador1 || other.transform.tag.Equals("Techo") && NombreJugColi == jugador1)
        {
            Debug.Log("ENTRAAAAAA");
            cliente.GetComponent<Cliente>().Colision("PELOTACHOQUESUELOTECHO|");
        }

        if (other.transform.name == "Pared1")
        {
            int nuevaPuntuacion = int.Parse(GameObject.Find("puntJ2").GetComponent<UnityEngine.UI.Text>().text) + 1;
            GameObject.Find("puntJ2").GetComponent<UnityEngine.UI.Text>().text = nuevaPuntuacion + "";
            miTransform.transform.position = new Vector3(0, 0, 0);
        }
        else if (other.transform.name == "Pared2")
        {
            int nuevaPuntuacion = int.Parse(GameObject.Find("puntJ1").GetComponent<UnityEngine.UI.Text>().text) + 1;
            GameObject.Find("puntJ1").GetComponent<UnityEngine.UI.Text>().text = nuevaPuntuacion + "";
            miTransform.transform.position = new Vector3(0, 0, 0);
        }
}*/
}
