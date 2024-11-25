using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization; // Needed for float parsing
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    // Path to the CSV file
    private string filePath = "C:\\Users\\arajko2021\\Documents\\REU\\xyz_data.csv";

    // Reference to the sphere GameObject
    public GameObject sphere;

    // Speed at which the sphere moves
    public float moveSpeed = 1.0f;

    void Start()
    {
        StartCoroutine(ReadCSVAndMoveSphere(filePath));
    }

    IEnumerator ReadCSVAndMoveSphere(string path)
    {
        List<Vector3> positions = new List<Vector3>();

        try
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(path);

            // Log the total number of lines read
            Debug.Log($"Total lines read: {lines.Length}");

            // Iterate through each line
            foreach (string line in lines)
            {
                // Split the line by comma to get each field
                string[] fields = line.Split(',');

                // Log the fields in the current line
                //Debug.Log($"Processing line: {line}");

                // Check if all fields are zero
                bool allZeros = true;
                foreach (string field in fields)
                {
                    if (field != "0" && field != "0.0" && field != "-0.0")
                    {
                        allZeros = false;
                        break;
                    }
                }

                // Skip the line if all fields are zero
                if (allZeros)
                {
                    //Debug.Log("Skipping line with all zero values.");
                    continue;
                }

                // Parse the XYZ coordinates
                float x, y, z;
                if (float.TryParse(fields[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x) &&
                    float.TryParse(fields[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y) &&
                    float.TryParse(fields[2], NumberStyles.Float, CultureInfo.InvariantCulture, out z))
                {
                    // Add the position to the list
                    positions.Add(new Vector3(x, y, z));
                }
                else
                {
                    //Debug.LogWarning($"Skipping line with invalid format: {line}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }

        // Move the sphere to each position
        foreach (Vector3 position in positions)
        {
            yield return StartCoroutine(MoveSphereToPosition(position));
        }
    }

    IEnumerator MoveSphereToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(sphere.transform.localPosition, targetPosition) > 0.01f)
        {
            sphere.transform.localPosition = Vector3.MoveTowards(sphere.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1); // Wait for 1 second before moving to the next position
    }
}
