using System.Collections.Generic;

public interface IFractionsData
{
    List<IFraction> Fractions { get; }
    IFraction CurrentFraction { get; set; }
    IFraction GetFraction(string name);
}