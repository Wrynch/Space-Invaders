using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoverDisparo2 : MonoBehaviour
{

    private Transform miTransform;
    public int velocidad;
    public Vector3 _velocidad;


    private TrailRenderer tr;
    // Use this for initialization
    void Start()
    {
        miTransform = this.transform;
        tr = GetComponent<TrailRenderer>();
        //Invoke("Reload", 1);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("enemigo"))
        {
            int nuevaPuntuacion = int.Parse(GameObject.Find("PuntosJ2").GetComponent<UnityEngine.UI.Text>().text) + 1;
            GameObject.Find("PuntosJ2").GetComponent<UnityEngine.UI.Text>().text = nuevaPuntuacion + "";
            //this.gameObject.SetActive(false);
            reiniciar();
        }
        /*else if (collision.transform.tag.Equals("fuera")) {
            reiniciar();
        }*/
    }
    private void OnEnable()
    {
        Invoke("reiniciar", 1);
    }

    // Update is called once per frame
    void Update()
    {
        miTransform.Translate(_velocidad * velocidad * Time.deltaTime);
    }

    public void reiniciar()
    {
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GameObject.Find("poolObjectManager2").GetComponent<PoolObject>().AnadirDisparos(gameObject);
        gameObject.SetActive(false);
        tr.Clear();
    }
}
