using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System;

// 3 cosas 
// 1. parsing de JSON
// 2. corrutinas 
// 3. eventos de unity (puede que quede mañana)


// si necesitamos parámetros en nuestro evento tenemos
// que declarar nuestro propio tipo
[Serializable]
public class RequestConArg : UnityEvent<ListaCarro>{}

public class RequestManager : MonoBehaviour
{

    [SerializeField]
    private RequestConArg _requestConArgumento;
    
    [SerializeField]
    private UnityEvent _requestSinArgumento;

    [SerializeField]
    private string _urlRequest = "http://127.0.0.1:5000/";

    private IEnumerator _enumeratorCorrutina;
    private Coroutine _corrutina;

    void Start(){
        /*
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
  */      
        _enumeratorCorrutina = Request(); 

        _corrutina = StartCoroutine(_enumeratorCorrutina);

        // agregar listener programaticamente
        _requestSinArgumento.AddListener(EscuchaDummy);
    }

    void EscuchaDummy() {
        print("METODO AGREGADO A EVENTO PROGRAMATICAMENTE");
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.A)){
            StopAllCoroutines();
        }

        if(Input.GetKeyDown(KeyCode.B)){
            StopCoroutine(_enumeratorCorrutina);
        }

        if(Input.GetKeyDown(KeyCode.C)){
            StopCoroutine(_corrutina);
        }
    }

    // CORRUTINAS 
    // mecanismo de manejo de pseudo concurrencia en Unity
    // NO es un hilo PERO se comporta como uno
    IEnumerator RequestRecurrente() {

        while(true){

            // empezamos haciendo request
            UnityWebRequest www = UnityWebRequest.Get(_urlRequest);

            // como en cualquier request este asunto es asíncrono
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success){
                Debug.LogError("PROBLEMA EN REQUEST");
            } else {
                print(www.downloadHandler.text);

                // hacer parsing de JSON
                ListaCarro listaCarro = JsonUtility.FromJson<ListaCarro>(
                    www.downloadHandler.text
                );


                // cuando decidamos avisarle a los observadores
                // utilizamos lo siguiente:
                _requestSinArgumento?.Invoke();

                _requestConArgumento?.Invoke(listaCarro);
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

    // ejemplo por si sólo se hace 1 request al inicio
    IEnumerator Request() {

        // 1. hacer request
        // 2. parsear datos en string a json
        // 3. invocar evento de unity para avisar que ya estuvo
        // NOTA: NO HACER LOOP

        print("CORRUTINA!");

        yield return new WaitForSeconds(0.5f);

        // OJO
        // AQUÍ LO HARDCODEAMOS PARA LA DEMO EN CLASE
        // PERO ESTO DEBE SER RESULTADO DEL PARSING DEL JSON
        ListaCarro datos = new ListaCarro();

        datos.steps = new Step[10];

        for(int i = 0; i < datos.steps.Length; i++){
            datos.steps[i] = new Step();

            datos.steps[i].carros = new Carro[10];
            datos.steps[i].semaforo = new Semaforo[2];

            for(int j = 0; j < datos.steps[i].carros.Length; j++){
                datos.steps[i].carros[j] = new Carro();
                datos.steps[i].carros[j].id = j;
                datos.steps[i].carros[j].x = j * 1.5f - 6f;
                datos.steps[i].carros[j].y = 0;
                datos.steps[i].carros[j].z = i * 1;
            }

            for(int j = 0; j < datos.steps[i].semaforo.Length; j++){
                datos.steps[i].semaforo[j] = new Semaforo();
                datos.steps[i].semaforo[j].color = 0;
            }
        }

        print(JsonUtility.ToJson(datos));

        /*
        {"steps":[{"carros":[{"id":0,"x":-6.0,"y":0.0,"z":0.0},{"id":1,"x":-4.5,"y":0.0,"z":0.0},{"id":2,"x":-3.0,"y":0.0,"z":0.0},{"id":3,"x":-1.5,"y":0.0,"z":0.0},{"id":4,"x":0.0,"y":0.0,"z":0.0},{"id":5,"x":1.5,"y":0.0,"z":0.0},{"id":6,"x":3.0,"y":0.0,"z":0.0},{"id":7,"x":4.5,"y":0.0,"z":0.0},{"id":8,"x":6.0,"y":0.0,"z":0.0},{"id":9,"x":7.5,"y":0.0,"z":0.0}],"semaforo":[{"color":0},{"color":0}]},{"carros":[{"id":0,"x":-6.0,"y":0.0,"z":-1.0},{"id":1,"x":-4.5,"y":0.0,"z":-1.0},{"id":2,"x":-3.0,"y":0.0,"z":-1.0},{"id":3,"x":-1.5,"y":0.0,"z":-1.0},{"id":4,"x":0.0,"y":0.0,"z":-1.0},{"id":5,"x":1.5,"y":0.0,"z":-1.0},{"id":6,"x":3.0,"y":0.0,"z":-1.0},{"id":7,"x":4.5,"y":0.0,"z":-1.0},{"id":8,"x":6.0,"y":0.0,"z":-1.0},{"id":9,"x":7.5,"y":0.0,"z":-1.0}],"semaforo":[{"color":0},{"color":0}]},{"carros":[{"id":0,"x":-6.0,"y":0.0,"z":-2.0},{"id":1,"x":-4.5,"y":0.0,"z":-2.0},{"id":2,"x":-3.0,"y":0.0,"z":-2.0},{"id":3,"x":-1.5,"y":0.0,"z":-2.0},{"id":4,"x":0.0,"y":0.0,"z":-2.0},{"id":5,"x":1.5,"y":0.0,"z":-2.0},{"id":6,"x":3.0,"y":0.0,"z":-2.0},{"id":7,"x":4.5,"y":0.0,"z":-2.0},{"id":8,"x":6.0,"y":0.0,"z":-2.0},{"id":9,"x":7.5,"y":0.0,"z":-2.0}],"semaforo":[{"color":0},{"color":0}]},{"carros":[{"id":0,"x":-6.0,"y":0.0,"z":-3.0},{"id":1,"x":-4.5,"y":0.0,"z":-3.0},{"id":2,"x":-3.0,"y":0.0,"z":-3.0},{"id":3,"x":-1.5,"y":0.0,"z":-3.0},{"id":4,"x":0.0,"y":0.0,"z":-3.0},{"id":5,"x":1.5,"y":0.0,"z":-3.0},{"id":6,"x":3.0,"y":0.0,"z":-3.0},{"id":7,"x":4.5,"y":0.0,"z":-3.0},{"id":8,"x":6.0,"y":0.0,"z":-3.0},{"id":9,"x":7.5,"y":0.0,"z":-3.0}],"semaforo":[{"color":0},{"color":0}]},{"carros":[{"id":0,"x":-6.0,"y":0.0,"z":-4.0},{"id":1,"x":-4.5,"y":0.0,"z":-4.0},{"id":2,"x":-3.0,"y":0.0,"z":-4.0},{"id":3,"x":-1.5,"y":0.0,"z":-4.0},{"id":4,"x":0.0,"y":0.0,"z":-4.0},{"id":5,"x":1.5,"y":0.0,"z":-4.0},{"id":6,"x":3.0,"y":0.0,"z":-4.0},{"id":7,"x":4.5,"y":0.0,"z":-4.0},{"id":8,"x":6.0,"y":0.0,"z":-4.0},{"id":9,"x":7.5,"y":0.0,"z":-4.0}],"semaforo":[{"color":0},{"color":0}]},{"carros":[{"id":0,"x":-6.0,"y":0.0,"z":-5.0},{"id":1,"x":-4.5,"y":0.0,"z":-5.0},{"id":2,"x":-3.0,"y":0.0,"z"
        */

        _requestConArgumento?.Invoke(datos);
    }
}