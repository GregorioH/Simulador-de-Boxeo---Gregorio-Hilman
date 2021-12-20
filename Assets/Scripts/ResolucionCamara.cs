using UnityEngine;
using System.Collections.Generic;

public class ResolucionCamara : MonoBehaviour
{
    #region TamañoPantalla
    private int TamañoPantallaX = 0;
    private int TamañoPantallaY = 0;
    #endregion

    #region Metodos

    #region Escalar Camara
    private void RescaleCamera()
    {

        if (Screen.width == TamañoPantallaX && Screen.height == TamañoPantallaY) return;

        float relacionAspecto = 16.0f / 9.0f;
        float relacionVentana = (float)Screen.width / (float)Screen.height;
        float alturaEscala = relacionVentana / relacionAspecto;
        Camera camera = GetComponent<Camera>();

        if (alturaEscala < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = alturaEscala;
            rect.x = 0;
            rect.y = (1.0f - alturaEscala) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float anchoEscala = 1.0f / alturaEscala;

            Rect rect = camera.rect;

            rect.width = anchoEscala;
            rect.height = 1.0f;
            rect.x = (1.0f - anchoEscala) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        TamañoPantallaX = Screen.width;
        TamañoPantallaY = Screen.height;
    }
    #endregion

    #endregion

    #region Metodos Unity

    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;

    }

    // Use this for initialization
    void Start()
    {
        RescaleCamera();
    }

    // Update is called once per frame
    void Update()
    {
        RescaleCamera();
    }
    #endregion
}
