using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationJWT.Models;
using AuthenticationJWT.Models.Custom;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationJWT.IServices;

public class AuthorizationServices :IAuthorizationService
{

    private readonly PruebaContext _pruebaContext;
    private readonly IConfiguration _configuration;

    public AuthorizationServices(PruebaContext context, IConfiguration configuration)
    {
        this._pruebaContext = context;
        this._configuration = configuration;
    }



    private string generateToken(string idUser)
    {
        var key = _configuration.GetValue<string>("JwtSettings:key");
        var keyByte = Encoding.ASCII.GetBytes(key);


        var claims = new ClaimsIdentity();
        
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUser));

        var credentialTokens = new SigningCredentials(
            new SymmetricSecurityKey(keyByte),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = credentialTokens
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);


        string tokenCreado = tokenHandler.WriteToken(tokenConfig);
        
        
        return tokenCreado;
    }
    
    public async Task<AuthorizationResponse> returnTokens(AuthorizationRequest authorizationRequest)
    {
        var usuarioEncontrado = _pruebaContext.Usuarios.FirstOrDefault(x =>
            x.Nombre == authorizationRequest.userName && x.Clave == authorizationRequest.password);

        if (usuarioEncontrado == null)
            return await Task.FromResult<AuthorizationResponse>(null);

        string tokenCreado = generateToken(usuarioEncontrado.Id.ToString());


        return new AuthorizationResponse() { Token = tokenCreado, Msg = "Ok", result = true };
    }