using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardUIParameters : ICardUIParameters
{
    [SerializeField] private Color @default, buffUp, debuff;
    [SerializeField] private LocalisationText name, description, fraction;
    [SerializeField] private Text initiative, attack, defense, health;

    public IBuffUIParametersFactory buffUIFactory;
    private ICardData cardData;

    public void ShowInitiative(int count, int change = 0) 
    {
        if (count == cardData.Initiative) initiative.color = @default;
        else if (count > cardData.Initiative) initiative.color = debuff;
        else initiative.color = buffUp;

        initiative.text = count.ToString();
        if (change != 0) DisplayChange(initiative, change, change < 0);
    }
    
    public void ShowAttack(int count, int change = 0) => Display(attack, cardData.Attack, count, change);
    public void ShowDefense(int count, int change = 0) => Display(defense, cardData.Defense, count, change);
    public void ShowHealth(int count, int change = 0) => Display(health, cardData.Health, count, change);

    private void Display(Text textParam, int def, int count, int change)
    {
        if (count == def) textParam.color = @default;
        else if (count > def) textParam.color = buffUp;
        else textParam.color = debuff;

        textParam.text = count.ToString();

        if (change != 0) DisplayChange(textParam, change, change > 0);
    }

    private void DisplayChange(Text textParam, int change, bool isBuffUp) =>
        buffUIFactory.GetBuffUI().SetBuff(textParam.transform, change.ToString(), isBuffUp);

    public void SetInitialValues(ICardData cardData)
    {
        this.cardData = cardData;
        name.SetKey(cardData.Name);
        description.SetKey(cardData.Description);
        fraction.SetKey(cardData.Fraction.Name);

        initiative.text = cardData.Initiative.ToString();
        attack.text = cardData.Attack.ToString();
        defense.text = cardData.Defense.ToString();
        health.text = cardData.Health.ToString();
    }
}