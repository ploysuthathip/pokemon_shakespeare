using Pokemon.Domain.DAOs.Responses;

namespace Pokemon.Application.Helpers;

public static class PokemonDescriptionStringConcatenator
{
    public static string ConcatenateString(IEnumerable<FlavorTextEntries> entries)
    {
        if (!entries.Any())
        {
            throw new InvalidOperationException(
                "Unable to complete request. Please ensure correct value is provided");
        }
            
        var uniqueText = entries
            .Where(e => e.Language.Name == "en")
            .Select(t => t.FlavorText.ReplaceLineEndings(" "))
            .Distinct()
            .Take(5)
            .ToList();

        var description = string.Concat(uniqueText);

        return description;
    }
}