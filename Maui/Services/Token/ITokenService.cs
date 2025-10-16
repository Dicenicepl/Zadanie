using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Services.Token
{
    public interface ITokenService
    {
        Task SaveTokenAsync (string token);
        Task<string?> GetTokenAsync();
        Task RemoveTokenAsync ();
    }
}
