using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] Grabbable grabbablePrefab; // Since I'm just saving walls I can just keep it to one prefab type, it's impossible to delete the audio source and listener

    public static List<Grabbable> objects = new List<Grabbable>(); // Create a list of all the walls in the scene

    const string OBJECT_SUB = "/objects"; // filepath for individual objects
    const string OBJECT_COUNT_SUB = "/objects.count"; // filepath for object count

    public void Save() // public function for ui buttons to access
    {
        SaveObjects();
    }

    public void Load()
    {
        LoadObjects();
    }
    

    void SaveObjects()
    {
        BinaryFormatter formatter = new BinaryFormatter(); // Formatting save files in binary
        string path = Application.persistentDataPath + OBJECT_SUB + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + OBJECT_COUNT_SUB + SceneManager.GetActiveScene().buildIndex;

        FileStream countStream = new FileStream(countPath, FileMode.Create); // Create file for object count

        formatter.Serialize(countStream, objects.Count);
        countStream.Close();

        for (int i = 0; i < objects.Count; i++) // iterate through object list and create save data for each object in scene
        {
            FileStream stream = new FileStream(path + i, FileMode.Create);
            ObjectData data = new ObjectData(objects[i]);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    void LoadObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject); // Destroy everything in the list so that reloading doesn't create duplicates
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + OBJECT_SUB + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + OBJECT_COUNT_SUB + SceneManager.GetActiveScene().buildIndex;
        int objectCount = 0;


        if (File.Exists(countPath))
        {
            FileStream countStream = new FileStream(countPath, FileMode.Open); // First open the object count save file to get a count of objects to instantiate

            objectCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }
        else
        {
            Debug.LogError("File path not found in " + countPath);
        }

        for (int i = 0; i < objectCount; i++) // iterate through each file and load the objects back into the scene
        {
            if (File.Exists(path + i))
            {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                ObjectData data = formatter.Deserialize(stream) as ObjectData;

                stream.Close();

                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                Quaternion rotation = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);

                Grabbable grabbable = Instantiate(grabbablePrefab, position, rotation);
            }
            else
            {
                Debug.LogError("File path not found in " + path + i);
            }
        }
    }
}
