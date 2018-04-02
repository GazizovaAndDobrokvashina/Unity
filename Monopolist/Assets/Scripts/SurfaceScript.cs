using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceScript : MonoBehaviour
{
    //номер грани, совпадает с количеством точек на ней
    [SerializeField] private int numberOfSurface;

    //находится ли грань в столкноении
    private bool SurfaceOnCollision;

    //скрипт родительского кубика
    private Dice parentDice;

    //инициализация скрипта родительского кубика
    private void Start()
    {
        parentDice = transform.parent.gameObject.GetComponent<Dice>();
    }

    //если грань находится в столкновении
    private void OnTriggerEnter(Collider other)
    {
        //обозначаем, что грань находится в столкновении
        SurfaceOnCollision = true;
        //parentDice.SetWhichSurfaceOnCollision(numberOfSurface, SurfaceOnCollision);
        
        //запускаем корутину ожидания
        StartCoroutine(WaitForChangeState());
    }

    //если грань вышла из столкновения
    private void OnTriggerExit(Collider other)
    {
        //обозначаем, что грань вышла из столкновения
        SurfaceOnCollision = false;
        
        //обрываем корутину ожидания
        StopCoroutine(WaitForChangeState());
        
        //отправляем родителю, что грань вышла из столкновения
        parentDice.SetWhichSurfaceOnCollision(numberOfSurface, SurfaceOnCollision);
    }

    // корутина ожидания
    private IEnumerator WaitForChangeState()
    {
        //ждем три секунды
        yield return new WaitForSeconds(3);
        
        //если грань всё ещё в столкновении
        if (SurfaceOnCollision)
        {
            //уведомляем родительский кубик, что грань находится в столкновении
            parentDice.SetWhichSurfaceOnCollision(numberOfSurface, SurfaceOnCollision);
        }

        //завершаем корутину
        yield break;
    }
}