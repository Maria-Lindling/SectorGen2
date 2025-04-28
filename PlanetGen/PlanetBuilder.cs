
namespace SectorGen2.PlanetGen;


public class PlanetBuilder
{
  private static Random _random = new () ;

  public PlanetBuilder()
  {
    
  }

  internal static DiceResult DiceRoll(int dice, int faces, int dicemodifier)
  {
    DiceResult result = new (dicemodifier) ;

    for( int i = 0 ; i < dice ; i++ )
    {
      result.DieRolls.Add( ( _random.Next() % faces ) + 1 ) ;
    }

    return result ;
  }
}