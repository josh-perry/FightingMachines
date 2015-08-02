using SFML.Graphics;

namespace FightingMachines
{
    public abstract class Tile
    {
        public string Glyph = "#";
        public Color Color;
        public Font Font;
        public bool Solid = false;

        public Text DrawableText
        {
            get
            {
                if (DrawableText == null)
                    DrawableText = new Text(Glyph, Font);

                return DrawableText;
            }
            set { }
        }
    }
}