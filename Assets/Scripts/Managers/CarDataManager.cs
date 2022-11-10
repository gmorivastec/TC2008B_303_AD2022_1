using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDataManager : MonoBehaviour
{

    [SerializeField]
    private Carro[] _listaDeCarros;

    private GameObject[] _carrosGO;

    void Start() 
    {
        _carrosGO = new GameObject[_listaDeCarros.Length];
        
        // activarlos por primera vez
        for(int i = 0; i < _listaDeCarros.Length; i++)
        {
            _carrosGO[i] = CarPoolManager.Instance.Activar(Vector3.zero);
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
    }
}
