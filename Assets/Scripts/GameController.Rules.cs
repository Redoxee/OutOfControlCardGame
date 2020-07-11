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

                    Text = "Cards can't be placed in the center column",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {false    , true , false  },
                        new bool[] {false    , true , false  },
                        new bool[] {false    , true , false  },
                    },

                    Text = "Cards can't be placed in the border column",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {false   , false , false  },
                        new bool[] {true    , true  , true  },
                        new bool[] {false   , false , false  },
                    },

                    Text = "Cards can't be placed in the top or bottom row",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {true    , true  , true  },
                        new bool[] {false   , false , false  },
                        new bool[] {true    , true  , true  },
                    },

                    Text = "Cards can't be placed in the central row",
                }
            );

        availableRules.Add(new StaticArrayRule
        {
            AllowedCells = new bool[][]
            {
                new bool[]{ true, true, true},
                new bool[]{ true, true, true},
                new bool[]{ true, true, true},
            },

            Text = "No constraint",
        });

        availableRules.Add(new DifferentSigilAdjascentRule
        {
        });

        availableRules.Add(new OddCardsOnOddCardsRule());

        availableRules.Add(new EvenCardsMustBeStackedRule());
    }
}
