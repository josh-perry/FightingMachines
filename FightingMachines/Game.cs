using System;
using SFML.Window;
using SFML.Graphics;

namespace FightingMachines
{
    public class Game
    {
        private readonly RenderWindow mainWindow;
        private readonly Font font;
        private Field currentField;

        public Game()
        {
            var sim = new GenePool(100);
            
            mainWindow = new RenderWindow(new VideoMode(800, 475), "FightingMachines");
            mainWindow.SetVisible(true);

            // Set up events
            mainWindow.Closed += MainWindowOnClosed;
            mainWindow.KeyPressed += MainWindowOnKeyPressed;
            mainWindow.KeyReleased += MainWindowOnKeyReleased;

            font = new Font("data/fonts/DejaVuSansMono.ttf");

            currentField = new Field();

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
            mainWindow.Close();
            Environment.Exit(0);
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
            mainWindow.Clear();

            currentField.Render(mainWindow, font);
            
            mainWindow.Display();
        }

        public void Update()
        {
            mainWindow.DispatchEvents();
        }
    }
}