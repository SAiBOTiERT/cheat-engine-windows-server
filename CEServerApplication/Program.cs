namespace CEServerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            CEServerWindows.CheatEngineServer server = new CEServerWindows.CheatEngineServer();

            server.StartAsync().Wait();
        }
    }
}
