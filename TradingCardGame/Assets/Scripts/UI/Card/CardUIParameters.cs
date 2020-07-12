using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardUIParameters : ICardUIParameters
{
    [SerializeField] private Color @default, buffUp, debuff;
    [SerializeField] private LocalisationText name = null, description = null, fraction = null;
    [SerializeField] private Text initiative = null, attack = null, defense = null, health = null;

    public IBuffUIParametersFactory buffUIParametersFactory;
    private ICardData cardData;

    private void SetName(string text) => name.SetKey(text);
    private void SetDescription(string text) => description.SetKey(text);
    private void SetFraction(string text) => fraction.SetKey(text);

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
        buffUIParametersFactory.GetBuffUI().SetBuff(textParam.transform, change.ToString(), isBuffUp);

    public void SetInitialValues(ICardData cardData)
    {
        this.cardData = cardData;

        SetName(cardData.Name);
        SetDescription(cardData.Description);
        SetFraction(cardData.Fraction.Name);
        ShowInitiative(cardData.Initiative);
        ShowAttack(cardData.Attack);
        ShowDefense(cardData.Defense);
        ShowHealth(cardData.Health);
    }
}