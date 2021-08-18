using System;

[Serializable]
public class Score
{
    public string name;
    public int score;

    public override string ToString()
    {
        return name+" "+score;
    }
}
