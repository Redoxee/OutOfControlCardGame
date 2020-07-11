using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum Sigil
{
    Star = 1 << 0,
    Cross = 1 << 1,
    Serpent = 1 << 2,
    Heart = 1 << 3,
}

public struct CardData
{
    public int NumberValue;
    public Sigil Sigil;
}
