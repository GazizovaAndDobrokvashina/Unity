using System.Collections;
using UnityEngine;

public class NetworkMapBuilder : Photon.MonoBehaviour
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
        NetworkDBwork data = Camera.main.GetComponent<NetworkDBwork>();
            data.OnSceneLoad();

        
        if (PhotonNetwork.isMasterClient) {
        NetworkStreetPath[] pathForBuys = data.GetAllPaths();
        for (int i = 1; i < pathForBuys.Length; i++)
        {
            GameObject newStreetPath = Instantiate(emptyStreet) as GameObject;
            newStreetPath.name = "StreetPath" + i;
            BoxCollider coll = newStreetPath.GetComponent<BoxCollider>();
            coll.size = new Vector3(GetVectorLength(pathForBuys[i].end - pathForBuys[i].start), 3, 8);

            if (pathForBuys[i].canBuy)
            {
                newStreetPath.AddComponent<NetworkPathForBuy>();
                newStreetPath.GetComponent<NetworkPathForBuy>().TakeData(data.GetPathForBuy(i));
                newStreetPath.GetComponent<NetworkPathForBuy>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<NetworkPathForBuy>());
            }
            else
            {
                newStreetPath.AddComponent<NetworkGovermentPath>();
                newStreetPath.GetComponent<NetworkGovermentPath>().TakeData(data.GetGovermentPath(i));
                if (newStreetPath.GetComponent<NetworkGovermentPath>().GetNameOfPrefab().Equals("Court"))
                    data.SetCourt(newStreetPath.GetComponent<NetworkGovermentPath>());
                newStreetPath.GetComponent<NetworkGovermentPath>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<NetworkGovermentPath>());
            }
            newStreetPath.transform.rotation =
                Quaternion.Euler(0f, Angle(pathForBuys[i].start, pathForBuys[i].end), 0f);
            newStreetPath.transform.position = GetCenter(pathForBuys[i].start, pathForBuys[i].end);
        }

        NetworkBuild[] builds = data.GetAllBuilds();

        for (int i = 1; i < builds.Length; i++)
        {
            GameObject newBuild = Instantiate(emptyBuild) as GameObject;
            newBuild.name = builds[i].NameBuild;

            newBuild.AddComponent<NetworkBuild>();
            newBuild.GetComponent<NetworkBuild>().TakeData(builds[i]);
            data.updateBuild(newBuild.GetComponent<NetworkBuild>());

            newBuild.transform.rotation = data.GetPathById(builds[i].IdStreetPath).transform.rotation;
            newBuild.transform.position = newBuild.GetComponent<NetworkBuild>().Place;
            newBuild.SetActive(newBuild.GetComponent<NetworkBuild>().Enable);
        }
           

        NetworkPlayer[] players = data.GetAllPlayers();

        for (int j = 1; j < players.Length; j++)
        {
            Debug.Log(players[j].NickName + "   " + players[j].isBot);
            int id1 = PhotonNetwork.AllocateViewID();
            if (!players[j].IsBot())
            {
                
                GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
                newPlayer.GetComponent<NetworkPlayer>().GetData(players[1]);
                newPlayer.GetComponent<NetworkPlayer>().ViewId = id1;
                newPlayer.transform.position = players[j].Destination;
                data.updatePlayer(newPlayer.GetComponent<NetworkPlayer>());
                transform.Find("/Town").GetComponent<Cameras>()
                    .SetCamera(newPlayer.GetComponentInChildren<Camera>());
                newPlayer.GetComponent<PhotonView>().viewID = id1;
                
            }
            else
            {
                GameObject newBot = Instantiate(emptyBot) as GameObject;
                newBot.GetComponent<NetworkBot>().GetData(players[j]);
                newBot.GetComponent<NetworkBot>().ViewId = id1;
                newBot.transform.position = players[j].Destination;
                data.updatePlayer(newBot.GetComponent<NetworkBot>());
                newBot.GetComponent<PhotonView>().viewID = id1;
            }

        }

        data.createWays();
        }
        else
        {
            StartCoroutine(WaitForUpdate());
        }
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

    IEnumerator WaitForUpdate()
    {
        NetworkDBwork data = Camera.main.GetComponent<NetworkDBwork>();
        while (!data.ready)
        {
            Debug.Log("Syncronization....");
            yield return new WaitForSeconds(2f);
        }
        
        data.createWays();
        
        NetworkStreetPath[] pathForBuys = data.GetAllPaths();
        for (int i = 1; i < pathForBuys.Length; i++)
        {
            GameObject newStreetPath = Instantiate(emptyStreet) as GameObject;
            newStreetPath.name = "StreetPath" + i;
            BoxCollider coll = newStreetPath.GetComponent<BoxCollider>();
            coll.size = new Vector3(GetVectorLength(pathForBuys[i].end - pathForBuys[i].start), 3, 8);

            if (pathForBuys[i].canBuy)
            {
                newStreetPath.AddComponent<NetworkPathForBuy>();
                newStreetPath.GetComponent<NetworkPathForBuy>().TakeData(data.GetPathForBuy(i));
                newStreetPath.GetComponent<NetworkPathForBuy>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<NetworkPathForBuy>());
            }
            else
            {
                newStreetPath.AddComponent<NetworkGovermentPath>();
                newStreetPath.GetComponent<NetworkGovermentPath>().TakeData(data.GetGovermentPath(i));
                if (newStreetPath.GetComponent<NetworkGovermentPath>().GetNameOfPrefab().Equals("Court"))
                    data.SetCourt(newStreetPath.GetComponent<NetworkGovermentPath>());
                newStreetPath.GetComponent<NetworkGovermentPath>().GetNeighbors();
                data.updatePath(newStreetPath.GetComponent<NetworkGovermentPath>());
            }
            newStreetPath.transform.rotation =
                Quaternion.Euler(0f, Angle(pathForBuys[i].start, pathForBuys[i].end), 0f);
            newStreetPath.transform.position = GetCenter(pathForBuys[i].start, pathForBuys[i].end);
        }

        NetworkBuild[] builds = data.GetAllBuilds();

        for (int i = 1; i < builds.Length; i++)
        {
            GameObject newBuild = Instantiate(emptyBuild) as GameObject;
            newBuild.name = builds[i].NameBuild;

            newBuild.AddComponent<NetworkBuild>();
            newBuild.GetComponent<NetworkBuild>().TakeData(builds[i]);
            data.updateBuild(newBuild.GetComponent<NetworkBuild>());

            newBuild.transform.rotation = data.GetPathById(builds[i].IdStreetPath).transform.rotation;
            newBuild.transform.position = newBuild.GetComponent<NetworkBuild>().Place;
            newBuild.SetActive(newBuild.GetComponent<NetworkBuild>().Enable);
        }
           

        NetworkPlayer[] players = data.GetAllPlayers();

        int id1 = PhotonNetwork.AllocateViewID();
        bool hasOwn = false;
        for (int j = 1; j < players.Length; j++)
        {
            if (!players[j].IsBot())
            {
                
                GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
                newPlayer.GetComponent<NetworkPlayer>().GetData(players[j]);
                newPlayer.transform.position = players[j].Destination;
                newPlayer.transform.GetComponentInChildren<Camera>().gameObject.SetActive(false);
                data.updatePlayer(newPlayer.GetComponent<NetworkPlayer>());
                newPlayer.GetComponent<PhotonView>().viewID = players[j].ViewId;
            }
            else
            {
                if (!hasOwn)
                {
                    hasOwn = true;
                    GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
                    newPlayer.GetComponent<NetworkPlayer>().GetData(players[j], "newName"+id1);
                    newPlayer.transform.position = players[j].Destination;
                    data.updatePlayer(newPlayer.GetComponent<NetworkPlayer>());
                    newPlayer.GetComponent<PhotonView>().viewID = id1;
                    //TODO: заменить на имя зашедшего игрока
                    GetComponent<PhotonView>().RPC("ReplaceBotWithPlayer",PhotonTargets.Others,j,id1,"newName"+id1);
                }
                else
                {
                    GameObject newBot = Instantiate(emptyBot) as GameObject;
                    newBot.GetComponent<NetworkBot>().GetData(players[j]);
                    newBot.transform.position = players[j].Destination;
                    data.updatePlayer(newBot.GetComponent<NetworkBot>());
                    newBot.GetComponent<PhotonView>().viewID = players[j].ViewId;
                }
            }

        }
    }

    [PunRPC]
    public void ReplaceBotWithPlayer(int playerId, int viewId, string name)
    {
        NetworkDBwork data = Camera.main.GetComponent<NetworkDBwork>();
        NetworkPlayer player = data.GetPlayerbyId(playerId);
        GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
        newPlayer.transform.GetComponentInChildren<Camera>().gameObject.SetActive(false);;
        newPlayer.GetComponent<NetworkPlayer>().GetData(player, name);
        newPlayer.transform.position = player.Destination;
        Destroy(player.gameObject);
        data.updatePlayer(newPlayer.GetComponent<NetworkPlayer>());
        newPlayer.GetComponent<PhotonView>().viewID = viewId;
    }

    void OnPhotonSerializeView()
    {
        
    }
}