namespace tk7texturevanilla
{
    /// <summary>
    /// A simple dictionary class.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    public class DictionaryMap<X, Y>
    {
        public X x { get; private set; }
        public Y y { get; private set; }
        public DictionaryMap(X x, Y y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
