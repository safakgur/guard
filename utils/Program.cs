namespace Dawn.Utils
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            if (args is null || args.Length == 0)
            {
                Console.WriteLine("No arguments specified.");
                return;
            }

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = !cts.IsCancellationRequested;
                cts.Cancel();
            };

            var command = args[0];
            Console.WriteLine("[{0}]", command);

            switch (command)
            {
                case "snippets":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("No path specified.");
                        return;
                    }

                    await Snippet.SaveSnippets(args[1], cts.Token).ConfigureAwait(false);
                    return;
            }
        }
    }
}
