using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FractionsData : IFractionsData
{
    public List<IFraction> Fractions { get; }
    public IFraction CurrentFraction { get; set; }

    public FractionsData()
    {
        var temp = Resources.LoadAll<FractionScriptable>("Data/Fractions/").ToList();
        Fractions = temp.Where(x => x.Name != "neutral").ToList<IFraction>();
        Fractions.Add(temp.Where(x => x.Name == "neutral").First()); //Добавить нейтралов последними в список фракций

        if (Fractions.Count > 0)
            CurrentFraction = Fractions[0];
        else Debug.LogError("Fractions not found!");
    }

    public IFraction GetFraction(string name) => Fractions.Where(x => x.Name == name).First();
}