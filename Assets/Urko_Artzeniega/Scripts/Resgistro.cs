using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Clase pura para guardar los datos
public class Jugador
{
    public string nombre;
    public int edad;
    public int id;

    public Jugador(int id, string nombre, int edad)
    {
        this.nombre = nombre;
        this.edad = edad;
        this.id = id;
    }
}

public class Registro : MonoBehaviour
{
    //En C# el map se llama Dictionary
    private Dictionary<int, Jugador> usuarios = new Dictionary<int, Jugador>();
    private int contadorId = 1; // Para ID unico

    [Header("Referencias UI")]
    public TMP_InputField inputNombre;
    public TMP_InputField inputEdad;
    public TextMeshProUGUI textoNotificaciones; // feedback temporal

    // Funcion pal OnClick() del boton REGISTER
    public void RegistrarNuevoUsuario()
    {
        // Comprobar que el input no este vacio
        if (string.IsNullOrEmpty(inputNombre.text) || string.IsNullOrEmpty(inputEdad.text))
        {
            MostrarNotificacion("Error: Faltan datos");
            return;
        }

        // Convertir con parse y si no es valido mostrar mensaje de error
        if (int.TryParse(inputEdad.text, out int edadValida))
        {
            // Añadir dictionary
            Jugador nuevoJugador = new Jugador(contadorId, inputNombre.text, edadValida);
            usuarios.Add(contadorId, nuevoJugador);

            MostrarNotificacion($"Usuario {inputNombre.text} añadido con ID {contadorId}");
            contadorId++;
        }
        else
        {
            MostrarNotificacion("Error: La edad debe ser un número");
        }
    }

    // Corrutina para los mensajes temporales
    private void MostrarNotificacion(string mensaje)
    {
        StartCoroutine(RutinaNotificacion(mensaje));
    }

    private IEnumerator RutinaNotificacion(string mensaje)
    {
        textoNotificaciones.text = mensaje;
        yield return new WaitForSeconds(2.5f); // Mostrar por 2.5 segs
        textoNotificaciones.text = "";
    }
}