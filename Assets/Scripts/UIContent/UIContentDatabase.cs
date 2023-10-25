using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class UIContentDatabase : MonoBehaviour
{
    public static TextAsset csvData; // 指向你导入的CSV文件的引用
    public static Dictionary<int, string> contentMap = new Dictionary<int, string>();

    void Start()
    {
        csvData = Resources.Load<TextAsset>("UIContent");
        if (csvData == null)
        {
            Debug.LogError("Failed to load UIContent.csv from Resources folder.");
            return;  
        }

        string[] lines = csvData.text.Split('\n');


        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrEmpty(line)) continue; 

            string[] fields = line.Split(',');
            if (fields.Length < 2) 
            {
                Debug.LogError($"Invalid line format in line {i + 1}: {line}");
                continue;  // Skip this line and proceed to the next
            }

            int key = int.Parse(fields[0].Trim());
            string value = fields[1].Trim();
            contentMap.Add(key, value);
        }
    }

    public static string GetContentById(int id)
    {
        return contentMap.ContainsKey(id) ? contentMap[id] : "Id not found";
    }
}
