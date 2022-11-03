using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPoolTester : MonoBehaviour
{

    [SerializeField]
    private Carro _carritoDummy;

    [SerializeField]
    private Carro[] _carritosDummy;

    // Update is called once per frame
    void Update()
    {
        // RECUERDEN EVITAR EN LA MEDIDA DE LO POSIBLE
        // eL FIND!
        // GameObject.Find
        if(Input.GetKeyDown(KeyCode.Space)){
            CarPoolManager.Instance.Activar(
                new Vector3(
                    Random.Range(0, 10),
                    Random.Range(0, 10),
                    Random.Range(0, 10)
                )
            );
        }
    }
}
