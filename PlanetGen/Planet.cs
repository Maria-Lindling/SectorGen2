
using System.Text;

namespace SectorGen2.PlanetGen;


public class Planet : CelestialBody, ICelestialBody
{
  private char _starport ;
  
  private short _size ;
  
  private short _atmosphere ;

  private short _hydrographics ; 
  
  private short _population ;

  private short _government ;

  private short _lawlevel ;

  private short _techlevel ; 


  private short _temperature ;

  private List<Faction> _factions ;

  private Faction _primaryfaction ;


  public short Government { get => _primaryfaction?.Government ?? _government ; } 

  public Planet()
  {
    _starport       = 'X' ;
    _size           = 0 ;
    _atmosphere     = 0 ;
    _hydrographics  = 0 ;
    _population     = 0 ;
    _government     = 0 ;
    _lawlevel       = 0 ;
    _techlevel      = 0 ;
    _temperature    = 0 ;
    _factions       = new () ;
    _primaryfaction = null! ;
  }

  internal Planet Generate()
  {
/// SIZE

    _size          = (short) Math.Max( PlanetBuilder.DiceRoll(2,6,-2).Result, 0 ) ;

/// ATMOSPHERE

    _atmosphere    = (short) ( _size + Math.Max( PlanetBuilder.DiceRoll(2,6,-7).Result, 0 ) );


/// TEMPERATURE
    int temperature_dm = 0 ;

    switch ( PlanetBuilder.DiceRoll(1,6,0).Result ) {

      /// inner edge of habitable zone
      case 1: temperature_dm += 4 ; break ;

      /// outer edge of habitable zone
      case 6: temperature_dm -= 4 ; break ;
    }

    switch ( _atmosphere ) {

      case 2: case 3: temperature_dm -= 2 ; break ;

      case 4: case 5: case 14: temperature_dm -= 1 ; break ;

      case 8: case 9: temperature_dm += 1 ; break ;

      case 10: case 13: case 15: temperature_dm += 2 ; break ;

      case 11: case 12: temperature_dm += 6 ; break ;
    }

    _temperature   = (short) Math.Max( PlanetBuilder.DiceRoll(2,6,temperature_dm).Result, 0 ) ;

/// HYDROGRAPHICS

    if( _size <= 1 )
    {
      _hydrographics = 0 ;
    } else {
      int hydrographics_dm = _atmosphere - 7 ;

      if( _atmosphere <= 1 || _atmosphere >= 10 ) hydrographics_dm -= 4 ;

      if( _temperature >= 10 ) hydrographics_dm -= 2 ;

      if( _temperature >= 12 ) hydrographics_dm -= 4 ;

      _hydrographics = (short) Math.Max( PlanetBuilder.DiceRoll(2,6,hydrographics_dm).Result, 0 ) ;
    }

/// POPULATION
    
    _population = (short) Math.Max( PlanetBuilder.DiceRoll(2,6,-2).Result, 0 ) ;


    if( _population > 0 )
    {
/// GOVERNMENT

      int factioncount = PlanetBuilder.DiceRoll(1,3,0).Result ;

      for( int i = 0 ; i < factioncount ; i++ )
      {
        Faction faction = Faction.RandomNew( _population, i == 0 ) ;

        if( faction.Strength == Enums.FactionStrength.Primary ) _primaryfaction = faction ;

        _factions.Add( faction ) ;
      }

/// LAW LEVEL

      _lawlevel = (short) Math.Max( PlanetBuilder.DiceRoll(2,6,_government-7).Result, 0 ) ;

/// STARPORT
/// TODO

/// TECH LEVEL
/// TODO
    }

    return this ;
  }

  public string PrintUWP()
  {
    return $"{_starport}{_size:X1}{_atmosphere:X1}{_hydrographics:X1}{_population:X1}{Government:X1}{_lawlevel:X1}-{_techlevel:X1}";
  }
}