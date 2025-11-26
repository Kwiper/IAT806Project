using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public float[] position = new float[3]; // Save position vector as a float array for data saving/loading
    public float[] rotation = new float[4]; // Save rotation quaternion as a float array for data saving/loading

    public ObjectData(Grabbable grabbable)
    {
        Vector3 objectPos = grabbable.transform.position; // store position
         
        Quaternion objectRot = grabbable.transform.rotation; // store rotation

        position = new float[]
        {
            objectPos.x, objectPos.y, objectPos.z // assign vector coordinates to float array
        };

        rotation = new float[]
        {
            objectRot.x, objectRot.y, objectRot.z, objectRot.w
        }; 
    }
}
