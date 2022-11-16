using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 3 cosas 
// 1. parsing de JSON
// 2. corrutinas 
// 3. eventos de unity (puede que quede mañana)

public class RequestManager : MonoBehaviour
{

    private IEnumerator enumeratorCorrutina;
    private Coroutine corrutina;

    void Start(){
        string json = "{\"carros\": [" + 
        "{\"id\": 0, \"x\": 0, \"y\": 0, \"z\": 0}," +
        "{\"id\": 1, \"x\": 1, \"y\": 1, \"z\": 1}," +
        "{\"id\": 2, \"x\": 2, \"y\": 2, \"z\": 2}," +
        "{\"id\": 3, \"x\": 3, \"y\": 3, \"z\": 3}," +
        "{\"id\": 4, \"x\": 4, \"y\": 4, \"z\": 4}" + 
        "]}";

        ListaCarro carros = JsonUtility.FromJson<ListaCarro>(json);
        for(int i = 0; i < carros.carros.Length; i++){
            print(carros.carros[i].x + " , "  +
                    carros.carros[i].y + " , "  +
                    carros.carros[i].z);
        }
        
        enumeratorCorrutina = Request(); 

        corrutina = StartCoroutine(enumeratorCorrutina);
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.A)){
            StopAllCoroutines();
        }

        if(Input.GetKeyDown(KeyCode.B)){
            StopCoroutine(enumeratorCorrutina);
        }

        if(Input.GetKeyDown(KeyCode.C)){
            StopCoroutine(corrutina);
        }
    }

    // CORRUTINAS 
    // mecanismo de manejo de pseudo concurrencia en Unity
    // NO es un hilo PERO se comporta como uno
    IEnumerator Request() {

        while(true){

            // empezamos haciendo request
            UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/");

            // como en cualquier request este asunto es asíncrono
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success){
                Debug.LogError("PROBLEMA EN REQUEST");
            } else {
                print(www.downloadHandler.text);
            }

            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator RequestSimulado() {
    
        while(true){

            // hacemos solicitud para obtener string
            string json = ServerSimulado.Instance.JSON;

            // hacemos interpretación de json
            ListaCarro listaCarro = JsonUtility.FromJson<ListaCarro>(json);
            //print(listaCarro);

            // LO QUE VA A SEGUIR:
            // AVISAR A LOS DEMÁS QUE ALGO SUCEDIÓ
            // (UnityEvents)

            // esperamos para ejecutar siguiente actualización
            yield return new WaitForSeconds(1);
        }
    }
}