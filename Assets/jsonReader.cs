/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader : MonoBehaviour
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

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    // Specify the path to the JSON file
    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    void Start()
    {
        LoadJson();
        CreateOrbs();
        StartCoroutine(MoveOrbs());
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

    void CreateOrbs()
    {
        if (orbDataList != null)
        {
            for (int i = 0; i < orbDataList.orbs.Count && i < 20; i++)
            {
                var data = orbDataList.orbs[i];
                Vector3 initialPosition = new Vector3(data.initial_position.x, data.initial_position.y, data.initial_position.z);

                if (orbs.ContainsKey(data.id))
                {
                    // Update the position of the existing orb
                    orbs[data.id].transform.position = initialPosition;
                    Debug.Log($"Orb with ID {data.id} updated to position {initialPosition}.");
                }
                else
                {
                    // Create a new orb
                    GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    orb.transform.position = initialPosition;
                    orbs.Add(data.id, orb);
                    Debug.Log($"Orb with ID {data.id} created at position {initialPosition}.");
                }
            }
        }
        else
        {
            Debug.LogError("Orb data list is null.");
        }
    }

    IEnumerator MoveOrbs()
    {
        while (true)
        {
            if (orbDataList != null)
            {
                for (int i = 0; i < orbDataList.orbs.Count && i < 20; i++)
                {
                    var data = orbDataList.orbs[i];
                    if (orbs.ContainsKey(data.id))
                    {
                        Vector3 newPosition = new Vector3(data.position.x, data.position.y, data.position.z);
                        StartCoroutine(MoveOrbToPosition(orbs[data.id], newPosition, 1f));
                        yield return new WaitForSeconds(1f); // Wait for 1 second before moving the next orb
                    }
                    else
                    {
                        Debug.LogError($"Orb with ID {data.id} not found in dictionary.");
                    }
                }
            }
            else
            {
                Debug.LogError("Orb data list is null in MoveOrbs.");
            }
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            orb.transform.position = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        orb.transform.position = newPosition;
        Debug.Log($"Orb moved to new position {newPosition}.");
    }
}*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader : MonoBehaviour
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

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;

    // Specify the path to the JSON file
    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    // Public variable to assign the parent GameObject in the Inspector
    public GameObject parentObject;

    void Start()
    {
        LoadJson();
        CreateOrbs();
        StartCoroutine(MoveOrbs());
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

    void CreateOrbs()
    {
        if (orbDataList != null && parentObject != null)
        {
            for (int i = 0; i < orbDataList.orbs.Count && i < 30; i++)
            {
                var data = orbDataList.orbs[i];
                //Vector3 initialPosition = new Vector3(data.initial_position.x, data.initial_position.z, -data.initial_position.y);
                Vector3 initialPosition = new Vector3(data.initial_position.x/2.0f, 1.0f, -data.initial_position.y/2.0f);

                if (orbs.ContainsKey(data.id))
                {
                    // Update the position of the existing orb relative to the parent object
                    orbs[data.id].transform.localPosition = initialPosition;
                    Debug.Log($"Orb with ID {data.id} updated to position {initialPosition} relative to {parentObject.name}.");
                }
                else
                {
                    // Create a new orb
                    GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    orb.transform.SetParent(parentObject.transform); // Set the parent of the orb
                    orb.transform.localPosition = initialPosition; // Set the local position relative to the parent
                    orb.gameObject.name = data.id.ToString();
                    orbs.Add(data.id, orb);
                    Debug.Log($"Orb with ID {data.id} created at position {initialPosition} relative to {parentObject.name}.");
                }
            }
        }
        else
        {
            Debug.LogError("Orb data list or parent object is null.");
        }
    }

    IEnumerator MoveOrbs()
    {
        while (true)
        {
            if (orbDataList != null)
            {
                for (int i = 0; i < orbDataList.orbs.Count && i < 20; i++)
                {
                    var data = orbDataList.orbs[i];
                    if (orbs.ContainsKey(data.id))
                    {
                        //Vector3 newPosition = new Vector3(data.position.x, data.position.z, -data.position.y);
                        Vector3 newPosition = new Vector3(data.position.x/2.0f, 1.0f, -data.position.y/2.0f);
                        StartCoroutine(MoveOrbToPosition(orbs[data.id], newPosition, 1f));
                        yield return new WaitForSeconds(1f); // Wait for 1 second before moving the next orb
                    }
                    else
                    {
                        Debug.LogError($"Orb with ID {data.id} not found in dictionary.");
                    }
                }
            }
            else
            {
                Debug.LogError("Orb data list is null in MoveOrbs.");
            }
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        orb.transform.localPosition = newPosition;
        Debug.Log($"Orb moved to new position {newPosition} relative to {parentObject.name}.");
    }
}*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader : MonoBehaviour
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

    private GameObject specificOrb;
    public GameObject object2; // Assign this in the inspector

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;
    private Dictionary<int, int> orbGroups = new Dictionary<int, int>();

    // Specify the path to the JSON file
    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    // Public variable to assign the parent GameObject in the Inspector
    public GameObject parentObject;

    // Threshold distance to determine if orbs are too close
    private float mergeDistance = 6.0f;

    void Start()
    {
        LoadJson();
        CreateOrbs();
        StartCoroutine(MoveOrbs());
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

    void CreateOrbs()
    {
        if (orbDataList != null && parentObject != null)
        {
            for (int i = 0; i < orbDataList.orbs.Count && i < 300; i++)
            {
                var data = orbDataList.orbs[i];
                Vector3 initialPosition = new Vector3(-data.initial_position.x, 1f, -data.initial_position.y);

                bool merged = false;

                foreach (var orb in orbs)
                {
                    if (Vector3.Distance(orb.Value.transform.localPosition, initialPosition) < mergeDistance)
                    {
                        // Merge the orbs by updating the existing orb's position to the average position
                        orb.Value.transform.localPosition = (orb.Value.transform.localPosition + initialPosition) / 2;
                        orbGroups[data.id] = orb.Key;
                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    if (orbs.ContainsKey(data.id))
                    {
                        // Update the position of the existing orb relative to the parent object
                        orbs[data.id].transform.localPosition = initialPosition;
                    }
                    else
                    {
                        // Create a new orb
                        GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        orb.transform.SetParent(parentObject.transform); // Set the parent of the orb
                        orb.transform.localPosition = initialPosition; // Set the local position relative to the parent
                        orb.gameObject.name = data.id.ToString();
                        orbs.Add(data.id, orb);
                        orbGroups[data.id] = data.id;

*//*                        // Check if this is the specific orb we're interested in
                        if (data.id == 590259)
                        {
                            specificOrb = orb;
                        }*//*
                    }
                }
            }
        }
    }

    IEnumerator MoveOrbs()
    {
        while (true)
        {
            if (orbDataList != null)
            {
                int totalOrbs = orbDataList.orbs.Count;
                for (int i = 0; i < totalOrbs; i += 25)
                {
                    List<Coroutine> activeCoroutines = new List<Coroutine>();

                    for (int j = i; j < i + 25 && j < totalOrbs; j++)
                    {
                        var data = orbDataList.orbs[j];
                        int groupId;
                        if (orbGroups.TryGetValue(data.id, out groupId) && orbs.ContainsKey(groupId))
                        {
                            Vector3 newPosition = new Vector3(-data.position.x, 1.0f, -data.position.y);
                            Coroutine moveCoroutine = StartCoroutine(MoveOrbToPosition(orbs[groupId], newPosition, 1f));
                            activeCoroutines.Add(moveCoroutine);
                        }
                        else
                        {
                            // Despawn the orb if it is not found
                            if (orbs.ContainsKey(data.id))
                            {
                                Destroy(orbs[data.id]);
                                orbs.Remove(data.id);
                                orbGroups.Remove(data.id);
                                Debug.Log($"Orb with ID {data.id} despawned.");
                            }
                        }
                    }

                    // Wait for all coroutines in the batch to finish
                    foreach (var coroutine in activeCoroutines)
                    {
                        yield return coroutine;
                    }
                }

*//*                // Calculate and print the distance for the specific orb
                if (specificOrb != null && object2 != null)
                {
                    float distance = Vector3.Distance(specificOrb.transform.position, object2.transform.position);
                    Debug.Log($"Distance between orb 590259 and sensor is {distance}");
                }*//*
            }
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        orb.transform.localPosition = newPosition;
    }
}*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader : MonoBehaviour
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

    private GameObject specificOrb;
    public GameObject object2; // Assign this in the inspector

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;
    private Dictionary<int, int> orbGroups = new Dictionary<int, int>();

    // Specify the path to the JSON file
    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    // Public variable to assign the parent GameObject in the Inspector
    public GameObject parentObject;

    // Public variable to assign the prefab in the Inspector
    public GameObject orbPrefab;

    // Threshold distance to determine if orbs are too close
    private float mergeDistance = 6.0f;

    void Start()
    {
        LoadJson();
        CreateOrbs();
        StartCoroutine(MoveOrbs());
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

    void CreateOrbs()
    {
        if (orbDataList != null && parentObject != null && orbPrefab != null)
        {
            for (int i = 0; i < orbDataList.orbs.Count && i < 1000; i++)
            {
                var data = orbDataList.orbs[i];
                Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);

                bool merged = false;

                foreach (var orb in orbs)
                {
                    if (Vector3.Distance(orb.Value.transform.localPosition, initialPosition) < mergeDistance)
                    {
                        // Merge the orbs by updating the existing orb's position to the average position
                        orb.Value.transform.localPosition = (orb.Value.transform.localPosition + initialPosition) / 2;
                        orbGroups[data.id] = orb.Key;
                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    if (orbs.ContainsKey(data.id))
                    {
                        // Update the position of the existing orb relative to the parent object
                        orbs[data.id].transform.localPosition = initialPosition;
                    }
                    else
                    {
                        // Create a new orb from the prefab
                        GameObject orb = Instantiate(orbPrefab, parentObject.transform);
                        orb.transform.localPosition = initialPosition; // Set the local position relative to the parent
                        orb.gameObject.name = data.id.ToString();
                        orbs.Add(data.id, orb);
                        orbGroups[data.id] = data.id;

                        // Check if this is the specific orb we're interested in
                        if (data.id == 590259)
                        {
                            specificOrb = orb;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Orb data list, parent object, or orb prefab is null.");
        }
    }

    IEnumerator MoveOrbs()
    {
        while (true)
        {
            if (orbDataList != null)
            {
                int totalOrbs = orbDataList.orbs.Count;
                for (int i = 0; i < totalOrbs; i += 10)
                {
                    List<Coroutine> activeCoroutines = new List<Coroutine>();

                    for (int j = i; j < i + 10 && j < totalOrbs; j++)
                    {
                        var data = orbDataList.orbs[j];
                        int groupId;
                        if (orbGroups.TryGetValue(data.id, out groupId) && orbs.ContainsKey(groupId))
                        {
                            Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                            Coroutine moveCoroutine = StartCoroutine(MoveOrbToPosition(orbs[groupId], newPosition, 0.1f));
                            activeCoroutines.Add(moveCoroutine);
                        }
                        else
                        {
                            // Despawn the orb if it is not found
                            if (orbs.ContainsKey(data.id))
                            {
                                Destroy(orbs[data.id]);
                                orbs.Remove(data.id);
                                orbGroups.Remove(data.id);
                                Debug.Log($"Orb with ID {data.id} despawned.");
                            }
                        }
                    }

                    // Wait for all coroutines in the batch to finish
                    foreach (var coroutine in activeCoroutines)
                    {
                        yield return coroutine;
                    }
                }

                // Calculate and print the distance for the specific orb
                if (specificOrb != null && object2 != null)
                {
                    float distance = Vector3.Distance(specificOrb.transform.position, object2.transform.position);
                    Debug.Log($"Distance between orb 590259 and sensor is {distance}");
                }
            }
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        // Calculate the direction and set the initial rotation
        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            // Continuously update the rotation to face the movement direction
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

public class jsonReader : MonoBehaviour
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

    private GameObject specificOrb;
    public GameObject object2; // Assign this in the inspector

    private Dictionary<int, GameObject> orbs = new Dictionary<int, GameObject>();
    private OrbDataList orbDataList;
    private Dictionary<int, int> orbGroups = new Dictionary<int, int>();

    // Specify the path to the JSON file
    private string jsonFilePath = @"C:\Users\arajko2021\Documents\REU\unknowns.json";

    // Public variable to assign the parent GameObject in the Inspector
    public GameObject parentObject;

    // Public variable to assign the prefab in the Inspector
    public GameObject orbPrefab;

    // Threshold distance to determine if orbs are too close
    private float mergeDistance = 6.0f;

    // Limit the number of JSON entries to read
    private int jsonEntryLimit = 100000;

    // Limit the number of orbs on the screen at the same time
    private int maxActiveOrbs = 10;

    void Start()
    {
        LoadJson();
        CreateOrbs();
        StartCoroutine(MoveOrbs());
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

    void CreateOrbs()
    {
        int activeOrbCount = 0;

        if (orbDataList != null && parentObject != null && orbPrefab != null)
        {
            for (int i = 0; i < orbDataList.orbs.Count && i < jsonEntryLimit; i++)
            {
                if (activeOrbCount >= maxActiveOrbs)
                {
                    break; // Stop creating orbs if the maximum active orb limit is reached
                }

                var data = orbDataList.orbs[i];
                Vector3 initialPosition = new Vector3(-data.initial_position.x, -1f, -data.initial_position.y);

                bool merged = false;

                foreach (var orb in orbs)
                {
                    if (Vector3.Distance(orb.Value.transform.localPosition, initialPosition) < mergeDistance)
                    {
                        // Merge the orbs by updating the existing orb's position to the average position
                        orb.Value.transform.localPosition = (orb.Value.transform.localPosition + initialPosition) / 2;
                        orbGroups[data.id] = orb.Key;
                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    if (orbs.ContainsKey(data.id))
                    {
                        // Update the position of the existing orb relative to the parent object
                        orbs[data.id].transform.localPosition = initialPosition;
                    }
                    else
                    {
                        // Create a new orb from the prefab
                        GameObject orb = Instantiate(orbPrefab, parentObject.transform);
                        orb.transform.localPosition = initialPosition; // Set the local position relative to the parent
                        orb.gameObject.name = data.id.ToString();
                        orbs.Add(data.id, orb);
                        orbGroups[data.id] = data.id;
                        activeOrbCount++; // Increment the active orb count

                        // Check if this is the specific orb we're interested in
                        if (data.id == 590259)
                        {
                            specificOrb = orb;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Orb data list, parent object, or orb prefab is null.");
        }
    }

    IEnumerator MoveOrbs()
    {
        while (true)
        {
            if (orbDataList != null)
            {
                int totalOrbs = Mathf.Min(orbDataList.orbs.Count, jsonEntryLimit); // Ensure we only process up to the jsonEntryLimit
                for (int i = 0; i < totalOrbs; i += 10) // Change the increment to adjust batch size
                {
                    List<Coroutine> activeCoroutines = new List<Coroutine>();

                    for (int j = i; j < i + 10 && j < totalOrbs; j++) // Change 10 to your desired batch size
                    {
                        var data = orbDataList.orbs[j];
                        int groupId;
                        if (orbGroups.TryGetValue(data.id, out groupId) && orbs.ContainsKey(groupId))
                        {
                            Vector3 newPosition = new Vector3(-data.position.x, -1f, -data.position.y);
                            Coroutine moveCoroutine = StartCoroutine(MoveOrbToPosition(orbs[groupId], newPosition, 0.1f)); // Reduced duration for faster movement
                            activeCoroutines.Add(moveCoroutine);
                        }
                        else
                        {
                            // Despawn the orb if it is not found
                            if (orbs.ContainsKey(data.id))
                            {
                                Destroy(orbs[data.id]);
                                orbs.Remove(data.id);
                                orbGroups.Remove(data.id);
                                Debug.Log($"Orb with ID {data.id} despawned.");
                            }
                        }
                    }

                    // Wait for all coroutines in the batch to finish
                    foreach (var coroutine in activeCoroutines)
                    {
                        yield return coroutine;
                    }
                }

                // Calculate and print the distance for the specific orb
                if (specificOrb != null && object2 != null)
                {
                    float distance = Vector3.Distance(specificOrb.transform.position, object2.transform.position);
                    Debug.Log($"Distance between orb 590259 and sensor is {distance}");
                }
            }
        }
    }

    IEnumerator MoveOrbToPosition(GameObject orb, Vector3 newPosition, float duration)
    {
        Vector3 startPosition = orb.transform.localPosition;
        float elapsedTime = 0f;

        // Calculate the direction and set the initial rotation
        Vector3 direction = (newPosition - startPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        orb.transform.rotation = lookRotation;

        while (elapsedTime < duration)
        {
            orb.transform.localPosition = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            // Continuously update the rotation to face the movement direction
            direction = (newPosition - orb.transform.localPosition).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            orb.transform.rotation = Quaternion.Slerp(orb.transform.rotation, lookRotation, elapsedTime / duration);

            yield return null;
        }

        orb.transform.localPosition = newPosition;
        orb.transform.rotation = lookRotation;
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonReader : MonoBehaviour
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

    private int jsonEntryLimit = 10000;
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