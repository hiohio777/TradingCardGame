public enum EventTriggerEnum
{
    Default,
    StartRound, // В начале раунда
    LeadUp, // В начале хода карты
    BeforeAttack, // Перед атакой
    AfterAttack, // После атаки
    FinalAbility, // Карта завершает ход 
    EndRound, // В конце раунда

    Death, // Смерть
    Implementation, // Способности во время фазы боя Призыв

    ResponsekDamage, //Способности реагирующие на получение картой урона(какой именно урон порлучен проверяется при проверке условий способности)
}
