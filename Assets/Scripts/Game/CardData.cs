using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum Sigil
{
    Diamond = 1 << 0,
    Heart   = 1 << 1,
    Clover  = 1 << 2,
    Spike   = 1 << 3,
}

public static class SigilUtils
{
    public static string SigilToChar(this Sigil sigil)
    {
        switch (sigil)
        {
            case Sigil.Diamond:
                return "<sprite name=\"Sym0\">";
            case Sigil.Heart:
                return "<sprite name=\"Sym1\">";
            case Sigil.Clover:
                return "<sprite name=\"Sym2\">";
            case Sigil.Spike:
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
