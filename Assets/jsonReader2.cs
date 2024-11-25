/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class OrbData
    {
        public int id;
        public Position initial_position;
        public Position position;
    }

    [System.Serializable]
    public class OrbDataList
    {
        public List<OrbData> orbs;
    }

    public GameObject object2; // Assign this in the inspector
    public GameObject parentObject;
    public GameObject orbPrefab;

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    //private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";
    private string jsonFilePath = @"C:\Users\Anton\Downloads\unknowns.json";


    private int jsonEntryLimit = 100000;
    private int currentChunkStart = 0;
    private HashSet<int> processedIDs = new HashSet<int>();

    void Start()
    {
        LoadJson();
        StartCoroutine(ProcessChunks());
    }

    void LoadJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            orbDataList = JsonUtility.FromJson<OrbDataList>("{\"orbs\":" + jsonString + "}");
            Debug.Log("JSON loaded and parsed successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + jsonFilePath);
        }
    }

    IEnumerator ProcessChunks()
    {
        while (currentChunkStart < orbDataList.orbs.Count)
        {
            int chunkEnd = Mathf.Min(currentChunkStart + jsonEntryLimit, orbDataList.orbs.Count);
            List<int> uniqueIDs = GetUniqueIDsInChunk(currentChunkStart, chunkEnd);

            foreach (int id in uniqueIDs)
            {
                yield return StartCoroutine(ProcessIDInChunk(id, currentChunkStart, chunkEnd));
            }

            currentChunkStart = chunkEnd;
        }
    }

    List<int> GetUniqueIDsInChunk(int start, int end)
    {
        HashSet<int> uniqueIDs = new HashSet<int>();
        for (int i = start; i < end; i++)
        {
            uniqueIDs.Add(orbDataList.orbs[i].id);
        }
        return new List<int>(uniqueIDs);
    }

    IEnumerator ProcessIDInChunk(int id, int start, int end)
    {
        List<OrbData> idDataList = new List<OrbData>();

        for (int i = start; i < end; i++)
        {
            if (orbDataList.orbs[i].id == id)
            {
                idDataList.Add(orbDataList.orbs[i]);
            }
        }

        if (idDataList.Count > 0)
        {
            CreateOrb(idDataList[0]);

            foreach (var data in idDataList)
            {
                Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                yield return StartCoroutine(MoveOrbToPosition(orbs[id], newPosition, 0.1f));
            }

            Destroy(orbs[id]);
            orbs.Remove(id);
            //Debug.Log("Orb Over");
        }
    }

    void CreateOrb(OrbData data)
    {
        if (!orbs.ContainsKey(data.id))
        {
            Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);
            GameObject orb = Instantiate(orbPrefab, parentObject.transform);
            orb.transform.localPosition = initialPosition;
            orb.gameObject.name = data.id.ToString();
            orbs[data.id] = orb;
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            direction = (newPosition - orb.transform.localPosition).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            orb.transform.rotation = Quaternion.Slerp(orb.transform.rotation, lookRotation, elapsedTime / duration);

            yield return null;
        }

        orb.transform.localPosition = newPosition;
        orb.transform.rotation = lookRotation;
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class OrbData
    {
        public int id;
        public Position initial_position;
        public Position position;
    }

    [System.Serializable]
    public class OrbDataList
    {
        public List<OrbData> orbs;
    }

    public GameObject object2; // Assign this in the inspector
    public GameObject parentObject;
    public GameObject orbPrefab;

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    private int jsonEntryLimit = 100000;
    private int currentChunkStart = 0;
    private HashSet<int> processedIDs = new HashSet<int>();

    private float movementThreshold = 5.0f; // Threshold for orb movement
    private int maxActiveOrbs = 2; // Maximum number of active orbs
    private int activeOrbCount = 0; // Current number of active orbs

    private float moveDuration = 2.0f; // Duration for the movement to slow down orbs

    void Start()
    {
        LoadJson();
        StartCoroutine(ProcessChunks());
    }

    void LoadJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            orbDataList = JsonUtility.FromJson<OrbDataList>("{\"orbs\":" + jsonString + "}");
            Debug.Log("JSON loaded and parsed successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + jsonFilePath);
        }
    }

    IEnumerator ProcessChunks()
    {
        while (currentChunkStart < orbDataList.orbs.Count)
        {
            int chunkEnd = Mathf.Min(currentChunkStart + jsonEntryLimit, orbDataList.orbs.Count);
            List<int> uniqueIDs = GetUniqueIDsInChunk(currentChunkStart, chunkEnd);

            foreach (int id in uniqueIDs)
            {
                if (activeOrbCount < maxActiveOrbs)
                {
                    StartCoroutine(ProcessIDInChunk(id, currentChunkStart, chunkEnd));
                }
                yield return null; // Allow other operations to run
            }

            currentChunkStart = chunkEnd;
            yield return null; // Allow other operations to run
        }
    }

    List<int> GetUniqueIDsInChunk(int start, int end)
    {
        HashSet<int> uniqueIDs = new HashSet<int>();
        for (int i = start; i < end; i++)
        {
            uniqueIDs.Add(orbDataList.orbs[i].id);
        }
        return new List<int>(uniqueIDs);
    }

    IEnumerator ProcessIDInChunk(int id, int start, int end)
    {
        List<OrbData> idDataList = new List<OrbData>();

        for (int i = start; i < end; i++)
        {
            if (orbDataList.orbs[i].id == id)
            {
                idDataList.Add(orbDataList.orbs[i]);
            }
        }

        if (idDataList.Count > 0)
        {
            Vector3 initialPosition = new Vector3(-idDataList[0].initial_position.x, -1f, -idDataList[0].initial_position.y);
            Vector3 worldPosition = parentObject.transform.TransformPoint(initialPosition);

            if (IsRoadUnderPosition(worldPosition) && HasMovedEnough(idDataList) && !IsPositionOccupied(worldPosition))
            {
                CreateOrb(idDataList[0]);

                foreach (var data in idDataList)
                {
                    Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                    StartCoroutine(MoveOrbToPosition(orbs[id], newPosition, moveDuration));
                }
            }
            else
            {
                Debug.Log($"Orb ID {id} did not move enough, has no road under it at position {initialPosition}, or the position is occupied");
            }
        }
        yield return null;
    }

    bool HasMovedEnough(List<OrbData> idDataList)
    {
        if (idDataList.Count < 2) return false;

        Vector3 startPosition = new Vector3(-idDataList[0].initial_position.x, -1f, -idDataList[0].initial_position.y);
        Vector3 endPosition = new Vector3(-idDataList[idDataList.Count - 1].position.x, -1f, -idDataList[idDataList.Count - 1].position.y);

        float distanceMoved = Vector3.Distance(startPosition, endPosition);

        Debug.Log($"Orb ID {idDataList[0].id} moved {distanceMoved} units");

        return distanceMoved > movementThreshold;
    }

    void CreateOrb(OrbData data)
    {
        if (!orbs.ContainsKey(data.id) && activeOrbCount < maxActiveOrbs)
        {
            Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);
            GameObject orb = Instantiate(orbPrefab, parentObject.transform);
            orb.transform.localPosition = initialPosition;
            orb.gameObject.name = data.id.ToString();
            orbs[data.id] = orb;
            activeOrbCount++; // Increase active orb count
            Debug.Log($"Orb with ID {data.id} created at position {initialPosition}");
        }
    }

    bool IsRoadUnderPosition(Vector3 position)
    {
        RaycastHit hit;
        Vector3 rayOrigin = position + Vector3.up * 10; // Start the ray 10 units above the position
        float rayLength = 20f; // Length of the ray

        //Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red, 5.0f);
        Debug.Log($"Casting ray from {rayOrigin} downwards with length {rayLength}");

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength))
        {
            Debug.Log($"Raycast hit {hit.collider.gameObject.name} at {hit.point} with tag {hit.collider.tag}");
            if (hit.collider.CompareTag("Road"))
            {
                Debug.Log($"Road found under position {position}");
                return true;
            }
            else
            {
                Debug.Log($"No road tag found under position {position}, found tag: {hit.collider.tag}");
            }
        }
        else
        {
            Debug.Log($"Raycast did not hit anything under position {position}");
        }
        return false;
    }

    bool IsPositionOccupied(Vector3 position)
    {
        foreach (var orb in orbs.Values)
        {
            if (Vector3.Distance(orb.transform.position, position) < 1.0f) // Adjust the distance threshold as needed
            {
                Debug.Log($"Position {position} is occupied by another orb");
                return true;
            }
        }
        return false;
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            direction = (newPosition - orb.transform.localPosition).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            orb.transform.rotation = Quaternion.Slerp(orb.transform.rotation, lookRotation, elapsedTime / duration);

            yield return null;
        }

        orb.transform.localPosition = newPosition;
        orb.transform.rotation = lookRotation;

        
        if (orbs.ContainsKey(int.Parse(orb.gameObject.name)))
        {
            orbs.Remove(int.Parse(orb.gameObject.name));
            Destroy(orb);
            activeOrbCount--; 
        }
    }
}






