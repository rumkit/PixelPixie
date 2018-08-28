namespace Pixie
{
    struct Pixel
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}