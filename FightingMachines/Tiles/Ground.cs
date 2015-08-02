using SFML.Graphics;

namespace FightingMachines.Tiles
{
    class Ground : Tile
    {
        public Ground()
        {
            Solid = false;
            Glyph = ".";
            Color = new Color(200, 200, 200);
        }
    }
}
