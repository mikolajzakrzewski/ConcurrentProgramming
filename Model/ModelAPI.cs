namespace Model
{
    public abstract class ModelAPI
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract void CreateBalls(int number, int radius);
        public abstract void Start();
        public abstract void ResetTable();
    }
}
