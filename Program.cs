
using SectorGen2.PlanetGen;

namespace SectorGen2;


public static class SectorGen
{
  public static void Main() {
    Console.WriteLine( new Planet().Generate().PrintUWP() );
  }
}