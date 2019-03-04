using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverNave : MonoBehaviour {
    private Transform miTransform;
    public int velocidad;

    public GameObject miPoolDisparos;
    public Transform posDisparo;


    // Use this for initialization
    void Start ()
    {
        miTransform = this.transform;

        //miPoolDisparos = GameObject.Find("poolObjectManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (miTransform.position.x >= 5f && velocidad < -1)
        {
            velocidad = 0;
        }
        else if (miTransform.position.x <= -5f && velocidad > 1)
        {
            velocidad = 0;
        }
        miTransform.Translate(Vector3.left * velocidad * Time.deltaTime);


    }

    public void CambiarDireccion(int velo)
    {
        this.velocidad = velo;
    }
 
    public void disparoNave()
    {
        miPoolDisparos.GetComponent<PoolObject>().CrearDisparos(posDisparo.position);
    }

    public void anadirDispa(GameObject dispa) {
        miPoolDisparos.GetComponent<PoolObject>().AnadirDisparos(dispa);
    }

    public void asignarPool(GameObject miPool)
    {
        this.miPoolDisparos = miPool;
    }
}
