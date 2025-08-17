using System.Security.Claims;

namespace StockPlaform.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {



            return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
            //            Yeh method JWT token ke andar se username nikaal kar deta hai, jo humne login ke time pe token me GivenName ke through dala tha. hmny TokenService my claims dale thy whi username niakl rhy hen






            // Agar aapko email ya kisi aur claim ki zaroorat hai, toh aap wo bhi nikal skty ho 



        }
    }
}