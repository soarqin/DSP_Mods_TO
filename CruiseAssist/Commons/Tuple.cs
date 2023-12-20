namespace CruiseAssist.Commons;

internal struct Tuple<T1, T2>(T1 v1, T2 v2)
{
    public T1 Item1 { get; set; } = v1;

    public T2 Item2 { get; set; } = v2;
}
