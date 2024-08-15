namespace NFind
{
    internal class ConsoleLineSource: ILineSource
    {
        private int number = 0;

        public ConsoleLineSource()
        {
        }

        public string Name => string.Empty;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public Line? ReadLine()
        {
            var s = Console.ReadLine();
            if (s == null)
            {
                return null;
            }
            else
            {
                return new Line() { LineNumber = ++number, Text = s };
            }
        }

    }
}