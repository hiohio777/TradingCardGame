using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TokensPanel : MonoBehaviour, ITokensPanel
{
    [SerializeField] private byte maxTokens = 3;
    private List<TokenImage> tokensImage = new List<TokenImage>();

    private void OnEnable()
    {
        tokensImage.ForEach(x => Destroy(x.gameObject));
        tokensImage.Clear();
    }

    public bool AddToken(ITokenData token, byte count)
    {
        if (tokensImage.Count >= maxTokens) return false;

        bool result = false;
        foreach (var item in tokensImage)
            if (item.Name == token.Name)
            {
                result = true;
                break;
            }

        if (result == false || token.IsSingle == false)
        {
            for (int i = 0; i < count; i++)
            {
                if (tokensImage.Count < maxTokens)
                    tokensImage.Add(TokenImage.CreatPrefab(token, transform));
            }
            return true;
        }
        else return false;
    }

    public void DeleteToken(string Name)
    {
        int i = 0;
        while (i < tokensImage.Count)
        {
            if (tokensImage[i].Name == Name)
                tokensImage.RemoveAt(i);
            else i++;
        }
    }

    public int GetCountToken(string Name)
    {
        if (tokensImage.Count == 0)
            return 0;

        int count = 0;
        tokensImage.ForEach(x => { if (x.Name == Name) count++; });

        return count;
    }
}
