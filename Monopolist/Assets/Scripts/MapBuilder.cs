using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    //префаб улицы
    public GameObject emptyStreet;

    //префаб игрока
    public GameObject emptyPlayer;

    //префаб бота
    public GameObject emptyBot;
    //префаб дома
    public GameObject emptyBuild;

    //создание и заполнение карты, основываясь на данных из базы данных
    void Start()
    {
        DBwork data = Camera.main.GetComponent<DBwork>();

        StreetPath[] pathForBuys = data.GetAllPaths();
        for (int i = 1; i < pathForBuys.Length; i++)
        {
            GameObject newStreetPath = Instantiate(emptyStreet) as GameObject;
            newStreetPath.name = "StreetPath" + i;
            BoxCollider coll = newStreetPath.GetComponent<BoxCollider>();
            coll.size = new Vector3(GetVectorLength(pathForBuys[i].end - pathForBuys[i].start), 3, 8);

            if (pathForBuys[i].canBuy)
            {
                newStreetPath.AddComponent<PathForBuy>();
                newStreetPath.GetComponent<PathForBuy>().TakeData(data.GetPathForBuy(i));
                newStreetPath.GetComponent<PathForBuy>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<PathForBuy>());
            }
            else
            {
                newStreetPath.AddComponent<GovermentPath>();
                newStreetPath.GetComponent<GovermentPath>().TakeData(data.GetGovermentPath(i));
                if (newStreetPath.GetComponent<GovermentPath>().GetNameOfPrefab().Equals("Court"))
                    data.SetCourt(newStreetPath.GetComponent<GovermentPath>());
                newStreetPath.GetComponent<GovermentPath>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<GovermentPath>());
            }
            newStreetPath.transform.rotation =
                Quaternion.Euler(0f, Angle(pathForBuys[i].start, pathForBuys[i].end), 0f);
            newStreetPath.transform.position = GetCenter(pathForBuys[i].start, pathForBuys[i].end);
        }

        Build[] builds = data.GetAllBuilds();

        for (int i = 1; i < builds.Length; i++)
        {
            GameObject newBuild = Instantiate(emptyBuild) as GameObject;
            newBuild.name = builds[i].NameBuild;

            newBuild.AddComponent<Build>();
            newBuild.GetComponent<Build>().TakeData(builds[i]);
            data.updateBuild(newBuild.GetComponent<Build>());

            newBuild.transform.rotation = data.GetPathById(builds[i].IdStreetPath).transform.rotation;
            newBuild.transform.position = newBuild.GetComponent<Build>().Place;
            newBuild.SetActive(newBuild.GetComponent<Build>().Enable);
        }

        Player[] players = data.GetAllPlayers();

        for (int j = 1; j < players.Length; j++)
        {
            if (!players[j].IsBot())
            {
                GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
                newPlayer.GetComponent<Player>().GetData(players[1]);
                newPlayer.transform.position = players[1].Destination;
                data.updatePlayer(newPlayer.GetComponent<Player>());transform.Find("/Town").GetComponent<Cameras>()
                    .SetCamera(newPlayer.GetComponentInChildren<Camera>());
            }
            else
            {
                GameObject newBot = Instantiate(emptyBot) as GameObject;
                newBot.GetComponent<Bot>().GetData(players[j]);
                newBot.transform.position = players[j].Destination;
                data.updatePlayer(newBot.GetComponent<Bot>());
            }

        }

        data.createWays();
    }

    //найти угол между векторами
    public static float Angle(Vector3 start, Vector3 end)
    {
        float angle = Mathf.Atan2(end.z - start.z, end.x - start.x) * 180 / Mathf.PI;
        if (0.0f > angle)
            angle += 360.0f;
        return angle;
    }

    //найти центр вектора
    public static Vector3 GetCenter(Vector3 start, Vector3 end)
    {
        Vector3 vec = new Vector3(start.x + ((end.x - start.x) / 2), start.y + ((end.y - start.y) / 2),
            start.z + (end.z - start.z) / 2);

        return vec;
    }

    //найти длину вектора
    float GetVectorLength(Vector3 vector3)
    {
        return vector3.magnitude;
    }
}