using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CSVWriter
{
    public void WriteToCSV(string fileName, List<string[]> data)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
        {
            foreach (string[] row in data)
            {
                writer.WriteLine(string.Join(",", row));
            }

            //Application.OpenURL(filePath);
            Debug.Log("End");
        }
    }
}
