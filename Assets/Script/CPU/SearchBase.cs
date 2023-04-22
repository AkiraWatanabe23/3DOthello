public abstract class SearchBase
{
    public abstract int[,] FlipSimurate(int[,] board, int x, int y);

    public virtual int SetSimurate(string pos) { return 0; }
}
