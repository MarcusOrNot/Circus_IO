using System.Collections.Generic;
using UnityEngine;

public class LoaderSceneSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _hunterPrefubs;
    [SerializeField] private float _distanceStep = 1;
    // Start is called before the first frame update
    void Start()
    {
        GenPrefabs(5);
    }

    private void GenPrefabs(int prefabsCount)
    {
        for (int i=1; i<prefabsCount; i++)
        {
            //var prefab = _hunterPrefubs[Random.Range(0, _hunterPrefubs.Count)] ;
            MakeBubble(transform.position + Vector3.left * _distanceStep * i);
            MakeBubble(transform.position + Vector3.right * _distanceStep * i);


            /*var elem = Instantiate(prefab);
            //elem.GetComponent<BubbleForm>().SetHat(HatType.MINER);
            var dirVector = i % 2 == 0 ? Vector3.left : Vector3.right;
            elem.transform.position = transform.position+dirVector*(_distanceStep/2)*i;
            elem.transform.rotation = transform.rotation;
            var size = Random.Range(1.0f, 2.0f);
            elem.transform.localScale = new Vector3(size,size,size);*/
            
        }
    }

    private void MakeBubble(Vector3 position)
    {
        var prefab = _hunterPrefubs[Random.Range(0, _hunterPrefubs.Count)];
        var elem = Instantiate(prefab);
        //elem.GetComponent<BubbleForm>().SetHat(HatType.MINER);
        //var dirVector = i % 2 == 0 ? Vector3.left : Vector3.right;
        elem.transform.position = position;
        elem.transform.rotation = transform.rotation;
        var size = Random.Range(1.0f, 2.0f);
        elem.transform.localScale = new Vector3(size, size, size);
    }
}
