using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HemtentaTdd2017.blog
{
    public class Blog : IBlog
    {
        IAuthenticator auth;

        public void SetAuthenticator(IAuthenticator auth)
        {
            this.auth = auth;
        }

        public bool UserIsLoggedIn { get; private set; }


        // Försöker logga in en användare. Man kan
        // se om inloggningen lyckades på property
        // UserIsLoggedIn.
        // Kastar ett exception om User är null.
        public void LoginUser(User u)
        {
            if (u == null)
                throw new ArgumentNullException();

            User dbUser = auth.GetUserFromDatabase(u.Name);

            if (dbUser == null)
                throw new NullReferenceException();

            if (u.Password == dbUser.Password)
            {
                UserIsLoggedIn = true;
            }
            
        }

        // Försöker logga ut en användare. Kastar
        // exception om User är null.
        public void LogoutUser(User u)
        {
            if (u == null)
                throw new ArgumentNullException();

            if (UserIsLoggedIn)
                UserIsLoggedIn = false;

        }


        // För att publicera en sida måste Page vara
        // ett giltigt Page-objekt och användaren
        // måste vara inloggad.
        // Returnerar true om det gick att publicera,
        // false om publicering misslyckades och
        // exception om Page har ett ogiltigt värde.
        public bool PublishPage(Page p)
        {
            throw new NotImplementedException();
        }

        // För att skicka e-post måste användaren vara
        // inloggad och alla parametrar ha giltiga värden.
        // Returnerar 1 om det gick att skicka mailet,
        // 0 annars.
        public int SendEmail(string address, string caption, string body)
        {
            const string emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                    + "@"
                                    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            if (string.IsNullOrEmpty(caption))
                throw new ArgumentException("Null or empty strings not valid", caption);

            if (string.IsNullOrEmpty(body))
                throw new ArgumentException("Null or empty strings not valid", body);

            if (string.IsNullOrEmpty(address) || !Regex.IsMatch(address, emailRegex))
                throw new ArgumentException("Not a valid e-mail address", address);


            return UserIsLoggedIn ? 1 : 0;

        }
    }
}
