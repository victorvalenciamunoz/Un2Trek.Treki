using ErrorOr;

namespace Un2Trek.Trekis.API;

internal static class Errors
{
    public static readonly Error InvalidCredentials = Error.Validation(code: "U002", description: "Usuario o contraseña erróneos");
}
