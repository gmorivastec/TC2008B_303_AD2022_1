using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSimulado : MonoBehaviour
{

    // 2 maneras de interactuar entre objetos
    // 1. polling 
    // 2. eventos 

    public static ServerSimulado Instance{
        get;
        private set;
    } 

    public string JSON 
    {
        get;
        private set;
    }

    void Awake() 
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerarJSON());
    }

    IEnumerator GenerarJSON() {
        /*
        while(true)
        {
            ListaCarro listaCarro = new ListaCarro();
            listaCarro.carros = new Carro[10];

            for(int i = 0; i < listaCarro.carros.Length; i++){
                listaCarro.carros[i] = new Carro();
                listaCarro.carros[i].x = Random.Range(0f, 10f);
                listaCarro.carros[i].y = Random.Range(0f, 10f);
                listaCarro.carros[i].z = Random.Range(0f, 10f);
            }

            JSON = JsonUtility.ToJson(listaCarro);
            print(JSON);
            yield return new WaitForSeconds(0.5f);
        }*/

        yield return new WaitForEndOfFrame();
    }
}
