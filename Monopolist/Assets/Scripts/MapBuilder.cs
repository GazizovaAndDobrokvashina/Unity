using UnityEngine;

    public class MapBuilder: MonoBehaviour
    {
        public GameObject emptyStreet;
        public GameObject emptyPlayer;
        public GameObject emptyBot;

        void Start()
        {
            DBwork data = Camera.main.GetComponent<DBwork>();

            StreetPath[] pathForBuys = data.GetAllPaths();
            for(int i = 1; i< pathForBuys.Length; i++)
            {
                
                GameObject newStreetPath = Instantiate(emptyStreet) as GameObject;
                newStreetPath.name = "StreetPath" + i;
                BoxCollider coll = newStreetPath.GetComponent<BoxCollider>();
                coll.size = new Vector3(GetVectorLength(pathForBuys[i].end - pathForBuys[i].start), 2, 1);

                newStreetPath.AddComponent<StreetPath>();
                newStreetPath.GetComponent<StreetPath>().TakeData(pathForBuys[i]);
                newStreetPath.GetComponent<StreetPath>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<StreetPath>());

                newStreetPath.transform.rotation = Quaternion.Euler(0f, Angle(pathForBuys[i].start, pathForBuys[i].end) ,0f);
                newStreetPath.transform.position = GetCenter(pathForBuys[i].start, pathForBuys[i].end);


            }

            Player[] players = data.GetAllPlayers();
            
            GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
            newPlayer.GetComponent<Player>().GetData(players[1]);
            newPlayer.transform.position = players[1].getDestination();
            data.updatePlayer(newPlayer.GetComponent<Player>());
        
            
            for (int j = 2; j < players.Length; j++)
            {
                GameObject newBot = Instantiate(emptyBot) as GameObject;
                newBot.GetComponent<Player>().GetData(players[j]);
                newBot.transform.position = players[j].getDestination();
                data.updatePlayer(newBot.GetComponent<Player>());
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
