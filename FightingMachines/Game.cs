using System;
using SFML.Window;

namespace FightingMachines
{
    public class Game
    {
        private Window mainWindow;

        public Game()
        {
            var sim = new GenePool(100);
            
            mainWindow = new Window(new VideoMode(640, 480), "FightingMachines");
            mainWindow.Closed += MainWindowOnClosed;
            mainWindow.KeyPressed += MainWindowOnKeyPressed;
            mainWindow.KeyReleased += MainWindowOnKeyReleased;

            Loop();
        }

        private void MainWindowOnKeyReleased(object sender, KeyEventArgs keyEventArgs)
        {
            Console.WriteLine(keyEventArgs.Code);
        }

        private void MainWindowOnKeyPressed(object sender, KeyEventArgs keyEventArgs)
        {
        }

        private void MainWindowOnClosed(object sender, EventArgs eventArgs)
        {
            
        }

        public void Loop()
        {
            while (mainWindow.IsOpen)
            {
                Update();
                Render();
            }
        }

        public void Render()
        {
            mainWindow.Display();
        }

        public void Update()
        {
            mainWindow.DispatchEvents();
        }
    }
}