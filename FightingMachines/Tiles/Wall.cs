using SFML.Graphics;

namespace FightingMachines.Tiles
{
    class Wall : Tile
    {
        public Wall()
        {
            Solid = true;
            Glyph = "#";
            Color = new Color(200, 200, 200);
        }
    }
}
