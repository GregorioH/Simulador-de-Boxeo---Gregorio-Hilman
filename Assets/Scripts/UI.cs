using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [Header("General")]
    public GameObject Boxeador;
    public GameObject saco;

    [Header("Cámaras")]
    public GameObject[] Camaras;

    [Header("Rotación del Boxeador")]
    public Slider rotacionBoxeadorX;
    public Slider rotacionBoxeadorY;
    public InputField textoValorRotX;
    public InputField textoValorRotY;
    private float valorAnteriorRotacionX;
    private float valorAnteriorRotacionY;
    private float cambioRotX;
    private float cambioRotY;

    [Header("Posición del Boxeador")]
    public Slider posicionBoxeador;
    public InputField textoValorPos;

    [Header("Fuerza de los Golpes")]
    public Slider fuerzaBoxeador;
    public GameObject puñoDerecho;
    public InputField textoFuerza;
    public float radio;

    [Header("Masa del Saco")]
    public Slider masaSaco;
    public InputField textoMasa;

    [Header("Gravedad")]
    public Slider gravedad;
    public InputField textoGravedad;
    private Vector3 valorInicialGravedad;

    void Start()
    {
        // Rotacion Boxeador
        rotacionBoxeadorX.onValueChanged.AddListener(cambioRotXValorRotacion);
        valorAnteriorRotacionX = rotacionBoxeadorX.value;
        textoValorRotX.text = rotacionBoxeadorX.value.ToString() + "°";

        rotacionBoxeadorY.onValueChanged.AddListener(cambioRotYValorRotacion);
        valorAnteriorRotacionY = rotacionBoxeadorY.value;
        textoValorRotY.text = rotacionBoxeadorY.value.ToString() + "°";

        // Posicion Boxeador
        posicionBoxeador.onValueChanged.AddListener(cambioValorPosicion);
        textoValorPos.text = posicionBoxeador.value.ToString() + "m";
        Boxeador.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.position = new Vector3(0, 0, -posicionBoxeador.value);

        // Fuerza Boxeador
        textoFuerza.text = fuerzaBoxeador.value.ToString() + "kg o " + (fuerzaBoxeador.value * 9.81).ToString() + "N";
        fuerzaBoxeador.onValueChanged.AddListener(cambioFuerzaGolpe);

        // Masa Saco
        textoMasa.text = masaSaco.value.ToString() + "kg";
        saco.GetComponent<Rigidbody>().mass = masaSaco.value;
        masaSaco.onValueChanged.AddListener(cambioMasa);

        // Gravedad
        Physics.gravity = new Vector3(0, -9.8f, 0);
        valorInicialGravedad = Physics.gravity;
        gravedad.value = Mathf.Round(gravedad.value * 100) / 100;
        Vector3 gravedad2 = Physics.gravity;
        textoGravedad.text = "x" + gravedad.value.ToString();
        gravedad2.y = valorInicialGravedad.y * gravedad.value;
        Physics.gravity = gravedad2;
        gravedad.onValueChanged.AddListener(cambioGravedad);
    }

    public void FixedUpdate()
    {
        cambioFuerzaGolpe(fuerzaBoxeador.value);
    }

    public void cambiarCamara()
    {
        if (Camaras[0].activeSelf)
        {
            Camaras[0].SetActive(false);
            Camaras[1].SetActive(true);
        }
        else if (Camaras[1].activeSelf)
        {
            Camaras[1].SetActive(false);
            Camaras[2].SetActive(true);
        }
        else if (Camaras[2].activeSelf)
        {
            Camaras[2].SetActive(false);
            Camaras[3].SetActive(true);
        }
        else if (Camaras[3].activeSelf)
        {
            Camaras[3].SetActive(false);
            Camaras[0].SetActive(true);
        }
    }

    public void reiniciarEscena()
    {
        SceneManager.LoadScene("Simulador");
    }

    public void cambioRotXValorRotacion(float valor)
    {
        if (!string.IsNullOrEmpty(textoValorRotX.text))
        {
            valor = Mathf.Round(valor * 30);
            cambioRotX = valor - valorAnteriorRotacionX;
            Boxeador.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.Rotate(Vector3.up * cambioRotX);
            valorAnteriorRotacionX = valor;
            textoValorRotX.text = valor.ToString() + "°";
        }
    }

    public void cambioRotYValorRotacion(float valor)
    {
        if (!string.IsNullOrEmpty(textoValorRotY.text))
        {
            valor = Mathf.Round(valor * 360);
            cambioRotY = valor - valorAnteriorRotacionY;
            Boxeador.transform.GetChild(0).gameObject.transform.Rotate(Vector3.up * cambioRotY);
            valorAnteriorRotacionY = valor;
            textoValorRotY.text = valor.ToString() + "°";
        }
    }

    public void cambioValorPosicion(float valor)
    {
        if (!string.IsNullOrEmpty(textoValorRotY.text))
        {
            valor = Mathf.Round(valor * 1000) / 1000;
            Vector3 adelante = transform.TransformDirection(transform.forward);
            Boxeador.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.position = new Vector3(0, 0, 0);
            Boxeador.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.localPosition = adelante * -posicionBoxeador.value;
            textoValorPos.text = valor.ToString() + "m";
        }
    }

    public void cambioFuerzaGolpe(float valor)
    {
        valor = Mathf.Round(valor * 100) / 100;
        fuerzaBoxeador.value = valor;

        Transform puñoDerecho = Boxeador.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).
            gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0);
        Vector3 diferencia = new Vector3(0.08f, -0.015f, 0);

        Collider[] colliders = Physics.OverlapBox(puñoDerecho.position + diferencia, puñoDerecho.transform.localScale / 8, Quaternion.identity);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (valor > (saco.GetComponent<Rigidbody>().mass))
                {
                    rb.AddExplosionForce(valor - saco.GetComponent<Rigidbody>().mass, puñoDerecho.GetComponent<Rigidbody>().transform.position, radio, 0f);
                }
            }
        }
        textoFuerza.text = valor.ToString() + "kg o " + (Mathf.Round(valor * 9.81f * 100) / 100).ToString() + "N";
    }

   public void cambioMasa(float valor)
    {
        valor = Mathf.Round(valor * 100) / 100;
        saco.GetComponent<Rigidbody>().mass = valor;

        textoMasa.text = valor.ToString() + "kg";
    }

    public void cambioGravedad(float valor)
    {
        Vector3 gravedad = Physics.gravity;
        valor = Mathf.Round(valor * 100) / 100;
        gravedad.y = valorInicialGravedad.y * valor;
        Physics.gravity = gravedad;

        textoGravedad.text = "x" + valor.ToString();
    }
}
