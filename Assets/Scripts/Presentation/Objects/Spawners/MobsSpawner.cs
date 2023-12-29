using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MobsSpawner : MonoBehaviour, IMobSpawner
{
    [Inject] private HunterFactory _factory;
    public void SpawnAtLocation(HunterType hunterType, HatType hat, Vector3 location)
    {
        var hunter = _factory.SpawnAIHunter(hunterType);
        hunter.SetHat(hat);
        StartCoroutine(SetPosition(hunter, location));
        //SetPosition(hunter, location);
    }

    private IEnumerator SetPosition(Hunter hunter, Vector3 position)
    {
        hunter.GetComponent<Rigidbody>().isKinematic = true;
        hunter.gameObject.transform.position = position;
        yield return new WaitForEndOfFrame();
        hunter.GetComponent<Rigidbody>().isKinematic = false;
    }

    /*private void SetPosition(Hunter hunter, Vector3 position)
    {
        hunter.GetComponent<Rigidbody>().isKinematic = true;
        hunter.gameObject.transform.position = position;
        hunter.GetComponent<Rigidbody>().isKinematic = false;
    }*/
}
