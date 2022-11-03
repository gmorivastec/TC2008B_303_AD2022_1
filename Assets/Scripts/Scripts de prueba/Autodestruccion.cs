using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestruccion : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Autodestruirse", 3);
    }

    void Autodestruirse() {
        CarPoolManager.Instance.Desactivar(gameObject);
    }
}
