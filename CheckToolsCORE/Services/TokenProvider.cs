
using CheckToolsCORE.OmaData.Context;
using CheckToolsCORE.OmaData.DbModel;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace CheckToolsCORE.Services
{
    public class TokenProvider
    {
        public Tuple<string,string,int> LoginUser(string userID, string session)
        {

            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(session))
                return null;

            DbContextMitico db = new DbContextMitico();
            
            var numSes = decimal.Parse(session);
            
            var sessioni = db.ApexWsSessions.Where(x => x.ApexSessionId ==numSes  && x.UserName == userID).ToList();
            
            if (sessioni == null || sessioni.Count == 0)
            {
                return null;
            }

            //Get user details for the user who is trying to login - JRozario
            //var user = UserList.SingleOrDefault(x => x.USERID == userID);


            var users = db.ApxUser.Where(x => x.LdapUser == userID || x.Username.ToUpper().Trim() == userID).FirstOrDefault();

            //Authenticate User, Check if its a registered user in DB 
            if (users == null )
                return null;

            //If its registered user, check user password stored in DB 
            //For demo, password is not hashed. Its just a string comparison - JRozario
            //In reality, password would be hashed and stored in DB. Before comparing, hash the password - JRozario


            //Authentication successful, Issue Token with user credentials - JRozario
            //Provide the security key which was given in the JWToken configuration in Startup.cs - JRozario
            var key = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
            //Generate Token for user - JRozario
            var JWToken = new JwtSecurityToken(
                issuer: "http://localhost:45092/",
                audience: "http://localhost:45092/",
                claims: GetUserClaims(users),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                   
                //Using HS256 Algorithm to encrypt Token - JRozario
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return new Tuple<string, string, int>( token, users.LDAP_USER,(int) users.ID);

        }
        //Using hard coded collection list as Data Store for demo. In reality, User data comes from Database or some other Data Source - JRozario
        private IEnumerable<Claim> GetUserClaims(ApxUser user)
        {
            List<Claim> claims = new List<Claim>();
            
            Claim _claim;
            
            _claim = new Claim(ClaimTypes.Name , user.COGNOME + " " + user.NOME);
            
                        
            claims.Add(_claim);


            _claim = new Claim(ClaimTypes.Role, "IDP");
            claims.Add(_claim);
            //_claim = new Claim("EMAILID", user.EMAILID);
            //claims.Add(_claim);
            //_claim = new Claim("PHONE", user.PHONE);
            //claims.Add(_claim);
            //_claim = new Claim(user.ACCESS_LEVEL, user.ACCESS_LEVEL);
            //claims.Add(_claim);

            //if (user.WRITE_ACCESS != "")
            //{
            //    _claim = new Claim(user.WRITE_ACCESS, user.WRITE_ACCESS);
            //    claims.Add(_claim);
            //}
            return claims.AsEnumerable<Claim>();
        }
    }
}