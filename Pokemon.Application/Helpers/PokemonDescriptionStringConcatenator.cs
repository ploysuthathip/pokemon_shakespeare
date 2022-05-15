using Pokemon.Application.Interfaces;
using Pokemon.Domain.DAOs.Responses;

namespace Pokemon.Application.Helpers;

public static class PokemonDescriptionStringConcatenator
{
    public static string ConcatenateString(IEnumerable<FlavorTextEntries> entries)
    {
        var uniqueText = entries
            .Where(e => e.Language.Name == "en")
            .Select(t => t.FlavorText.ReplaceLineEndings(" "))
            .Distinct()
            .ToList();
        
        var description = string.Join(" ", uniqueText);

        return description;
    }
}