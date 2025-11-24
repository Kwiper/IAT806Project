using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public float[] position = new float[3];
    public float[] rotation = new float[4]; 

    public ObjectData(Grabbable grabbable)
    {
        Vector3 objectPos = grabbable.transform.position;

        Quaternion objectRot = grabbable.transform.rotation;

        position = new float[]
        {
            objectPos.x, objectPos.y, objectPos.z
        };

        rotation = new float[]
        {
            objectRot.x, objectRot.y, objectRot.z, objectRot.w
        }; 
    }
}
