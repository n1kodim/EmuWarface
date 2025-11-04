using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmuWarface.Core;

public static class CommandHandler
{
    public static readonly List<ICmd> Handlers = new();

    public static void Init()
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var type in assembly.GetTypes())
        {
            if (!type.GetInterfaces().Contains(typeof(ICmd))) continue;
            var handler = (ICmd)Activator.CreateInstance(type);
            Handlers.Add(handler);
        }

        Log.Info("[CommandHandler] Loaded {0} commands", Handlers.Count);

        Task.Factory.StartNew(ReadConsole, TaskCreationOptions.LongRunning);
    }

    private static void ReadConsole()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Split(' ').ToList();
            if (input == null) continue;
            input.RemoveAll(inputString => inputString is " " or "");

            if (input.Count == 0) continue;

            var cmdName = input[0];
            var args = input.Skip(1).ToArray();

            var cmd = Handlers.FirstOrDefault(command => command.Names.Contains(cmdName));

            var result = string.Empty;
            if (cmd == null)
            {
                result = "Unknown command. Use 'help' for get command list.";
            }
            else
            {
                try
                {
                    result = cmd.OnCommand(Permission.Admin, args);
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }

            if (!string.IsNullOrEmpty(result))
                Log.Info(result);
        }
    }
}