using SectorGen2.PlanetGen.Enums;

namespace SectorGen2.PlanetGen ;

internal class Faction
{
  private short _government ;

  private FactionStrength _strength ;


  internal short Government { get => _government ; }

  internal FactionStrength Strength { get => _strength ; }


  internal Faction()
  {
  
  }

  internal static Faction RandomNew( int population, bool primary = false )
  {
    FactionStrength factionstrength = primary ? FactionStrength.Primary : FactionStrength.Extinct ;

    if( !primary )
    {
      switch ( PlanetBuilder.DiceRoll(2,6,0).Result )
      {
        case 2: case 3:   factionstrength = FactionStrength.Obscure ;      break ;
        case 4: case 5:   factionstrength = FactionStrength.Fringe ;       break ;
        case 6: case 7:   factionstrength = FactionStrength.Minor ;        break ;
        case 8: case 9:   factionstrength = FactionStrength.Notable ;      break ;
        case 10: case 11: factionstrength = FactionStrength.Significant ;  break ;
        case 12:          factionstrength = FactionStrength.Overwhelming ; break ;
      }
    }

    Faction result = new () {
      _government = (short) Math.Max( PlanetBuilder.DiceRoll(2,6,population-7).Result, 0 ),
      _strength = factionstrength,
    } ;

    return result ;
  } 
}