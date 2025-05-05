
using System.Collections.Specialized;
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
      short starport_dm = 0 ;

      if( _population <= 2 ) starport_dm -= 2 ;
      else if( _population <= 4 ) starport_dm -= 1 ;

      if( _population >= 10 ) starport_dm += 2 ;
      else if( _population >= 8 ) starport_dm += 1 ;

      _starport = Math.Max(PlanetBuilder.DiceRoll(2, 6, starport_dm).Result, 0) switch
      {
        0 or 1 or 2 => 'X',
        3 or 4      => 'E',
        5 or 6      => 'D',
        7 or 8      => 'C',
        9 or 10     => 'B',
        _           => 'A',
      } ;

/// TECH LEVEL
/// TODO
      short techlevel_dm = 0 ;
      short minimum_techlevel = _atmosphere switch
      {
        0 or 1 or 10 or 15  => 8,
        2 or 3 or 13 or 14  => 5,
        4 or 7 or 9         => 3,
        11                  => 9,
        12                  => 10,
        _                   => 0,
      } ;

      switch ( _starport )
      {
        case 'X': techlevel_dm -= 4 ; break;
        case 'C': techlevel_dm += 2 ; break;
        case 'B': techlevel_dm += 4 ; break;
        case 'A': techlevel_dm += 6 ; break;
      }

      if( _size <= 1 ) techlevel_dm += 2 ;
      else if (_size <= 4 ) techlevel_dm += 1 ;

      if( _atmosphere <= 3 || _atmosphere >= 10 ) techlevel_dm += 1 ;

      if( _hydrographics == 0 || _hydrographics == 9 ) techlevel_dm += 1 ;
      else if ( _hydrographics == 10 ) techlevel_dm += 2 ;

      if( (_population >= 1 && _population <= 5) || _population == 8 ) techlevel_dm += 1 ;
      else if( _population == 9 ) techlevel_dm += 2 ;
      else if( _population == 10 ) techlevel_dm += 4 ;

      if( _government == 0 || _government == 5 ) techlevel_dm += 1 ;
      else if( _government == 7 ) techlevel_dm += 2 ;
      else if( _government == 13 || _government == 14 ) techlevel_dm -= 2 ;

      _techlevel = (short) Math.Max(PlanetBuilder.DiceRoll(1, 6, techlevel_dm).Result, minimum_techlevel) ;
    }

    return this ;
  }

  public string PrintUWP()
  {
    return $"{_starport}{_size:X1}{_atmosphere:X1}{_hydrographics:X1}{_population:X1}{Government:X1}{_lawlevel:X1}-{_techlevel:X1}";
  }
}