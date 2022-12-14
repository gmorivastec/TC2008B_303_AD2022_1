using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDataManager : MonoBehaviour
{

    [SerializeField]
    private Carro[] _listaDeCarros;

    [SerializeField]

    private CarroSO[] _carritosScriptableObjects;

    private GameObject[] _carrosGO;

    private Vector3[] _direcciones;
    

    void Start() 
    {
        _carrosGO = new GameObject[_listaDeCarros.Length];
        
        // activarlos por primera vez
        for(int i = 0; i < _listaDeCarros.Length; i++)
        {
            _carrosGO[i] = CarPoolManager.Instance.Activar(Vector3.zero);
            // actualizar scriptable object de carrito
        }

        PosicionarCarros();
    }

    private void PosicionarCarros() {
        // activar los 10 carritos en las posiciones congruentes
        for(int i = 0; i < _listaDeCarros.Length; i++) 
        {
            _carrosGO[i].transform.position = new Vector3(
                _listaDeCarros[i].x,
                _listaDeCarros[i].y,
                _listaDeCarros[i].z
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // MUY IMPORTANTE
        // CENTRALIZAR ENTRADA
        if(Input.GetKeyDown(KeyCode.R)){

            // recalcular posiciones
            for(int i = 0; i < _listaDeCarros.Length; i++) 
            {
                _listaDeCarros[i] = new Carro();
                _listaDeCarros[i].id = 0;
                _listaDeCarros[i].x = Random.Range(0f, 10f);
                _listaDeCarros[i].y = Random.Range(0f, 10f);
                _listaDeCarros[i].z = Random.Range(0f, 10f);
            }

            // posicionar carritos en nuevo lugar
            PosicionarCarros();
        }
        */

        if(_direcciones != null){
            for(int i = 0; i < _carrosGO.Length; i++){
                // modificar orientación de vehículo 
                _carrosGO[i].transform.forward = _direcciones[i].normalized;

                // desplazar utilizando vectores
                _carrosGO[i].transform.Translate(_direcciones[i] * Time.deltaTime);
            }
        }
        
         

    }

    public void EscucharSinArgumentos() {

        print("EVENTO LANZADO SIN ARGUMENTOS");
    }

    public void EscucharConArgumentos(ListaCarro datos) {

        print("RECIBIDO: " + datos);

        // actualizar _listaDeCarros
        // invocar PosicionarCarros()

        _direcciones = new Vector3[datos.steps[0].carros.Length];
        for(int i = 0; i < _direcciones.Length; i++){
            _direcciones[i] = new Vector3();
        }

        StartCoroutine(ActualizarPosicionesConDatos(datos));
    }

    IEnumerator ActualizarPosicionesConDatos(ListaCarro datos){

        for(int i = 0; i < datos.steps.Length; i++){

            // actualizar posición
            _listaDeCarros = datos.steps[i].carros;
            PosicionarCarros();

            // recalcular direccion 

            for(int j = 0; j < _direcciones.Length; j++){
                
                
                if(i < datos.steps.Length - 1){
                    _direcciones[j] = new Vector3(
                        datos.steps[i + 1].carros[j].x - datos.steps[i].carros[j].x,
                        datos.steps[i + 1].carros[j].y - datos.steps[i].carros[j].y,
                        datos.steps[i + 1].carros[j].z - datos.steps[i].carros[j].z 
                    );
                } else {
                    _direcciones[j] = Vector3.zero;
                }
            }

            

            // esperar un poquito
            yield return new WaitForSeconds(1);
        }
    }
}
