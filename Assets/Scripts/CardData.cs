using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum Sigil
{
    Star = 1 << 0,
    Moon = 1 << 1,
    Diamond = 1 << 2,
    Leaf = 1 << 3,
}

public static class SigilUtils
{
    public static string SigilToChar(this Sigil sigil)
    {
        switch (sigil)
        {
            case Sigil.Star:
                return "<sprite name=\"Sym0\">";
            case Sigil.Moon:
                return "<sprite name=\"Sym1\">";
            case Sigil.Diamond:
                return "<sprite name=\"Sym2\">";
            case Sigil.Leaf:
                return "<sprite name=\"Sym3\">";
            default:
                return "?";
        }
    }
}

public struct CardData
{
    public int NumberValue;
    public Sigil Sigil;
}
