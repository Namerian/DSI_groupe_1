using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _chunkPrefabs;

    [SerializeField]
    private GameObject _startChunk;

    private List<ChunkScript> _chunks = new List<ChunkScript>();

    // Use this for initialization
    void Start()
    {
        _chunks.Add(_startChunk.GetComponent<ChunkScript>());
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        ChunkScript upperChunkScript = _chunks[_chunks.Count - 1];

        float cameraUpperYPos = Camera.main.transform.position.y + Camera.main.orthographicSize;
        float chunksUpperYPos = upperChunkScript.TopPosition.y;

        if (cameraUpperYPos + 1 >= chunksUpperYPos)
        {
            int randomIndex = Random.Range(0, _chunkPrefabs.Count - 1);
            GameObject newChunk = Instantiate<GameObject>(_chunkPrefabs[randomIndex], this.transform);

            ChunkScript newChunkScript = newChunk.GetComponent<ChunkScript>();
            float newChunkHalfHeight = Mathf.Abs(newChunkScript.BottomPosition.y - newChunkScript.transform.position.y);

            newChunk.transform.position = new Vector3(0, chunksUpperYPos + newChunkHalfHeight);

            _chunks.Add(newChunkScript);
        }
    }
}
