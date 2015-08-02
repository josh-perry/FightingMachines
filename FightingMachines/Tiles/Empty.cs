using SFML.Graphics;

namespace FightingMachines.Tiles
{
    class Empty : Tile
    {
        public Empty()
        {
            Solid = false;
            Glyph = " ";
            Color = new Color(64, 64, 64);
        }
    }
}
