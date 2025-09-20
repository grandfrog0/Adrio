using System.Collections.Generic;

public class InitComparer : IComparer<IInitializable>
{
    public int Compare(IInitializable x, IInitializable y)
    {
        int res1 = x.Order.CompareTo(y.Order);
        if (res1 != 0) return res1;

        return x.GetHashCode().CompareTo(y.GetHashCode());
    }
}
