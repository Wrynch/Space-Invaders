using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolObject : MonoBehaviour
{

    public GameObject disparos;
    public int cantidad;
    private List<GameObject> listaDisparos;

    // Use this for initialization
    void Start()
    {
        listaDisparos = new List<GameObject>();

        for (int i = 0; i < cantidad; i++)
        {
            listaDisparos.Add(Instantiate(disparos));
        }
    }

    public void CrearDisparos(Vector3 pos)
    {
        GameObject disparosColocar = listaDisparos[0];
        listaDisparos.RemoveAt(0);
        Debug.Log("quitaaaaaaaaaaaaaaaaaaaaaaa" + listaDisparos.Count);
        disparosColocar.transform.position = pos;
        //Debug.Log("Antes************" + disparosColocar.activeSelf);
        disparosColocar.SetActive(true);
        //Debug.Log("Despues************" + disparosColocar.activeSelf);
    }

    public void AnadirDisparos(GameObject disparosAnadir)
    {
        Debug.Log("añadeeeeeeeeeeeeeeeeeeeeeee"+ listaDisparos.Count + "dddd" + disparosAnadir);
        
        listaDisparos.Add(disparosAnadir);
    }


}