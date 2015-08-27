using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace Wii2Scratch.ScratchHelper
{


    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.Title = "Wii2Scratch";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" __          ___ _ ___   _____                _       _     ");
            Console.WriteLine(@" \ \        / (_|_)__ \ / ____|              | |     | |    ");
            Console.WriteLine(@"  \ \  /\  / / _ _   ) | (___   ___ _ __ __ _| |_ ___| |__  ");
            Console.WriteLine(@"   \ \/  \/ / | | | / / \___ \ / __| '__/ _` | __/ __| '_ \ ");
            Console.WriteLine(@"    \  /\  /  | | |/ /_ ____) | (__| | | (_| | || (__| | | |");
            Console.WriteLine(@"     \/  \/   |_|_|____|_____/ \___|_|  \__,_|\__\___|_| |_|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            if (!FindWiiMotes())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("***************************************");
                Console.WriteLine(@" ______ _____  _____   ____  _____  _ ");
                Console.WriteLine(@"|  ____|  __ \|  __ \ / __ \|  __ \| |");
                Console.WriteLine(@"| |__  | |__) | |__) | |  | | |__) | |");
                Console.WriteLine(@"|  __| |  _  /|  _  /| |  | |  _  /| |");
                Console.WriteLine(@"| |____| | \ \| | \ \| |__| | | \ \|_|");
                Console.WriteLine(@"|______|_|  \_\_|  \_\\____/|_|  \_(_)");
                Console.WriteLine();
                Console.WriteLine("\tIt looks like there is no Wii controller");
                Console.WriteLine("\tconnected to you computer :-(");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
                Console.WriteLine("\t  - Go to bluetooth settings");
                Console.WriteLine("\t  - Make sure your computer is discovering bluetooth devices");
                Console.WriteLine("\t  - Press and hold the '1' and '2' button at your Wii controller");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tPress enter to stop...");
                Console.WriteLine();
                Console.WriteLine("***************************************");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine();

            var port = 15002;

            try
            {
                // Start OWIN host 
                var startOptions = new StartOptions()
                {
                    Port = port,
                    ServerFactory = "Nowin"
                };
                using (var app = WebApp.Start<Startup>(startOptions))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("*****************************************************");
                    Console.WriteLine(@" __          __         _                         _ ");
                    Console.WriteLine(@" \ \        / /        | |                       | |");
                    Console.WriteLine(@"  \ \  /\  / /__   ___ | |__   ___   _____      _| |");
                    Console.WriteLine(@"   \ \/  \/ / _ \ / _ \| '_ \ / _ \ / _ \ \ /\ / / |");
                    Console.WriteLine(@"    \  /\  / (_) | (_) | | | | (_) | (_) \ V  V /|_|");
                    Console.WriteLine(@"     \/  \/ \___/ \___/|_| |_|\___/ \___/ \_/\_/ (_)");
                    Console.WriteLine();
                    Console.WriteLine("\tAll is ok!");
                    Console.WriteLine("\tListening on port" + port.ToString());
                    Console.WriteLine("\tCheck http://localhost:" + port.ToString() + "/poll (It's cool :-)");
                    Console.WriteLine();
                    Console.WriteLine("\tPress enter to stop...");
                    Console.WriteLine();
                    Console.WriteLine("*****************************************************");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("*****************************************************");
                Console.WriteLine("ERROR - Cannot listen on port " + port.ToString() + " : " + ex.ToString());
                Console.WriteLine();
                Console.WriteLine("Press enter to stop...");
                Console.WriteLine();
                Console.WriteLine("*****************************************************");
                Console.ReadLine();
            }
        }

        private static bool FindWiiMotes()
        {

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Looking for Wii controllers...");
            Console.WriteLine();

            var wiimoteCollection = new WiimoteCollection();

            try
            {
                wiimoteCollection.FindAllWiimotes();
            }
            catch (WiimoteNotFoundException ex)
            {
                Console.WriteLine("No Wii controller found: " + ex.Message);
                return false;
            }
            catch (WiimoteException ex)
            {
                Console.WriteLine("Wii controller error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error: " + ex.Message);
                return false;
            }

            Console.WriteLine("Found {0} Wii controller{1}", wiimoteCollection.Count, wiimoteCollection.Count == 1 ? "" : "s");
            Console.WriteLine();

            var index = 1;
            foreach (var wiimote in wiimoteCollection)
            {
                AppState.WiiControllers.Add(new WiiController(index, wiimote));
                index++;
            }
            return true;
        }
    }
}


