using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _chunkPrefabs;

    [SerializeField]
    private GameObject _startChunk;

    [SerializeField]
    private GameObject _backgroundPrefab;

    private List<ChunkScript> _chunks = new List<ChunkScript>();
    private List<ChunkScript> _backgrounds = new List<ChunkScript>();

    // Use this for initialization
    void Start()
    {
        _chunks.Add(_startChunk.GetComponent<ChunkScript>());

        GameObject newBackground = Instantiate<GameObject>(_backgroundPrefab, this.transform);
        ChunkScript bgScript = newBackground.GetComponent<ChunkScript>();
        _backgrounds.Add(bgScript);
    }

    // Update is called once per frame
    void Update()
    {
        //**********************************************
        // chunk spawning

        Camera camera = Camera.main;

        float cameraUpperYPos = camera.transform.position.y + camera.orthographicSize;
        float chunksUpperYPos = _chunks[_chunks.Count - 1].TopPosition.y;

        if (cameraUpperYPos + 1 >= chunksUpperYPos)
        {
            int randomIndex = Random.Range(0, _chunkPrefabs.Count - 1);
            GameObject newChunk = Instantiate<GameObject>(_chunkPrefabs[randomIndex], this.transform);
            ChunkScript newChunkScript = newChunk.GetComponent<ChunkScript>();

            float newChunkHalfHeight = Mathf.Abs(newChunkScript.BottomPosition.y - newChunkScript.transform.position.y);
            newChunk.transform.position = new Vector3(0, chunksUpperYPos + newChunkHalfHeight);

            _chunks.Add(newChunkScript);
        }

        //**********************************************
        // bg spawning

        float bgsUpperYPos = _backgrounds[_backgrounds.Count - 1].TopPosition.y;

        if (cameraUpperYPos + 1 >= bgsUpperYPos)
        {
            GameObject newBackground = Instantiate<GameObject>(_backgroundPrefab, this.transform);
            ChunkScript bgScript = newBackground.GetComponent<ChunkScript>();

            float newBackgroundHalfHeight = Mathf.Abs(bgScript.BottomPosition.y - bgScript.transform.position.y);
            newBackground.transform.position = new Vector3(0, bgsUpperYPos + newBackgroundHalfHeight);

            _backgrounds.Add(bgScript);
        }

        //**********************************************
        // chunk despawning

        ChunkScript oldestChunk = _chunks[0];
        float cameraBottomYPos = camera.transform.position.y - camera.orthographicSize;
        float oldestChunkTopYPos = oldestChunk.TopPosition.y;

        if (cameraBottomYPos > oldestChunkTopYPos)
        {
            _chunks.Remove(oldestChunk);
            Destroy(oldestChunk.gameObject);
        }

        //**********************************************
        // bg despawning

        ChunkScript oldestBg = _backgrounds[0];
        float oldestBgTopYPos = oldestBg.TopPosition.y;

        if (cameraBottomYPos > oldestBgTopYPos)
        {
            _backgrounds.Remove(oldestBg);
            Destroy(oldestBg.gameObject);
        }
    }
}
