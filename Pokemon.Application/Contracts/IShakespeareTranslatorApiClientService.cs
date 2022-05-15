using Pokemon.Domain.DAOs.Requests;
using Pokemon.Domain.DAOs.Responses;

namespace Pokemon.Application.Contracts;

public interface IShakespeareTranslatorApiClientService
{
    Task<ShakespeareApiResponse> GetTranslatedShakespeareText(ShakeSpearApiRequest shakeSpearApiRequest);
}