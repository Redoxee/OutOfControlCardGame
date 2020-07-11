using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GameController
{
    private void InitializeRuleData()
    {
        this.availableRules.Clear();

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {false   , true  , false },
                        new bool[] {true    , false , true  },
                        new bool[] {false   , true  , false },
                    },

                    Text = "Cards can't be placed on diagonals",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {true    , false , true  },
                        new bool[] {true    , false , true  },
                        new bool[] {true    , false , true  },
                    },

                    Text = "Cards can't be placed the center column",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    ForceOnArray = false,
                    AllowedCells = new bool[][]
                    {
                        new bool[] {true    , false , true },
                        new bool[] {true    , false , true  },
                        new bool[] {true    , false , true  },
                    },

                    Text = "Cards must be placed the center column",
                }
            );
    }
}
