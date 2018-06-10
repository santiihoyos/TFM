using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CajaMunicionController : MonoBehaviour, IRecogible {
	
	private void Start()
	{
		StartCoroutine(DestruirDelayed());
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * 5f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			print("Recogida");
			var controlador = other.gameObject.GetComponent<ControladorJugador>();
			var armaARecargar = controlador.Armas[controlador.ArmaSeleccionada];
			var totalBalasACargar = armaARecargar.BalasPorCargador * 3;
			controlador.BalasDeArmasPoseidas[controlador.ArmaSeleccionada] = totalBalasACargar;
			Destroy(gameObject);
		}
	}

	public void Recoger()
	{
		
	}

	IEnumerator DestruirDelayed()
	{
		yield return new WaitForSeconds(20f);
		Destroy(gameObject);
	}
}
