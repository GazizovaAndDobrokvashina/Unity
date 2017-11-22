using UnityEngine;

    public class MapBuilder: MonoBehaviour
    {
        public GameObject empty;

        void Start()
        {
            DBwork data = Camera.main.GetComponent<DBwork>();

            StreetPath[] pathForBuys = data.GetAllPaths();
            for(int i = 1; i< pathForBuys.Length; i++)
            {
                
                GameObject newStreetPath = Instantiate(empty) as GameObject;
                newStreetPath.name = "StreetPath" + i;
                BoxCollider coll = newStreetPath.GetComponent<BoxCollider>();
                coll.size = new Vector3(GetVectorLength(pathForBuys[i].end - pathForBuys[i].start), 2, 1);

                newStreetPath.AddComponent<StreetPath>();
                newStreetPath.GetComponent<StreetPath>().TakeData(pathForBuys[i]);

                newStreetPath.transform.rotation = Quaternion.Euler(0f, Angle(pathForBuys[i].start, pathForBuys[i].end) ,0f);
                newStreetPath.transform.position = GetCenter(pathForBuys[i].start, pathForBuys[i].end);


            }
        }

        public static float Angle(Vector3 start, Vector3 end)
        {
            float angle=Mathf.Atan2(end.z-start.z, end.x-start.x)*180/Mathf.PI;
            if(0.0f>angle)
                angle+=360.0f;
            return angle;
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
