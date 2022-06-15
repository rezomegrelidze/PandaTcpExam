namespace PandaTcpExam.Server;

public static class Helpers
{
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rand = new Random();
        for (int i = 0; i < list.Count; i++)
        {
            int j = rand.Next(i);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}