using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

public class Registrar : MonoBehaviour
{
    private Dictionary<int, Jugador> usuarios = new Dictionary<int, Jugador>();
    private int contadorId = 1;

    [Header("Añadir Usuario")]
    public TMP_InputField inputNombre;
    public TMP_InputField inputEdad;

    [Header("Búsqueda y Borrado")]
    public TMP_InputField inputIDGeneral;

    [Header("Listado y Feedback")]
    public TextMeshProUGUI textoListado;
    public TextMeshProUGUI textoNotificaciones;
    public Slider sliderBorrado;

    public void RegistrarNuevoUsuario()
    {
        if (string.IsNullOrEmpty(inputNombre.text) || string.IsNullOrEmpty(inputEdad.text))
        {
            MostrarNotificacion("Error: Rellena nombre y edad");
            return;
        }

        if (int.TryParse(inputEdad.text, out int edadValida) && edadValida >= 0)
        {
            Jugador nuevoJugador = new Jugador(contadorId, inputNombre.text, edadValida);
            usuarios.Add(contadorId, nuevoJugador);
            MostrarNotificacion($"Usuario añadido con ID: {contadorId}");
            contadorId++;
            inputNombre.text = "";
            inputEdad.text = "";
            MostrarListado();
        }
        else
        {
            MostrarNotificacion("Error: Edad no válida");
        }
    }

    public void BuscarUsuarioPorID()
    {
        if (int.TryParse(inputIDGeneral.text, out int idBusqueda) && usuarios.TryGetValue(idBusqueda, out Jugador j))
        {
            MostrarNotificacion($"Encontrado: {j.nombre} ({j.edad} años)");
        }
        else
        {
            MostrarNotificacion("Error: ID no encontrado o inválido");
        }
    }

    public void MostrarUsuariosMayorEdad()
    {
        if (usuarios.Count == 0)
        {
            MostrarNotificacion("No hay usuarios");
            return;
        }

        int edadMaxima = -1;
        List<string> nombres = new List<string>();

        foreach (var kvp in usuarios)
        {
            if (kvp.Value.edad > edadMaxima)
            {
                edadMaxima = kvp.Value.edad;
                nombres.Clear();
                nombres.Add(kvp.Value.nombre);
            }
            else if (kvp.Value.edad == edadMaxima)
            {
                nombres.Add(kvp.Value.nombre);
            }
        }
        MostrarNotificacion($"Mayor(es) ({edadMaxima} años): {string.Join(", ", nombres)}");
    }

    public void IniciarBorrado()
    {
        if (int.TryParse(inputIDGeneral.text, out int idBorrar) && usuarios.ContainsKey(idBorrar))
        {
            StartCoroutine(RutinaBorrarUsuario(idBorrar));
        }
        else
        {
            MostrarNotificacion("Error: ID a borrar no existe");
        }
    }

    private void MostrarListado()
    {
        textoListado.text = "LISTA DE USUARIOS:\n";
        foreach (var kvp in usuarios)
        {
            textoListado.text += $"ID: {kvp.Key} | {kvp.Value.nombre} | {kvp.Value.edad} años\n";
        }
    }

    private IEnumerator RutinaBorrarUsuario(int id)
    {
        sliderBorrado.gameObject.SetActive(true);
        sliderBorrado.value = 0;
        float tiempo = 0;

        while (tiempo < 3f)
        {
            tiempo += Time.deltaTime;
            sliderBorrado.value = tiempo / 3f;
            yield return null;
        }

        usuarios.Remove(id);
        sliderBorrado.gameObject.SetActive(false);
        MostrarNotificacion($"Usuario {id} eliminado");
        MostrarListado();
    }

    private void MostrarNotificacion(string mensaje)
    {
        StopAllCoroutines();
        StartCoroutine(RutinaNotificacion(mensaje));
    }

    private IEnumerator RutinaNotificacion(string mensaje)
    {
        textoNotificaciones.text = mensaje;
        yield return new WaitForSeconds(3f);
        textoNotificaciones.text = "";
    }
}