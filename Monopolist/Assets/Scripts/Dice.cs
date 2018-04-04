using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    
    //массив знначений того, касаются ли поверхности кубика других поверхностей
    private bool[] surfaces;

    private int indexOfSurface;

    //инициализация массива 
    private void Start()
    {
        //размерность 7, чтоб индексы совпадали с количеством точек на грани кубика, нулевой всегда будет false
        surfaces = new bool[7];
    }
    
    //метод, вызываемый гранью кубика, чтобы обозначить, что она столкнулась с чем-то или наоборот вышла из столкновения
    public void SetWhichSurfaceOnCollision(int index, bool surfOnColl)
    {
        surfaces[index] = surfOnColl;
    }

    public void SetPosition(Vector3 positVector3)
    {
        transform.position = positVector3;
    }
    
    //метод, опрашивающий все грани и выясняющий, какая из них находится в коллизии
    private int WhichSurfaceOnCollision()
    {
        
        //счетчик граней в колизии
        int count = 0;
        //номер грани, находящийся в колизии
        int index = 0;
        
        //обход граней
        for (int i = 1; i < surfaces.Length; i++)
        {
            //если грань в коллизии, то запоминаем её номер и увеличиваем счетчик
            if (surfaces[i])
            {
                count++;
                index = i;
            }
        }

        //если грань в коллизии только одна, то всё хорошо и возвращаем её номер
        if (count == 1)
        {
            return index;
        }

        //иначе возвращаем -1
        return -1;
    }

    //возвращаем индекс грани, находящейся в колиззии
    public int GetIndexOfSurface()
    {
        return indexOfSurface;
    }
    
    //обнуляем индекс
    public void resetIndex()
    {
        indexOfSurface = 0;
        surfaces = new bool[7];
    }
    
    //корутина, проверяющая, что  в течение трех секунд грань, находящаяся в колизии не изменилась
    public IEnumerator WaitForAllSurfaces()
    {
       
        yield return new WaitUntil(()=> WhichSurfaceOnCollision()!=-1);
        
        //запоминаем первый результат
        int firstResult = WhichSurfaceOnCollision();
        //ждем три секунды
        //yield return new WaitForSeconds(3);
        //запоминаем второй результат
        //int secondResult = WhichSurfaceOnCollision();
        
        //если первый и второй результат совпадают
//        if (firstResult == secondResult)
//        {
            //то записываем, какая грань находится в коллизии
            indexOfSurface = firstResult;
//        }
//        else
//        {    
//            //иначе записываем -1
//            indexOfSurface = -1;
//        }
            
    }
}