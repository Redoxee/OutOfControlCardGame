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
                        new bool[] {false    , true , true  },
                        new bool[] {false    , true , true  },
                        new bool[] {false    , true , true  },
                    },

                    Text = "Cards can't be placed in the left column",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] { true, true , false  },
                        new bool[] { true, true , false  },
                        new bool[] { true, true , false  },
                    },

                    Text = "Cards can't be placed in the right column",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {false   , false , false  },
                        new bool[] {true    , true  , true  },
                        new bool[] {true   , true , true  },
                    },

                    Text = "Cards can't be placed in the top row",
                }
            );

        availableRules.Add(
                new StaticArrayRule
                {
                    AllowedCells = new bool[][]
                    {
                        new bool[] {true    , true  , true  },
                        new bool[] {true   , true , true  },
                        new bool[] {false   , false , false  },
                    },

                    Text = "Cards can't be placed in the bottom row",
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

        availableRules.Add(new NoConstraintRule());

        availableRules.Add(new DifferentSigilAdjascentRule
        {
        });

        availableRules.Add(new OddCardsOnOddCardsRule());

        availableRules.Add(new EvenCardsMustBeStackedRule());

        this.GenerateCombinedRules();
    }

    private List<RuleData> combinedRules = new List<RuleData>();
    private void GenerateCombinedRules()
    {
        int baseNumberOfRules = this.availableRules.Count;
        for (int index = 0; index < (baseNumberOfRules - 1); ++index)
        {
            for (int jndex = index + 1; jndex < baseNumberOfRules; ++jndex)
            {
                if (this.availableRules[index] is NoConstraintRule || 
                    this.availableRules[jndex] is NoConstraintRule)
                {
                    continue;
                }

                this.combinedRules.Add(new CombinedRule
                {
                    rule1 = this.availableRules[index],
                    rule2 = this.availableRules[jndex],
                });
            }
        }
    }

    private void TransferCombinedRules()
    {
        int numberOfCombinedRules = this.combinedRules.Count;
        for (int index = 0; index < numberOfCombinedRules; ++index)
        {
            this.availableRules.Add(this.combinedRules[index]);
        }

        this.combinedRules.Clear();
    }

    private void TransferOneCombinedRule()
    {
        if (this.combinedRules.Count == 0)
        {
            return;
        }

        int selectedIndex = UnityEngine.Random.Range(0, this.combinedRules.Count);
        RuleData data = this.combinedRules[selectedIndex];
        this.combinedRules.RemoveAt(selectedIndex);
        this.availableRules.Add(data);
    }
}
