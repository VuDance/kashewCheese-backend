using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Authentication
{
    public class AuthenticationResponse
    {
        public string token {  get; set; }
        public string refeshToken {  get; set; }
    }
}
