using MovingObjects.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationHelpers
{
    public static class Token
    {
        private static string separator = ":::"; 
        // Token is in the format: hashForPasswordAndUserName + ":" + hashForId
        public static string GenerateToken(string hash, int id)
        {   
            var token = hash + separator + id.ToString();
            return token;
        }

        public static bool ValidateToken(string token, Player player) 
        {
            var separatorIndex = token.LastIndexOf(separator);
            var passwordIsValid = PasswordHash.ValidatePassword(player.Password,
                token.Substring(0,separatorIndex));
            var idIsValid = ValidateId(token.Substring(separatorIndex+separator.Length),player.Id);

            return passwordIsValid && idIsValid;
        }

        private static bool ValidateId(string idString, int id)
        {
            int idStringToNumber;
            bool isInteger;
            isInteger = int.TryParse(idString, out idStringToNumber);
            if (!isInteger)
            {
                return false;
            }
            else
            {
                return id == idStringToNumber;
            }
        }
    }
}
