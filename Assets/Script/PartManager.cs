using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartManager : MonoBehaviour
{

    public Part[] parts;
    Dictionary<int, Part> partDict = new Dictionary<int, Part>();
    int playId=0;
    // Start is called before the first frame update
    private void Awake()
    {
       
    }

    void PlayInOrder(int id)
    {
        if (!partDict.ContainsKey(id)) return;
        if (partDict[id].isLoad)
        {
            if (partDict.ContainsKey(id+1))
            {
                partDict[id+1].gameObject.SetActive(true);
            }
        }
        partDict[id].gameObject.SetActive(true);
        partDict[id].RunAni(OnPartEnd);
    }

    void OnPartEnd(int id)
    {
        playId=id+1;
        if (playId < parts.Length)
        {
            PlayInOrder(playId);
        }
    }

    void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].PartID = i;
            partDict.Add(i, parts[i]);
        }
        PlayInOrder(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
