using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPoolManager : MonoBehaviour
{

    // vamos a hacer un pseudo singleton

    // voy a utilizar una propiedad 
    // mecanismo propio de C# ! 

    // en este caso manipula una variable anónima
    public static CarPoolManager Instance {
        get;
        private set;
    }


    // EJEMPLO DE PROPIEDAD CON VARIABLE DECLARADA EXPLÍCITAMENTE
    /*
    private float variableDummy;

    public float VariableDummy {
        get {
            return variableDummy;
        }
        set {
            variableDummy = value;
        }
    }
    */

    // para hacer un pool necesito un original 

    [SerializeField]
    private GameObject _carritoOriginal;

    [SerializeField]
    private int _tamanioPool;
    private Queue<GameObject> _pool;

    void Awake() {

        // Singleton correctivo! 
        // normalmente evitamos la creación de nuevas instancias
        // como no tenemos acceso a constructor aquí destruimos nuevas instancias

        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if(_carritoOriginal == null){
            throw new System.Exception("CAR POOL MANAGER: ES NECESARIO QUE PONGAS UN CARRITO");
        }

        _pool = new Queue<GameObject>();

        // creamos objetos y agregamos al pool
        for(int i = 0; i < _tamanioPool; i++){

            // método para crear objetos
            GameObject actual = Instantiate<GameObject>(_carritoOriginal);
            actual.SetActive(false);
            _pool.Enqueue(actual);
        }
    }


    public GameObject Activar(Vector3 posicion) {

        // evitamos error - no hay objetos en pool
        if(_pool.Count == 0){
            Debug.LogError("TE QUEDASTE SIN OBJETOS");
            return null;
        }

        // obtengo objeto de pool
        GameObject actual = _pool.Dequeue();

        actual.SetActive(true);
        actual.transform.position = posicion;
        
        return actual;
    }

    public void Desactivar(GameObject objetoADesactivar){

        objetoADesactivar.SetActive(false);
        _pool.Enqueue(objetoADesactivar);
    }
}
