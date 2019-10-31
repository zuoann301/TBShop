using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Admin.Common
{
    public class AdminSession : AceSession<string>
    {
        public string AccountName { get; set; }
        public string Name { get; set; }
        // public string OrgIds { get; set; }
        //public string RoleIds { get; set; }
        public string LoginIP { get; set; }
        //public DateTime LoginTime { get; set; }
        public bool IsAdmin { get; set; }


        public List<Claim> ToClaims()
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("UserId", this.UserId ?? ""));
            claims.Add(new Claim("AccountName", this.AccountName ?? ""));
            claims.Add(new Claim("Name", this.Name ?? ""));
            //claims.Add(new Claim("OrgIds", this.OrgIds ?? ""));
            //claims.Add(new Claim("RoleIds", this.RoleIds ?? ""));
            claims.Add(new Claim("LoginIP", this.LoginIP ?? ""));
            claims.Add(new Claim("IsAdmin", this.IsAdmin.ToString()));

            return claims;
        }

        public static AdminSession Parse(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                AdminSession session = new AdminSession()
                {
                    UserId = claims.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "",
                    AccountName = claims.Claims.FirstOrDefault(x => x.Type == "AccountName")?.Value ?? "",
                    Name = claims.Claims.FirstOrDefault(x => x.Type == "Name")?.Value ?? "",
                    //DepartmentId = claims.Claims.FirstOrDefault(x => x.Type == "DepartmentId")?.Value ?? "",
                    //DutyId = claims.Claims.FirstOrDefault(x => x.Type == "DutyId")?.Value ?? "",
                    //RoleId = claims.Claims.FirstOrDefault(x => x.Type == "RoleId")?.Value ?? "",
                    //LoginIP = claims.Claims.FirstOrDefault(x => x.Type == "LoginIP")?.Value ?? "",
                    //LoginTime = DateTimeHelper.Parse(long.Parse(claims.Claims.FirstOrDefault(x => x.Type == "LoginTime")?.Value ?? "0")),
                    IsAdmin = bool.Parse(claims.Claims.FirstOrDefault(x => x.Type == "IsAdmin")?.Value ?? "false")
                };
                return session;
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
