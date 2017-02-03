using System;
using System.Text.RegularExpressions;

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

        public void LogoutUser(User u)
        {
            if (u == null)
                throw new ArgumentNullException();

            if (UserIsLoggedIn)
                UserIsLoggedIn = false;

        }

        public bool PublishPage(Page p)
        {
            if (p == null)
                throw new ArgumentNullException();

            if (UserIsLoggedIn)
                return true;

            return false;
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
