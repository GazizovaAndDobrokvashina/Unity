using UnityEngine;

    public class MapBuilder: MonoBehaviour
    {
        public Transform empty;

        void Start()
        {
            DBwork data = Camera.main.GetComponent<DBwork>();
            
            foreach (StreetPath path in data.GetAllPaths())
            {
                Transform newStreetPath = Instantiate(empty);
                
                BoxCollider coll = newStreetPath.GetComponent<BoxCollider>();
                //coll.center = GetCenter(path.start, path.end);
                coll.size = new Vector3(GetVectorLength(path.end - path.start),2,1);

                newStreetPath.gameObject.AddComponent<StreetPath>();
                newStreetPath.GetComponent<StreetPath>().TakeData(path);

                newStreetPath.rotation = Quaternion.FromToRotation(path.start, path.end);
                newStreetPath.position = GetCenter(path.start, path.end);

            }
        }


        Vector3 GetCenter(Vector3 start, Vector3 end)
        {
            Vector3 vec = new Vector3(start.x +((end.x - start.x)/2), start.y +((end.y - start.y)/2), start.z +((end.z - start.z)/2) );

            return vec;
        }

        float GetVectorLength(Vector3 vector3)
        {
            
            return vector3.magnitude;
        }
    }
