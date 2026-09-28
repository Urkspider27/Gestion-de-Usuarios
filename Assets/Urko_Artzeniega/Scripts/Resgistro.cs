using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Requerido para usar TextMeshPro
using UnityEngine.UI; // Requerido para el Slider de borrado

// Clase pura de C# para guardar los datos (no hereda de MonoBehaviour)
public class Jugador
{
    public int id;
    public string nombre;
    public int edad;

    public Jugador(int id, string nombre, int edad)
    {
        this.id = id;
        this.nombre = nombre;
        this.edad = edad;
    }
}

public class Registro : MonoBehaviour
{
    // El "unordered_map" de C#
    private Dictionary<int, Jugador> usuarios = new Dictionary<int, Jugador>();
    private int contadorId = 1; // Para asegurar que el ID es único y lo crea la app

    [Header("Referencias UI")]
    public TMP_InputField inputNombre;
    public TMP_InputField inputEdad;
    public TextMeshProUGUI textoNotificaciones; // Para dar feedback temporal

    public void RegistrarNuevoUsuario()
    {
        if (string.IsNullOrEmpty(inputNombre.text) || string.IsNullOrEmpty(inputEdad.text))
        {
            MostrarNotificacion("Error: Datos incompletos");
            return;
        }

        if (int.TryParse(inputEdad.text, out int edadValida))
        {
            Jugador nuevoJugador = new Jugador(contadorId, inputNombre.text, edadValida);
            usuarios.Add(contadorId, nuevoJugador);

            MostrarNotificacion($"Usuario {inputNombre.text} añadido con ID {contadorId}");
            contadorId++; // Preparar el siguiente ID único
        }
        else
        {
            MostrarNotificacion("Error: La edad debe ser un número");
        }
    }

    private void MostrarNotificacion(string mensaje)
    {
        StartCoroutine(RutinaNotificacion(mensaje));
    }

    private IEnumerator RutinaNotificacion(string mensaje)
    {
        textoNotificaciones.text = mensaje;
        yield return new WaitForSeconds(2.5f); // Mostrar durante 2.5 segundos
        textoNotificaciones.text = "";
    }
}