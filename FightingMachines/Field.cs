using FightingMachines.Tiles;
using SFML.Graphics;
using SFML.System;

namespace FightingMachines
{
    public class Field
    {
        public Tile[,] Tiles;
        public int Width;
        public int Height;
        public RenderTarget RenderTarget;

        public Field(int width = 80, int height = 25)
        {
            Width = width;
            Height = height;

            Tiles = new Tile[width, height];

            for(var w = 0; w < Width; w++)
            {
                for (var h = 0; h < Height; h++)
                {
                    Tile tile;

                    if(w == 0 || w == Width - 1 || h == 0 || h == Height - 1)
                        tile = new Empty();
                    else if (w == 1 || w == Width - 2 || h == 1 || h == Height - 2)
                        tile = new Wall { Glyph = "#" };
                    else
                        tile = new Ground();

                    Tiles[w, h] = tile;
                }
            }
        }

        public void Render(RenderWindow window, Font font)
        {   
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var tile = Tiles[x, y];
                    var t = new Text(Tiles[x, y].Glyph, font, 16) {Position = new Vector2f(x*10, y*19)};

                    var s = new RectangleShape(new Vector2f(10, 19))
                    {
                        FillColor = new Color(tile.Color.R, tile.Color.G, tile.Color.B, (byte) (tile.Color.A / 2)),
                        Position = new Vector2f(x*10, y*19)
                    };

                    window.Draw(s);
                    window.Draw(t);
                }
            }
        }
    }
}
