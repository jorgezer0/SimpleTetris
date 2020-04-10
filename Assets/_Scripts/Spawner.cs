using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private List<int> next = new List<int>();
    [SerializeField] private Transform[] positions;
    
    // Start is called before the first frame update
    private void Start()
    {
        EventBus.OnCallNewBlock += OnCallNewBlock;
        
        SetNextBlocks();
    }

    private void OnDestroy()
    {
        EventBus.OnCallNewBlock -= OnCallNewBlock;
    }

    private void OnCallNewBlock(object sender, EventArgs e)
    {
        NewBlock(next[0]);
        next.Remove(next[0]);
        UpdateNext();
    }

    private void NewBlock(int block)
    {
        Instantiate(blocks[block], transform.position, Quaternion.identity);
    }

    private void SetNextBlocks()
    {
        foreach (var position in positions)
        {
            var randomBlock = Random.Range(0, blocks.Length);
            next.Add(randomBlock);
            position.GetChild(randomBlock).transform.localScale = Vector3.one * 0.75f;
            position.GetChild(randomBlock).gameObject.SetActive(true);
        }
        
        NewBlock(next[0]);
        next.Remove(next[0]);
        UpdateNext();
    }

    private void UpdateNext()
    {
        var randomBlock = Random.Range(0, blocks.Length);
        next.Add(randomBlock);
        for (var i = 0; i < next.Count; i++)
        {
            for (var j = 0; j < positions[i].childCount; j++)
            {
                positions[i].GetChild(j).gameObject.SetActive(false);
                if (j != next[i]) continue;
                var tempNext = positions[i].GetChild(next[i]);
                tempNext.localPosition = Vector3.zero;
                tempNext.localScale = Vector3.one * 0.75f;
                tempNext.gameObject.SetActive(true);
            }
        }
    }

}
