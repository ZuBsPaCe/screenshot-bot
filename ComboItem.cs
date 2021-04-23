namespace ScreenShotBot
{
    public class ComboItem<T>
    {
        public ComboItem(
            string displayName,
            T value)
        {
            DisplayName = displayName;
            Value = value;
        }

        public string DisplayName;
        public T Value;

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
