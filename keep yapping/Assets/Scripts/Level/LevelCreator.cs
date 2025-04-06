using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public string seed;
    public GameObject[] chunk;

    private Vector3 spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = Vector3.zero;
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateLevel()
    {
        for(int i=0;i<seed.Length;i++)
        {
            int chunkIndex = int.Parse(seed[i].ToString());
            GameObject currentChunk = Instantiate(chunk[chunkIndex]);

            Transform entryPoint = currentChunk.transform.Find("ConnectionPoint");
            Vector3 offset = currentChunk.transform.position - entryPoint.position;

            currentChunk.transform.position = spawnPoint + offset;

            spawnPoint = currentChunk.transform.Find("NextChunkSpawnPoint").position;
        }
    }
}
