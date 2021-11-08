using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    private float chunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    [SerializeField] private float firstChunkSpawnPosition = -10;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float despawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        ResetWorld();
    }

    private void Start()
    {
        if (chunkPrefab.Count == 0)
        {
            Debug.LogError("No chunk prefab found  on the world generation, please assign some chunks");
            return;
        }
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("We have assigned ameraTransform automaticly to the camera.main ");
        }
    }

    public void ScanPosition()
    {
        float cameraZ = cameraTransform.transform.position.z;
        Chunk lastChunk = activeChunks.Peek();

        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLenght + despawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }
    private void SpawnNewChunk()
    {
        int randomIndex = Random.Range(0, chunkPrefab.Count);
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == chunkPrefab[randomIndex].name + "Clone");

        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();      
        }
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLenght;

        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }
    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }
    public void ResetWorld()
    {
        chunkSpawnZ = firstChunkSpawnPosition;

        for(int i = activeChunks.Count ; i!=0; i-- )
        {
            DeleteLastChunk();
        }
        for (int i = 0; i < chunkOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }
}
