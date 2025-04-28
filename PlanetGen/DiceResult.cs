using System.Collections ;

namespace SectorGen2.PlanetGen ;

internal class DiceResult
{

  private List<int> _dierolls ;

  private int _dicemodifier ;


  internal int Result { get => _dierolls.Aggregate((int)_dicemodifier, (acc, x) => acc + x); }

  internal List<int> DieRolls { get => _dierolls ; }


  internal DiceResult(int dicemodifier = 0)
  {
    _dierolls     = new () ;
    _dicemodifier = dicemodifier ;
  }
}