/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class OrbData
    {
        public int id;
        public Position initial_position;
        public Position position;
    }

    [System.Serializable]
    public class OrbDataList
    {
        public List<OrbData> orbs;
    }

    public GameObject object2; // Assign this in the inspector
    public GameObject parentObject;
    public GameObject orbPrefab;

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    private int jsonEntryLimit = 1000;
    private int currentChunkStart = 0;

    void Start()
    {
        LoadJson();
        StartCoroutine(ProcessChunks());
    }

    void LoadJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            orbDataList = JsonUtility.FromJson<OrbDataList>("{\"orbs\":" + jsonString + "}");
            Debug.Log("JSON loaded and parsed successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + jsonFilePath);
        }
    }

    IEnumerator ProcessChunks()
    {
        while (currentChunkStart < orbDataList.orbs.Count)
        {
            int chunkEnd = Mathf.Min(currentChunkStart + jsonEntryLimit, orbDataList.orbs.Count);
            List<int> uniqueIDs = GetUniqueIDsInChunk(currentChunkStart, chunkEnd);

            for (int i = 0; i < uniqueIDs.Count; i += 5)
            {
                List<int> currentIDs = uniqueIDs.GetRange(i, Mathf.Min(5, uniqueIDs.Count - i));
                yield return StartCoroutine(ProcessIDsInChunk(currentIDs, currentChunkStart, chunkEnd));
            }

            currentChunkStart = chunkEnd;
        }
    }

    List<int> GetUniqueIDsInChunk(int start, int end)
    {
        HashSet<int> uniqueIDs = new HashSet<int>();
        for (int i = start; i < end; i++)
        {
            uniqueIDs.Add(orbDataList.orbs[i].id);
        }
        return new List<int>(uniqueIDs);
    }

    IEnumerator ProcessIDsInChunk(List<int> ids, int start, int end)
    {
        List<List<OrbData>> idDataLists = new List<List<OrbData>>();
        foreach (int id in ids)
        {
            List<OrbData> idDataList = new List<OrbData>();
            for (int i = start; i < end; i++)
            {
                if (orbDataList.orbs[i].id == id)
                {
                    idDataList.Add(orbDataList.orbs[i]);
                }
            }
            idDataLists.Add(idDataList);
        }

        foreach (var idDataList in idDataLists)
        {
            if (idDataList.Count > 0)
            {
                CreateOrb(idDataList[0]);

                foreach (var data in idDataList)
                {
                    Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                    yield return StartCoroutine(MoveOrbToPosition(orbs[idDataList[0].id], newPosition, 0.1f));
                }

                Destroy(orbs[idDataList[0].id]);
                orbs.Remove(idDataList[0].id);
            }
        }
    }

    void CreateOrb(OrbData data)
    {
        if (!orbs.ContainsKey(data.id))
        {
            Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);
            GameObject orb = Instantiate(orbPrefab, parentObject.transform);
            orb.transform.localPosition = initialPosition;
            orb.gameObject.name = data.id.ToString();
            orbs[data.id] = orb;
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            direction = (newPosition - orb.transform.localPosition).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            orb.transform.rotation = Quaternion.Slerp(orb.transform.rotation, lookRotation, elapsedTime / duration);

            yield return null;
        }

        orb.transform.localPosition = newPosition;
        orb.transform.rotation = lookRotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class OrbData
    {
        public int id;
        public Position initial_position;
        public Position position;
    }

    [System.Serializable]
    public class OrbDataList
    {
        public List<OrbData> orbs;
    }

    public GameObject object2; // Assign this in the inspector
    public GameObject parentObject;
    public GameObject orbPrefab;

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    private int jsonEntryLimit = 1000;
    private int currentChunkStart = 0;

    void Start()
    {
        LoadJson();
        StartCoroutine(ProcessChunks());
    }

    void LoadJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            orbDataList = JsonUtility.FromJson<OrbDataList>("{\"orbs\":" + jsonString + "}");
            Debug.Log("JSON loaded and parsed successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + jsonFilePath);
        }
    }

    IEnumerator ProcessChunks()
    {
        while (currentChunkStart < orbDataList.orbs.Count)
        {
            int chunkEnd = Mathf.Min(currentChunkStart + jsonEntryLimit, orbDataList.orbs.Count);
            List<int> uniqueIDs = GetUniqueIDsInChunk(currentChunkStart, chunkEnd);

            for (int i = 0; i < uniqueIDs.Count; i += 5)
            {
                List<int> currentIDs = uniqueIDs.GetRange(i, Mathf.Min(5, uniqueIDs.Count - i));
                yield return StartCoroutine(ProcessIDsInChunk(currentIDs, currentChunkStart, chunkEnd));
            }

            currentChunkStart = chunkEnd;
        }
    }

    List<int> GetUniqueIDsInChunk(int start, int end)
    {
        HashSet<int> uniqueIDs = new HashSet<int>();
        for (int i = start; i < end; i++)
        {
            uniqueIDs.Add(orbDataList.orbs[i].id);
        }
        return new List<int>(uniqueIDs);
    }

    IEnumerator ProcessIDsInChunk(List<int> ids, int start, int end)
    {
        foreach (int id in ids)
        {
            List<OrbData> idDataList = new List<OrbData>();
            for (int i = start; i < end; i++)
            {
                if (orbDataList.orbs[i].id == id)
                {
                    idDataList.Add(orbDataList.orbs[i]);
                }
            }

            if (idDataList.Count > 0)
            {
                CreateOrb(idDataList[0]);

                foreach (var data in idDataList)
                {
                    Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                    yield return StartCoroutine(MoveOrbToPosition(orbs[id], newPosition, 0.1f));
                }

                Destroy(orbs[id]);
                orbs.Remove(id);
            }
        }
    }

    void CreateOrb(OrbData data)
    {
        if (!orbs.ContainsKey(data.id))
        {
            Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);
            GameObject orb = Instantiate(orbPrefab, parentObject.transform);
            orb.transform.localPosition = initialPosition;
            orb.gameObject.name = data.id.ToString();
            orbs[data.id] = orb;
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            direction = (newPosition - orb.transform.localPosition).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            orb.transform.rotation = Quaternion.Slerp(orb.transform.rotation, lookRotation, elapsedTime / duration);

            yield return null;
        }

        orb.transform.localPosition = newPosition;
        orb.transform.rotation = lookRotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class OrbData
    {
        public int id;
        public Position initial_position;
        public Position position;
    }

    [System.Serializable]
    public class OrbDataList
    {
        public List<OrbData> orbs;
    }

    public GameObject object2; // Assign this in the inspector
    public GameObject parentObject;
    public GameObject orbPrefab;

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    private int jsonEntryLimit = 1000;
    private int currentChunkStart = 0;

    void Start()
    {
        LoadJson();
        StartCoroutine(ProcessChunks());
    }

    void LoadJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            orbDataList = JsonUtility.FromJson<OrbDataList>("{\"orbs\":" + jsonString + "}");
            Debug.Log("JSON loaded and parsed successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + jsonFilePath);
        }
    }

    IEnumerator ProcessChunks()
    {
        while (currentChunkStart < orbDataList.orbs.Count)
        {
            int chunkEnd = Mathf.Min(currentChunkStart + jsonEntryLimit, orbDataList.orbs.Count);
            List<int> uniqueIDs = GetUniqueIDsInChunk(currentChunkStart, chunkEnd);

            for (int i = 0; i < uniqueIDs.Count; i += 5)
            {
                List<int> currentIDs = uniqueIDs.GetRange(i, Mathf.Min(5, uniqueIDs.Count - i));
                yield return StartCoroutine(ProcessIDsInChunk(currentIDs, currentChunkStart, chunkEnd));
            }

            currentChunkStart = chunkEnd;
        }
    }

    List<int> GetUniqueIDsInChunk(int start, int end)
    {
        HashSet<int> uniqueIDs = new HashSet<int>();
        for (int i = start; i < end; i++)
        {
            uniqueIDs.Add(orbDataList.orbs[i].id);
        }
        return new List<int>(uniqueIDs);
    }

    IEnumerator ProcessIDsInChunk(List<int> ids, int start, int end)
    {
        foreach (int id in ids)
        {
            List<OrbData> idDataList = new List<OrbData>();
            for (int i = start; i < end; i++)
            {
                if (orbDataList.orbs[i].id == id)
                {
                    idDataList.Add(orbDataList.orbs[i]);
                }
            }

            if (idDataList.Count > 0)
            {
                CreateOrb(idDataList[0]);

                foreach (var data in idDataList)
                {
                    Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                    yield return StartCoroutine(MoveOrbToPosition(orbs[id], newPosition, 0.1f));
                }

                Destroy(orbs[id]);
                orbs.Remove(id);
            }
        }
    }

    void CreateOrb(OrbData data)
    {
        if (!orbs.ContainsKey(data.id))
        {
            Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);
            GameObject orb = Instantiate(orbPrefab, parentObject.transform);
            orb.transform.localPosition = initialPosition;
            orb.gameObject.name = data.id.ToString();
            orbs[data.id] = orb;
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            direction = (newPosition - orb.transform.localPosition).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            orb.transform.rotation = Quaternion.Slerp(orb.transform.rotation, lookRotation, elapsedTime / duration);

            yield return null;
        }

        orb.transform.localPosition = newPosition;
        orb.transform.rotation = lookRotation;
    }
}
*/