using server.Dtos;

namespace server.Data
{
    public static class TownStore
    {
        public static List<TownDto> townList = new()
        {
            new() {
                Id = 1,
                Name = "Vista a la piscina",
                Occupants = 3,
                SquareMeter = 50
            },
            new() {
                Id = 2,
                Name = "Vista al mar",
                Occupants = 4,
                SquareMeter = 80
            }
        };
    }
}