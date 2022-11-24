using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField]
    private Camera[] _camaras;

    [SerializeField]
    private int _camaraActiva;

    public static CameraManager Instance {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start() 
    {
        HabilitarCamara(_camaraActiva);
    }
    private void HabilitarCamara(int camaraAHabilitar) {

        // nota para siempre:
        // hagan chequeo de excepciones

        // fail gracefully - fallar con gracia

        if(camaraAHabilitar < 0 || camaraAHabilitar >= _camaras.Length){
            throw new System.Exception("INDICE DE CÁMARA A HABILITAR FUERA DE RANGO");
        }

        if(_camaras == null){
            throw new System.Exception("ARREGLO DE CAMARAS NULO");
        }

        for(int i = 0; i < _camaras.Length; i++){

            if(i == camaraAHabilitar){
                _camaras[i].gameObject.SetActive(true);
            } else {
                _camaras[i].gameObject.SetActive(false);
            }
        }
    }

    public void IntercambiarCamara() {

        // moverme a siguiente cámara
        _camaraActiva++;

        // módulo para asegurarnos que no se exceda del tamaño
        _camaraActiva %= _camaras.Length;

        HabilitarCamara(_camaraActiva);
    }
}
