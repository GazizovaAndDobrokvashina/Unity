﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoList
{
     /**
     * 1. сделать две сцены с городами и отладить их запуск
     * 2. дописать создание новой игры в дбворке для онлайна 
     * 3. прописать для ботов мухлёж и в целом более продивнутый интеллект
     * 4. торговля между игроками (дописать подтверждение)
     * 5. прописать на какие улицы можно сходить за раз при виде от первого лица и от третьего (стрелочки)
     * 6. добавить проверку на запрет прохождения одной улицы дважды за один ход
     * 7. Дописать влияние количества домов на ренту (дописать влияние домов на стоимость закладывания улиц)
     * 8. расчитать баланс между стоимостью и количеством домов, улиц, монополий
     * 9. прописать банкротство и победу игрока. 
     * 10. придать случайности движению кубиков
     * 11. перенести ДБворк с камеры на emty объект
     * 12. переделать getCurrentPlayer в GameCanvas так как не подходит для мультиплеера (всегда возвращает только первого игрока) (в Gamecontroller тоже есть currentplayer)
     * 13. сделать запись сохранения количества сделанных ходов
     * 14. дописаны ли методы в mousecontroller для зданий и игроков?
     * 15. до какого отчаяния мы дошли, что сохраняем канву в плеере? переписать, чтоб канва хранилась в дбворке
     * 16. юнька ругается, что в mousecontroller много объявленных, но не используемых переменных (смотри предупреждения)
     * 17. реализовать поворот моделек
     * 18. в плеере в апдейте закидываются данные в канву. завязано на айдишнике,  не грохнет ли в мультиплеере?
     * 19. а где вообще можно посмотреть какие есть монополии и какие улицы туда входят? где доп инфу о монополии посмотреть, она же есть в бд
     * 20. разобраться с LOD
     * 21. сделать запись выпавшего числа ходов  на кубиках
     */
     
    /**
     * todoList не срочно:
     * 1. закомментировать DataService
     * 2. Смена времени суток
     * 3. дописать третий режим камеры
     * 4. добавить парковки
     * 
     */
    
     /**
      *todoList костыли:
      * 1. В геймканвас в выводе только улиц игрока лагает индексация. сейчас стоит +1 и работает норм, но откуда он берется не понятно.
      * 2. в плеере findMyPath ретернет иногда паф от 1. мб лучше поставить там 0 и проверять если чё? 
      * 3. в геймконтроллере стоит проверка баланса меньше нуля. это значит, что у нас деньги могут уйти в минус.
      * это нормально или переработаем везде (например, при StepOnMe на улицах), чтоб если уходит в минус, запоминало сколько не хватает, рисовало ноль в капитале и предлагало заложить что-то?
      
      */


}
